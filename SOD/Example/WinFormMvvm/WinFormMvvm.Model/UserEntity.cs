using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormMvvm.Model
{
   public class UserEntity:EntityBase
    {
       public UserEntity()
       {
           TableName = "Tb_User";
           PrimaryKeys.Add("UserID");
       }
        public int ID {
            get { return getProperty<int>("UserID"); }
            set { setProperty("UserID", value); }
        }

        public string Name
        {
            get { return getProperty<string>("UserName"); }
            set { setProperty("UserName", value); }
        }

    }
}
