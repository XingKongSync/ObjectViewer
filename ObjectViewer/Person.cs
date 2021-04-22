using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectViewer
{
    public class Person
    {
        public string Name { get; set; }
        public int Age;
        public Sex Sex { get; set; }

        public Person Father { get; set; }
    }

    public enum Sex
    {
        Male,
        Female
    }
}
