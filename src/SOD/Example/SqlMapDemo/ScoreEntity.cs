using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlMapDemo
{
    public class ScoreEntity:EntityBase
    {
        public ScoreEntity()
        {
            TableName = "Score";
            EntityMap = PWMIS.Common.EntityMapType.Table;
            PrimaryKeys.Add("stuID");
            IdentityName = "stuID";
        }

        public int StudentID 
        { 
            get { return getProperty<int>("stuID"); }
            set { setProperty("stuID", value); }
        }

        public string CategoryName
        {
            get { return getProperty<string>("category"); }
            set { setProperty("category", value,50); }
        }

        public int Score
        {
            get { return getProperty<int>("score"); }
            set { setProperty("score", value); }
        }
    }
}
