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
        static private string space = "    ";
        public static void ShowStart(string mqttUrl, string mqttTopic, string neplanServiceUrl)
        {
            Console.WriteLine("%%%%% Start: Application (V 1.1) %%%%%\n");
            Console.WriteLine("MQTT-Client details:");
            Console.WriteLine("    broker:   " + mqttUrl + "(set in C#)");
            Console.WriteLine("    topic:    " + mqttTopic + "(set in C#)\n");

            Console.WriteLine("Neplan-Client details :");
            Console.WriteLine("    Server:   " + neplanServiceUrl + " (set in C#)");
            Console.WriteLine("    username: not set (pending via mqtt)");
            Console.WriteLine("    password: not set (pending via mqtt)");
            Console.WriteLine("    project:  not set (pending via mqtt)\n");
        }
        public static void ShowMsgReceived()
        {
            Console.WriteLine("--> MESSAGE RECEIVED\n");
        }
        public static void ShowFunction(Command cmd)
        {
            Console.WriteLine("--> FUNCTION NAME: " + cmd.FunctionName);
            if (!cmd.Error)
                Console.WriteLine(space + "SUCCESS: function found\n");
        }

        public static void ShowParameters(Command cmd)
        {
            // get values
            List<string[]> stringArrays = new List<string[]>();
            string[] titles = { "name", "type", "isRequired", "isSetByInput", "value" };
            stringArrays.Add(titles);
            foreach (Parameter thisPar in cmd.Input.Values)
            {
                string[] str = { thisPar.Name, (thisPar.Type ?? "-"), thisPar.Reuired.ToString(), thisPar.SetByInput.ToString(), (thisPar.Value ?? "-").ToString() };
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

            // set fromat
            string format = "|";
            for (int i = 0; i < titles.Length; i++)
            {
                format = format + "{" + i.ToString() + ",-" + sizes[i] + "}|";
            }


            // print out
            string info = "--> PARAMETERS:";
            if (stringArrays.Count > 1)
            {
                Console.WriteLine(info);
                for (int i = 0; i < stringArrays.Count; i++)
                {
                    string stringLine = String.Format(format, stringArrays[i]);
                    if (i == 0)
                        Console.WriteLine(space + new String('-', stringLine.Length));
                    Console.WriteLine(space + String.Format(format, stringArrays[i]));
                    if (i == 0 || i == stringArrays.Count - 1)
                        Console.WriteLine(space + new String('-', stringLine.Length));
                }
            }
            else
            {
                Console.WriteLine(info + " none");
            }
            if (!cmd.Error)
                Console.WriteLine(space + "SUCCESS: all parameters set\n");

        }
        public static void ShowResults(Command cmd)
        {
            string info = "--> RESULTS:";
            
            if (cmd.Output != null)
            {
                Console.WriteLine(info);
                Console.WriteLine(space + "Type: " + cmd.Output.GetType().ToString());
            }
            else
            {
                Console.WriteLine(info + " none");
            }


            if (!cmd.Error)
                Console.WriteLine(space + "SUCCESS: function was called successfully\n");
        }
        public static void ShowEnd(bool isOK)
        {
            if (isOK)
                Console.WriteLine("\n\n%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% SUCCESS & DONE! WAITING FOR NEW COMMANDS ... \n\n");
            else
                Console.WriteLine("\n\n%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% NO SUCCESS but DONE! WAITING FOR NEW COMMANDS ... \n\n");
        }
    }
}
