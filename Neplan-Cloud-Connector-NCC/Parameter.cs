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
        public Parameter()
        {

        }
        public void SetValue(Object Input)
        {
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
}
