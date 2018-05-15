#predict added resistance with deep learning

import pandas as pd

import matplotlib
matplotlib.use('TkAgg')
import matplotlib.pyplot as plt
import numpy as np

from sklearn.model_selection import train_test_split
from keras.models import Sequential

df = pd.read_csv("dataset1.csv")

#preprocessing
df = df.dropna( axis=0, how ="any")

df_X = df.drop(["time","label"], axis=1)
df_Y = df["label"]
#print(df.info())

#split for training
X_train, X_test, Y_train, Y_test = train_test_split( df_X, df_Y, test_size=0.20, random_state=42)

# train model
model = Sequential()

from keras.layers.core import Dense, Activation, Dropout
from keras.optimizers import Adagrad

model.add(Dense(32, input_dim=8))

# now the model will take as input arrays of shape (*, 9)
# and output arrays of shape (*, 32)

model.add(Dense(32))
# created middle layer

model.add(Dense(1))
# created output layer we have output values as intergers then dimension is 1

model.add(Dense(32))
#created middle layer

model.add(Dense(1))
#created output layer we have output values as intergers then dimension is 1

model.compile(loss='mse', optimizer='adam')

model.fit(X_train, Y_train)

#predict results
Y_pred = model.predict(X_test)

# # Plot outputs
plt.scatter(np.arange(X_test.__len__()), Y_test, color='black')
plt.plot(np.arange(X_test.__len__()), Y_pred, color='blue', linewidth=2)

plt.xticks(())
plt.yticks(())

plt.show()