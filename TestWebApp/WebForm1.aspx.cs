using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PWMIS.DataMap.Entity;

namespace TestWebApp
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            TestOQL();
            GC.Collect();
            DisplayMemory();
        }

        void TestOQL()
        {
            //假如GoodsBaseInfo 对象的实例info 是长生命周期的，那么应该使用下面的方式使用OQL和OQLCompare
            GoodsBaseInfo info = new GoodsBaseInfo();
            using (OQL q = new OQL (info))
            {
                OQLCompare cmp = new OQLCompare(info);
                q.Select(info.GoodsName).Where(cmp.Comparer(info.SerialNumber, "=", "123456"));
                string sql = q.ToString();
                
            }

            DisplayMemory();
           
        }

        void DisplayMemory()
        {
            string str = string.Format("Total memory： {0:###,###,###,##0} bytes", GC.GetTotalMemory(true));
            //System.Diagnostics.Debug.WriteLine(string.Format（"Total memory： {0：###，###，###，##0} bytes"， GC.GetTotalMemory（true））；
            System.Diagnostics.Debug.WriteLine(str);
        }
    }

    /// <summary>
    /// 商品基本信息实体
    /// </summary>
    public class GoodsBaseInfo : EntityBase
    {
        public GoodsBaseInfo()
        {
            TableName = "商品信息表";
            PrimaryKeys.Add("条码号");
        }

        protected override void SetFieldNames()
        {
            PropertyNames = new string[] { "条码号", "商品名称", "厂商名称", "保质期" };
        }

        public string SerialNumber
        {
            get { return getProperty<string>("条码号"); }
            set { setProperty("条码号", value); }
        }

        public string GoodsName
        {
            get { return getProperty<string>("商品名称"); }
            set { setProperty("商品名称", value); }
        }

        public string Manufacturer
        {
            get { return getProperty<string>("厂商名称"); }
            set { setProperty("厂商名称", value); }
        }

        public int CanUserMonth
        {
            get { return getProperty<int>("保质期"); }
            set { setProperty("保质期", value); }
        }
    }

}