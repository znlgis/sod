using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserSystem;

namespace UserSystem
{
    public partial class User
    {

        public int ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public bool IsLeaf
        {
            get;
            set;
        }
       
    }
}
