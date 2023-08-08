using System;
using System.Text;
using System.Web.UI;
using System.Xml;
using System.Xml.Serialization;
using TestWebAppModel;

namespace WebApplication1
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Dictionary<string, int> StringFieldSize = new Dictionary<string, int>();


            //序列化测试
            var sb = new StringBuilder();
            var xw = XmlWriter.Create(sb);
            var xs = new XmlSerializer(typeof(Ser_UserInfo));


            var user = new Tb_UserInfo(); //创建一个用户对象 
            user.UserName = "张三";
            user.ID = 20;

            var sUser = new Ser_UserInfo(user);

            xs.Serialize(xw, sUser);

            TextBox1.Text = sb.ToString();
        }
    }
}