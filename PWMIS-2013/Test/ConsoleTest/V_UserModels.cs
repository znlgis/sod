

using System;
using PWMIS.Common;
using PWMIS.DataMap.Entity;
using System.Collections.Generic;

namespace MvcApplication1.Models 
{
  [Serializable()]
  public  class V_UserModels : EntityBase
  {
    public V_UserModels()
    {
            TableName = "MvcApplication1.Models.V_UserModels";
            EntityMap=EntityMapType.SqlMap;
            //IdentityName = "标识字段名";
    IdentityName="C_USER_INFO_ID";

            //PrimaryKeys.Add("主键字段名");
    PrimaryKeys.Add("C_USER_INFO_ID");

            
    }

      protected override void SetFieldNames()
      {
           PropertyNames = new string[] { "C_USER_INFO_ID","USERNAME","USERNAME_EN","USER_CODE","PWD","SEX","REGION_ID","BIGTEAM_ID","SMALLTEAM_ID","INDATE","REMARK","ISDEL","AGENT_CODE","AGENT_PWS","STATUS","AGENT_STATUS","LOGIN","IS_REST","SEC_ROLE_ID","REGION_NAME","BIGTEAM_NAME","SMALLTEAM_NAME","NAME" };
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
      public System.Int32? REGION_ID
      {
          get{return getProperty<System.Int32>("REGION_ID");}
          set{setProperty("REGION_ID",value );}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.Int32? BIGTEAM_ID
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

      /// <summary>
      /// 
      /// </summary>
      public System.String REGION_NAME
      {
          get{return getProperty<System.String>("REGION_NAME");}
          set{setProperty("REGION_NAME",value ,100);}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String BIGTEAM_NAME
      {
          get{return getProperty<System.String>("BIGTEAM_NAME");}
          set{setProperty("BIGTEAM_NAME",value ,100);}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String SMALLTEAM_NAME
      {
          get{return getProperty<System.String>("SMALLTEAM_NAME");}
          set{setProperty("SMALLTEAM_NAME",value ,100);}
      }

      /// <summary>
      /// 
      /// </summary>
      public System.String NAME
      {
          get{return getProperty<System.String>("NAME");}
          set{setProperty("NAME",value ,20);}
      }

      public static List<V_UserModels> GetAllList()
      {
          V_UserModels model = new V_UserModels();
          OQL q = new OQL(model);
          List<V_UserModels> list = EntityQuery<V_UserModels>.QueryList(q.Select().END);
          return list;
      }
      public static List<V_UserModels> GetAllList(int pageSzie,int pageNum)
      {
          V_UserModels model = new V_UserModels();
          OQL q = new OQL(model);
          q.PageEnable = true;
          q.PageNumber = pageNum;
          q.PageSize = pageSzie;
          q.PageWithAllRecordCount = GetCount();
          List<V_UserModels> list = EntityQuery<V_UserModels>.QueryList(q.Select().END);
          return list;
      }

      public static int GetCount()
      {
          V_UserModels model = new V_UserModels();
          OQL q = new OQL(model);
          model = EntityQuery<V_UserModels>.QueryObject(q.Select().Count(model.C_USER_INFO_ID, "count").END);
          int count = model.getProperty<int>("count");
          return count;
      }
  }
}
