using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neplan_Cloud_Connector_NCC
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (Process proc in Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName))
                if (proc.Id != Process.GetCurrentProcess().Id)
                    proc.Kill();

            string[] path = System.IO.File.ReadAllLines(@"..\path.txt");
            Controller controller = new Controller(path[0]);
        }
    }
}
