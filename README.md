# Neplan-Cloud-Connector-NCC
This is a program to connect to the Neplan Cloud
All About the Neplan Cloud: http://www.neplan.ch/neplanproduct/de-neplan-360-cloud

The program allows any other program to send neplan commands and inputs via mqtt and to get the results also via mqtt. Like this, every program in any programming language can communicate to the Neplan Cloud.

So far I made a client class for Matlab: https://github.com/schmocker/NCC-Matlab-Client
Similar to the Matlab class one could write a class for Python, Java or any other programming language.

## Requirements
* Mqtt-Broker
* Neplan Cloud Login

There are many free and ready to use Mqtt-Brokers available. For example: iot.eclipse.org with port 1883



## Required NuGet packages:
* M2MqttDotnetCore by Hamidreza Mohaqeq (>1.0.7)
* Newtonsoft.Json by James Newton-King (>10.0.3)
