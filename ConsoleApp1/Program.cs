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

namespace NeplanMqttService
{
    class Program
    {
        public static Mqtt_Client client;
        public static Webservice webservice;

        static void Main(string[] args)
        {
            Console.WriteLine("%%%%% Start: Application %%%%%\n");

            client = new Mqtt_Client();
            Console.WriteLine("MQTT coneection details:");
            Console.WriteLine("    broker:   " + Mqtt_Client.url + "(preset in C#)");
            Console.WriteLine("    topic:    " + Mqtt_Client.topic + "(preset in C#)\n");

            Console.WriteLine("Neplan Server details :");
            Console.WriteLine("    Server:   https://demo.neplan.ch/NEPLAN360_Demo/Services/External/NeplanService.svc (preset in C#)");
            Console.WriteLine("    username: not set (pending via mqtt)");
            Console.WriteLine("    password: not set (pending via mqtt)");
            Console.WriteLine("    project:  not set (pending via mqtt)\n");
            Console.WriteLine("%%%%% Finished start procedure %%%%%\n");

        }

        public static void HandleCommand (string id, string fnc, Dictionary<string, object> pars)
        {
            Console.WriteLine("%%%%% START: " + fnc + " %%%%%");

            Dictionary<string, object> output = new Dictionary<string, object>();
            output["id"] = id;
            output["status"] = "received";
            Mqtt_Client.Publish(output);

            Console.WriteLine("- start processing");
            switch (fnc)
            {
                case "openWebservice":
                    webservice = new Webservice(pars["username"].ToString(), pars["password"].ToString(), pars["project"].ToString());
                    output["webservice"] = "connected";
                    break;
                case "closeWebservice":
                    webservice.CloseWebservice();
                    output["webservice"] = "disconnected";
                    break;
                // functions from the neplan webservice
                default:
                    Command command = new Command(webservice, fnc, pars);
                    output["result"] = command.result;
                    output["error"] = command.error;
                    break;
            }
            Console.WriteLine("- end processing");



            output["status"] = "done";
            Mqtt_Client.Publish(output);
            Console.WriteLine("- results sent");
            Console.WriteLine("%%%%% Finished %%%%%");
            Console.WriteLine(" ");
        }
    }
}
