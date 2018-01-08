using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Class3
    {
        public ch.neplan.demo.NeplanService nps;

        public Class3()
        {
            nps = new ch.neplan.demo.NeplanService();
            nps.Url = "2212dsff";
            
        }

        public void open()
        {
            
        }
    }
}
