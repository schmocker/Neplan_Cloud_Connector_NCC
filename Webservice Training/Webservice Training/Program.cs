using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Webservice_Training.NeplanService;
using System.Security.Cryptography;
using System.Xml;
using System.Globalization;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Webservice_Training
{
    class Program
    {
        bool CalcOk = true;

        static void Main(string[] args)
        {
            MqttClient client = new MqttClient("www.tobiasschmocker.ch");
            byte code = client.Connect(Guid.NewGuid().ToString());
            client.ProtocolVersion = MqttProtocolVersion.Version_3_1;
            





            Webservice webservice = new Webservice();
            Program example = new Program();
            if (webservice.nepService != null && webservice.ext != null)      //Checks if the interface was created
            {
                example.RunLoadFlow(webservice);                                //runs a load flow
                example.GetResultsLoadFlow(webservice);                         //gets results
                example.OpenSwitch(webservice);                                 //open a switch                     
                example.ChangePsetting(webservice);                             //sets P of a 1-port element                
                example.RepeatLoadFLowandResults(webservice);                   //repeats the process
                example.RestoreOriginalValues(webservice);
                example.RunSwitchingOptimisation(webservice);
                example.GetResultsSwitchingOptimization(webservice);
            }
            webservice.CloseWebservice();
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine("Received = " + Encoding.UTF8.GetString(e.Message) + " on topic " + e.Topic);
        }

        internal void RunLoadFlow(Webservice webservice)
        {
            if (!CalcOk)
                return;

            //run load flow. It is possible to define the operational state
            AnalysisReturnInfo analysis = webservice.nepService.AnalyseVariant(webservice.ext, Guid.NewGuid().ToString(), "LoadFlow", "Default", string.Empty, string.Empty, string.Empty);
            if (analysis.ReturnInfo == 1 && analysis.HasConverged)      //returninfo should be 1 if the calculation was done successfully. HasConverged is a special flag only to be used for Load Flow
                Console.WriteLine("Load Flow run successfully!");
            else
            {
                Console.WriteLine("Could not run Load Flow!");
                CalcOk = false;
            }

        }

        internal void GetResultsLoadFlow(Webservice webservice)
        {
            if (!CalcOk)
                return;

            int networkTypeGroup = 0;           //identifier for network results (other indexes refer to area, zone, feeder etc.)
            string[] networkresults = webservice.nepService.GetListResultSummary(webservice.ext, "LoadFlow", new DateTime(), networkTypeGroup, null);
            if (networkresults == null || networkresults.Count() != 1)
            {
                CalcOk = false;
                Console.WriteLine("Could not get the network results!");
                return;
            }
            else
            {
                string plosses = GetXMLAttribute(networkresults[0], "PLosses");
                string qlosses = GetXMLAttribute(networkresults[0], "QLosses");
                Console.WriteLine(string.Format("Network Active Losses: {0:0.000}kW", Convert.ToDouble(plosses, CultureInfo.InvariantCulture) * 1000));
                Console.WriteLine(string.Format("Network Reactive Losses: {0:0.000}kVar", Convert.ToDouble(qlosses, CultureInfo.InvariantCulture) * 1000));
            }

            int portNumber = 0;          //0 is the first port connection of an element in Neplan 
            string elementname = "Netz";
            string elementtype = "ExternalGrid";
            //gets the element results for the external grid  
            string result = webservice.nepService.GetResultElementByName(webservice.ext, elementname, elementtype, portNumber, "LoadFlow", new DateTime());
            if (result == null)
            {
                CalcOk = false;
                Console.WriteLine("Could not get the results of ");
                return;
            }
            else
            {
                string power = GetXMLAttribute(result, "P");        //gets the power import for the network
                Console.WriteLine(string.Format("The network is supplied with: {0:0.00} kW", -Convert.ToDouble(power, CultureInfo.InvariantCulture) * 1000));
            }
        }


        internal void OpenSwitch(Webservice webservice)
        {
        }

        internal void ChangePsetting(Webservice webservice)
        {
        }

        internal void RepeatLoadFLowandResults(Webservice webservice)
        {
        }

        internal void RestoreOriginalValues(Webservice webservice)
        {
        }

        internal void RunSwitchingOptimisation(Webservice webservice)
        {
        }

        internal void GetResultsSwitchingOptimization(Webservice webservice)
        {
        }

        /// <summary>
        /// gets the value of an xml element
        /// </summary>
        /// <param name="result"></param>
        /// <param name="xmlelement"></param>
        /// <returns></returns>
        private string GetXMLAttribute(string result, string xmlelement)
        {
            if (string.IsNullOrEmpty(result))
                return null;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(result);
            XmlNodeList nodeList = xmldoc.GetElementsByTagName(xmlelement);
            if (nodeList == null || nodeList.Count != 1)
            {
                CalcOk = false;
                return null;
            }
            else

                return nodeList[0].InnerText;
        }
    }
}

public class Webservice
{
    public NeplanServiceClient nepService;
    public ExternalProject ext;
    private string username = "christoph.hunziker@fhnw.ch";
    private string password = "nep360FH2017";
    public string project = "NeplanMatlab2";

    public Webservice()
    {
        nepService = new NeplanServiceClient();                                 //instantiates the neplaservice
        nepService.ClientCredentials.UserName.UserName = username;              //give the username       
        nepService.ClientCredentials.UserName.Password = getMd5Hash(password);  //give the password
        try
        {
            nepService.Open();                              //open the service
            Console.WriteLine("Opened service");
            ext = nepService.GetProject(project, null, null, null);   //get the project             
            if (ext != null)
                Console.WriteLine("Got project");
            else
                Console.WriteLine("Cannot get project");

        }
        catch
        {
            Console.WriteLine("Cannot open service");
        }
    }

    public void CloseWebservice()
    {
        try
        {
            nepService.Close();         //close the service
            Console.WriteLine("Closed service");
        }
        catch
        {
            Console.WriteLine("Cannot close service");
        }
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
