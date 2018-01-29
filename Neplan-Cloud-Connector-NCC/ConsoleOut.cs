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
        // field for spaces
        static private string space = "    ";

        // methd to show start process
        public static void ShowStart(string mqttUrl, string mqttTopic, 
            string neplanServiceUrl, string nccUser, string project)
        {
            Console.WriteLine("%%%%% Start: Application (V 1.0) %%%%%\n");

            // show mqtt deatils
            Console.WriteLine("MQTT-Client details:");
            Console.WriteLine("    broker:   " + mqttUrl);
            Console.WriteLine("    topic:    " + mqttTopic + "\n");

            // show ncc deatils
            Console.WriteLine("Neplan-Client details :");
            Console.WriteLine("    Server:   " + neplanServiceUrl + " (set in C#)");
            Console.WriteLine("    username: " + nccUser);
            Console.WriteLine("    password: *******");
            Console.WriteLine("    project: " + project + "\n");
        }

        // method to show received message
        public static void ShowMsgReceived()
        {
            Console.WriteLine("--> MESSAGE RECEIVED\n");
        }

        // method to show function name
        public static void ShowFunction(Command cmd)
        {
            Console.WriteLine("--> FUNCTION NAME: " + cmd.FunctionName);
            if (!cmd.Error)
                Console.WriteLine(space + "SUCCESS: function found\n");
        }

        // method to show parameter table
        public static void ShowParameters(Command cmd)
        {
            // creat empty list with string arrays
            List<string[]> rows = new List<string[]>();

            // set table titles and add them as first row
            string[] titles = { "name", "type", "isRequired",
                "isSetByInput", "value" };
            rows.Add(titles);

            // add one row for each parameter and add strings to the fields
            foreach (Parameter thisPar in cmd.Input.Values)
            {
                string[] str = { thisPar.Name,
                    (thisPar.Type ?? "-"),
                    thisPar.Reuired.ToString(),
                    thisPar.SetByInput.ToString(),
                    (thisPar.Value ?? "-").ToString() };
                rows.Add(str);
            }

            // get the minimum with of each column
            List<int> sizes = new List<int>(new int[titles.Length]);
            foreach (string[] stringArray in rows)
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

            // print out form the list line by line with the given format
            string info = "--> PARAMETERS:";
            if (rows.Count > 1)
            {
                Console.WriteLine(info);
                for (int i = 0; i < rows.Count; i++)
                {
                    string stringLine = String.Format(format, rows[i]);
                    if (i == 0)
                        Console.WriteLine(space
                            + new String('-', stringLine.Length));
                    Console.WriteLine(space + String.Format(format, rows[i]));
                    if (i == 0 || i == rows.Count - 1)
                        Console.WriteLine(space 
                            + new String('-', stringLine.Length));
                }
            }
            else
            {
                Console.WriteLine(info + " none");
            }
            if (!cmd.Error)
                Console.WriteLine(space + "SUCCESS: all parameters set\n");

        }

        // method to show the results
        public static void ShowResults(Command cmd)
        {
            string info = "--> RESULTS:";
            
            if (cmd.Output != null)
            {
                Console.WriteLine(info);
                Console.WriteLine(space + "Type: " 
                    + cmd.Output.GetType().ToString());
            }
            else
            {
                Console.WriteLine(info + " none");
            }


            if (!cmd.Error)
                Console.WriteLine(space 
                    + "SUCCESS: function was called successfully\n");
        }
        // method to show the end of a command
        public static void ShowEnd(bool isOK)
        {
            if (isOK)
                Console.WriteLine("\n\n%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%"
                    + " SUCCESS & DONE! WAITING FOR NEW COMMANDS ... \n\n");
            else
                Console.WriteLine("\n\n%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%"
                    + " NO SUCCESS but DONE! WAITING FOR NEW COMMANDS ... \n\n");
        }
    }
}
