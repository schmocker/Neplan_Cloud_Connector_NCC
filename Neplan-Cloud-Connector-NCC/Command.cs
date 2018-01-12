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
        public string MethodName;

        public Object ObjectHandler = null;
        public MethodInfo Method = null;

        public Dictionary<string, object> Ins;

        
        public List<Parameter> Input = new List<Parameter>();
        public List<object> InputValues = new List<object>();
        public object Output;

        public bool Error = false;
        public bool Received = false;
        public bool Done = false;


        public void DecodeJson(string jsonString)
        {
            Dictionary<string, object> message = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
            MethodName = message["fnc"].ToString();
            string InputJson = message["input"].ToString();
            Ins = JsonConvert.DeserializeObject<Dictionary<string, object>>(InputJson);
        }
        public void LocatMethodObject(object[] possibleObjects)
        {
            foreach (var item in possibleObjects)
            {

            }
        }

        public void ReferMethod()
        {
            Method = ObjectHandler.GetType().GetMethod(MethodName);
        }

        public void PrepareParameters()
        {
            

            ParameterInfo[] parameters = Method.GetParameters();
            // Converting all required parameters
            for (int i = 0; i < parameters.Length; i++)
            {
                Input.Add(new Parameter());
                Input[i].Reuired = true;
                Input[i].Name = parameters[i].Name;
                Input[i].Type = parameters[i].ParameterType.ToString();
                if (Ins.ContainsKey(Input[i].Name))
                {
                    Input[i].SetValue(Ins[Input[i].Name]);
                    Ins.Remove(Input[i].Name);
                    Input[i].SetByInput = true;
                }
                InputValues.Add(Input[i].Value);
            }
            // Handling all not required inputs
            int ii = Input.Count();
            foreach (var item in Ins)
            {
                Input.Add(new Parameter());
                Input[ii].Reuired = true;
                Input[ii].Name = item.Key;
                Input[ii].Value = item.Value;
                Input[ii].SetByInput = true;
                ii++;
            }
            // Inputs.Add("analysisRefenceID", Guid.NewGuid().ToString());
        }
        public void Invoke()
        {
            try
            {
                Output = Method.Invoke(ObjectHandler, InputValues.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            
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
}
