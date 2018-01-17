using Neplan_Cloud_Connector_NCC.NeplanService;
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
        public string FunctionName;

        //public object ObjectHandler;
        //public MethodInfo Method;


        public Dictionary<string, Parameter> Input = new Dictionary<string, Parameter>();
        public object Output;

        public bool Error = false;
        public bool Received = false;
        public bool Done = false;
        public string ErrorMsg, ExceptionMsg;

        // Constructor Cmethods
        public Command(object objectHandler, string fcnName, Dictionary<string, object> input)
        {
            ////////////////////////////////////////
            // check object handler
            ////////////////////////////////////////
            if (objectHandler == null)
            {
                SetError("Method could not be found");
                return;
            }



            ////////////////////////////////////////
            // set method
            ////////////////////////////////////////
            MethodInfo Method;
            try
            {
                FunctionName = fcnName;
                Method = objectHandler.GetType().GetMethod(FunctionName);
            }
            catch (Exception e)
            {
                SetError("Method could not be found",e);
                return;
            }
            try
            {
                ConsoleOut.ShowFunction(this);
            }
            catch (Exception e)
            {
                SetWarning("method could not be shown in console", e);
            }

            


            ////////////////////////////////////////
            // set parameters
            ////////////////////////////////////////
            // create list for parametervalues to invoke the method with
            List<object> values = new List<object>(); 
            // set required parameters
            try
            {
                
                foreach (ParameterInfo parInfo in Method.GetParameters())
                {
                    Parameter p = new Parameter
                    {
                        Name = parInfo.Name,
                        Type = parInfo.ParameterType.ToString(),
                        Reuired = true
                    };
                    if (input.ContainsKey(parInfo.Name))
                    {
                        switch (p.Type)
                        {
                            case "Neplan_Cloud_Connector_NCC.NeplanService.ExternalProject":
                                p.Value = ((NeplanServiceClient)objectHandler).GetProject((string)input[p.Name], null, null, null);
                                break;
                            case "System.String":
                                p.Value = (string)input[p.Name];
                                break;
                            case "System.Int32":
                                p.Value = Convert.ToInt32(input[p.Name]);
                                break;
                            case "System.DateTime":
                                int[] DateVec = ((JArray)input[p.Name]).ToObject<int[]>();
                                p.Value = new DateTime(DateVec[0], DateVec[1], DateVec[2], DateVec[3], DateVec[4], DateVec[5]);
                                break;
                            default:
                                p.Value = null;
                                SetError("Could not convert " + p.Name + " to datatype " + p.Type + ". There is no method for this datatype.");
                                break;
                        }
                        values.Add(p.Value);
                        p.SetByInput = true;
                        input.Remove(p.Name);
                    }
                    else
                    {
                        p.SetByInput = false;
                        SetError("parameter " + parInfo.Name + " missing");
                    }
                    Input.Add(parInfo.Name, p);
                }
            }
            catch (Exception e)
            {
                SetError("required parameter could not be set", e);
            }
            // set additional parameters
            if (!Error)
            {
                try
                {
                    foreach (KeyValuePair<string, object> item in input)
                    {
                        Parameter p = new Parameter
                        {
                            Name = item.Key,
                            Value = item.Value,
                            Type = item.Value.GetType().ToString(),
                            Reuired = false,
                            SetByInput = true
                        };
                        Input.Add(item.Key, p);
                    }
                }
                catch (Exception e)
                {
                    SetWarning("unused parameter could not be set", e);
                }
            }
            // show parameters in console
            try
            {
                ConsoleOut.ShowParameters(this);
            }
            catch (Exception e)
            {
                SetWarning("parameters could not be shown in console", e);
            }
            if (Error) return;
            ////////////////////////////////////////


            ////////////////////////////////////////
            // invoke methode
            ////////////////////////////////////////
            try
            {
                Output = Method.Invoke(objectHandler, values.ToArray());
            }
            catch (Exception e)
            {
                SetError("method could not be invoked", e);
                return;
            }

            ////////////////////////////////////////
            // conver output
            ////////////////////////////////////////
            try
            {
                if (Output != null)
                {
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
            catch (Exception e)
            {
                SetWarning("output could not be converted", e);
            }

            ////////////////////////////////////////
            // conver output
            ////////////////////////////////////////
            try
            {
                ConsoleOut.ShowResults(this);
            }
            catch (Exception e)
            {
                SetWarning("output could not be shown in console", e);
            }
            
        }


        private static Dictionary<string, object> xml2dir(string xml_string)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml_string);
            string json = JsonConvert.SerializeXmlNode(doc);
            Dictionary<string, object> obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            return obj;
        }


        public void SetError(string msg, Exception e = null)
        {
            Error = true;
            ErrorMsg = msg;
            Console.WriteLine("--> Error: " + ErrorMsg + "\n");
            if (e != null)
            {
                ExceptionMsg = e.ToString();
                Console.WriteLine("    System message: " + ExceptionMsg + "\n");
            }
        }
        public void SetWarning(string msg, Exception e = null)
        {
            Console.WriteLine("--> Warrning: " + msg + "\n");
            if (e != null)
            {
                Console.WriteLine("    System message: " + e.ToString() + "\n");
            }
        }
    }
}