namespace Siren.Net.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WebApiContrib.Formatting.Siren.Client;

    internal class TestPerson : SirenDocument
    {       

        public string Name
        {
            get { return (string) Properties["name"]; }
            set { Properties["name"] = value; }
        }

        public int Age
        {
            get { return (int)Convert.ChangeType(Properties["age"], typeof(int)); }
            set { Properties["age"] = value; }
        }

    }
}
