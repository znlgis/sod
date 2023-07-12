*菜鸟*：“怎么使用EF（Entity Framework）框架啊？遇到麻烦了，救命！”

*Beginner*："How to use Entity Framework? SOS!"

*老鸟*：“试试SOD开发框架！”

*Senior men*："Try using the SOD Framework!"

一直使用EF并且老是遇到麻烦？何不解放自己并且试试SOD框架呢！
它是简单的，并且容易使用的，轻量级的框架。

Still using *EF* and get stuck? Why not release yourself and try *SOD*!
It is easy, and simple.

----

# 1，为什么需要SOD框架 --Why Need the SOD Framework?

__EF框架或大部分ORM框架的缺点就是SOD框架的优点，它拥有超过15年的项目应用历史，为你而生！__

__The disadvantage of Entity Framework or most ORM frameworks is the advantage of SOD framework,It has more than 15 years of project application history, born for you!__

*SOD* **不止是一个ORM框架，还包括 SQL-MAP,DataControls,但它却是一个非常轻量级的框架，也是一个企业级数据应用开发的解决方案。** 了解更多，看[这里](http://www.pwmis.com/sqlmap)

*SOD* _not only_ a ORM framework,include SQL-MAP,DataControls,detai ,but it is a light weight framework,and also it is an enterprise level data application development solution . see  [this page] (http://www.pwmis.com/sqlmap) 

<p align="center"><img width = '200' height ='200' 
src ="https://avatars3.githubusercontent.com/u/2637208?s=460&u=ec5ff1f40f8de3275506a2ffd41e23c1172f3df7&v=4"/>
<br/>
 Begin 2006-06-06
 </p>
 
 SOD框架特别适合于以下类型的企业项目：
 
 The SOD framework is particularly suitable for the following types of enterprise projects：  
 
 1. 对数据操作安全有严格要求的金融行业；--Financial industry with strict requirements for data operation security  
 2. 对数据访问速度、内存和CPU资源有苛刻要求的互联网行业；--Internet industry with stringent requirements for data access speed, memory and CPU resources
 2. 对需求常常变化，项目经常迭代，要求快速开发上线的项目；--Requirements often change, projects often iterate, requiring rapid development of online projects    
 3. 对稳定性要求高，需要长期维护的企业级应用如MIS、ERP、MES等行业；--Enterprise applications requiring high stability and long-term maintenance, such as MIS, ERP, MES and other industries    
 4. 需要低成本开发，人员技能偏低的中小型项目。 --Small and medium-sized projects requiring low-cost development and low personnel skills   
 

**SOD框架是少数仍然支持 .NET 2.0的框架**，当然，它也支持 .NET 3.x,.NET 4.x，以及.Net core 和马上到来的.NET 5 。

**The SOD framework is one of the few that still supports. Net 2.0.** Of course, it also supports. Net 3. X,. Net 4. X, as well as. Net core and. Net 5.

# 2,功能特性 --functional and features

SOD框架由PDF.NET框架发展而来，它包括以下功能：

The SOD framework contains the following functional features:

## 核心三大功能 --Main purpose（S，O，D）：

SQL-MAP  

    XML SQL config and Map DAL  --基于XML配置的SQL查询和数据访问层映射
    SQL Map Entity              --SQL语句映射为实体类

ORM  

    OQL(ORM Query Language)      --ORM查询语言：OQL 
    Data Container               --数据容器
    Entity Indexer               --实体类索引器访问
    Table Map route Query        --分表查询支持
    Micro ORM                    --微型ORM

Data Controls 

    Consistent Data Froms        --一致的数据窗体访问技术
    WebForm Data Controls        --Web窗体数据控件
    WinForm Data Controls        --Windows窗体数据控件

----

## 有用的功能组件：

    Hot Use Cache               --热缓存（缓存最常用的数据）
    Binary Serialization        --二进制序列化
    Query Log                   --查询日志
    Command Pipeline            --命令管道
    Distributed Identification  --分布式ID

## 企业级解决方案：

    MVVM (Web/WinForm)               --MVVM数据窗体
    Memory Database                  --内存数据库
    Transaction Log Data Replication --事务日志数据复制
    Data Synchronization             --数据同步
    Distributed transaction          --分布式事务
    OData Client                     --OData 客户端

## 工具：

    Integrated Development Tool     --集成开发工具，包括实体类生成、SQL-MAP代码自动生成和多种数据库访问工具。
    Nuget support                   --Nuget 支持
 

## 源码和社区：

    Code: https://github.com/znlgis/sod or https://gitee.com/znlgis/sod
    Home: [http://www.pwmis.com/sqlmap](http://www.pwmis.com/sqlmap)
    Blog: [https://www.cnblogs.com/bluedoctor](https://www.cnblogs.com/bluedoctor)
    QQ Group:18215717,154224970


Learning more,see [this page](https://www.cnblogs.com/bluedoctor/p/4306131.html). 

要了解更多，请看[这篇文章:.NET ORM 的 “SOD蜜”--零基础入门篇](https://www.cnblogs.com/bluedoctor/p/4306131.html)
或者参考框架作者编著的图书：**《[SOD框架企业级应用数据架构实战](http://www.pwmis.com/sod/)》**。

# 3,ORM简单示例 --use ORM Simple example

下面一个简单的SOD框架ORM使用的实例：
在开始工作之前，先建立一个控制台项目，然后在程序包管理控制台，添加SOD框架的Nuget 包：
 ```
Install-Package PDF.NET.SOD 
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
* SOD框架的实体类采用“动态元数据映射”，这些元数据包括映射的表名称、主外键、标识字段、属性映射的字段名称和字段类型、长度等。这些元数据都是可以在程序运行时进行修改的，因此它与Entity Framework等ORM框架的实体类映射方式有很大不同。这个特点使得实体类的定义和元数据映射可以在一个类代码中完成，并且不依赖于.NET特性声明。这种动态性使得SOD框架可以脱离繁琐的数据库表元数据映射过程，简化数据访问配置，并且能够轻松的支持“分表、分库”访问。
* 元数据的映射可以是“逻辑映射”，例如指定要映射外键字段，但数据库可以没有物理的外键字段，或者指定一个虚拟的主键。也可以不做任何元数据映射，这样实体类可以作为一个类似的“字典”对象来使用，或者用于UI层数据对象。
* 如果元数据全部采用默认映射，也可以使用接口类型动态创建实体类：
```c#
IUser user = EntityBuilder.CreateEntity<IUser>(); 
```

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
providerName 是SOD框架的数据访问提供程序，PWMIS.Core.dll内置的可选简略名称有：Access | SqlServer | Oracle | SqlCe | OleDb | Odbc

在其它提供程序中，SOD框架提供了对 MySQL/Oracle/PostgreSQL/SQLite 等常见数据库的支持（扩展程序集），**只要数据库提供了ADO.Net驱动程序，那么SOD框架经过简单包装即可保证支持。**

如果是其它的**扩展程序集**，那么providerName应该写成下面的形式：

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

如果你只需进行“单表查询”，那么用泛型OQL是一个更好的选择。例如下面的示例，分页查询用户表的数据，并能够在第一次查询的时候自动探测到该查询未分页的时候的记录总数，然后在后续查询的时候传递该记录总数，从而实现一个高效的查询：
```c#
       void Test3(int pageNum, int pageSize)
        {
            var goql = OQL.From<User>()
                .Select()
                .OrderBy((o, u) => o.Asc(u.ID));
            // .Limit(pageSize, pageNum, true);
            if (this.txtRecNumber.Text == "0" || this.txtRecNumber.Text == "")
            {
                goql.Limit(pageSize, pageNum, true);//第三个参数为true，表示在本次查询中自动探查符合查询条件的记录总数
            }
            else
            {
                int allCount = int.Parse(this.txtRecNumber.Text);
                goql.Limit(pageSize, pageNum, allCount);//记录总数，以便准确分页。
            }
            var list = goql.ToList();
            // 单行泛型OQL方式：
            // var list = OQL.From<User>()..Select().OrderBy((o, u) => o.Asc(u.ID)).Limit(pageSize, pageNum, true) ;   
            
            this.dataGridView1.DataSource = list;
            int recCount = goql.AllCount;

            this.txtRecNumber.Text = recCount.ToString();
        }

```

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
 
 __附注：__
  OQL也可以支持复杂的多表联合查询，如下面的例子：
 ```c#
OQL q=OQL.From(entity1)
         .Join(entity2).On(entity1.PK,entity2.FK)
         //.Select(entity1.Field1,entity2.Field2) //不再需要指定查询的属性
         .Select()
      .End;
EntityContainer ec=new EntityContainer(q);
var list=ec.MapToList(()=>
         {
            return new {
                         Property1=entity1.Field1, 
                         Property2=entity2.Field2 
                       };
         });

foreache(var item in list)
{
    Console.WriteLine("Property1={0},Property2={1}",item.Property1,item.Property2);
}
 
```
相比EF等ORM框架，SOD有更方便的批量插入和更新方式，都能通过OQL完成。比如OQL 的 Update 方法更新指定的实体类属性数据到数据，Where 方法 的条件表达式表示更新指定 RoleID 的所有数据，如果更新条件对应的数据是多条的，那么 即可实现“批量更新”的效果。 
 ```c#
void TestUpdate() {  
  Users user = new Users() {     
 AddTime=DateTime.Now.AddDays(-1),       
 Authority="Read",       
 NickName = "菜鸟"  
 };  
 OQL q = OQL.From(user)
          .Update(user.AddTime, user.Authority, user.NickName)
          .Where(cmp => cmp.Property(user.RoleID) == 100)
      .END; 
 
 Console.WriteLine("OQL update:\r\n{0}\r\n",q); 
 Console.WriteLine(q.PrintParameterInfo()); 
} 
```
程序输出：
 ```text
OQL update: UPDATE [LT_Users]  SET
      [AddTime] = @P0,
      [Authority] = @P1,
      [NickName] = @P2 
     WHERE  [RoleID] = @P3 
 
--------OQL Parameters information----------
  have 4 parameter,detail: 
   @P0=2013/7/28 22:15:38      Type:DateTime
    @P1=Read      Type:String
    @P2=菜鸟      Type:String
    @P3=100      Type:Int32  
------------------End------------------------ 
```
有关OQL的高级用法和详细示例，请参考这篇文章和它的系列链接：[ORM查询语言（OQL）简介--实例篇](https://www.cnblogs.com/bluedoctor/archive/2013/04/01/2992981.html)

# 4,其它
Thank you for your donation
欢迎您捐助本项目，捐赠地址：[框架官网](http://www.pwmis.com/sqlmap )

