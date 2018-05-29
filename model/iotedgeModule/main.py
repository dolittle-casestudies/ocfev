# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for
# full license information.

#katya modified

import os
import random
import time
import sys
import iothub_client
from iothub_client import IoTHubClient, IoTHubClientError, IoTHubTransportProvider
from iothub_client import IoTHubMessage, IoTHubMessageDispositionResult, IoTHubError
import json
from keras.models import load_model
import numpy as np
from keras.models import model_from_json
import tensorflow as tf

TEMPERATURE_THRESHOLD = 25
TWIN_CALLBACKS = 0

# messageTimeout - the maximum time in milliseconds until a message times out.
# The timeout period starts at IoTHubClient.send_event_async.
# By default, messages do not expire.
MESSAGE_TIMEOUT = 10000

# global counters
RECEIVE_CALLBACKS = 0
SEND_CALLBACKS = 0

# Choose HTTP, AMQP or MQTT as transport protocol.  Currently only MQTT is supported.
PROTOCOL = IoTHubTransportProvider.MQTT

# String containing Hostname, Device Id & Device Key & Module Id in the format:
# "HostName=<host_name>;DeviceId=<device_id>;SharedAccessKey=<device_key>;ModuleId=<module_id>;GatewayHostName=<gateway>"
CONNECTION_STRING = "[Device Connection String]"

model = None
graph = None


def scale_input(raw_shipdata):

    transform = [[0.0028011204481792713, 1.5818787038937665e-17],

                 [0.000679485384948855, -0.001561457414612541],

                 [0.21331058020477808, 0.5039462457337883],

                 [0.0684861144402972, 0.005513132212444326],

                 [0.05402339212879177, -0.16207017638637536],

                 [0.0166940727694632, 1.5818787038937665e-17],

                 [0.0704225352112676, -0.4225352112676056],

                 [0.7707129094412334, 0.6057803468208092]]
    out = []
    for i in range(0,len(raw_shipdata)):
        out.append(transform[i][0]*raw_shipdata[i]+transform[i][1])
    norm_shipdata = np.array([out])
    return norm_shipdata

# Callback received when the message that we're forwarding is processed.
def send_confirmation_callback(message, result, user_context):
    global SEND_CALLBACKS
    print ( "Confirmation[%d] received for message with result = %s" % (user_context, result) )
    map_properties = message.properties()
    key_value_pair = map_properties.get_internals()
    print ( "    Properties: %s" % key_value_pair )
    SEND_CALLBACKS += 1
    print ( "    Total calls confirmed: %d" % SEND_CALLBACKS )

# receive_message_callback is invoked when an incoming message arrives on the specified 
# input queue (in the case of this sample, "input1").  Because this is a filter module, 
# we will forward this message onto the "output1" queue.
def receive_message_callback(message, hubManager):
    global RECEIVE_CALLBACKS
    global TEMPERATURE_THRESHOLD
    global model
    global graph

    RECEIVE_CALLBACKS += 1
    #print("    Total calls received: {:d}".format(RECEIVE_CALLBACKS))

    # processing message
    print( "Message received: simdata")

    message_buffer = message.get_bytearray()
    size = len(message_buffer)
    message_text = message_buffer[:size].decode('utf-8')
    data = json.loads(message_text)
    #print("    Data: <<<{}>>> & Size={:d}".format(message_text, size))


    print("Model prediction placeholder")
    shipdata_raw = [   data["angle_wind_relative"],
                            data["depth"],
                            data["list"],
                            data["power"],
                            data["sog"],
                            data["relative_windspeed"],
                            data["stw"],
                            data["trim"]]

    shipdata = scale_input(shipdata_raw)
    with graph.as_default():
        ar_pred = model.predict(shipdata, verbose=1)

    print("Custom message from predicted sklearn model {}".format(ar_pred))

#old code from sample, irrelevant, delete aftewards:

    #map_properties = message.properties()
    #key_value_pair = map_properties.get_internals()
    #print("    Properties: {}".format(key_value_pair))
    #if "machine" in data and "temperature" in data["machine"] and data["machine"]["temperature"] > TEMPERATURE_THRESHOLD:
    #   map_properties.add("MessageType", "Alert")
    #   print("Machine temperature {} exceeds threshold {}".format(data["machine"]["temperature"], TEMPERATURE_THRESHOLD))

    hubManager.forward_event_to_output("output1", message, 0)
    return IoTHubMessageDispositionResult.ACCEPTED

# device_twin_callback is invoked when twin's desired properties are updated.
def device_twin_callback(update_state, payload, user_context):
    global TWIN_CALLBACKS
    global TEMPERATURE_THRESHOLD
    print("\nTwin callback called with:\nupdateStatus = {}\npayload = {}\ncontext = {}".format(update_state, payload, user_context))
    data = json.loads(payload)
    if "desired" in data and "TemperatureThreshold" in data["desired"]:
        TEMPERATURE_THRESHOLD = data["desired"]["TemperatureThreshold"]
    if "TemperatureThreshold" in data:
        TEMPERATURE_THRESHOLD = data["TemperatureThreshold"]
    TWIN_CALLBACKS += 1
    print("Total calls confirmed: {:d}\n".format(TWIN_CALLBACKS))

class HubManager(object):

    def __init__( self, connection_string):

        self.client_protocol = PROTOCOL
        self.client = IoTHubClient(connection_string, PROTOCOL)

        # set the time until a message times out
        self.client.set_option("messageTimeout", MESSAGE_TIMEOUT)
        # some embedded platforms need certificate information
        self.set_certificates()
        
        # sets the callback when a message arrives on "input1" queue.  Messages sent to 
        # other inputs or to the default will be silently discarded.
        self.client.set_message_callback("input1", receive_message_callback, self)

        # sets the callback when a twin's desired properties are updated.
        self.client.set_device_twin_callback(device_twin_callback, self)

    def set_certificates(self):
        isWindows = sys.platform.lower() in ['windows', 'win32']
        if not isWindows:
            CERT_FILE = os.environ['EdgeModuleCACertificateFile']        
            print("Adding TrustedCerts from: {0}".format(CERT_FILE))
            
            # this brings in x509 privateKey and certificate
            file = open(CERT_FILE)
            try:
                self.client.set_option("TrustedCerts", file.read())
                print ( "set_option TrustedCerts successful" )
            except IoTHubClientError as iothub_client_error:
                print ( "set_option TrustedCerts failed (%s)" % iothub_client_error )

            file.close()

    # Forwards the message received onto the next stage in the process.
    def forward_event_to_output(self, outputQueueName, event, send_context):
        self.client.send_event_async(
            outputQueueName, event, send_confirmation_callback, send_context)

def main(connection_string):
    global model
    try:
        print ( "CSE hackfest : IoT Hub Client for Python" )

        #model = load_model("AR_pred_full.h5")
        #print("main: loaded model")
        #print(model.summary())
        # regr = joblib.load('regr.pkl')
        # print("loaded linear model sklearn")

        # load 1
        json_file = open('AR_pred_model_architecture.json', 'r')
        loaded_model_json = json_file.read()
        model = model_from_json(loaded_model_json)
        # load weights into new model
        model.load_weights("AR_pred_weights.h5")
        print(model.summary())
        print("Model keras loaded")

        global graph
        graph = tf.get_default_graph()

        hub_manager = HubManager(connection_string)

        print ( "Starting the IoT Hub Python sample using protocol %s..." % hub_manager.client_protocol )
        print ( "The sample is now waiting for messages and will indefinitely.  Press Ctrl-C to exit. ")

        while True:
            time.sleep(1000)

    except IoTHubError as iothub_error:
        print ( "Unexpected error %s from IoTHub" % iothub_error )
        return
    except KeyboardInterrupt:
        print ( "IoTHubClient sample stopped" )

if __name__ == '__main__':

    try:
        CONNECTION_STRING = os.environ['EdgeHubConnectionString']

    except Exception as error:
        print ( error )
        sys.exit(1)

    main(CONNECTION_STRING)