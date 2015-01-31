
/* 
 本类由PWMIS 实体类生成工具(Ver 4.1)自动生成
 http://www.pwmis.com/sqlmap
 使用前请先在项目工程中引用 PWMIS.Core.dll
 2013-01-20 21:14:06
*/

using System;
using PWMIS.Common;
using PWMIS.DataMap.Entity;
using System.Collections.Generic;

namespace MvcApplication1.Models 
{
  [Serializable()]
  public  class UserModels : EntityBase
  {
    public UserModels()
    {
            TableName = "C_USER_INFO";
            EntityMap=EntityMapType.Table;
            //IdentityName = "标识字段名";
    IdentityName="C_USER_INFO_ID";

            //PrimaryKeys.Add("主键字段名");
    PrimaryKeys.Add("C_USER_INFO_ID");

            
    }


      protected override void SetFieldNames()
      {
           PropertyNames = new string[] { "C_USER_INFO_ID","USERNAME","USERNAME_EN","USER_CODE","PWD","SEX","REGION_ID","BIGTEAM_ID","SMALLTEAM_ID","INDATE","REMARK","ISDEL","AGENT_CODE","AGENT_PWS","STATUS","AGENT_STATUS","LOGIN","IS_REST","SEC_ROLE_ID" };
      }



      /// <summary>
      /// 
      /// </summary>
      public System.Int32 C_USER_INFO_ID
      {
          get{return getProperty<System.Int32>("C_USER_INFO_ID");}
          set{setProperty("C_USER_INFO_ID",value );}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String USERNAME
      {
          get{return getProperty<System.String>("USERNAME");}
          set{setProperty("USERNAME",value ,50);}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String USERNAME_EN
      {
          get{return getProperty<System.String>("USERNAME_EN");}
          set{setProperty("USERNAME_EN",value ,50);}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String USER_CODE
      {
          get{return getProperty<System.String>("USER_CODE");}
          set{setProperty("USER_CODE",value ,50);}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String PWD
      {
          get{return getProperty<System.String>("PWD");}
          set{setProperty("PWD",value ,50);}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String SEX
      {
          get{return getProperty<System.String>("SEX");}
          set{setProperty("SEX",value ,50);}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.Int32 REGION_ID
      {
          get{return getProperty<System.Int32>("REGION_ID");}
          set{setProperty("REGION_ID",value );}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.Int32 BIGTEAM_ID
      {
          get{return getProperty<System.Int32>("BIGTEAM_ID");}
          set{setProperty("BIGTEAM_ID",value );}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.Int32? SMALLTEAM_ID
      {
          get{return getProperty<System.Int32>("SMALLTEAM_ID");}
          set{setProperty("SMALLTEAM_ID",value );}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.DateTime INDATE
      {
          get{return getProperty<System.DateTime>("INDATE");}
          set{setProperty("INDATE",value );}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String REMARK
      {
          get{return getProperty<System.String>("REMARK");}
          set{setProperty("REMARK",value ,50);}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.Boolean ISDEL
      {
          get{return getProperty<System.Boolean>("ISDEL");}
          set{setProperty("ISDEL",value );}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String AGENT_CODE
      {
          get{return getProperty<System.String>("AGENT_CODE");}
          set{setProperty("AGENT_CODE",value ,20);}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String AGENT_PWS
      {
          get{return getProperty<System.String>("AGENT_PWS");}
          set{setProperty("AGENT_PWS",value ,20);}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.Boolean STATUS
      {
          get{return getProperty<System.Boolean>("STATUS");}
          set{setProperty("STATUS",value );}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String AGENT_STATUS
      {
          get{return getProperty<System.String>("AGENT_STATUS");}
          set{setProperty("AGENT_STATUS",value ,50);}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.Int32 LOGIN
      {
          get{return getProperty<System.Int32>("LOGIN");}
          set{setProperty("LOGIN",value );}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.Int32 IS_REST
      {
          get{return getProperty<System.Int32>("IS_REST");}
          set{setProperty("IS_REST",value );}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String SEC_ROLE_ID
      {
          get{return getProperty<System.String>("SEC_ROLE_ID");}
          set{setProperty("SEC_ROLE_ID",value ,50);}
      }

      public static void Save(UserModels user)
      {
          try
          {
              //user.INDATE = DateTime.Now;
              EntityQuery<UserModels> query = new EntityQuery<UserModels>(user);
              int num = query.SaveAllChanges();
          }
          catch (Exception ex)
          {

              throw ex;
          }
      }

      public static void Delete(UserModels user)
      {
          try
          {
              EntityQuery<UserModels> query = new EntityQuery<UserModels>(user);
              query.Delete(user);
          }
          catch (Exception ex)
          {

              throw;
          }
      }

      public static List<UserModels> GetUser(int page,int num)
      {
          try
          {
              UserModels model = new UserModels();
              OQL q = new OQL(model);
              q.PageEnable = true;
              q.PageNumber = num;
              q.PageSize = page;
              q.PageWithAllRecordCount = UserModels.GetCount();
              return EntityQuery<UserModels>.QueryList(q.Select().END);
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public static List<UserModels> GetUser()
      {
          try
          {
              UserModels model = new UserModels();
              OQL q = new OQL(model);
              return EntityQuery<UserModels>.QueryList(q.Select().END);
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static int GetCount()
      {
          UserModels model = new UserModels();
          OQL q = new OQL(model);
          model = EntityQuery<UserModels>.QueryObject(q.Select().Count(model.C_USER_INFO_ID, "count").END);
          int count = model.getProperty<int>("count");
          return count;
      }
  }
}
