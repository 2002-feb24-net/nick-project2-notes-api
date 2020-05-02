#!/bin/sh

# creating AKS cluster and preparing for a CI pipeline deploying to it.
# (don't actually run this script as-is.)

# cluster owner commands to set up the cluster:

    az group create -l southcentralus -n 2002-training-aks

    # necessary because of longstanding race condition problem with SPs created during the aks create command
    az ad sp create-for-rbac -n 2002-training-aks-sp-1 --skip-assignment

    # fill in the SP and secret based on previous command's output
    # Standard_B2s is the cheapest VM supporting AKS
    # takes 5-10 min
    az aks create -g 2002-training-aks -n aks1 -c 1 -s Standard_B2s --service-principal 00000000-0000-0000-0000-000000000000 --client-secret 00000000-0000-0000-0000-000000000000

    # give local kubectl the credentials to this cluster, create a context for it, and make it the current context
    az aks get-credentials -g 2002-training-aks -n aks1

    # allow people outside the azure subscription to connect to the cluster with admin privileges using the default service account
    kubectl create clusterrolebinding serviceaccounts-cluster-admin --clusterrole=cluster-admin --group=system:serviceaccounts

    # get the server URL, pass on to those who need to access the cluster.
    kubectl config view -o jsonpath="{.clusters[?(@.name=='aks1')].cluster.server}"

    # get the secret token, pass on to those who need to access the cluster.
    kubectl get secret --field-selector type=kubernetes.io/service-account-token -o jsonpath="{.items[0].data.token}"

# this is a quick & dirty way to authenticate and not the most secure ever.

# cluster user commands to connect kubectl:

    kubectl config set-cluster aks1 --insecure-skip-tls-verify --server https://server.url.given.by.cluster.owner

    kubectl config set-credentials aks1 --token SecretTokenGivenByClusterOwner

    kubectl config set-context aks1 --cluster aks1 --user aks1

    kubectl config use-context aks1

# there's some dependencies among the services based on not having DNS...
# backend CORS configuration needs the frontend service external IP. i have set up CORS origins to be read from k8s-provided environment vars, but even so, the frontend service has to exist and then get an IP hardcoded into the backend's manifest file.
# frontend TS code needs the backend service external IP (so the browser can connect). we can't even build the frontend image until the backend service exists.
# so without domain name, creating the loadbalancer services the first time is in the "initial setup" category, and then the repos have to be updated with those IPs before the pipeline will result in a working app.

# accordingly, cluster user setup before the pipeline:

    kubectl apply -f k8s/service
    # the same needs to be run for the frontend.

    kubectl get services
    # use the external IPs given there to update the CORS config in the backend (k8s/deployment/notes-api) and the environment.prod.ts in the frontend.

# to set up the pipeline, create a kubernetes namespace resource in the environment
# referenced in the deployment job.

    # choose "Generic provider" type
    # fill in the server url given by the cluster owner
    # default namespace

    # follow the directions to get the secret value (it needs more data than just the token).
    # the service account name is default and the namespace is default.

    # accept untrusted certificates

# from then on, pipeline needs to:
# 1. kubectl apply -f k8s -R
# (apply all the manifests recursively in that folder)
# 2. kubectl rollout restart -f k8s/deployment
# (restart the deployments to pick up new docker images)
# (except there are azure pipelines tasks to wrap those commands and connect to the k8s resource, as seen in the pipeline in this repo)

# the second step is necessary so long as new images are being pushed with the same tags,
# instead of different tags with the manifest being transformed at build-time to use the appropriately image tag.
# so long as the manifest is the same byte-for-byte, kubectl apply won't result in any pods restarting with the latest image.
# (that alternative would be better practice, but is more work to setup with the pipeline)
