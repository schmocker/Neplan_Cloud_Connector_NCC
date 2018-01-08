classdef MQTT_Client < handle
    %UNTITLED3 Summary of this class goes here
    %   Detailed explanation goes here
    
    properties
        broker_url = 'tcp://www.tobiasschmocker.ch'
        topic = 'Neplan'
        
        broker
        subscription
        
        fnc
        input
        msg
    end
    
    methods
        function obj = MQTT_Client()
            try
                obj.broker = mqtt(obj.broker_url);
            catch
                error('qweqw');
            end
            obj.subscription = subscribe(obj.broker,...
                [obj.topic, '/fromService'],...
                'QoS', 1,...
                'Callback',@obj.receive);
            obj.msg.id = 0;
            
        end
        
        function obj = receive(obj,src,evt)
            %METHOD1 Summary of this method goes here
            %   Detailed explanation goes here
            msg = jsondecode(evt{1});
            if strcmp(msg.status,'received')
                disp('server received query');
            else
                disp('server sent results');
                fprintf('\n');
            end
        end
        
        function obj = send(obj)
            obj.msg.id = obj.msg.id + 1;
            obj.msg.fnc = obj.fnc;
            obj.msg.input = jsonencode(obj.input);
            msg_json = jsonencode(obj.msg);
            publish(obj.broker,[obj.topic, '/toService'],msg_json);
            obj.fnc = "";
            obj.input = struct;
            disp('--- new query sent');
        end
    end
    
end

