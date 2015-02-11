using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityTest
{
    public class UserDto:IUser
    {
        public int Age
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string LasttName
        {
            get;
            set;
        }

        public int UserID
        {
            get;
            set;
        }
    }
}
