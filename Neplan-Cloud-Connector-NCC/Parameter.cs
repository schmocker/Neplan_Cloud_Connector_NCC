using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neplan_Cloud_Connector_NCC
{
    class Parameter
    {
        public string Name;
        public string Type = null;
        public object Value = null;
        public bool Reuired = false;
        public bool SetByInput = false;

        public void SetValue(Object Input)
        {
            switch (Name)
            {
                case "analysisRefenceID":
                    Input = Guid.NewGuid().ToString();
                    break;
                case "project":
                    break;
                case "projectName":
                    break;
                default:
                    break;
            }
            switch (Type)
            {
                case "Neplan_Cloud_Connector_NCC.NeplanService.ExternalProject":
                    Value = Input;
                    break;
                case "System.String":
                    Value = (string)Input;
                    break;
                case "System.Int32":
                    Value = Convert.ToInt32(Input);
                    break;
                case "System.DateTime":
                    int[] DateVec = ((JArray)Input).ToObject<int[]>();
                    Value = new DateTime(DateVec[0], DateVec[1], DateVec[2], DateVec[3], DateVec[4], DateVec[5]);
                    break;
                default:
                    Value = null;
                    Console.WriteLine("Could not convert " + Name + " to datatype " + Type + ". There is no method for this datatype.");
                    break;
            }
        }
    }
    class ParameterDictionary : Dictionary<string, Parameter>
    {
        public void AddRequired(ParameterInfo parInfo)
        {
            Parameter par = new Parameter();
            par.Name = parInfo.Name;
            par.Type = parInfo.ParameterType.ToString();
            par.Reuired = true;
            this.Add(parInfo.Name, par);
        }
        public void AddUnused(string name)
        {
            Parameter par = new Parameter();
            par.Name = name;
            par.Reuired = false;
            this.Add(name,par);
        }
        public object[] GetRequiredValues()
        {
            List<object> d = new List<object>();
            foreach (Parameter par in this.Values)
            {
                if (par.Reuired)
                {
                    d.Add(par.Value);
                }
            }
            return d.ToArray();
        }
    }
}
