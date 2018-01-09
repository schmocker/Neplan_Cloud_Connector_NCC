classdef Neplan_MQTT_Client < handle
    %UNTITLED3 Summary of this class goes here
    %   Detailed explanation goes here
    properties
        Broker
        Subscription
        Topic(1,:) char
        Cmd Command
        State(1,:) char {mustBeMember(State,{'ready','busy'})} = 'ready'
        TimeDelayForSending(1,1) {mustBeInteger, mustBeNonnegative} = 3
        TimerForSending timer
        TimeDelayForPending(1,1) {mustBeInteger, mustBeNonnegative} = 5
        TimerForPending timer
    end
    
    methods
        function obj = Neplan_MQTT_Client(url,topic)
            obj.Topic = topic;
            obj.Broker = mqtt(url);
            obj.Subscription = subscribe(obj.Broker,...
                [topic, '/fromService'],...
                'QoS', 1,...
                'Callback',@obj.receive);
            % set the timers
            obj.TimerForSending = timer('TimerFcn', @(src,evt)stopSending(obj,src,evt),...
                'StartDelay',obj.TimeDelayForSending);
            obj.TimerForPending = timer('TimerFcn', @(src,evt)stopPending(obj,src,evt),...
                'StartDelay',obj.TimeDelayForPending);
        end
        function delete(obj)
            obj.Broker.unsubscribeAll();
        end
        %% execute command
        function cmd = do(obj,fnc,input)
            % -> prepare
            obj.State = 'busy';
            obj.Cmd = Command(fnc,input);
            cmd = obj.Cmd; % get object handle
            cmd.State = 'sending';
            % -> send
            waitfor(cmd,'State','sending');
            start(obj.TimerForSending)
            publish(obj.Broker,[obj.Topic, '/toService'],cmd.encode());
            
            % -> arrive
            waitfor(cmd,'State','pending');
            stop(obj.TimerForSending)
            start(obj.TimerForPending)
            
            
            % -> receive
            waitfor(cmd,'State','completing');
            stop(obj.TimerForPending)
            
            cmd.State = 'done';
            obj.State = 'ready';
        end
        
        function receive(obj,topic,data)
            %METHOD1 Summary of this method goes here
            %   Detailed explanation goes here
            obj.Cmd.decode(data)
            if strcmp(obj.Cmd.State,'pending')
                disp('')
            elseif strcmp(obj.Cmd.State,'completing')
            end
        end
        
        function stopSending(obj,src,evt)
            stop(obj.TimerForSending)
            if strcmp(obj.Cmd.State,'sending')
                obj.Cmd.State = 'pending';
                error('Sending took to long');
            end
        end
        function stopPending(obj,src,evt)
            stop(obj.TimerForPending)
            if strcmp(obj.Cmd.State,'pending')
                obj.Cmd.State = 'completing';
                error('Calculating took to long');
            end
        end
        
    end
end