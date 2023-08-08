using System;
using PWMIS.DataMap.Entity;

namespace OQLTest
{
    internal class UserMessageEntity : EntityBase
    {
        public UserMessageEntity()
        {
            TableName = "UserMessage";
            PrimaryKeys.Add("ID");
        }

        public int ID
        {
            get => getProperty<int>("Id");
            set => setProperty("Id", value);
        }

        public int UserID
        {
            get => getProperty<int>("UserID");
            set => setProperty("UserID", value);
        }

        public string Message
        {
            get => getProperty<string>("Message");
            set => setProperty("Message", value, 50);
        }

        public DateTime SendTime
        {
            get => getProperty<DateTime>("SendTime");
            set => setProperty("SendTime", value);
        }
    }
}