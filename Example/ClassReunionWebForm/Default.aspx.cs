using PWMIS.DataForms.Adapter;
using PWMIS.DataMap;
using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication2.Model;
using WebApplication2.Repository;

namespace WebApplication2
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(! IsPostBack)
                BindLIstData();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (this.dtName.Text == "")
            {
                this.dlMsg.Text = "请输入姓名！";
                return;
            }
            if (this.dtPersonID.Text == "")
            {
                this.dlMsg.Text = "请输入身份证号！";
                return;
            }
            else
            {
                //保存数据
                //实例化一个控件数据映射对象
                ControlDataMap cdm = new ControlDataMap();
                //收集数据到实体对象
                ContactInfo info = cdm.CollectDataToObject<ContactInfo>(
                    MyWebForm.GetIBControls(this.Controls)
                    );

                //调用业务类，保存数据
                ClassReunionRepository rep = new ClassReunionRepository();
                int count= rep.Add<ContactInfo>(info);

                if (count>0)
                {
                    this.dlMsg.Text = "保存成功！";
                    //重新绑定数据
                    BindLIstData();
                }
                else
                {
                    this.dlMsg.Text = "保存失败！[数据库操作异常，详细信息请检查SQL日志]";
                }
            }
        }

        private void BindLIstData()
        { 
             ClassReunionRepository rep = new ClassReunionRepository();
             ContactInfo s = new ContactInfo();
             var q = OQL.From(s)
                 .Select(s.CID, s.Name, s.NeedRoom, s.OtherInfo )
                 .OrderBy(s.CID )
                 .END;
             var list = rep.UserQuery.GetList(q);

             this.GridView1.DataSource = list;
             this.GridView1.DataBind();
        }
    }
}