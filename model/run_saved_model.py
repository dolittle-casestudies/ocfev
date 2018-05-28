from keras.models import model_from_json
from keras.models import load_model
import numpy as np

shipdata = np.array([[0.0, 0.85, 0.5, 0.7, 0.65, 0.5, 0.65, 0.0]])
shipdata = np.float32(shipdata)


#load 1
json_file = open('AR_pred_model_architecture.json', 'r')
loaded_model_json = json_file.read()
loaded_model1 = model_from_json(loaded_model_json)
# load weights into new model
loaded_model1.load_weights("AR_pred_weights.h5")
print(loaded_model1.summary())
ar_pred1 = loaded_model1.predict(shipdata, verbose=1)
print(ar_pred1)

#load 2
print("Load full model model2")
model2 = load_model("AR_pred_full.h5")
ar_pred2 = model2.predict(shipdata, verbose=1)
print(ar_pred2)