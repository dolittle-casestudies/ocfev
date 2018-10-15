#!/bin/bash
cd ../..
docker build --add-host "kafka-service:137.117.175.54" -t dolittle/ocfev-visualization -f Source/Visualization/Dockerfile .
