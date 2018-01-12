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
    }
}
