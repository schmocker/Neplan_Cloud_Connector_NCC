using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Neplan_Cloud_Connector_NCC
{
    class Command
    {
        public string FunctionName, ClassName;

        public object ObjectHandler;
        public MethodInfo Method;


        public ParameterDictionary Input = new ParameterDictionary();
        public object Output;

        public bool Error = false;
        public bool Received = false;
        public bool Done = false;
        public string ErrorMsg, ExceptionMsg;

        // Constructor Cmethods
        public Command(string fcnName)
        {
            this.FunctionName = fcnName;
        }
        public Command(string fcnName, object objectHandler)
        {
            this.FunctionName = fcnName;
            this.ObjectHandler = objectHandler;
            ClassName = objectHandler.GetType().ToString();
            Method = objectHandler.GetType().GetMethod(fcnName);

            foreach (var par in Method.GetParameters())
                Input.AddRequired(par);
        }
        // Clone method to copy an instance
        public Command Clone()
        {
            return (Command)this.MemberwiseClone();
        }


        // Run method
        public void Run()
        {
            

        }

        public void setError(string msg, Exception e = null)
        {
            Error = true;
            ErrorMsg = msg;
            Console.WriteLine("--> Error: "+ ErrorMsg + "\n");
            if (e != null)
            {
                ExceptionMsg = e.ToString();
                Console.WriteLine("    System message: " + ExceptionMsg + "\n");
            }
        }

        public void SetParameters(Dictionary<string, object> input)
        {
            try
            {
                foreach (Parameter par in Input.Values)
                {
                    if (input.ContainsKey(par.Name))
                    {
                        par.SetValue(input[par.Name]);
                        input.Remove(par.Name);
                        par.SetByInput = true;
                    }
                    else
                    {
                        setError("Parameter '"+par.Name+"' missing");
                    }
                }
                foreach (var item in input)
                {
                    // unbenötigte parameter -> ergänzen
                }
            }
            catch (Exception e)
            {
                setError("Parameters not set",e);
            }
        }


        public void Invoke()
        {
            try
            {
                Output = Method.Invoke(ObjectHandler, Input.GetRequiredValues());
                Console.WriteLine("Method invoked");
            }
            catch (Exception e)
            {
                setError("Method not invoked", e);
            }
        }
        public void ConvertOutput()
        {
            if (Output != null)
            {
                Console.WriteLine(Output.GetType().ToString());
                switch (Output.GetType().ToString())
                {
                    case "System.String[]":
                        string[] a = (string[])Output;
                        Output = xml2dir(a[0]);
                        break;
                    default:
                        break;
                }
            }
        }

        public Dictionary<string, object> Results()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("Input", Input);
            result.Add("Output", Output);
            result.Add("Received", Received);
            result.Add("Done", Done);
            return result;
        }

        private static Dictionary<string, object> xml2dir(string xml_string)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml_string);
            string json = JsonConvert.SerializeXmlNode(doc);
            Dictionary<string, object> obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            return obj;
        }
    }
    class CommandDictionary : Dictionary<string, Command>
    {
        public void AddNew(string methodName, object objectHandler)
        {
            Command cmd = new Command(methodName, objectHandler);
            this.Add(methodName, cmd);
        }
    }
}
