
/* 
 本类由PWMIS 实体类生成工具(Ver 4.1)自动生成
 http://www.pwmis.com/sqlmap
 使用前请先在项目工程中引用 PWMIS.Core.dll
 2012/4/9 14:40:28
*/

using System;
using PWMIS.Common;
using PWMIS.DataMap.Entity;

namespace TranstarAuction.Repository.Entitys 
{
  [Serializable()]
  public partial class AuctionOperationLog : EntityBase
  {
    public AuctionOperationLog()
    {
            TableName = "AuctionOperationLog";
            EntityMap=EntityMapType.Table;
            //IdentityName = "标识字段名";
    IdentityName="OptID";

            //PrimaryKeys.Add("主键字段名");
    PrimaryKeys.Add("OptID");

            
    }


      protected override void SetFieldNames()
      {
           PropertyNames = new string[] { "OptID","LogSource","OperaterID","Module","Operation","AtDateTime" };
      }



      /// <summary>
      /// 
      /// </summary>
      public System.Int32 OptID
      {
          get{return getProperty<System.Int32>("OptID");}
          set{setProperty("OptID",value );}
      }

      /// <summary>
      /// 来源，例如PC，Web
      /// </summary>
      public System.String LogSource
      {
          get{return getProperty<System.String>("LogSource");}
          set{setProperty("LogSource",value ,5);}
      }

      /// <summary>
      /// 操作人ID
      /// </summary>
      public System.Int32 OperaterID
      {
          get{return getProperty<System.Int32>("OperaterID");}
          set{setProperty("OperaterID",value );}
      }

      /// <summary>
      /// 操作的模块编码或名称
      /// </summary>
      public System.String Module
      {
          get{return getProperty<System.String>("Module");}
          set{setProperty("Module",value ,10);}
      }

      /// <summary>
      /// 具体的操作内容
      /// </summary>
      public System.String Operation
      {
          get{return getProperty<System.String>("Operation");}
          set{setProperty("Operation",value ,50);}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.DateTime AtDateTime
      {
          get{return getProperty<System.DateTime>("AtDateTime");}
          set{setProperty("AtDateTime",value );}
      }


  }
}
