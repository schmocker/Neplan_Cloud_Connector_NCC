using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace NeplanMqttService
{
    class Program
    {
        public static Mqtt_Client client;
        public static Webservice webservice;

        static void Main(string[] args)
        {
            Console.WriteLine("%%%%% Start %%%%%");

            client = new Mqtt_Client();
            Console.WriteLine("- MQTT connectioon details:");
            Console.WriteLine("broker: " + Mqtt_Client.url + "(preset in C#)");
            Console.WriteLine("topic:  " + Mqtt_Client.topic + "(preset in C#)");
            Console.WriteLine(" ");

            Console.WriteLine("- Neplan Server:");
            Console.WriteLine("Server:   https://demo.neplan.ch/NEPLAN360_Demo/Services/External/NeplanService.svc (preset in C#)");
            Console.WriteLine("username: not set (pending via mqtt)");
            Console.WriteLine("password: not set (pending via mqtt)");
            Console.WriteLine("project:  not set (pending via mqtt)");
            Console.WriteLine(" ");

        }

        public static void HandleCommand (string id, string fnc, Dictionary<string, string> pars)
        {
            Console.WriteLine("%%%%% new command received %%%%%");
            Console.WriteLine("- function: " + fnc);

            Dictionary<string, string> output = new Dictionary<string, string>();
            output["id"] = id;
            output["status"] = "received";
            Mqtt_Client.Publish(output);

            switch (fnc)
            {
                case "openWebservice":
                    string username = pars["username"];
                    string password = pars["password"];
                    string project = pars["project"];
                    webservice = new Webservice(username, password, project);
                    output["webservice"] = "connected";
                    break;
                case "closeWebservice":
                    webservice.CloseWebservice();
                    output["webservice"] = "disconnected";
                    break;
                default:
                    output["error"] = "invalid function";
                    Console.WriteLine(output["error"]);
                    break;
            }




            Console.WriteLine(" ");

            output["status"] = "done";
            Mqtt_Client.Publish(output);
        }



    }



}
