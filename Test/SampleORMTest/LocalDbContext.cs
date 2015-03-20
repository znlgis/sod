using PWMIS.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleORMTest
{
    /// <summary>
    /// 用来测试的本地SqlServer 数据库上下文类
    /// </summary>
    public class LocalDbContext : OracleDbContext  // SqlServerDbContext
    {
        public LocalDbContext()
            : base("local")
        {
            //local 是连接字符串名字
        }

        #region 父类抽象方法的实现

        protected override bool CheckAllTableExists()
        {
            //创建用户表
            CheckTableExists<User>();
            return true;
        }

        #endregion
    }
}
