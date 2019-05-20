using PWMIS.DataForms.Adapter;
using PWMIS.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PWMIS.Common;

namespace FileSyncGui
{
    public partial class MainForm : MvvmForm
    {
        MainViewModel DataContext { get; set; }

        public MainForm()
        {
            InitializeComponent();
            this.lstFilesInfo.DataSource = this.bindingSource1;

            DataContext = new FileSyncGui.MainViewModel();
            DataContext.View = this;
            DataContext.UploadFiles = this.bindingSource1;
            DataContext.OnStartSync += DataContext_OnStartSync;
        }

        private void DataContext_OnStartSync(object sender, EventArgs e)
        {
            if (DataContext.IsStarting)
            {
                this.btnStart.Text = "停止";
            }
            else
            {
                this.btnStart.Text = "启动";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int num= DateTime.Now.Second % 12;
            string str = new string('-', num);
            string str2 = new string('-', 12 - num);

            int left= this.DataContext.SetTimerTick(1);
            this.lblDateTime.Text = DateTime.Now.ToString() + "  " + str + ">>" + str2+" ("+ left+")";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            base.BindCommandControls(this.btnStart);
            this.progressBar1.DataBindings.Add("Value", this.DataContext, "FileSyncInfo.FolderProgress");
            
            this.timer1.Start();
            //this.DataContext.Init();
            
        }

        protected override void BindDataControl(IDataControl control, object dataSource, string dataMember)
        {
            if (control is CheckBox)
            {
                ((CheckBox)control).DataBindings.Add("Checked", dataSource, control.LinkProperty);
            }
        }

        private void RunFilesSyncTask()
        {
            MessageBox.Show("RunFilesSyncTask");
        }

       
    }
}
