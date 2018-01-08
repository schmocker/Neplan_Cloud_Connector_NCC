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

            Console.WriteLine("- start processing");
            switch (fnc)
            {
                case "openWebservice":
                    string username = pars["username"];
                    string password = pars["password"];
                    string projectName = pars["project"];
                    webservice = new Webservice(username, password, projectName);
                    output["webservice"] = "connected";
                    break;
                case "closeWebservice":
                    webservice.CloseWebservice();
                    output["webservice"] = "disconnected";
                    break;
                // functions from the neplan webservice
                case "AnalyseVariant":
                    output["result"] = AnalyseVariant(pars);
                    break;
                case "GetListResultSummary":
                    output["result"] = GetListResultSummary(pars);
                    break;
                default:
                    output["error"] = "invalid function";
                    Console.WriteLine(output["error"]);
                    break;
            }
            Console.WriteLine("- end processing");



            output["status"] = "done";
            Mqtt_Client.Publish(output);
            Console.WriteLine("- results sent");
            Console.WriteLine("%%%%% Finished %%%%%");
            Console.WriteLine(" ");
        }

        // Funktionen
        public static string AnalyseVariant(Dictionary<string, string> pars)
        {
            ExternalProject project = webservice.ext;
            string analysisRefenceID = Guid.NewGuid().ToString();
            string analysisModule = pars["analysisModule"];
            string calcNameID = pars["calcNameID"];
            string analysisMethode = pars["analysisMethode"];
            string conditions = pars["conditions"];
            string analysisLoadOptionXML = pars["analysisLoadOptionXML"];
            AnalysisReturnInfo output = webservice.nepService.AnalyseVariant(
                project,
                analysisRefenceID,
                analysisModule,
                calcNameID,
                analysisMethode,
                conditions,
                analysisLoadOptionXML);
            return "";
        }
        public static string GetListResultSummary(Dictionary<string, string> pars)
        {
            ExternalProject project = webservice.ext;
            string analysisType = pars["analysisType"];
            DateTime simulationDateTime = new DateTime(); //  string to date - pending
            int networkTypeGroup = Convert.ToInt32(pars["networkTypeGroup"]); //  string to int - done
            string networkTypeGroupID = pars["networkTypeGroupID"];
            string[] output = webservice.nepService.GetListResultSummary(
                project, 
                analysisType, 
                simulationDateTime, 
                networkTypeGroup, 
                networkTypeGroupID);
            return output[0];
        }



    }



}
