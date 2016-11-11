using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.Core.Extensions;

namespace SampleORMTest
{
    class UserLoginService
    {
        /// <summary>
        /// 使用用户对象来登录，OQL最简单最常见的使用方式
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Login(User user)
        {
            OQL q = OQL.From(user)
                .Select()
                .Where(user.Name, user.Pwd) //以用户名和密码来验证登录
            .END;

            User dbUser =q.ToEntity<User>();//ToEntity，OQL扩展方法 
            return dbUser != null; //查询到用户实体类，表示登录成功
        }

        /// <summary>
        /// 使用用户对象来登录，但是使用 OQLCompare 对象的 EqualValue 相等比较方式 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Login1(User user)
        {
            OQL q = OQL.From(user)
                 .Select(user.ID) //仅查询一个属性字段 ID
                 .Where(cmp => cmp.EqualValue(user.Name) & cmp.EqualValue(user.Pwd))
              .END;

            User dbUser = EntityQuery<User>.QueryObject(q);
            return dbUser != null; //查询到用户实体类，表示登录成功
        }

        /// <summary>
        /// 使用用户名密码参数来登录，采用 EntityQuery 泛型查询方法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool Login2(string name, string pwd)
        {
            User user = new User()
            {
                Name = name,
                Pwd = pwd
            };

            OQL q = OQL.From(user)
                .Select(user.ID)
                .Where(user.Name, user.Pwd)
            .END;
            User dbUser = EntityQuery<User>.QueryObject(q);

            return dbUser != null; //查询到用户实体类，表示登录成功
        }

        /// <summary>
        /// 使用用户名密码参数来登录，使用早期的实例化OQL对象的方式，并使用OQLConditon 对象为查询条件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool Login3(string name, string pwd)
        {
            User user = new User();
            OQL q = new OQL(user);
            q.Select(user.ID).Where(q.Condition.AND(user.Name, "=", name).AND(user.Pwd, "=", pwd));

            User dbUser = EntityQuery<User>.QueryObject(q);
            return dbUser != null; //查询到用户实体类，表示登录成功
        }

        /// <summary>
        /// 使用用户名密码参数来登录，并使用操作符重载的查询条件比较方式
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool Login4(string name, string pwd)
        {
            User user = new User();

            OQL q = OQL.From(user)
                  .Select()
                  .Where( cmp => cmp.Property(user.Name) == name 
                               & cmp.Property(user.Pwd)  == pwd  )
               .END;

            User dbUser = EntityQuery<User>.QueryObject(q);
            return dbUser != null; //查询到用户实体类，表示登录成功
        }

        /// <summary>
        /// 使用用户名密码参数来登录，使用泛型OQL查询（GOQL），对于单实体类查询最简单的使用方式。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool Login5(string name, string pwd)
        {
            User dbUser = OQL.From<User>()
                 .Select()
                 .Where((cmp, user) => cmp.Property(user.Name) == name 
                                     & cmp.Property(user.Pwd)  == pwd  )
            .END
            .ToObject();

            return dbUser != null; //查询到用户实体类，表示登录成功
        }

        /// <summary>
        /// 使用用户名密码参数来登录，但是根据实体类的主键来填充实体类并判断是否成功。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool Login6(string name, string pwd)
        {
            User user = new User();
            user.PrimaryKeys.Clear();
            user.PrimaryKeys.Add("Name");
            user.PrimaryKeys.Add("Pwd");

            user.Name = name;
            user.Pwd = pwd;
            bool result= EntityQuery<User>.Fill(user);//静态方法，使用默认的连接对象
            return result;
        }

        /// <summary>
        /// 模糊查询用户，返回用户列表，使用OQLCompare 委托
        /// </summary>
        /// <param name="likeName">要匹配的用户名</param>
        /// <returns>用户列表</returns>
        public List<User> FuzzyQueryUser(string likeName)
        {
            User user = new User();
            OQL q = OQL.From(user)
              .Select()
              .Where(cmp => cmp.Comparer(user.Name, "like", likeName+"%") )
              .OrderBy (user.ID )
            .END;
            q.Limit(10,5);

            List<User> users = EntityQuery<User>.QueryList(q);
            return users;
        }
    }
}
