using PWMIS.DataMap.Entity;
using System;

namespace Sod.Pwmis.com.Repository.Entities
{
    public class DbVersion : EntityBase
    {
        public DbVersion()
        {
            TableName = "DbVersion";
        }

        public int Version
        {
            get { return getProperty<int>(nameof(Version)); }
            set { setProperty(nameof(Version), value); }
        }

        public string BizName
        {
            get { return getProperty<string>(nameof(BizName)); }
            set { setProperty(nameof(BizName), value, 50); }
        }

        public DateTime UpdateTime
        {
            get { return getProperty<DateTime>(nameof(UpdateTime)); }
            set { setProperty(nameof(UpdateTime), value); }
        }
    }
}
