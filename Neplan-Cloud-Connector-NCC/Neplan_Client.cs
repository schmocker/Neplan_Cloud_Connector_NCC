using Neplan_Cloud_Connector_NCC.NeplanService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Neplan_Cloud_Connector_NCC
{
    class Neplan_Client
    {
        private Controller controller;

        private NeplanServiceClient neplanServiceClient = new NeplanServiceClient();
        private ExternalProject project;

        public Neplan_Client()
        {

        }
        public void setController(Controller controller)
        {
            this.controller = controller;
        }


        public Command treatCommand(Command command)
        {
            command.Inputs["project"] = project;
            command.Inputs["projectName"] = project;

            // short name variables
            Dictionary<string, object> p = command.Inputs;
            object result;

            // get Method
            MethodInfo method = neplanServiceClient.GetType().GetMethod(command.MethodName);
            ParameterInfo[] parameters = method.GetParameters();
            string[] parName = new string[parameters.Length];
            string[] parType = new string[parameters.Length];
            object[] parInput = new object[parameters.Length];
            object[] par = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                parName[i] = parameters[i].Name;
                parType[i] = parameters[i].ParameterType.ToString();
                bool hasInput = command.Inputs.ContainsKey(parName[i]);
                parInput[i] = hasInput ? command.Inputs[parName[i]] : null;

                // Convert parameter
                switch (parType[i])
                {
                    case "Neplan_Cloud_Connector_NCC.NeplanService.ExternalProject":
                        break;
                    case "System.String":
                        break;
                    case "System.Int32":
                        break;
                    case "System.DateTime":
                        break;
                    default:
                        break;
                }

                // pass parameter to parameter array
                par[i] = command.Inputs[parName[i]];
            }

            result = method.Invoke(neplanServiceClient, par);

            command.Outputs = result;

            return command;
        }


        // 
        public bool hasMethod(String methodName)
        {
            Type classType = neplanServiceClient.GetType();
            MethodInfo methodInfo = classType.GetMethod(methodName);
            return (methodInfo != null);
        }

        // methods to connect or disconnect
        public void StartNeplanServiceClient(string username, string password, string projectName)
        {
            neplanServiceClient = new NeplanServiceClient();
            neplanServiceClient.ClientCredentials.UserName.UserName = username;              //give the username       
            neplanServiceClient.ClientCredentials.UserName.Password = getMd5Hash(password);  //give the password
            try
            {
                neplanServiceClient.Open();                              //open the service
                Console.WriteLine("Opened service");
                project = neplanServiceClient.GetProject(projectName, null, null, null);   //get the project             
                if (project != null)
                    Console.WriteLine("Got project");
                else
                    Console.WriteLine("Cannot get project");
            }
            catch
            {
                Console.WriteLine("Cannot open service");
            }


            // Show all methods
            /*
            MethodInfo[] methods = neplanServiceClient.GetType().GetMethods();
            foreach (var item in methods)
            {
                Console.WriteLine(item + "\n");
            }
            */
        }

        public void StopNeplanServiceClient()
        {
            try
            {
                neplanServiceClient.Close();         //close the service
                Console.WriteLine("Closed service");
            }
            catch
            {
                Console.WriteLine("Cannot close service");
            }
        }


        // methods to decode the results returned from neplan
        private static Dictionary<string, object> xml3dir(string xml_string)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml_string);
            string json = JsonConvert.SerializeXmlNode(doc);
            Dictionary<string, object> obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            return obj;
        }
        private static Dictionary<string, object> xml2dir(string xml_string)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml_string);
            string json = JsonConvert.SerializeXmlNode(doc);
            Dictionary<string, object> obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            return obj;
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
}
