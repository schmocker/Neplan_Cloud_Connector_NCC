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
            Console.WriteLine("%%%%% Start: Application (V 1.0) %%%%%\n");
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

        public static void ShowMethodInfo(Command cmd)
        {
            Console.WriteLine("--> Funktion:");
            Console.WriteLine(cmd.MethodName + "\n");
            Console.WriteLine("--> Benötigte Parameter:");
            string format = "|{0,-20}|{1,-50}|{2,-10}|{3,-10}|{4,-30}|";
            string fs = String.Format(format, "name", "type", "isRequired", "isSetByInput", "value");
            Console.WriteLine(new String('-', fs.Length));
            Console.WriteLine(fs);
            Console.WriteLine(new String('-', fs.Length));



            object[,] obj = new object[cmd.Input.Count, 4];
            object[][] siz;
            int i = 0;
            foreach (Parameter thisPar in cmd.Input.Values)
            {
                obj[i, 0] = thisPar.Name;
                obj[i, 1] = thisPar.Type;
                obj[i, 2] = thisPar.Reuired;
                obj[i, 3] = thisPar.SetByInput;
                obj[i, 4] = thisPar.Value;
                i++;
                Console.WriteLine(String.Format(format, thisPar.Name, thisPar.Type, thisPar.Reuired, thisPar.SetByInput, thisPar.Value));
            }
            Console.WriteLine(new String('-', fs.Length));
            Console.WriteLine("\n");


            
        }
    }
}
