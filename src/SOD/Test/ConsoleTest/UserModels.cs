/*
 本类由PWMIS 实体类生成工具(Ver 4.1)自动生成
 http://www.pwmis.com/sqlmap
 使用前请先在项目工程中引用 PWMIS.Core.dll
 2013-01-20 21:14:06
*/

using System;
using System.Collections.Generic;
using PWMIS.Common;
using PWMIS.DataMap.Entity;

namespace MvcApplication1.Models
{
    [Serializable]
    public class UserModels : EntityBase
    {
        public UserModels()
        {
            TableName = "C_USER_INFO";
            EntityMap = EntityMapType.Table;
            //IdentityName = "标识字段名";
            IdentityName = "C_USER_INFO_ID";

            //PrimaryKeys.Add("主键字段名");
            PrimaryKeys.Add("C_USER_INFO_ID");
        }


        /// <summary>
        /// </summary>
        public int C_USER_INFO_ID
        {
            get => getProperty<int>("C_USER_INFO_ID");
            set => setProperty("C_USER_INFO_ID", value);
        }

        /// <summary>
        /// </summary>
        public string USERNAME
        {
            get => getProperty<string>("USERNAME");
            set => setProperty("USERNAME", value, 50);
        }

        /// <summary>
        /// </summary>
        public string USERNAME_EN
        {
            get => getProperty<string>("USERNAME_EN");
            set => setProperty("USERNAME_EN", value, 50);
        }

        /// <summary>
        /// </summary>
        public string USER_CODE
        {
            get => getProperty<string>("USER_CODE");
            set => setProperty("USER_CODE", value, 50);
        }

        /// <summary>
        /// </summary>
        public string PWD
        {
            get => getProperty<string>("PWD");
            set => setProperty("PWD", value, 50);
        }

        /// <summary>
        /// </summary>
        public string SEX
        {
            get => getProperty<string>("SEX");
            set => setProperty("SEX", value, 50);
        }

        /// <summary>
        /// </summary>
        public int REGION_ID
        {
            get => getProperty<int>("REGION_ID");
            set => setProperty("REGION_ID", value);
        }

        /// <summary>
        /// </summary>
        public int BIGTEAM_ID
        {
            get => getProperty<int>("BIGTEAM_ID");
            set => setProperty("BIGTEAM_ID", value);
        }

        /// <summary>
        /// </summary>
        public int? SMALLTEAM_ID
        {
            get => getProperty<int>("SMALLTEAM_ID");
            set => setProperty("SMALLTEAM_ID", value);
        }

        /// <summary>
        /// </summary>
        public DateTime INDATE
        {
            get => getProperty<DateTime>("INDATE");
            set => setProperty("INDATE", value);
        }

        /// <summary>
        /// </summary>
        public string REMARK
        {
            get => getProperty<string>("REMARK");
            set => setProperty("REMARK", value, 50);
        }

        /// <summary>
        /// </summary>
        public bool ISDEL
        {
            get => getProperty<bool>("ISDEL");
            set => setProperty("ISDEL", value);
        }

        /// <summary>
        /// </summary>
        public string AGENT_CODE
        {
            get => getProperty<string>("AGENT_CODE");
            set => setProperty("AGENT_CODE", value, 20);
        }

        /// <summary>
        /// </summary>
        public string AGENT_PWS
        {
            get => getProperty<string>("AGENT_PWS");
            set => setProperty("AGENT_PWS", value, 20);
        }

        /// <summary>
        /// </summary>
        public bool STATUS
        {
            get => getProperty<bool>("STATUS");
            set => setProperty("STATUS", value);
        }

        /// <summary>
        /// </summary>
        public string AGENT_STATUS
        {
            get => getProperty<string>("AGENT_STATUS");
            set => setProperty("AGENT_STATUS", value, 50);
        }

        /// <summary>
        /// </summary>
        public int LOGIN
        {
            get => getProperty<int>("LOGIN");
            set => setProperty("LOGIN", value);
        }

        /// <summary>
        /// </summary>
        public int IS_REST
        {
            get => getProperty<int>("IS_REST");
            set => setProperty("IS_REST", value);
        }

        /// <summary>
        /// </summary>
        public string SEC_ROLE_ID
        {
            get => getProperty<string>("SEC_ROLE_ID");
            set => setProperty("SEC_ROLE_ID", value, 50);
        }


        protected override void SetFieldNames()
        {
            PropertyNames = new[]
            {
                "C_USER_INFO_ID", "USERNAME", "USERNAME_EN", "USER_CODE", "PWD", "SEX", "REGION_ID", "BIGTEAM_ID",
                "SMALLTEAM_ID", "INDATE", "REMARK", "ISDEL", "AGENT_CODE", "AGENT_PWS", "STATUS", "AGENT_STATUS",
                "LOGIN", "IS_REST", "SEC_ROLE_ID"
            };
        }

        public static void Save(UserModels user)
        {
            try
            {
                //user.INDATE = DateTime.Now;
                var query = new EntityQuery<UserModels>(user);
                var num = query.SaveAllChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Delete(UserModels user)
        {
            var query = new EntityQuery<UserModels>(user);
            query.Delete(user);
        }

        public static List<UserModels> GetUser(int page, int num)
        {
            try
            {
                var model = new UserModels();
                var q = new OQL(model);
                q.PageEnable = true;
                q.PageNumber = num;
                q.PageSize = page;
                q.PageWithAllRecordCount = GetCount();
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
                var model = new UserModels();
                var q = new OQL(model);
                return EntityQuery<UserModels>.QueryList(q.Select().END);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int GetCount()
        {
            var model = new UserModels();
            var q = new OQL(model);
            model = EntityQuery<UserModels>.QueryObject(q.Select().Count(model.C_USER_INFO_ID, "count").END);
            var count = model.getProperty<int>("count");
            return count;
        }
    }
}