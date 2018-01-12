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
        private string[] neplanMethods;

        public Controller()
        {
            mqttClient = new Mqtt_Client(mqttUrl, mqttTopic);
            mqttClient.setController(this);

            neplanClient = new Neplan_Client();
            neplanClient.setController(this);

            neplanMethods = neplanClient.getMethodNames();

            ConsoleOut.ShowStart(mqttUrl, mqttTopic, neplanServiceUrl);
        }

        public void TreatCommand(Command command)
        {
            Console.WriteLine("%%%%% Start: " + command.MethodName + " %%%%%\n\n");

            // set object handler
            // the object handler is the object, that handlse the method bzw. the command.
            if (localMethods.Contains(command.MethodName))
                command.ObjectHandler = this;
            else if (neplanMethods.Contains(command.MethodName))
            {
                command.ObjectHandler = neplanClient.GetObjectHandler();
                // spezielle Parameter setzen
                command.Ins.Add("project", neplanClient.project);
                command.Ins.Add("projectName", neplanClient.project);
                command.Ins.Add("analysisRefenceID", Guid.NewGuid().ToString());
            }
            else
            {
                command.Error = true;
                Console.WriteLine("!!!!! no valid function found !!!!!\n");
            }
            Console.WriteLine("Objekt gefunden");


            



            // Methode verbinden
            command.ReferMethod();
            Console.WriteLine("Methode gefunden");
            ConsoleOut.ShowMethodInfo(command.Method);

            // Parameter anpassen
            command.PrepareParameters();
            Console.WriteLine("Parameter angepasst");

            // Methode ausführen
            command.Invoke();
            Console.WriteLine("Funktion ausgeführt");



            // publish results
            command.Done = true;
            mqttClient.Publish(command);
            Console.WriteLine("%%%%% END: " + command.MethodName + " %%%%%\n\n");
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
