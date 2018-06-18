using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCC;
using System.IO;


namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // declare variables
            string ws_adress = "https://demo.neplan.ch/NEPLAN360_Demo/Services/External/NeplanService.svc";
            string username = "christoph.hunziker@fhnw.ch";
            string password = "nep360FH2017";
            string project_name = "NCC";

            // instanciate and open webservice
            WebService ws = new WebService(ws_adress, username, password);
            ws.Open();

            // get the project
            ExternalProject project = ws.GetProject(project_name, null, null, null);

            // stream
            string path = @"C:\Users\tobias.schmocker\Google Drive\Development\FHNW - NCC\new\NCC\Test\A.nepmeas";

            bool x = ws.UploadXML(project, path, "Messdaten3");
            

            RunTimeSimulation(ws, project);

            // close  webservice
            ws.Close();
        }

        static internal void RunTimeSimulation(WebService ws, ExternalProject project)
        {
            bool isOk;
            var from = "2018-04-23T00:00:00";
            var to = "2018-04-23T00:45:00";

            isOk = ws.SetCalcParameterAttribute(project, "LoadFlowTimeSimulation", "FromDateTime", from);
            isOk = ws.SetCalcParameterAttribute(project, "LoadFlowTimeSimulation", "ToDateTime", to);
            isOk = ws.SetCalcParameterAttribute(project, "LoadFlowTimeSimulation", "IncrementTime", "30");
            isOk = ws.SetCalcParameterAttribute(project, "LoadFlowTimeSimulation", "CalcMethod", "1"); //0: One Load Flow, 1: Time Simulation

            var analysis = ws.AnalyseVariant(project, Guid.NewGuid().ToString(), "LoadFlowTimeSimulation", "Default", "", "", "");

            //Here the save results needs to be activated (you can do it either from the Gui or like I set other time simulation parameters before)
            var res = ws.GetAllElementResults(project, "LoadFlowTimeSimulation");

            //you need to define this measurement definition and select it in the timesimulation dialog
            double[] meas = new double[2] { 10, 10 };   //KW, Kvar
            var output = ws.InsertMeasurement(project, "Measurements2018", "Verbraucher A", 0, DateTime.Parse("2018-01-01T00:00:00"), 2, meas, 0);
            Console.Write(output);
        }
    }
}
