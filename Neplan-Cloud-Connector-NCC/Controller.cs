using Neplan_Cloud_Connector_NCC.NeplanService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Neplan_Cloud_Connector_NCC
{
    class Controller
    {
        // field for the MQTT-Client
        private Mqtt_Client mqttClient;

        // field for the Neplan web service
        public NeplanServiceClient neplanServiceClient;

        // field for the Neplan project
        public ExternalProject project;

        // constructor method called by Program.Main()
        public Controller(string propertiesPath)
        {
            // prepare variables for the properties
            string nccUsername = null;
            string nccPW = null;
            string nccPprojectname = null;
            string mqttUrl = null;
            string mqttTopic = null;

            // try to read the propertier from the given path and
            // save them into the properties variables
            try
            {
                // read all lines of the file to an array
                string[] lines = File.ReadAllLines(propertiesPath);
                // for each line read the Category, Name and Value and 
                // save the values if category and name are correct.
                for (int i = 0; i < lines.Length; i++)
                {
                    String[] s = lines[i].Split(new char[] { ' ', '\t' }, 
                        StringSplitOptions.RemoveEmptyEntries);
                    if (s[0] == "MQTT")
                    {
                        if (s[1] == "url") mqttUrl = s[2];
                        if (s[1] == "topic") mqttTopic = s[2];
                    }

                    if (s[0] == "NCC")
                    {
                        if (s[1] == "username") nccUsername = s[2];
                        if (s[1] == "password") nccPW = getMd5Hash(s[2]);
                        if (s[1] == "project") nccPprojectname = s[2];
                    }
                }
            }
            catch (Exception e)
            {
                // error reporting to console
                Console.WriteLine("properties could not be importet from " 
                    + propertiesPath);
                Console.WriteLine(e);
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                Environment.Exit(1);
            }

            // try to connect to the mqtt broker, then connect to the 
            // Neplan web servicesand get the project
            try
            {
                // create object of class Mqtt_Client and connect to broker
                mqttClient = new Mqtt_Client(mqttUrl, mqttTopic);
                mqttClient.setController(this);

                // create object of clas NeplanServiceClient and set
                // username and password
                neplanServiceClient = new NeplanServiceClient();
                neplanServiceClient.ClientCredentials.UserName.UserName
                    = nccUsername;      
                neplanServiceClient.ClientCredentials.UserName.Password
                    = nccPW;
                try
                {
                    // connect to webservices
                    neplanServiceClient.Open();
                    Console.WriteLine("Opened service");

                    // get the project from the web service
                    project = neplanServiceClient.GetProject(
                        nccPprojectname, null, null, null);             
                    if (project != null)
                        Console.WriteLine("Got project");
                    else
                        // error reporting to console
                        Console.WriteLine("Cannot get project");
                }
                catch
                {
                    // error reporting to console
                    Console.WriteLine("Cannot open service");
                }
                // show start information in the console
                string neplanServiceUrl
                    = neplanServiceClient.Endpoint.Address.Uri.AbsoluteUri;
                ConsoleOut.ShowStart(mqttUrl, mqttTopic, neplanServiceUrl, nccUsername, nccPprojectname);
                ConsoleOut.ShowEnd(true);

                // tell the NCC-Client that this program is up and running
                mqttClient.PublishMsg("NCCisRunning", true);
            }
            catch (Exception e)
            {
                // error reporting to console
                Console.WriteLine(e);
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                Environment.Exit(1);
            }
        } // end of Controller construction

        // Method TreatCommand will be called by the MQTT-Client as soon 
        // as it receives a massage with a command to be treated. The 
        // MQTT-Client will deliver the id, the method name and all the 
        // parameters for the method to be called
        public void TreatCommand(string id, string methodName,
            Dictionary<string,object> input)
        {
            // create new object of class Command - by creating the 
            // command, it will be trggerd automatically
            Command cmd = new Command(neplanServiceClient, 
                id, methodName, input);


            // publish the whole command
            cmd.Done = true;
            mqttClient.PublishMsg(cmd);
            ConsoleOut.ShowEnd(!cmd.Error);
        }
        
        // this method was taken from Neplan
        private static string getMd5Hash(string input)
        {
            #region Not important for the training
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            // Return the hexadecimal string.
            return sBuilder.ToString();
            #endregion
        }
    }
    /// <summary>
    /// Summery
    /// </summary>
    class Parameter
    {
        // the class Parameter holds only the following 5 fields
        // and no methods - so it is more like data type
        public string Name;
        public object Value;
        public string Type;
        public bool Reuired;
        public bool SetByInput;
    }
}

