*Beginner*：How to use Entity Framework? SOS!
*Senior men*：Try using the SOD Framework!

*菜鸟*：怎么使用EF框架啊？遇到麻烦了，救命！
*老鸟*：试试SOD开发框架！

Still using *EF* and get stuck? Why not release yourself and try *SOD*!
It is easy, and simple.

一直使用EF并且老是遇到麻烦？何不解放自己并且试试SOD框架呢！
它是简单的，并且容易使用的，轻量级的框架。

*SOD* _not only_ a ORM framework,include SQL-MAP,DataControls,detai ,but it is a light weight framework . see  [url:this page. |http://www.pwmis.com/sqlmap] 

*SOD* 不仅仅是一个ORM框架，还包括 SQL-MAP,DataControls,但它却是一个非常轻量级的框架。了解更多，看[url:这里。 |http://www.pwmis.com/sqlmap]


Learning more,see [url:this page. |https://pwmis.codeplex.com/wikipage?title=Framework%20details&version=2] 

要了解更多，请看[url:这篇文章 |https://pwmis.codeplex.com/wikipage?title=Framework%20details&version=2]

----

下面一个简单的SOD框架ORM使用的实例：
在开始工作之前，先建立一个控制台项目，然后在程序包管理控制台，添加SOD框架的Nuget 包：
{{
Install-Package PDF.NET 
}}
这样即可获取到最新的SOD框架包并且添加引用，然后，就可以开始下面的工作了。
已经建立好的当前Demo程序下载，[url:看这里 |http://pwmis.codeplex.com/downloads/get/1522232]
 
* 1，首先建立一个实体类： 
  {code:c#}
   public  class User : EntityBase 
   { 
       public User() 
       { 
           TableName = "Tb_User"; 
           IdentityName = "UserID"; 
           PrimaryKeys.Add("UserID"); 
      } 
  
       public  int ID 
       { 
           get { return getProperty<int>("UserID"); } 
           set { setProperty("UserID", value); } 
       } 
  
       public  string Name 
       { 
           get { return getProperty<string>("Name"); } 
           set { setProperty("Name", value, 50); } 
       } 
  
       public  string Pwd 
       { 
           get { return getProperty<string>("Pwd"); } 
           set { setProperty("Pwd", value, 50); } 
       } 
    } 
{code:c#}

* 2，然后建立一个 DbContext: 
{code:c#}
  class  LocalDbContext:DbContext 
   { 
       public LocalDbContext() 
           : base("local") 
       { 
           //local 是连接字符串名字 
       } 
  
       protected  override bool CheckAllTableExists() 
       { 
           //创建用户表 
           CheckTableExists<User>(); 
           return  true; 
       } 
   }
 
{code:c#}

* 3，修改下App.config 文件的连接配置： 

{code:xml}
<?xml version="1.0" encoding="utf-8" ?> 
<configuration> 
<connectionStrings> 
   <add name="local" connectionString="Data Source=.;database=TestDB; Integrated Security=True" providerName="SqlServer"/> 
</connectionStrings> 
</configuration> 
{code:xml}
providerName 是SOD框架提供的驱动程序，可选的内容有：

# Access
# SqlServer
# Oracle
# SqlCe
# OleDb
# Odbc

如果是其它的扩展程序集，那么providerName应该写成下面的形式：
{{
    providerName="PWMIS.DataProvider.Data.OracleDataAccess.Oracle,PWMIS.OracleClient"
}}
其中，“,”号前是驱动程序类型的全名称， “,”号后是驱动程序所在的程序集名称，要求该程序集必须放到 跟PWMIS.Core.dll 同一个目录下，且是同一个兼容版本。
有关数据库连接配置的详细内容，请参考：[2.2.3 扩展数据访问类配置]

* 4，然后，像下面这样使用，即可自动创建数据库和表，并且添加一条初始数据：
{code:c#}
           //创建数据库和表
           LocalDbContext context=new  LocalDbContext();
          //重新指定主键，删除旧的测试数据 
           User oldUser = new  User(); 
           oldUser.PrimaryKeys.Clear(); 
           oldUser.PrimaryKeys.Add("Name"); 
           oldUser["Name"] = "zhang san"; //索引器使用 
           int count= context.Remove<User>(oldUser); 
  
           User zhang_san = new  User() { Name = "zhang san", Pwd = "123" }; 
           count = context.Add<User>(zhang_san);//采用 DbContext 方式插入数据 
  
{code:c#}

当然插入数据的方式很多，具体请看本文提供的源码下载。 

*  5，最后，像下面这样使用查询即可：

{code:c#}
 
           User user = new  User() { Name = "zhang san" }; 
           OQL q = OQL.From(user) 
             .Select() 
             .Where(user.Name) 
           .END; 
  
           PrintOQL(q); 
           List<User> users = EntityQuery<User>.QueryList(q); 

{code:c#}

 这种方式适合简单的相等条件查询，如果需要复杂的条件，可以修改成下面这个样子：

{code:c#}

           //示例：采用操作符重载写比较条件 
           User user = new  User(); 
           OQL q = OQL.From(user) 
                         .Select() 
                         .Where(cmp => cmp.Property(user.Name) == "zhang san") 
                       .END; 
         PrintOQL(q); 
           //使用扩展方法 using PWMIS.Core.Extensions; 
           List<User> users = q.ToList<User>();

{code:c#}

示例代码中的  可以修改成 >,<,like 等SQL支持的比较符号。 
 如果需要更多条件，可以使用 &表示SQL的AND，| 表示 SQL的OR 逻辑关系，比如： 

{code:c#}

           //示例：采用操作符重载写比较条件 
           User user = new  User(); 
           OQL q = OQL.From(user) 
                         .Select() 
                         .Where(cmp => cmp.Property(user.Name) == "zhang san" 
                         & cmp.Comparer(user.Pwd ,"=","123") ) 
                       .END; 
         PrintOQL(q); 
           //使用扩展方法 using PWMIS.Core.Extensions; 
           List<User> users = q.ToList<User>(); 

{code:c#}

实际上，框架提供了至少6种查询方式，详细内容，请看Demo程序下载，[url:看这里 |http://pwmis.codeplex.com/downloads/get/1522232]

  附注：

{code:c#}

       private static void PrintOQL(OQL q)
       { 
           Console.WriteLine("OQL to SQL:\r\n{0}", q.ToString()); 
           Console.WriteLine("SQL Parameters:\r\n{0}", q.PrintParameterInfo()); 
       } 

{code:c#}

 该方法可以打印OQL的SQL和参数信息，为你调试代码带来方便。 
 
----
 
 这样，一个简单的ORM使用实例就做好了。上面这段ORM例子，不仅仅适用于Oracle,使用在其它数据库都是可以得，只需要修改 连接字符串配置的 providerName和 connectionString 即可。

 详细可以参考  [url: Oracle 免费的数据库--Database 快捷版 11g 安装使用与"SOD框架"对Oracle的CodeFirst支持 |http://www.cnblogs.com/bluedoctor/p/4359878.html]
 
   
