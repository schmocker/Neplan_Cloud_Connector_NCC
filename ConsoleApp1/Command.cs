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
        Webservice webservice;
        ExternalProject project;
        string analysisRefenceID = Guid.NewGuid().ToString();
        string analysisModule = "";
        string calcNameID = "";
        string analysisMethode = "";
        string conditions = "";
        string analysisLoadOptionXML = "";
        string analysisType = "";
        DateTime simulationDateTime = new DateTime(); //  string to date - pending
        int networkTypeGroup = 1; // Convert.ToInt32(pars["networkTypeGroup"]); //  string to int - done
        string networkTypeGroupID = "";

        public string result;
        public string error;

        public Command(Webservice webservice, string fnc, Dictionary<string, object> pars)
        {
            this.webservice = webservice;
            this.project = webservice.ext;

            switch (fnc)
            {
                case "AnalyseVariant":
                    AnalysisReturnInfo output_AnalyseVariant = webservice.nepService.AnalyseVariant(project, analysisRefenceID, analysisModule, calcNameID, analysisMethode, conditions, analysisLoadOptionXML);
                    break;
                case "GetListResultSummary":
                    string[] output_GetListResultSummary = webservice.nepService.GetListResultSummary(project,analysisType,simulationDateTime,networkTypeGroup,networkTypeGroupID);
                    result = output_GetListResultSummary[0];
                    break;
                default:
                    result = "invalid function name";
                    break;
            }
        }
    }
}
