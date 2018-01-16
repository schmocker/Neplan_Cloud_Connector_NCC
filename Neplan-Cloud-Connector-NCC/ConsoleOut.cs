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
            Console.WriteLine(cmd.FunctionName + "\n");
            Console.WriteLine("--> Benötigte Parameter:");


            // get values
            List<string[]> stringArrays = new List<string[]>();
            string[] titles = { "name", "type", "isRequired", "isSetByInput", "value" };
            stringArrays.Add(titles);
            foreach (Parameter thisPar in cmd.Input.Values)
            {
                string[] str = { thisPar.Name, thisPar.Type, thisPar.Reuired.ToString(), thisPar.SetByInput.ToString(), (thisPar.Value ?? "-").ToString() };
                stringArrays.Add(str);
            }

            // count sizes
            List<int> sizes = new List<int>(new int[titles.Length]);
            foreach (string[] stringArray in stringArrays)
            {
                for (int i = 0; i < stringArray.Length; i++)
                {
                    sizes[i] = Math.Max(sizes[i], stringArray[i].Length+1);
                }
            }

            // print out
            string format = "|{0,-" + sizes[0] + "}|{1,-" + sizes[1] + "}|{2,-" + sizes[2] + "}|{3,-" + sizes[3] + "}|{4,-" + sizes[4] + "}|";
            for (int i = 0; i < stringArrays.Capacity; i++)
            {
                string stringLine = String.Format(format, stringArrays[i]);
                if (i == 0)
                    Console.WriteLine(new String('-', stringLine.Length));
                Console.WriteLine(String.Format(format, stringArrays[i]));
                if (i == 0 || i == stringArrays.Capacity - 1)
                    Console.WriteLine(new String('-', stringLine.Length));
            }
            Console.WriteLine("\n");
            
        }
    }
}
