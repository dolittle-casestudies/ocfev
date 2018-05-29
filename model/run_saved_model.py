from keras.models import model_from_json
from keras.models import load_model
import numpy as np
from sklearn.externals import joblib
import pickle
import tensorflow as tf
from sklearn.preprocessing import MinMaxScaler

shipdata = np.array([[0.9, 0.13, 0.00019, 0.11, 0.17, 0.16, 0.16, -0.1000946]])
#shipdata = np.array([[0.0, 0.85, 0.5, 0.7, 0.65, 0.5, 0.65, 0.0]])
#shipdata = np.float32(shipdata)

shipdata_raw = [
  344.3096358255994,
  8614.9631792749115,
  39.216566562753428,
  31.922113053464383,
  20.916180985940706,
  49.62574034446186,
  7.6303721143074217,
  4.8518696473221627 ]


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

shipdata_norm  = scale_input(shipdata_raw)


#load regr
#
# regr = joblib.load('regr.pkl')
# ar_pred_r1 = regr.predict(shipdata)
# print(ar_pred_r1)

#load 1
json_file = open('AR_pred_model_architecture.json', 'r')
loaded_model_json = json_file.read()
loaded_model1 = model_from_json(loaded_model_json)
# load weights into new model
loaded_model1.load_weights("AR_pred_weights.h5")
print(loaded_model1.summary())

print("Predict value:")

#graph = tf.get_default_graph()
#with graph.as_default():
#    ar_pred1 = loaded_model1.predict(shipdata, verbose=1)
ar_pred1 = loaded_model1.predict(shipdata_norm, verbose=1)
print(ar_pred1)

#load 2
print("Load full model model2")
model2 = load_model("AR_pred_full.h5")

graph = tf.get_default_graph()
with graph.as_default():
    ar_pred2 = model2.predict(shipdata_norm, verbose=1)
#ar_pred2 = model2.predict(shipdata, verbose=1)
print(ar_pred2)