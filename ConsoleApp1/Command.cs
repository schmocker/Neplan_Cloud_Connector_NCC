using NeplanMqttService.NeplanService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace NeplanMqttService
{
    class Command
    {
        private NeplanServiceClient neplanServiceClient;
        private ExternalProject project;
        private string methodName;
        private string[] otherValidMethods = { "StartNeplanServiceClient", "StopNeplanServiceClient" };

        public Dictionary<string, object> p = new Dictionary<string, object>();

        public object result;

        public Command(NeplanServiceClient neplanServiceClient, ExternalProject project, string methodName, Dictionary<string, object> par)
        {
            this.methodName = methodName;
            this.neplanServiceClient = neplanServiceClient;
            this.project = project;
            
            if (isNeplanMethod())
            {
                Console.WriteLine("--> " + methodName + " is a valid function for the neplan webservice");
                // valid function for neplan webservic

                showMethodInfo(methodName);
            }
            else if (isOtherMethod())
            {
                Console.WriteLine("--> " + methodName + " is a valid function for the neplan connector");
                switch (methodName)
                {
                    case "StartNeplanServiceClient":
                        StartNeplanServiceClient((string)par["username"], (string)par["password"], (string)par["project"]);
                        break;
                    case "StopNeplanServiceClient":
                        StopNeplanServiceClient();
                        break;
                }
            }
            else
            {
                Console.WriteLine("--> no such function");
            }

            p = par;
            // 
            p.Add("analysisRefenceID", Guid.NewGuid().ToString());
            p.Add("simulationDateTime", new DateTime());

            

            // Funktion ausführen
            switch (methodName)
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
            
            
        }
        

        

        private bool isNeplanMethod()
        {
            Type classType = neplanServiceClient.GetType();
            MethodInfo methodInfo = classType.GetMethod(methodName);
            return (methodInfo != null);
        }
        private bool isOtherMethod()
        {
            bool t = otherValidMethods.Contains(methodName);
            return otherValidMethods.Contains(methodName);
        }

        private Dictionary<string, Type> methodParameters(object obj, string methodName)
        {
            if (isNeplanMethod())
            {
                Type classType = obj.GetType();
                MethodInfo methodInfo = classType.GetMethod(methodName);
                return new Dictionary<string, Type>();
            }
            else
            {
                return null;
            }
        }

        // Connector Funktionen
        private void StartNeplanServiceClient(string username, string password, string projectName)
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
        
        private void StopNeplanServiceClient()
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
        



        // Anzeigen
        private void showMethodInfo(string methodName)
        {
            MethodInfo thisMethod = typeof(NeplanServiceClient).GetMethod(methodName);
            ParameterInfo[] allPars = thisMethod.GetParameters();
            Console.WriteLine("--> Funktion:");
            Console.WriteLine(methodName + "\n");
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

        // Anderes
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
