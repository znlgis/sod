
/*
 本类由PWMIS 实体类生成工具(Ver 1.1)自动生成
 http://www.pwmis.com/sqlmap
 使用前请先在项目工程中引用 PWMIS.SqlMapper.Entity.dll
 2010-2-1 17:16:42

*/

using System;
using PWMIS.DataMap.Entity;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace TestWebAppModel 
{

  public partial class Tb_UserInfo : EntityBase 
  {
    public Tb_UserInfo()
    {
            TableName = "Tb_UserInfo";
            IdentityName = "ID";
            PrimaryKeys.Add("ID");

            PropertyNames = new string[] { "ID", "UserName", "Sex", "IDCode", "Nation", "Stature", "Remark", "Birthday" };
            PropertyValues = new object[PropertyNames.Length];
            //AddProperty("ID", default(System.Int32));
            //AddProperty("UserName", default(System.String));
            //AddProperty("Sex", default(System.Boolean));
            //AddProperty("IDCode", default(System.String));
            //AddProperty("Nation", default(System.String));
            //AddProperty("Stature", default(System.Double));
            //AddProperty("Remark", default(System.String));
            //AddProperty("Birthday", default(System.DateTime));
    }


      public System.Int32 ID
      {
          get{return (System.Int32)getProperty("ID", TypeCode.Int32 );}
          set{setProperty("ID",value);}
      }

      public System.String UserName
      {
          get{return (System.String)getProperty("UserName", TypeCode.String );}
          set{setProperty("UserName",value);}
      }

      public System.Boolean Sex
      {
          get { return (System.Boolean)getProperty("Sex", TypeCode.Boolean ); }
          set{setProperty("Sex",value);}
      }

      public System.String IDCode
      {
          get { return (System.String)getProperty("IDCode", TypeCode.String ); }
          set{setProperty("IDCode",value);}
      }

      public System.String Nation
      {
          get { return (System.String)getProperty("Nation", TypeCode.String ); }
          set{setProperty("Nation",value);}
      }

      public System.Double Stature
      {
          get { return (System.Double)getProperty("Stature", TypeCode.Double ); }
          set{setProperty("Stature",value);}
      }

      public System.String Remark
      {
          get { return (System.String)getProperty("Remark", TypeCode.String ); }
          set{setProperty("Remark",value);}
      }

      public System.DateTime Birthday
      {
          get { return (System.DateTime)getProperty("Birthday", TypeCode.DateTime ); }
          set{setProperty("Birthday",value);}
      }



      #region ISerializable 成员

     
      //void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
      //{
      //    //base.GetObjectData(info, context);
      //}

     
      //private Tb_UserInfo(SerializationInfo info, StreamingContext context)
      //{
      //    //PropertyNames = (string[])info.GetValue("names", typeof(string[]));
      //    //PropertyValues = (object[])info.GetValue("values", typeof(object[]));
      //    //_identity = info.GetString("_identity");
      //    //_pks = (List<string>)info.GetValue("_pks", typeof(List<string>));
      //    //_tableName = info.GetString("_tableName");
      //}

      #endregion
  }
}
