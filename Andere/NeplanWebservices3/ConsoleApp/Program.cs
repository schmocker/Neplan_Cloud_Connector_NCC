using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ClassLibrary1.Class3 a = new ClassLibrary1.Class3();
            a.open();
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }
    }
}
