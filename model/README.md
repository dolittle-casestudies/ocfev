# Machine learning Models

Predict added resistance 

To predict added resistance a period when the was clean hull was selected. 
For a initial test the variables below was used to estimate the added resistance. 
The added resistance was compared to a vessel Benchmark described in (2). Furthermore,
some additional filtering was introduced prior to training due to spurious power output. 
   
   wind angle
   depth list 
   power
   speed
   speed though water (STW)
   wind speed 
   trim
   
   these data are timeseries, extracted data averaged by 1 min interval
   
resistance label is a added resistance value using known benchmark

    AR(STW, power) = Power/Benchmark

Model: 

The problem described translates into regression model that predicts value of added resistance. 
We have used fully connected neural network. The model is implemented with 
sklearn and keras frameworks.

Model evaluation on our train dataset: r2 score: 0.96976


   



