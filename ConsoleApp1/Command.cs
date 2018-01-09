using NeplanMqttService.NeplanService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeplanMqttService
{
    class Command
    {
        public Dictionary<string, object> p = new Dictionary<string, object>();

        public object result;
        public string error;

        public Command(Webservice webservice, string fnc, Dictionary<string, object> par)
        {
            p = par;
            // 
            //p.Add("webservice", webservice);
            p.Add("project", webservice.ext);
            p.Add("analysisRefenceID", Guid.NewGuid().ToString());
            //p.Add("analysisModule", null);
            //p.Add("calcNameID", null);
            //p.Add("analysisMethode", null);
            //p.Add("conditions", null);
            //p.Add("analysisLoadOptionXML", null);
            //p.Add("analysisType", null);
            p.Add("simulationDateTime", new DateTime());
            //p.Add("networkTypeGroup", 0);
            //p.Add("networkTypeGroupID", "");

            

            // Funktion ausführen
            switch (fnc)
            {
                case "AnalyseVariant":
                    result = webservice.nepService.AnalyseVariant(
                        (ExternalProject)p["project"],
                        (string)p["analysisRefenceID"],
                        (string)p["analysisModule"],
                        (string)p["calcNameID"],
                        (string)p["analysisMethode"],
                        (string)p["conditions"],
                        (string)p["analysisLoadOptionXML"]);
                    break;
                case "GetListResultSummary":
                    result = webservice.nepService.GetListResultSummary(
                        (ExternalProject)p["project"],
                        (string)p["analysisType"],
                        (DateTime)p["simulationDateTime"],
                        Convert.ToInt32(p["networkTypeGroup"]),
                        (string)p["networkTypeGroupID"]);
                    break;
                default:
                    result = "invalid function name";
                    break;
            }
        }
    }
}
