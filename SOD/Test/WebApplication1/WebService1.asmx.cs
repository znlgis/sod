using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using TestWebAppModel;
using PWMIS.DataMap.Entity;

namespace WebApplication1
{
    /// <summary>
    /// WebService1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(Description="PDF.NET实体类序列化方式1")]
        public Ser_UserInfo GetUserInfo(int uid)
        {
            Tb_UserInfo user = new Tb_UserInfo() { ID = uid, UserName = "abc" };
            Ser_UserInfo sUser = new Ser_UserInfo();
            sUser.SetNameValues(user.GetNameValues());
            return sUser;
            //return new Ser_UserInfo(user);
        }

        [WebMethod(Description = "PDF.NET实体类序列化方式2")]
        public PropertyNameValues GetNameValues(int uid)
        {
            Tb_UserInfo user = new Tb_UserInfo() { ID = uid, UserName = "abc" };
            return user.GetNameValues();
        }
    }
}
