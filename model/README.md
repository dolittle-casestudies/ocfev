# Machine learning Models

## Mode 1 : Predict added resistance for full scale vessel

To predict added resistance a period when the was clean hull was selected. 
For a initial test the variables below was used to estimate the added resistance. 
The added resistance was compared to a vessel Benchmark described in (2). Furthermore,
some additional filtering was introduced prior to training due to spurious power output. 
   
   ```
   wind angle
   depth list 
   power
   speed
   speed though water (STW)
   wind speed 
   trim
   ```
   
   these data are timeseries, extracted data averaged by 1 min interval
   
resistance label is a added resistance value using known benchmark

    ```
    AR(STW, power) = Power/Benchmark
    ```

The problem described translates into regression model that predicts value of added resistance. 
We have used fully connected neural network. The model is implemented with 
sklearn and keras frameworks.

Fro experimentation purposes a model trained with full scale vessel data from 
proprietary Wilhemsen database.

Model evaluation on our train dataset: r2 score: 0.96976


## Model 2: Dolittle boat

## Data collection for Dolittle boat
Firstly, the ship model will be tested for different speeds in the model basins. This will provide
a power - speed relation ship for the ship model without enviromental disturbance. Secondly, 
the same test would be carried out with different trim to measure the added power. Lastly, a new ML 
model can be created since the added power, e.g. label, is obtained. 





   



