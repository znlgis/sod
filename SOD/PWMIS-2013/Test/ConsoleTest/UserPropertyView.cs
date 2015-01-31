
/* 
 本类由PWMIS 实体类生成工具(Ver 4.1)自动生成
 http://www.pwmis.com/sqlmap
 使用前请先在项目工程中引用 PWMIS.Core.dll
 2012/11/16 19:21:38
*/

using System;
using PWMIS.Common;
using PWMIS.DataMap.Entity;

namespace ConsoleTest 
{
  [Serializable()]
  public partial class UserPropertyView : EntityBase
  {
    public UserPropertyView()
    {
            TableName = "LocalDB.用户属性视图";
            EntityMap=EntityMapType.SqlMap;
            //IdentityName = "标识字段名";
    IdentityName="UID";

            //PrimaryKeys.Add("主键字段名");
    PrimaryKeys.Add("UID");

            
    }


      protected override void SetFieldNames()
      {
           PropertyNames = new string[] { "UID","Name","PropertyName","PropertyValue" };
      }



      /// <summary>
      /// 
      /// </summary>
      public System.Int32 UID
      {
          get{return getProperty<System.Int32>("UID");}
          set{setProperty("UID",value );}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String Name
      {
          get{return getProperty<System.String>("Name");}
          set{setProperty("Name",value ,50);}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String PropertyName
      {
          get{return getProperty<System.String>("PropertyName");}
          set{setProperty("PropertyName",value ,10);}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String PropertyValue
      {
          get{return getProperty<System.String>("PropertyValue");}
          set{setProperty("PropertyValue",value ,10);}
      }


  }
}
