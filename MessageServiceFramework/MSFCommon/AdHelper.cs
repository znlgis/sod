using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;

namespace PWMIS.EnterpriseFramework.Common
{
    public static class AdHelper
    {
        //private static string ADPath = System.Configuration.ConfigurationManager.AppSettings["domain"].ToString();//读取域信息

        /// <summary>
        /// 域组内是否存在当前登录windows用户
        /// </summary>
        /// <returns></returns>
        public static bool IsExistUser(string adPath,string role,string loginName,out string userName)
        {
            //string roleName = System.Configuration.ConfigurationManager.AppSettings["role"].ToString();
            //string loginName = "yuanzhijun";
            //string loginName = Environment.UserName;
            userName = loginName;
            return UserisGroupMember(adPath, loginName, role);
        }
        /// <summary>
        /// 判断用户是否为域组成员
        /// </summary>
        /// <param name="UserLogin">用户名</param>
        /// <param name="RoleName">域组名</param>
        /// <returns></returns>
        public static bool UserisGroupMember(string adPath,string UserLogin, string RoleName)
        {
            DirectoryEntry entry = new DirectoryEntry(adPath);
            DirectorySearcher mySearcher = new DirectorySearcher(entry);

            mySearcher.Filter = string.Format("(&(objectClass=user)(sAMAccountName={0})) ", UserLogin);
            mySearcher.PropertiesToLoad.Add("memberof");
            SearchResult mysr = mySearcher.FindOne();

            if (mysr != null && mysr.Properties.Count > 1)  // 返回两个属性,一个是内置的adspath,另一个是PropertiesToLoad加载的 
            {
                string[] memberof = new string[mysr.Properties["memberof"].Count];
                int i = 0;
                foreach (Object myColl in mysr.Properties["memberof"])
                {
                    memberof[i] = myColl.ToString().Substring(3, myColl.ToString().IndexOf(",") - 3);
                    if (memberof[i] == RoleName)
                        return true;
                    i++;
                }
                //其实这一层循环是广度优先算法,因为考虑到一个人直接属于某个安全组的可能性要大一些,这样做效率更高.如果把下面这个循环放到上面的if的esle中,就是完全的深度优先了. 
                foreach (string GroupName in memberof)
                {
                    if (MemberisGroupMember(adPath,GroupName, RoleName))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否为指定域下面的用户账号
        /// </summary>
        /// <param name="usrId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static bool IsDomainUser(string domain, string usrId, string pwd)
        {
            try
            {
                using (DirectoryEntry de = new DirectoryEntry())
                {
                    de.Path = "LDAP://"+domain;
                    de.Username = domain+"\\" + usrId;
                    de.Password = pwd;
                    DirectorySearcher search = new DirectorySearcher(de);
                    search.Filter = "(SAMAccountName=" + usrId + ")";
                    search.PropertiesToLoad.Add("CN");
                    SearchResult r = search.FindOne();
                    if (r == null)
                    {
                        de.Close();
                        return false;

                    }
                    else
                    {
                        de.Close();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        } 





        private static bool MemberisGroupMember(string adPath,string GroupName,string RoleName)
        {
            bool isfind = false;
            DirectoryEntry entry = new DirectoryEntry(adPath);
            DirectorySearcher mySearcher = new DirectorySearcher(entry);
            mySearcher.Filter = string.Format("(&(objectClass=group)(CN={0})) ", GroupName);
            mySearcher.PropertiesToLoad.Add("memberof");
            SearchResult mysr = mySearcher.FindOne();
            string memberof;
            try
            {
                if (mysr != null && mysr.Properties.Count > 1) // 返回两个属性,一个是内置的adspath,另一个是PropertiesToLoad加载的 
                {
                    foreach (Object myColl in mysr.Properties["memberof"])
                    {
                        memberof = myColl.ToString().Substring(3, myColl.ToString().IndexOf(",") - 3);
                        if (memberof == RoleName)
                        {
                            isfind = true;
                            break;
                        }
                        else if (MemberisGroupMember(adPath,memberof, RoleName))
                        {
                            isfind = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            return isfind;
        }      
    }
}
