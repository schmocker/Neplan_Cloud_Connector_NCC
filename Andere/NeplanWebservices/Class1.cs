using System;

namespace Class1
{
	public Class1()
    {
        ¨public void Main(string[] args)
        {
            Webservice webservice = new Webservice();
            Program example = new Program();
            if (webservice.nepService != null && webservice.ext != null)      //Checks if the interface was created
            {
                example.RunLoadFlow(webservice);                                //runs a load flow
                example.GetResultsLoadFlow(webservice);                         //gets results
                example.OpenSwitch(webservice);                                 //open a switch                     
                example.ChangePsetting(webservice);                             //sets P of a 1-port element                
                example.RepeatLoadFLowandResults(webservice);                   //repeats the process
                example.RestoreOriginalValues(webservice);
                example.RunSwitchingOptimisation(webservice);
                example.GetResultsSwitchingOptimization(webservice);
            }
            webservice.CloseWebservice();
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }
    }
}
