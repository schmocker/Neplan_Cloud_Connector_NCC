classdef MQTT_Client
    %UNTITLED3 Summary of this class goes here
    %   Detailed explanation goes here
    
    properties
        broker_url = 'tcp://www.tobiasschmocker.ch'
        topic = 'Neplan'
        broker
        subscription
    end
    
    methods
        function obj = MQTT_Client()
            obj.broker = mqtt(obj.broker_url);
            obj.subscription = subscribe(obj.broker,...
                [obj.topic, '/fromService'],...
                'QoS', 1,...
                'Callback',@obj.receive);
            
        end
        
        function receive(obj,src,evt)
            %METHOD1 Summary of this method goes here
            %   Detailed explanation goes here
            output = evt
        end
        
        function send(obj,fnc,input)
            msg.fnc = fnc;
            msg.input = input;
            msg = jsonencode(msg);
            publish(obj.broker,[obj.topic, '/toService'],msg);
        end
    end
    
end

