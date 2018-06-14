#!/bin/bash
cd ../..
docker build --add-host "kafka-service:104.45.19.232" -t dolittle/mqtt-iot-edge -f Modules/MQTT/Dockerfile .
