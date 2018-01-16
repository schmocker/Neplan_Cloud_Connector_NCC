using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neplan_Cloud_Connector_NCC
{
    class Controller
    {
        private Mqtt_Client mqttClient;
        private Neplan_Client neplanClient;

        private string mqttUrl = "www.tobiasschmocker.ch";
        private string mqttTopic = "Neplan";
        private string neplanServiceUrl = "https://demo.neplan.ch/NEPLAN360_Demo/Services/External/NeplanService.svc";

        private string[] localMethods = { "StartNeplanClient", "StopNeplanClient" };
        CommandDictionary allCommands = new CommandDictionary();

        public Controller()
        {
            mqttClient = new Mqtt_Client(mqttUrl, mqttTopic);
            mqttClient.setController(this);

            neplanClient = new Neplan_Client();
            neplanClient.setController(this);

            ConsoleOut.ShowStart(mqttUrl, mqttTopic, neplanServiceUrl);
            GatherAllMethods();
        }

        public void GatherAllMethods()
        {
            // get all local mathods
            foreach (string methodName in localMethods)
                allCommands.AddNew(methodName, this);
            // get all methods from Neplan
            foreach (var methodName in neplanClient.getMethodNames())
                allCommands.AddNew(methodName, neplanClient.GetObjectHandler());
        }



        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="input"></param>
        public void TreatCommand(string methodName, Dictionary<string,object> input)
        {
            Console.WriteLine("%%%%% Start: " + methodName + " %%%%%\n\n");

            // set object handler
            // the object handler is the object, that handlse the method bzw. the command.
            Command cmd;
            if (allCommands.ContainsKey(methodName))
            {
                cmd = allCommands[methodName].Clone();
            }
            else
            {
                cmd = new Command(methodName);
                cmd.setError("Method could not be found");
            }


            if (cmd.ObjectHandler.GetType() == neplanClient.GetObjectHandler().GetType())
            {
                cmd.ObjectHandler = neplanClient.GetObjectHandler();
                input["project"] = neplanClient.project;
                input["projectName"] = neplanClient.project;
                input["analysisRefenceID"] = Guid.NewGuid().ToString();
            }

            if (!cmd.Error)
            {
                // Anjust the recieved Inputs to the desired parameters
                cmd.SetParameters(input);
                ConsoleOut.ShowMethodInfo(cmd);
            }


            if (!cmd.Error)
            {
                // Invoke method
                cmd.Invoke();
            }


            Console.WriteLine("Funktion ausgeführt");
            


            // publish results
            cmd.Done = true;


            Command c = new Command(cmd.FunctionName);
            c.Input = cmd.Input;
            c.Output = cmd.Output;
            c.Error = cmd.Error;
            c.Received = cmd.Received;
            c.Done = cmd.Done;
            c.ErrorMsg = cmd.ErrorMsg;
            c.ExceptionMsg = cmd.ExceptionMsg;
            
            string msg_json = JsonConvert.SerializeObject(c);


            mqttClient.Publish(msg_json);
            Console.WriteLine("%%%%% END: " + cmd.FunctionName + " %%%%%\n\n");
        }










        // local methods that can be used via mqtt
        public void StartNeplanClient(string username, string password, string project)
        {
            neplanClient.StartNeplanServiceClient(username, password, project);
        }
        public void StopNeplanClient()
        {
            neplanClient.StopNeplanServiceClient();
            // Environment.Exit(0);
        }
    }

    
    
}
