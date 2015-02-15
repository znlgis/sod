using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PWIMIS.Core;
using PWMIS.Common;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Data;

using FreeDrag.Core.ORM.Models;

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
            GOQL<TblTestModel> q = OQL.From<TblTestModel>()
                .Select(s => new object[] { s.ID, s.Title, s.ReadCount, s.CreateTime }).END;

            dataGV1.DataSource = q.ToList();
            dataGV1.Refresh();
            
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            TblTestModel model = new TblTestModel();
            model.Title = "测试标题";
            model.ArtContent = "测试内容";
            model.CreateTime = DateTime.Now;
            EntityQuery<TblTestModel>.Instance.Insert(model);

            OQL q = new OQL(model);
            q.Select().OrderBy(model.CreateTime, "desc");
            model = EntityQuery<TblTestModel>.QueryList(q).ToList<TblTestModel>()[0];
            model.Title = "已修改" + model.ID.ToString();
            model.CreateTime = DateTime.Now;
            EntityQuery<TblTestModel>.Instance.Update(model);
            dataGV1.DataSource = EntityQuery<TblTestModel>.QueryList(q).ToList<TblTestModel>();
            dataGV1.Refresh();
        }
    }
}
