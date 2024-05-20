
/* 
 使用前请先在项目工程中引用 PWMIS.Core.dll
 2015/12/3 10:51:28
*/

using System;
using PWMIS.Common;
using PWMIS.DataMap.Entity;
using System.Configuration;

namespace OQLTest
{
  [Serializable()]
  public partial class v_userFavorites : EntityBase
  {
    public v_userFavorites()
    {
            TableName = "v_userFavorites";
            EntityMap = EntityMapType.View;
            /*
            if (ConfigurationManager.AppSettings["Schema"] == "1")
            {
                Schema="";
            }
            
           
            */
            //IdentityName = "ID";

            PrimaryKeys.Add("ID");
        }


        protected override void SetFieldNames()
      {
          PropertyNames = new string[] { "ID", "FavoritesID", "PatientID", "note", "Name", "usercode", "UserId", "Idx", "FupdateTime", "PatientNo", "PatientName", "SexName", "DateTimeOfBirth", "AgeYear", "Color" };
      }

      /// <summary>
      /// 
      /// </summary>
      public System.Int32 ID
      {
          get { return getProperty<System.Int32>("ID"); }
          set { setProperty("ID", value); }
      }

      /// <summary>
      /// 
      /// </summary>
      public System.Int32 FavoritesID
      {
          get{return getProperty<System.Int32>("FavoritesID");}
          set{setProperty("FavoritesID",value );}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.Int32 PatientID
      {
          get{return getProperty<System.Int32>("PatientID");}
          set{setProperty("PatientID",value );}
      }
    
      /// <summary>
      /// 
      /// </summary>
      public System.String note
      {
          get { return getProperty<System.String>("note"); }
          set { setProperty("note", value, 200); }
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String Name
      {
          get { return getProperty<System.String>("Name"); }
          set { setProperty("Name", value, 50); }
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String usercode
      {
          get { return getProperty<System.String>("usercode"); }
          set { setProperty("usercode", value, 10); }
      }

      /// <summary>
      /// 
      /// </summary>
      public System.Int32 UserId
      {
          get { return getProperty<System.Int32>("UserId"); }
          set { setProperty("UserId", value); }
      }

      /// <summary>
      /// 
      /// </summary>
      public System.Int32 Idx
      {
          get { return getProperty<System.Int32>("Idx"); }
          set { setProperty("Idx", value); }
      }

      /// <summary>
      /// 
      /// </summary>
      public System.DateTime FupdateTime
      {
          get { return getProperty<System.DateTime>("FupdateTime"); }
          set { setProperty("FupdateTime", value); }
      }

      /// <summary>
      /// 
      /// </summary>
      public System.Int32 PatientNo
      {
          get{return getProperty<System.Int32>("PatientNo");}
          set{setProperty("PatientNo",value );}
      }
      
      /// <summary>
      /// 
      /// </summary>
      public System.String PatientName
      {
          get{return getProperty<System.String>("PatientName");}
          set{setProperty("PatientName",value ,40);}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String SexName
      {
          get{return getProperty<System.String>("SexName");}
          set{setProperty("SexName",value ,40);}
      }


      /// <summary>
      /// 
      /// </summary>
      public System.DateTime DateTimeOfBirth
      {
          get { return getProperty<System.DateTime>("DateTimeOfBirth"); }
          set { setProperty("DateTimeOfBirth", value); }
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String AgeYear
      {
          get { return getProperty<System.String>("AgeYear"); }
          set { setProperty("AgeYear", value,20); }
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String Color
      {
          get { return getProperty<System.String>("Color"); }
          set { setProperty("Color", value, 20); }
      }
  }
}
