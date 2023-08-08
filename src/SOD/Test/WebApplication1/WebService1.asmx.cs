using System.ComponentModel;
using System.Web.Services;
using PWMIS.DataMap.Entity;
using TestWebAppModel;

namespace WebApplication1
{
    /// <summary>
    ///     WebService1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class WebService1 : WebService
    {
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(Description = "PDF.NET实体类序列化方式1")]
        public Ser_UserInfo GetUserInfo(int uid)
        {
            var user = new Tb_UserInfo { ID = uid, UserName = "abc" };
            var sUser = new Ser_UserInfo();
            sUser.SetNameValues(user.GetNameValues());
            return sUser;
            //return new Ser_UserInfo(user);
        }

        [WebMethod(Description = "PDF.NET实体类序列化方式2")]
        public PropertyNameValues GetNameValues(int uid)
        {
            var user = new Tb_UserInfo { ID = uid, UserName = "abc" };
            return user.GetNameValues();
        }
    }
}