#!/bin/bash
cd ../..
docker build -t dolittle/ocfev-visualization -f Source/Visualization/Dockerfile .
