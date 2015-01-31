
/* 
 本类由PWMIS 实体类生成工具(Ver 4.1)自动生成
 http://www.pwmis.com/sqlmap
 使用前请先在项目工程中引用 PWMIS.Core.dll
 2013-8-21 14:26:55
*/

using System;
using PWMIS.Common;
using PWMIS.DataMap.Entity;

namespace PDFNETClassLib.Model 
{
  [Serializable()]
  public partial class p_hege_detail : EntityBase
  {
    public p_hege_detail()
    {
            TableName = "p_hege_detail";
            EntityMap=EntityMapType.Table;
            //IdentityName = "标识字段名";
    IdentityName="id";

            //PrimaryKeys.Add("主键字段名");
    PrimaryKeys.Add("id");

            
    }


      protected override void SetFieldNames()
      {
           PropertyNames = new string[] { "id","hegeID","coName","coType","coMessage","faMessage" };
      }



      /// <summary>
      /// 
      /// </summary>
      public System.Int32 id
      {
          get{return getProperty<System.Int32>("id");}
          set{setProperty("id",value );}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.Int32 hegeID
      {
          get{return getProperty<System.Int32>("hegeID");}
          set{setProperty("hegeID",value );}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String coName
      {
          get{return getProperty<System.String>("coName");}
          set{setProperty("coName",value ,50);}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String coType
      {
          get{return getProperty<System.String>("coType");}
          set{setProperty("coType",value ,10);}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String coMessage
      {
          get{return getProperty<System.String>("coMessage");}
          set{setProperty("coMessage",value ,50);}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String faMessage
      {
          get{return getProperty<System.String>("faMessage");}
          set{setProperty("faMessage",value ,50);}
      }


  }
}
