#!/bin/bash
docker push dolittle/ocfev-visualization
kubectl patch deployment visualization --namespace ocfev -p "{\"spec\":{\"template\":{\"metadata\":{\"labels\":{\"date\":\"`date +'%s'`\"}}}}}"
sleep 30
kubectl delete -f K8s/ingress.yml
kubectl apply -f K8s/ingress.yml