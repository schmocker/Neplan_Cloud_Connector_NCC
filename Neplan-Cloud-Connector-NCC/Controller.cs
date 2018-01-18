using Neplan_Cloud_Connector_NCC.NeplanService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Neplan_Cloud_Connector_NCC
{
    class Controller
    {
        private Mqtt_Client mqttClient;

        string propPath = @"C:\Users\tobias.schmocker\desktop\properties.txt";

        public NeplanServiceClient neplanServiceClient;
        private string neplanServiceUrl = "https://demo.neplan.ch/NEPLAN360_Demo/Services/External/NeplanService.svc";
        public ExternalProject project;

        List<string> validMethodNames = new List<string>();
        public Controller()
        {
            string nccUsername = null;
            string nccPassword = null;
            string nccPprojectname = null;
            string mqttUrl = null;
            string mqttTopic = null;

            try
            {
                string[] lines = System.IO.File.ReadAllLines(propPath);
                for (int i = 0; i < lines.Length; i++)
                {
                    String[] s = lines[i].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (s[0] == "MQTT" && s[1] == "url") mqttUrl = s[2];
                    if (s[0] == "MQTT" && s[1] == "topic") mqttTopic = s[2];
                    if (s[0] == "NCC" && s[1] == "username") nccUsername = s[2];
                    if (s[0] == "NCC" && s[1] == "password") nccPassword = getMd5Hash(s[2]);
                    if (s[0] == "NCC" && s[1] == "project") nccPprojectname = s[2];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("properties could not be importet from " + propPath);
                Console.WriteLine(e);
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                Environment.Exit(1);
            }

            try
            {
                mqttClient = new Mqtt_Client(mqttUrl, mqttTopic);
                mqttClient.setController(this);

                neplanServiceClient = new NeplanServiceClient();
                neplanServiceClient.ClientCredentials.UserName.UserName = nccUsername;              //give the username       
                neplanServiceClient.ClientCredentials.UserName.Password = nccPassword;  //give the password
                try
                {
                    neplanServiceClient.Open();                              //open the service
                    Console.WriteLine("Opened service");

                    Dictionary<string, object> input = new Dictionary<string, object>();

                    project = neplanServiceClient.GetProject(nccPprojectname, null, null, null);   //get the project             
                    if (project != null)
                        Console.WriteLine("Got project");
                    else
                        Console.WriteLine("Cannot get project");

                    foreach (var item in neplanServiceClient.GetType().GetMethods())
                    {
                        validMethodNames.Add(item.Name);
                    }
                }
                catch
                {
                    Console.WriteLine("Cannot open service");
                }

                ConsoleOut.ShowStart(mqttUrl, mqttTopic, neplanServiceUrl);
                ConsoleOut.ShowEnd(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="input"></param>
        public void TreatCommand(string methodName, Dictionary<string,object> input)
        {
            // set object handler & name & input
            // the object handler is the object, that handlse the method bzw. the command.
            Command cmd;
            if (validMethodNames.Contains(methodName))
                cmd = new Command(neplanServiceClient, methodName, input);
            else
                cmd = new Command(null, methodName, input);
            // publish results
            cmd.Done = true;
            string msg_json = JsonConvert.SerializeObject(cmd);
            mqttClient.PublishMsg(msg_json);
            ConsoleOut.ShowEnd(!cmd.Error);
        }
        
        // methods taken from Neplan
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
    class Parameter
    {
        public string Name;
        public object Value;
        public string Type;
        public bool Reuired;
        public bool SetByInput;
    }
}

