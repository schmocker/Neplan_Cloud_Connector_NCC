using NeplanMqttService.NeplanService;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Threading;
using System.Xml.Linq;
using System.Reflection;

namespace NeplanMqttService
{
    class Program
    {
        public static Mqtt_Client client;
        public static NeplanServiceClient neplanServiceClient = new NeplanServiceClient();
        public static ExternalProject project;
        private string[] otherValidMethods = { "StartNeplanServiceClient", "StopNeplanServiceClient" };

        static void Main(string[] args)
        {


            //throw new Exception("Test Exception");

            // Start
            Console.WriteLine("%%%%% Start: Application %%%%%\n");

            client = new Mqtt_Client();
            Console.WriteLine("MQTT coneection details:");
            Console.WriteLine("    broker:   " + Mqtt_Client.url + "(set in C#)");
            Console.WriteLine("    topic:    " + Mqtt_Client.topic + "(set in C#)\n");

            Console.WriteLine("Neplan Server details :");
            Console.WriteLine("    Server:   https://demo.neplan.ch/NEPLAN360_Demo/Services/External/NeplanService.svc (set in C#)");
            Console.WriteLine("    username: not set (pending via mqtt)");
            Console.WriteLine("    password: not set (pending via mqtt)");
            Console.WriteLine("    project:  not set (pending via mqtt)\n");
            Console.WriteLine("%%%%% Finished start procedure %%%%%\n");

        }

        public static void HandleCommand (string fnc, Dictionary<string, object> pars)
        {
            Console.WriteLine("%%%%% START: " + fnc + " %%%%%");

            Dictionary<string, object> output = new Dictionary<string, object>();
            output["state"] = "received";
            Mqtt_Client.Publish(output);
            

            Console.WriteLine("- start processing\n");

            Command command = new Command(neplanServiceClient, project, fnc, pars);
            
            Console.WriteLine("- end processing");



            output["state"] = "done";
            Mqtt_Client.Publish(output);
            Console.WriteLine("- results sent");
            Console.WriteLine("%%%%% Finished %%%%%");
            Console.WriteLine(" ");
        }
    }
}
