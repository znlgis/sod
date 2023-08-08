using System;
using System.Configuration;
using System.Web.UI;
using PWMIS.DataForms.Adapter;
using PWMIS.DataMap.Entity;
using WebApplication2.Model;
using WebApplication2.Repository;

namespace ClassReunionWeb
{
    public partial class _Default : Page
    {
        protected string Head_Holidays = "";
        protected string Head_reg_link = "";
        protected string Head_reg_memo = "";
        protected string Head_ReunionDate = "";
        protected string Head_ReunionNote = "";
        protected int MemberCount = 3;
        protected string SchoolName = "";
        protected int SchoolYear = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindLIstData();
                SchoolName = ConfigurationManager.AppSettings["SchoolName"];
                SchoolYear = int.Parse(ConfigurationManager.AppSettings["SchoolYear"]);
                Head_reg_link = ConfigurationManager.AppSettings["Head_reg_link"];
                Head_reg_memo = ConfigurationManager.AppSettings["Head_reg_memo"];
                Head_Holidays = ConfigurationManager.AppSettings["Head_Holidays"];
                Head_ReunionDate = ConfigurationManager.AppSettings["Head_ReunionDate"];
                Head_ReunionNote = ConfigurationManager.AppSettings["Head_ReunionNote"];
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            dlMsg.CssClass = "label label-danger";
            if (dtName.Text == "")
            {
                dlMsg.Text = "请输入姓名！";
                return;
            }

            if (dtPhone.Text == "")
            {
                dlMsg.Text = "请输入电话！";
                return;
            }

            if (dtPersonID.Text == "")
            {
                dlMsg.Text = "请输入身份ID！";
                return;
            }

            //从页面收集数据到实体类
            var info = MyWebForm.DataMap.CollectDataToObject<ContactInfo>(
                MyWebForm.GetIBControls(Controls));
            info.AtTime = DateTime.Now;
            //保存数据到数据库
            var rep = new ClassReunionRepository();
            //如果查询到名字和身份证号一致的数据，则修改，否则添加数据
            var existsInfo = rep.UserQuery.GetObject(
                OQL.From(info)
                    .Select()
                    .Where(cmp => cmp.EqualValue(info.Name) & cmp.EqualValue(info.PersonID))
                    .END
            );

            var count = 0;
            var optMsg = "";
            if (existsInfo == null)
            {
                optMsg = "添加";
                count = rep.Add(info);
            }
            else
            {
                optMsg = "修改";
                info.CID = existsInfo.CID; //指定ID主键的值，才可以修改
                count = rep.Update(info);
            }

            if (count > 0)
            {
                dlMsg.CssClass = "label label-info";
                dlMsg.Text = optMsg + " 成功！";
                //重新绑定数据
                BindLIstData();
            }
            else
            {
                dlMsg.CssClass = "label label-danger";
                dlMsg.Text = optMsg + " 失败！[数据库操作异常，详细信息请检查SQL日志]";
            }
        }

        private void BindLIstData()
        {
            var rep = new ClassReunionRepository();
            var s = new ContactInfo();
            var q = OQL.From(s)
                .Select(s.CID, s.Name, s.ClassNum, s.ContactPhone, s.ComeFrom, s.NeedRoom, s.HomeMemberCount,
                    s.OtherInfo)
                .OrderBy(s.CID)
                .END;
            var list = rep.UserQuery.GetList(q);
            //转换成DataTable 类型，表头可以绑定表格的字段而不是实体类的属性名
            var dataTable = EntityQueryAnonymous.EntitysToDataTable(list);
            GridView1.DataSource = dataTable;
            GridView1.DataBind();
            MemberCount = list.Count;
        }
    }
}