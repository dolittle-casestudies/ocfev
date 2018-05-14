# placeholder added reisitance prediciton model code

import pandas as pd

import matplotlib
matplotlib.use('TkAgg')
import matplotlib.pyplot as plt
import numpy as np

from sklearn import linear_model
from sklearn.model_selection import train_test_split

from sklearn.metrics import mean_squared_error, r2_score

df = pd.read_csv("dataset1.csv")

#preprocessing
df = df.dropna( axis=0, how ="any")

#print(df.info())

df_X = df.drop(["time","label"], axis=1)
df_Y = df["label"]

#split for training
X_train, X_test, Y_train, Y_test = train_test_split( df_X, df_Y, test_size=0.20, random_state=42)

# train model
regr = linear_model.LinearRegression()
regr.fit(X_train, Y_train)

#predict results
Y_pred = regr.predict(X_test)

# # Plot outputs
plt.scatter(np.arange(X_test.__len__()), Y_test, color='black')
plt.plot(np.arange(X_test.__len__()), Y_pred, color='blue', linewidth=2)

plt.xticks(())
plt.yticks(())

plt.show()