using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBindExample
{
    class Person
    {
        private string fname;
        private string lname;
        private int age;
        private string job;
        private bool active;

        public Person(string fname, string lname, int age, string job, bool active)
        {
            this.fname = fname; this.lname = lname;
            this.age = age; this.job = job;
            this.active = active;
        }

        public String Fname { get { return fname; }  }
        public String Lname { get { return lname; } }
        public int Age { get { return age; } }
        public String Job { get { return job; } }

        public bool Active { get { return active; } }

        public String FullName  { get { return fname + " " + lname; } }
    }
}
