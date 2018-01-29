using Neplan_Cloud_Connector_NCC.NeplanService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Neplan_Cloud_Connector_NCC
{
    class Command
    {
        // fields for the ID and the funtion Name of each command
        public string ID, FunctionName, Direction;
        
        // directionry fpr the paramete names and the values
        public Dictionary<string, Parameter> Input 
            = new Dictionary<string, Parameter>();

        // fild for the result from the web-services. As an object,
        // every datatype is allowed
        public object Output;

        // boolean fields for errors and completion
        public bool Error = false, Done = false;

        // fields for the error and exceptation massages
        public string ErrorMsg, ExceptionMsg;

        // constructor method will be called by the controller.
        // Within the construction the whole command will be processed.
        // This method is devidet in to chapters for a bether structure and 
        // understanding
        public Command(object objectHandler, string id,
            string fcnName, Dictionary<string, object> input)
        {
            // set this ID to the given string
            ID = id;
            ////////////////////////////////////////
            // check object handler
            ////////////////////////////////////////
            if (objectHandler == null)
            {
                // if there is no valid object hanlde, set an error
                SetError("Method could not be found");
                // and return the comand and send it to mqtt-broker
                return;
            }



            ////////////////////////////////////////
            // set method
            ////////////////////////////////////////
            MethodInfo Method;
            try
            {
                // set the function name to the given string
                FunctionName = fcnName;
                // get the method if possible
                Method = objectHandler.GetType().GetMethod(FunctionName);
            }
            catch (Exception e)
            {
                // if there occures an error while getting the method, 
                // an error will be set.
                SetError("Method could not be found",e);
                // return the comand and send it to mqtt-broker
                return;
            }

            try
            {
                // try to show the funtion name in the console
                ConsoleOut.ShowFunction(this);
            }
            catch (Exception e)
            {
                SetWarning("method could not be shown in console", e);
            }




            ////////////////////////////////////////
            // set required parameters
            ////////////////////////////////////////
            // create list for parameter values to invoke the method with
            List<object> values = new List<object>(); 
            try
            {
                // loop eache required parameter of the method
                foreach (ParameterInfo parInfo in Method.GetParameters())
                {
                    // create temporary variable p for the parameter with
                    // requested function name and datatype
                    Parameter p = new Parameter
                    {
                        Name = parInfo.Name,
                        Type = parInfo.ParameterType.ToString(),
                        Reuired = true
                    };

                    // if the requestet parameter is set by the input,
                    // convert the datatype and save the value into the
                    // dictionary and into the values list.
                    if (input.ContainsKey(parInfo.Name))
                    {
                        // swith the data type and make the corresponding
                        // convertion
                        switch (p.Type)
                        {
                            case "Neplan_Cloud_Connector_NCC.NeplanService." +
                            "ExternalProject":
                                // check for an external project with the given
                                //project name
                                p.Value = (
                                    (NeplanServiceClient)objectHandler).
                                    GetProject((string)input[p.Name], 
                                    null, null, null);
                                break;
                            case "System.String":
                                p.Value = (string)input[p.Name];
                                break;
                            case "System.Int32":
                                p.Value = Convert.ToInt32(input[p.Name]);
                                break;
                            case "System.DateTime":
                                // create datetime out of the given date vector
                                int[] DateVec 
                                    = ((JArray)input[p.Name]).ToObject<int[]>();
                                p.Value = new DateTime(
                                    DateVec[0], DateVec[1], DateVec[2],
                                    DateVec[3], DateVec[4], DateVec[5]);
                                break;
                            default:
                                // if datetype could not be converted, set the 
                                // value to null
                                p.Value = null;
                                SetError("Could not convert " + p.Name
                                    + " to datatype " + p.Type 
                                    + ". There is no method for this datatype.");
                                break;
                        }
                        // add the parameter value to the value list
                        values.Add(p.Value);
                        // confirm that the value is set by the input
                        p.SetByInput = true;
                        // remove the parameter from the input dictionary
                        input.Remove(p.Name);
                    }
                    // if the requested parameter is not within the input 
                    // dictionary, set the corresponding error
                    else
                    {
                        p.SetByInput = false;
                        SetError("parameter " + parInfo.Name + " missing");
                    }

                    // add the parameter to the Input dictionarry of the command
                    Input.Add(parInfo.Name, p);
                }
            }
            catch (Exception e)
            {
                // set an error if anything went wrong
                SetError("required parameter could not be set", e);
            }

            ////////////////////////////////////////
            // set additional parameters
            ////////////////////////////////////////
            try
            {
                // loop all parameters which remain in the input dictionary 
                // and add them as unrequired to the Input dictionary for 
                // comleting the table within the console 
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

            ////////////////////////////////////////
            // show parameters in console
            ////////////////////////////////////////
            try
            {
                // show the parameter table in the console
                ConsoleOut.ShowParameters(this);
            }
            catch (Exception e)
            {
                SetWarning("parameters could not be shown in console", e);
            }
            // if any error accures till here, return the comand and send it 
            // to mqtt-broker
            if (Error) return;
            ////////////////////////////////////////


            ////////////////////////////////////////
            // invoke methode
            ////////////////////////////////////////
            try
            {
                // try to invoke the method with the given parameters and save 
                // the result
                Output = Method.Invoke(objectHandler, values.ToArray());
            }
            catch (Exception e)
            {
                // if the method could not be invoked, set an error
                SetError("method could not be invoked", e);
                // return the comand and send it to mqtt-broker
                return;
            }

            ////////////////////////////////////////
            // conver output
            ////////////////////////////////////////
            try
            {
                // if the result is not null check if it needs to be converted.
                if (Output != null)
                {
                    // switch between datatypes
                    switch (Output.GetType().ToString())
                    {
                        case "System.String[]":
                            // if the data type is a sting array
                            string[] strArr = (string[])Output;
                            // ... & if there is just one field in the string array
                            if (strArr.Length == 1)
                                // ... & the string starts with '<'
                                if (strArr[0].ToString().IndexOf('<') == 0)
                                    // ... it is an xml-string and can be 
                                    // converted to an object
                                    Output = xml2dir((string)Output);
                            break;
                        case "System.String":
                            // if the data type is a string and starts with '<'
                            if (Output.ToString().IndexOf('<') == 0)
                                // ... it is an xml-string and can be converted
                                // to an object
                                Output = xml2dir((string)Output);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                // show a warrning if the output could now be converted
                SetWarning("output could not be converted", e);
            }

            ////////////////////////////////////////
            // show results output
            ////////////////////////////////////////
            try
            {
                // try to show the results in the console
                ConsoleOut.ShowResults(this);
            }
            catch (Exception e)
            {
                SetWarning("output could not be shown in console", e);
            }
            // end of the constructor method
        }

        // converts xml_strings to objects
        private static object xml2dir(string xml_string)
        {
            // create new xml document
            XmlDocument doc = new XmlDocument();
            // write the string to it
            doc.LoadXml(xml_string);
            // convert it to json string
            string json =  JsonConvert.SerializeXmlNode(doc, 
                Newtonsoft.Json.Formatting.None);
            // convert the json to object
            return JsonConvert.DeserializeObject(json);
        }

        // method to handle errors easily
        public void SetError(string msg, Exception e = null)
        {
            // set errors, messages and exceptions to return them to the NCC-Client
            // also show the messages and exceptions in the console
            Error = true;
            ErrorMsg = msg;
            Console.WriteLine("--> Error: " + ErrorMsg + "\n");
            if (e != null)
            {
                ExceptionMsg = e.ToString();
                Console.WriteLine("    System message: " + ExceptionMsg + "\n");
            }
        }
        // method to handle warrning easily
        public void SetWarning(string msg, Exception e = null)
        {
            // show the messages and exceptions in the console
            Console.WriteLine("--> Warrning: " + msg + "\n");
            if (e != null)
            {
                Console.WriteLine("    System message: " + e.ToString() + "\n");
            }
        }
    }
}

