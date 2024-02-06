﻿using PWMIS.DataForms.Adapter;
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

namespace ClassReunionWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected int MemberCount = 3;
        protected string SchoolName = "";
        protected int SchoolYear = 1;
        protected string Head_reg_link = "";
        protected string Head_reg_memo = "";
        protected string Head_Holidays = "";
        protected string Head_ReunionDate = "";
        protected string Head_ReunionNote = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindLIstData();
                SchoolName = System.Configuration.ConfigurationManager.AppSettings["SchoolName"];
                SchoolYear = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SchoolYear"]);
                Head_reg_link = System.Configuration.ConfigurationManager.AppSettings["Head_reg_link"];
                Head_reg_memo = System.Configuration.ConfigurationManager.AppSettings["Head_reg_memo"];
                Head_Holidays = System.Configuration.ConfigurationManager.AppSettings["Head_Holidays"];
                Head_ReunionDate = System.Configuration.ConfigurationManager.AppSettings["Head_ReunionDate"];
                Head_ReunionNote = System.Configuration.ConfigurationManager.AppSettings["Head_ReunionNote"];

            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.dlMsg.CssClass = "label label-danger";
            if (this.dtName.Text == "")
            {
                this.dlMsg.Text = "请输入姓名！";
                return;
            }
            if (this.dtPhone.Text == "")
            {
                this.dlMsg.Text = "请输入电话！";
                return;
            }
            if (this.dtPersonID.Text == "")
            {
                this.dlMsg.Text = "请输入身份ID！";
                return;
            }
            else
            {
                //从页面收集数据到实体类
                ContactInfo info= MyWebForm.DataMap.CollectDataToObject<ContactInfo>(
                    MyWebForm.GetIBControls(this.Controls));
                info.AtTime = DateTime.Now;
                //保存数据到数据库
                ClassReunionRepository rep = new ClassReunionRepository();
                //如果查询到名字和身份证号一致的数据，则修改，否则添加数据
                var existsInfo = rep.UserQuery.GetObject (
                    OQL.From(info)
                    .Select()
                    .Where (cmp=> cmp.EqualValue (info.Name ) & cmp.EqualValue(info.PersonID ))
                    .END
                    );

                int count=0;
                string optMsg = "";
                if (existsInfo == null)
                {
                    optMsg = "添加";
                    count = rep.Add<ContactInfo>(info);
                }
                else
                {
                    optMsg = "修改";
                    info.CID = existsInfo.CID; //指定ID主键的值，才可以修改
                    count = rep.Update<ContactInfo>(info);
                }

                if (count>0)
                {
                    this.dlMsg.CssClass = "label label-info";
                    this.dlMsg.Text = optMsg+" 成功！";
                    //重新绑定数据
                    BindLIstData();
                }
                else
                {
                    this.dlMsg.CssClass = "label label-danger";
                    this.dlMsg.Text = optMsg+ " 失败！[数据库操作异常，详细信息请检查SQL日志]";
                }
            }
        }

        private void BindLIstData()
        { 
             ClassReunionRepository rep = new ClassReunionRepository();
             ContactInfo s = new ContactInfo();
             var q = OQL.From(s)
                 .Select(s.CID, s.Name, s.ClassNum, s.ContactPhone, s.ComeFrom, s.NeedRoom, s.HomeMemberCount, s.OtherInfo)
                 .OrderBy(s.CID )
                 .END;
             var list = rep.UserQuery.GetList(q);
             //转换成DataTable 类型，表头可以绑定表格的字段而不是实体类的属性名
             var dataTable = EntityQueryAnonymous.EntitysToDataTable<ContactInfo>(list);
             this.GridView1.DataSource = dataTable;
             this.GridView1.DataBind();
             this.MemberCount = list.Count;
        }
    }
}