# PeriodicTimer
Periodic Timer Microservice

## Overview
This service sends a simple MQTT message on a periodic basis to the specific MQTT topic.

## Deployment Instructions
* Deploy the Config Map (./Deploy/PeriodicTimerConfigMap.yaml)
* Edit the Config Map to specify MQTT broker, credentials and time period
* Deploy the Deployment (./Deploy/PeriodicTimerDeployment.yaml)
