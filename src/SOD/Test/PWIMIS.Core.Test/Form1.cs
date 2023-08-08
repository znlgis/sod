using System;
using System.Linq;
using System.Windows.Forms;
using FreeDrag.Core.ORM.Models;
using PWMIS.DataMap.Entity;

namespace PWIMIS.Core.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            var q = OQL.From<TblTestModel>()
                .Select(s => new object[] { s.ID, s.Title, s.ReadCount, s.CreateTime }).END;

            dataGV1.DataSource = q.ToList();
            dataGV1.Refresh();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            var model = new TblTestModel();
            model.Title = "测试标题";
            model.ArtContent = "测试内容";
            model.CreateTime = DateTime.Now;
            EntityQuery<TblTestModel>.Instance.Insert(model);

            var q = new OQL(model);
            q.Select().OrderBy(model.CreateTime, "desc");
            model = EntityQuery<TblTestModel>.QueryList(q).ToList()[0];
            model.Title = "已修改" + model.ID;
            model.CreateTime = DateTime.Now;
            EntityQuery<TblTestModel>.Instance.Update(model);
            dataGV1.DataSource = EntityQuery<TblTestModel>.QueryList(q).ToList();
            dataGV1.Refresh();
        }
    }
}