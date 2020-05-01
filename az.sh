#!/bin/sh

# creating AKS cluster and preparing for a CI pipeline deploying to it.
# (don't actually run this script as-is.)

az group create -l southcentralus -n 2002-training-aks

# necessary because of longstanding race condition problem with SPs created during the aks create command
az ad sp create-for-rbac -n 2002-training-aks-sp-1 --skip-assignment

# fill in the SP and secret based on previous command's output
# Standard_B2s is the cheapest VM supporting AKS
# takes 5-10 min
az aks create -g 2002-training-aks -n aks1 -c 1 -s Standard_B2s --service-principal 00000000-0000-0000-0000-000000000000 --client-secret 00000000-0000-0000-0000-000000000000

# give local kubectl the credentials to this cluster, create a context for it, and make it the current context
az aks get-credentials -g 2002-training-aks -n aks1

# there's some dependencies among the services based on not having DNS...
# backend CORS configuration needs the frontend service external IP. i have set up CORS origins to be read from k8s-provided environment vars, but even so, the frontend service has to exist and then get an IP hardcoded into the backend's manifest file.
# frontend TS code needs the backend service external IP (so the browser can connect). we can't even build the frontend image until the backend service exists.
# so without domain name, creating the loadbalancer services the first time is in the "initial setup" category, and then the repos have to be updated with those IPs before the pipeline will result in a working app.
kubectl apply -f k8s/service
# the same needs to be run for the frontend.

kubectl get services
# use the external IPs given there to update the CORS config in the backend (k8s/deployment/notes-api) and the environment.prod.ts in the frontend.

# ----- PIPELINE -----

# if and only if anyone outside the azure subscription needs to connect to the cluster, then some steps are needed:

    # give cluster-admin role to all service accounts
    kubectl create clusterrolebinding serviceaccounts-cluster-admin --clusterrole=cluster-admin --group=system:serviceaccounts

    # get the server name (to enter in azure pipelines)
    kubectl config view --minify -o 'jsonpath={.clusters[0].cluster.server}'

    # get the secret name
    kubectl get secrets

    # fill in secret name based on previous command
    # copy the secret's data to the clipboard (to enter in azure pipelines)
    kubectl get secret default-token-00000 -n default -o json | clip

    # azure pipelines needs those things to create a (Generic provider) Kubernetes namespace resource in an environment.
    # fill in default for the namespace, and accept untrusted certificates (dunno why it isn't trusted, it's MS to MS after all...)

# from then on, pipeline needs to:
# 1. kubectl apply -f k8s -R
# 2. kubectl rollout restart -f k8s/deployment
# (except there are azure pipelines tasks to wrap those commands and connect to the k8s resource i guess)

# the second step is necessary so long as new images are being pushed with the same tags,
# instead of different tags with the manifest being transformed at build-time to use the appropriately image tag.
# so long as the manifest is the same byte-for-byte, kubectl apply won't result in any pods restarting with the latest image.
# (that alternative would be better practice, but is more work to setup with the pipeline)
