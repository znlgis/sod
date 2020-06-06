*菜鸟*：“怎么使用EF（Entity Framework）框架啊？遇到麻烦了，救命！”

*Beginner*："How to use Entity Framework? SOS!"

*老鸟*：“试试SOD开发框架！”

*Senior men*："Try using the SOD Framework!"

一直使用EF并且老是遇到麻烦？何不解放自己并且试试SOD框架呢！
它是简单的，并且容易使用的，轻量级的框架。

Still using *EF* and get stuck? Why not release yourself and try *SOD*!
It is easy, and simple.

------
EF框架 或大部分ORM框架的缺点就是SOD框架的优点，它为此而生！

The disadvantage of Entity Framework or most ORM frameworks is the advantage of SOD framework,which is born for this!

*SOD* **不仅仅是一个ORM框架，还包括 SQL-MAP,DataControls,但它却是一个非常轻量级的框架。** 了解更多，看[这里](http://www.pwmis.com/sqlmap)

*SOD* _not only_ a ORM framework,include SQL-MAP,DataControls,detai ,but it is a light weight framework . see  [this page] (http://www.pwmis.com/sqlmap) 

SOD框架由PDF.NET框架发展而来，它包括以下功能：
The SOD framework contains the following functional features:

--核心三大功能（S，O，D）：

SQL-MAP  

    XML SQL config and Map DAL  
    SQL Map Entity

ORM  

    OQL(ORM Query Language)  
    Data Container  
    Entity Indexer  
    Table Map route Query(分表查询）

 

Data Controls (Web/WinForm)

 

--有用的功能组件：

    Hot Use Cache
    Binary Serialization
    Query Log
    Command Pipeline
    Distributed Identification

 

--企业级解决方案：

    MVVM (Web/WinForm)，参考链接
    Memory Database
    Transaction Log Data Replication
    Data Synchronization
    Distributed transaction
    OData Client

--工具：

    Integrated Development Tool

 

--源码和社区：

    Code: https://github.com/znlgis/sod or https://gitee.com/znlgis/sod
    Home: http://www.pwmis.com/sqlmap
    Blog: https://www.cnblogs.com/bluedoctor
    QQ Group:18215717,154224970


Learning more,see [this page](https://www.cnblogs.com/bluedoctor/p/4306131.html). 

要了解更多，请看[这篇文章:.NET ORM 的 “SOD蜜”--零基础入门篇](https://www.cnblogs.com/bluedoctor/p/4306131.html)
或者参考框架作者编著的图书：**《SOD框架企业级应用数据架构实战》**。

----

下面一个简单的SOD框架ORM使用的实例：
在开始工作之前，先建立一个控制台项目，然后在程序包管理控制台，添加SOD框架的Nuget 包：
 ```
Install-Package PDF.NET 
 ```
这样即可获取到最新的SOD框架包并且添加引用，然后，就可以开始下面的工作了。
已经建立好的当前Demo程序下载，[看这里](http://pwmis.codeplex.com/downloads/get/1522232)
 
* 1，首先建立一个实体类： 
 ```c#
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
```
* SOD框架的实体类采用“动态元数据映射”，这些元数据包括映射的表名称、主外键、标识字段、属性映射的字段名称和字段类型、长度等。这些元数据都是可以在程序运行时进行修改的，因此它与Entity Framework等ORM框架的实体类映射方式有很大不同。这个特点使得实体类的定义和元数据映射可以在一个类代码中完成，并且不依赖于.NET特性申明。这种动态性使得SOD框架可以脱离繁琐的数据库表元数据映射过程，简化数据访问配置，并且能够轻松的支持“分表、分库”访问。
* 元数据的映射可以是“逻辑映射”，例如指定要映射外键字段，但数据库可以没有物理的外键字段，或者指定一个虚拟的主键。也可以不做任何元数据映射，这样实体类可以作为一个类似的“字典”对象来使用，或者用于UI层数据对象。

* 2，然后建立一个 DbContext: 
```c#
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
 
```
*注意：这一步骤只是为了 code first ，如果表事先已经存在，可以略去本步骤，当然下面的代码会有所调整。*

* 3，修改下App.config 文件的连接配置： 

```xml
<?xml version="1.0" encoding="utf-8" ?> 
<configuration> 
<connectionStrings> 
   <add name="local" connectionString="Data Source=.;database=TestDB; Integrated Security=True" providerName="SqlServer"/> 
</connectionStrings> 
</configuration> 
```
providerName 是SOD框架提供的驱动程序，可选的内容有：Access | SqlServer | Oracle | SqlCe | OleDb | Odbc

如果是其它的扩展程序集，那么providerName应该写成下面的形式：

providerName="<提供程序类全名称>,<提供程序类所在程序集>"

比如使用SOD封装过的Oracle官方的ADO.NET提供程序类：
```xml
    providerName="PWMIS.DataProvider.Data.OracleDataAccess.Oracle,PWMIS.OracleClient"
```
*注意：提供程序程序集必须放到 跟PWMIS.Core.dll 同一个目录下，且是同一个兼容版本。*
有关数据库连接配置的详细内容，请参考作者图书：《SOD框架企业级应用架构实战》。

* 4，然后，像下面这样使用，即可自动创建数据库和表，并且添加一条初始数据：
```c#
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
  
```

当然插入数据的方式很多，具体请看本文提供的源码下载。 

*  5，最后，像下面这样使用查询即可：

```c#
 
           User user = new  User() { Name = "zhang san" }; 
           OQL q = OQL.From(user) 
             .Select() 
             .Where(user.Name) 
           .END; 
  
           PrintOQL(q); 
           List<User> users = EntityQuery<User>.QueryList(q); 

```

 这种方式适合简单的相等条件查询，如果需要复杂的条件，可以修改成下面这个样子：

```c#

           //示例：采用操作符重载写比较条件 
           User user = new  User(); 
           OQL q = OQL.From(user) 
                         .Select() 
                         .Where(cmp => cmp.Property(user.Name) == "zhang san") 
                       .END; 
         PrintOQL(q); 
           //使用扩展方法 using PWMIS.Core.Extensions; 
           List<User> users = q.ToList<User>();

```

示例代码中的  可以修改成 >,<,like 等SQL支持的比较符号。 
 如果需要更多条件，可以使用 &表示SQL的AND，| 表示 SQL的OR 逻辑关系，比如： 

```c#

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
```

实际上，框架提供了至少8种查询方式，详细内容，请看[.NET ORM 的 “SOD蜜”--零基础入门篇](https://www.cnblogs.com/bluedoctor/p/4306131.html)

  附注：

```c#

       private static void PrintOQL(OQL q)
       { 
           Console.WriteLine("OQL to SQL:\r\n{0}", q.ToString()); 
           Console.WriteLine("SQL Parameters:\r\n{0}", q.PrintParameterInfo()); 
       } 

```

 该方法可以打印OQL的SQL和参数信息，为你调试代码带来方便。 
 
----
 
 这样，一个简单的ORM使用实例就做好了。上面这段ORM例子，不仅仅适用于Oracle,使用在其它数据库都是可以得，只需要修改 连接字符串配置的 providerName和 connectionString 即可。

 详细可以参考  [Oracle 免费的数据库--Database 快捷版 11g 安装使用与"SOD框架"对Oracle的CodeFirst支持] (http://www.cnblogs.com/bluedoctor/p/4359878.html)
 
   
