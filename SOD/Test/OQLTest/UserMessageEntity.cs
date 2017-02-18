using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OQLTest
{
    class UserMessageEntity:EntityBase
    {
        public UserMessageEntity()
        {
            TableName = "UserMessage";
            PrimaryKeys.Add("ID");
        }

        public int ID
        {
            get { return getProperty<int>("Id"); }
            set { setProperty("Id", value); }
        }

        public int UserID
        {
            get { return getProperty<int>("UserID"); }
            set { setProperty("UserID", value); }
        }

        public string Message
        {
            get { return getProperty<string>("Message"); }
            set { setProperty("Message", value, 50); }
        }

        public DateTime SendTime
        {
            get { return getProperty<DateTime>("SendTime"); }
            set { setProperty("SendTime", value); }
        }
    }
}
