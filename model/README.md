# machine learning models

Predict added resistance

1. Extract relevat data from sql database
   
   timestamp
   wind angle
   depth list 
   power
   speed
   speed though water (STW)
   wind speed 
   trim
   
   these data are timeseries, extracted data averaged by 5 min interval
   
2.  Create label, added resistance value using known benchmark

    AR(STW, power) = Power/Benchmark
    
    benchmark:

dataset 1 
data for one year
AR label calculated as described above.

dataset 2

Extract data with fauling close to 0. 3 month data
AR label calculated as described above.
   



