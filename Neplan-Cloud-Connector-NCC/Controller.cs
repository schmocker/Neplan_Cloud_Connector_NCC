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

        private string[] localMethods = { "StartNeplanClient", "StopNeplanClient" };

        public Controller()
        {
            mqttClient = new Mqtt_Client(mqttUrl, mqttTopic);
            mqttClient.setController(this);

            neplanClient = new Neplan_Client();
            neplanClient.setController(this);

            Console.WriteLine("%%%%% Start: Application %%%%%\n");
            Console.WriteLine("MQTT-Client details:");
            Console.WriteLine("    broker:   " + mqttUrl + "(set in C#)");
            Console.WriteLine("    topic:    " + mqttTopic + "(set in C#)\n");

            Console.WriteLine("Neplan-Client details :");
            Console.WriteLine("    Server:   https://demo.neplan.ch/NEPLAN360_Demo/Services/External/NeplanService.svc (set in C#)");
            Console.WriteLine("    username: not set (pending via mqtt)");
            Console.WriteLine("    password: not set (pending via mqtt)");
            Console.WriteLine("    project:  not set (pending via mqtt)\n");
            Console.WriteLine("%%%%% Ready to receive commands %%%%%\n");
        }

        public void treatCommand(Command command)
        {
            Console.WriteLine("%%%%% START: " + command.MethodName + " %%%%%\n");
            if (neplanClient.hasMethod(command.MethodName))
            {
                neplanClient.treatCommand(command);
                showMethodInfo(command.MethodName);
            }
            else if (localMethods.Contains(command.MethodName))
            {
                switch (command.MethodName)
                {
                    case "StartNeplanClient":
                        string username = (string)command.Inputs["username"];
                        string password = (string)command.Inputs["password"];
                        string projectName = (string)command.Inputs["project"];
                        StartNeplaneClient(username, password, projectName);
                        break;
                    case "StopNeplanClient":
                        StopNeplanClient();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                command.Error = true;
                Console.WriteLine("!!!!! no valid function found !!!!!\n");
            }


            // publish results
            command.Done = true;
            mqttClient.Publish(command);
            Console.WriteLine("%%%%% END: " + command.MethodName + " %%%%%\n\n");
        }

        // local methods that can be used via mqtt
        public void StartNeplaneClient(string username, string password, string project)
        {
            neplanClient.StartNeplanServiceClient(username, password, project);
        }
        public void StopNeplanClient()
        {
            neplanClient.StopNeplanServiceClient();
            // Environment.Exit(0);
        }

        // console outputs
        private void showMethodInfo(string methodName)
        {
            MethodInfo thisMethod = typeof(NeplanService.NeplanService).GetMethod(methodName);
            ParameterInfo[] allPars = thisMethod.GetParameters();
            Console.WriteLine("--> Funktion:");
            Console.WriteLine(methodName + "\n");
            Console.WriteLine("--> Benötigte Parameter:");
            string format = "|{0,-30}|{1,-50}|";
            string fs = String.Format(format, "ParameterName", "ParameterType");
            Console.WriteLine(new String('-', fs.Length));
            Console.WriteLine(fs);
            Console.WriteLine(new String('-', fs.Length));
            foreach (var thisPar in allPars)
            {
                Console.WriteLine(String.Format(format, thisPar.Name, thisPar.ParameterType));
            }
            Console.WriteLine(new String('-', fs.Length));
            Console.WriteLine("\n");
        }
    }
}
