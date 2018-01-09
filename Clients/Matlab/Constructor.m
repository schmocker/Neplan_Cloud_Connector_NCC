
try
    delete(client)
end
clear all
clc
client = Neplan_MQTT_Client('tcp://www.tobiasschmocker.ch','Neplan');