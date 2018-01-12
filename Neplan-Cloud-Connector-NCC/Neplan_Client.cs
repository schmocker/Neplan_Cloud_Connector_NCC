using Neplan_Cloud_Connector_NCC.NeplanService;
using Newtonsoft.Json;
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
            // Standard values
            command.Inputs.Add("analysisRefenceID", Guid.NewGuid().ToString());
            command.Inputs.Add("simulationDateTime", new DateTime());

            // short name variables
            Dictionary<string, object> p = command.Inputs;
            object result;

            // Funktion ausführen
            switch (command.MethodName)
            {
                case "AnalyseVariant":
                    result = neplanServiceClient.AnalyseVariant(project,
                        (string)p["analysisRefenceID"],
                        (string)p["analysisModule"],
                        (string)p["calcNameID"],
                        (string)p["analysisMethode"],
                        (string)p["conditions"],
                        (string)p["analysisLoadOptionXML"]);
                    break;
                case "GetListResultSummary":
                    string[] r = neplanServiceClient.GetListResultSummary(project,
                        (string)p["analysisType"],
                        (DateTime)p["simulationDateTime"],
                        Convert.ToInt32(p["networkTypeGroup"]),
                        (string)p["networkTypeGroupID"]);
                    result = xml2dir(r[0]);
                    break;
                case "GetResultElementByName":
                    string d = neplanServiceClient.GetResultElementByName(project,
                        (string)p["elementName"],
                        (string)p["elementTypeName"],
                        Convert.ToInt32(p["portNr"]),
                        (string)p["analysisType"],
                        (DateTime)p["simulationDateTime"]);
                    result = xml2dir(d);
                    break;
                default:
                    result = "invalid function name";
                    break;
            }

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
