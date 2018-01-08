clc
clear all

client = MQTT_Client();


fnc = 'getProperty';
pars.number = 5;
pars.name = 'Project1';
client.send(fnc,pars)

