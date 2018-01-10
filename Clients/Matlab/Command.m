classdef Command < handle
    %UNTITLED Summary of this class goes here
    %   Detailed explanation goes here
    
    properties
        State(1,:) char {mustBeMember(State,{'preparing','sending','pending','completing','done','error'})} = 'preparing'
        Error(1,:) char
        Fnc(1,:) char
        Inputs struct
        Outputs struct
    end
    %% Methods
    methods
        %% Constructor
        function obj = Command(fnc,inputs)
            %UNTITLED Construct an instance of this class
            %   Detailed explanation goes here
            obj.Fnc = fnc;
            obj.Inputs = inputs;
        end
        function msg_json = encode(obj)
            msg.fnc = obj.Fnc;
            msg.input = jsonencode(obj.Inputs);
            msg_json = jsonencode(msg);
        end
        function decode(obj,msg_json)
            outputs = jsondecode(msg_json{1});
            if outputs.Received && not(outputs.Done)
                obj.State = 'pending';
            elseif outputs.Done
                obj.State = 'completing';
                obj.Outputs = outputs;
            else
                disp('Error: No valid State');
                obj.State = 'error';
            end
        end
    end
end