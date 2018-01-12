using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neplan_Cloud_Connector_NCC
{
    class ConsoleOut
    {
        public static void ShowStart(string mqttUrl, string mqttTopic, string neplanServiceUrl)
        {
            Console.WriteLine("%%%%% Start: Application %%%%%\n");
            Console.WriteLine("MQTT-Client details:");
            Console.WriteLine("    broker:   " + mqttUrl + "(set in C#)");
            Console.WriteLine("    topic:    " + mqttTopic + "(set in C#)\n");

            Console.WriteLine("Neplan-Client details :");
            Console.WriteLine("    Server:   " + neplanServiceUrl + " (set in C#)");
            Console.WriteLine("    username: not set (pending via mqtt)");
            Console.WriteLine("    password: not set (pending via mqtt)");
            Console.WriteLine("    project:  not set (pending via mqtt)\n");
            Console.WriteLine("%%%%% Ready to receive commands %%%%%\n");
        }

        public static void ShowMethodInfo(MethodInfo thisMethod)
        {
            ParameterInfo[] allPars = thisMethod.GetParameters();
            Console.WriteLine("--> Funktion:");
            Console.WriteLine(thisMethod.Name + "\n");
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
