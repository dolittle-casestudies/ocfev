#!/bin/bash
docker push dolittle/ocfev-visualization
kubectl patch deployment visualization --namespace ocfev -p "{\"spec\":{\"template\":{\"metadata\":{\"labels\":{\"date\":\"`date +'%s'`\"}}}}}"
