using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neplan_Cloud_Connector_NCC
{
    class Command
    {
        public string MethodName;
        public Dictionary<string, object> Inputs;
        public object Outputs;
        public bool Error = false;
        public bool Received = false;
        public bool Done = false;

        public void DecodeJson(string jsonString)
        {
            Dictionary<string, object> message = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

            MethodName = message["fnc"].ToString();
            Inputs = JsonConvert.DeserializeObject<Dictionary<string, object>>(message["input"].ToString());

            Inputs.Add("analysisRefenceID", Guid.NewGuid().ToString());

            // Convert Int Parameters
            string[] ints = { "networkTypeGroup", "portNr" };
            foreach (var item in ints)
            {
                if (Inputs.ContainsKey(item)) ConvertInt(item);
            }

            // Convert DateTime Parameters
            string[] dateTimes = { "simulationDateTime" };
            foreach (var item in dateTimes)
            {
                if (Inputs.ContainsKey(item)) ConvertDateTime(item);
            }
        }

        private void ConvertDateTime(string parName)
        {
            int[] DateVec = ((JArray)Inputs[parName]).ToObject<int[]>();
            Inputs[parName] = new DateTime(DateVec[0], DateVec[1], DateVec[2], DateVec[3], DateVec[4], DateVec[5]);
        }
        private void ConvertInt(string parName)
        {
            Inputs[parName] = Convert.ToInt32(Inputs[parName]);
        }
    }
}
