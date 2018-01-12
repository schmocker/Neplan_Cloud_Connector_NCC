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
        public string Type;
        public object Value;

        public Parameter(ParameterInfo param)
        {
            Name = param.Name;
            Type = param.ParameterType.ToString();
        }
        public void setValue(object input)
        {
            switch (Type)
            {
                case "Neplan_Cloud_Connector_NCC.NeplanService.ExternalProject":
                    break;
                case "System.String":
                    Value = (string)input;
                    break;
                case "System.Int32":
                    Value = Convert.ToInt32(input);
                    break;
                case "System.DateTime":
                    int[] DateVec = ((JArray)input).ToObject<int[]>();
                    Value = new DateTime(DateVec[0], DateVec[1], DateVec[2], DateVec[3], DateVec[4], DateVec[5]);
                    break;
                default:
                    Value = null;
                    Console.WriteLine("Could not convert " + Name + " to datatype " + Type + ". There is no method for this datatype.");
                    break;
            }
        }
    }
}
