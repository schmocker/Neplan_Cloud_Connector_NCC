using System;
using System.Diagnostics;
using System.IO;

namespace Neplan_Cloud_Connector_NCC
{
    class Program
    {
        // starting point of the programm due to the method Main()
        static void Main(string[] args)
        {
            // change the name of the console title
            Console.Title = "Neplan Cloud Connector (NCC)";

            // kill all running copies of this application,
            // so that this program is unique
            string procName = Process.GetCurrentProcess().ProcessName;
            foreach (Process proc in Process.GetProcessesByName(procName))
                if (proc.Id != Process.GetCurrentProcess().Id)
                    proc.Kill();

            // get the path to the properties file. The path should
            // be saved in a txt-file one folder above where this program
            // is saved.

            string pathFilePath = 
                Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
                + @"\path.txt";

            string[] path = File.ReadAllLines(pathFilePath);

            // create new instance of class Controller
            Controller controller = new Controller(path[0]);
        }
    }
}
