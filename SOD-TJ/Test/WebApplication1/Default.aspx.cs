using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using TestWebAppModel;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace WebApplication1
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Dictionary<string, int> StringFieldSize = new Dictionary<string, int>();
            

            //序列化测试
            StringBuilder sb = new StringBuilder();
            XmlWriter xw = XmlWriter.Create(sb);
            XmlSerializer xs = new XmlSerializer(typeof(Ser_UserInfo));


            Tb_UserInfo user = new Tb_UserInfo();//创建一个用户对象 
            user.UserName = "张三";
            user.ID = 20;

            Ser_UserInfo sUser = new Ser_UserInfo(user);

            xs.Serialize(xw, sUser);

            this.TextBox1.Text = sb.ToString();
        }
    }
}
