namespace FileSyncGui
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lstFilesInfo = new System.Windows.Forms.ListBox();
            this.ckbAuto = new PWMIS.Windows.Controls.DataCheckBox();
            this.btnStart = new PWMIS.Windows.Controls.CommandButon(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnOpenLog = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSyncSpan = new PWMIS.Windows.Controls.DataTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataLabel8 = new PWMIS.Windows.Controls.DataLabel();
            this.label12 = new System.Windows.Forms.Label();
            this.dataLabel1 = new PWMIS.Windows.Controls.DataLabel();
            this.dataLabel2 = new PWMIS.Windows.Controls.DataLabel();
            this.dataLabel3 = new PWMIS.Windows.Controls.DataLabel();
            this.dataLabel4 = new PWMIS.Windows.Controls.DataLabel();
            this.dataLabel5 = new PWMIS.Windows.Controls.DataLabel();
            this.dataLabel6 = new PWMIS.Windows.Controls.DataLabel();
            this.dataLabel7 = new PWMIS.Windows.Controls.DataLabel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblProgress = new PWMIS.Windows.Controls.DataLabel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel7.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // lstFilesInfo
            // 
            this.lstFilesInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstFilesInfo.FormattingEnabled = true;
            this.lstFilesInfo.ItemHeight = 18;
            this.lstFilesInfo.Location = new System.Drawing.Point(0, 0);
            this.lstFilesInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstFilesInfo.Name = "lstFilesInfo";
            this.lstFilesInfo.Size = new System.Drawing.Size(1472, 289);
            this.lstFilesInfo.TabIndex = 0;
            // 
            // ckbAuto
            // 
            this.ckbAuto.AutoSize = true;
            this.ckbAuto.CompareSymbol = null;
            this.ckbAuto.LinkObject = "DataContext";
            this.ckbAuto.LinkProperty = "FileSyncInfo.IsAutoSync";
            this.ckbAuto.Location = new System.Drawing.Point(267, 225);
            this.ckbAuto.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ckbAuto.Name = "ckbAuto";
            this.ckbAuto.PrimaryKey = false;
            this.ckbAuto.QueryFormatString = null;
            this.ckbAuto.ReadOnly = false;
            this.ckbAuto.Size = new System.Drawing.Size(106, 22);
            this.ckbAuto.SysTypeCode = System.TypeCode.Boolean;
            this.ckbAuto.TabIndex = 1;
            this.ckbAuto.Text = "自动启动";
            this.ckbAuto.UseVisualStyleBackColor = true;
            this.ckbAuto.Value = "True";
            // 
            // btnStart
            // 
            this.btnStart.CommandName = "StartSync";
            this.btnStart.CommandObject = "DataContext";
            this.btnStart.ControlEvent = "Click";
            this.btnStart.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStart.Location = new System.Drawing.Point(267, 66);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStart.Name = "btnStart";
            this.btnStart.ParameterObject = null;
            this.btnStart.ParameterProperty = null;
            this.btnStart.Size = new System.Drawing.Size(112, 90);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "启动";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(5, 337);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(292, 83);
            this.label1.TabIndex = 3;
            this.label1.Text = "当前同步进度：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(515, 1);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(292, 83);
            this.label2.TabIndex = 4;
            this.label2.Text = "服务启动时间：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(5, 1);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(292, 83);
            this.label3.TabIndex = 5;
            this.label3.Text = "上次同步时间：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(5, 169);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(292, 83);
            this.label4.TabIndex = 6;
            this.label4.Text = "Mini Server 同步文件夹：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("幼圆", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(406, 34);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(455, 53);
            this.label5.TabIndex = 7;
            this.label5.Text = "文件同步服务面板";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 33);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(152, 18);
            this.label6.TabIndex = 8;
            this.label6.Text = "文件夹同步进度：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(5, 85);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(292, 83);
            this.label7.TabIndex = 9;
            this.label7.Text = "Mini Server 服务地址：";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(515, 85);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(292, 83);
            this.label8.TabIndex = 10;
            this.label8.Text = "BIZ Server  服务地址：";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(5, 253);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(292, 83);
            this.label9.TabIndex = 11;
            this.label9.Text = "当前同步信息：";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOpenLog
            // 
            this.btnOpenLog.Location = new System.Drawing.Point(1290, 33);
            this.btnOpenLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOpenLog.Name = "btnOpenLog";
            this.btnOpenLog.Size = new System.Drawing.Size(140, 34);
            this.btnOpenLog.TabIndex = 12;
            this.btnOpenLog.Text = "打开日志文件";
            this.btnOpenLog.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(62, 304);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(170, 18);
            this.label10.TabIndex = 13;
            this.label10.Text = "同步间隔（分钟）：";
            // 
            // txtSyncSpan
            // 
            this.txtSyncSpan.ClientValidationFunctionString = null;
            this.txtSyncSpan.CompareSymbol = null;
            this.txtSyncSpan.DataFormatString = null;
            this.txtSyncSpan.ErrorMessage = null;
            this.txtSyncSpan.LinkObject = "DataContext";
            this.txtSyncSpan.LinkProperty = "FileSyncInfo.SyncInterval";
            this.txtSyncSpan.Location = new System.Drawing.Point(267, 291);
            this.txtSyncSpan.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSyncSpan.MessageType = PWMIS.Windows.Validate.EnumMessageType.层;
            this.txtSyncSpan.Name = "txtSyncSpan";
            this.txtSyncSpan.OftenType = "无";
            this.txtSyncSpan.PrimaryKey = false;
            this.txtSyncSpan.QueryFormatString = null;
            this.txtSyncSpan.RegexName = null;
            this.txtSyncSpan.RegexString = null;
            this.txtSyncSpan.Size = new System.Drawing.Size(106, 28);
            this.txtSyncSpan.SysTypeCode = System.TypeCode.Int32;
            this.txtSyncSpan.TabIndex = 14;
            this.txtSyncSpan.Type = PWMIS.Windows.ValidationDataType.String;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel8);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1474, 530);
            this.panel1.TabIndex = 15;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 107);
            this.panel5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1472, 421);
            this.panel5.TabIndex = 18;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.tableLayoutPanel1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1022, 421);
            this.panel7.TabIndex = 18;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.dataLabel8, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label12, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label8, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.dataLabel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataLabel2, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataLabel3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.dataLabel4, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.dataLabel5, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.dataLabel6, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.dataLabel7, 3, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1022, 421);
            this.tableLayoutPanel1.TabIndex = 16;
            // 
            // dataLabel8
            // 
            this.dataLabel8.AutoSize = true;
            this.dataLabel8.DataFormatString = null;
            this.dataLabel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLabel8.LinkObject = "DataContext";
            this.dataLabel8.LinkProperty = "FileSyncInfo.FileProgress";
            this.dataLabel8.Location = new System.Drawing.Point(306, 337);
            this.dataLabel8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dataLabel8.MaxLength = 0;
            this.dataLabel8.Name = "dataLabel8";
            this.dataLabel8.PrimaryKey = false;
            this.dataLabel8.ReadOnly = true;
            this.dataLabel8.Size = new System.Drawing.Size(200, 83);
            this.dataLabel8.SysTypeCode = System.TypeCode.Int32;
            this.dataLabel8.TabIndex = 20;
            this.dataLabel8.Text = "dataLabel8";
            this.dataLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Location = new System.Drawing.Point(515, 337);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(292, 83);
            this.label12.TabIndex = 19;
            this.label12.Text = "同步次数：";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataLabel1
            // 
            this.dataLabel1.AutoSize = true;
            this.dataLabel1.DataFormatString = null;
            this.dataLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLabel1.LinkObject = "DataContext";
            this.dataLabel1.LinkProperty = "FileSyncInfo.LastSyncTime";
            this.dataLabel1.Location = new System.Drawing.Point(306, 1);
            this.dataLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dataLabel1.MaxLength = 0;
            this.dataLabel1.Name = "dataLabel1";
            this.dataLabel1.PrimaryKey = false;
            this.dataLabel1.ReadOnly = true;
            this.dataLabel1.Size = new System.Drawing.Size(200, 83);
            this.dataLabel1.SysTypeCode = System.TypeCode.DateTime;
            this.dataLabel1.TabIndex = 12;
            this.dataLabel1.Text = "dataLabel1";
            this.dataLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataLabel2
            // 
            this.dataLabel2.AutoSize = true;
            this.dataLabel2.DataFormatString = null;
            this.dataLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLabel2.LinkObject = "DataContext";
            this.dataLabel2.LinkProperty = "FileSyncInfo.ServiceStartTime";
            this.dataLabel2.Location = new System.Drawing.Point(816, 1);
            this.dataLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dataLabel2.MaxLength = 0;
            this.dataLabel2.Name = "dataLabel2";
            this.dataLabel2.PrimaryKey = false;
            this.dataLabel2.ReadOnly = true;
            this.dataLabel2.Size = new System.Drawing.Size(201, 83);
            this.dataLabel2.SysTypeCode = System.TypeCode.DateTime;
            this.dataLabel2.TabIndex = 13;
            this.dataLabel2.Text = "dataLabel2";
            this.dataLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataLabel3
            // 
            this.dataLabel3.AutoSize = true;
            this.dataLabel3.DataFormatString = null;
            this.dataLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLabel3.LinkObject = "DataContext";
            this.dataLabel3.LinkProperty = "FileSyncInfo.MiniServerHost";
            this.dataLabel3.Location = new System.Drawing.Point(306, 85);
            this.dataLabel3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dataLabel3.MaxLength = 0;
            this.dataLabel3.Name = "dataLabel3";
            this.dataLabel3.PrimaryKey = false;
            this.dataLabel3.ReadOnly = true;
            this.dataLabel3.Size = new System.Drawing.Size(200, 83);
            this.dataLabel3.TabIndex = 14;
            this.dataLabel3.Text = "dataLabel3";
            this.dataLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataLabel4
            // 
            this.dataLabel4.AutoSize = true;
            this.dataLabel4.DataFormatString = null;
            this.dataLabel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLabel4.LinkObject = "DataContext";
            this.dataLabel4.LinkProperty = "FileSyncInfo.BizServerHost";
            this.dataLabel4.Location = new System.Drawing.Point(816, 85);
            this.dataLabel4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dataLabel4.MaxLength = 0;
            this.dataLabel4.Name = "dataLabel4";
            this.dataLabel4.PrimaryKey = false;
            this.dataLabel4.ReadOnly = true;
            this.dataLabel4.Size = new System.Drawing.Size(201, 83);
            this.dataLabel4.TabIndex = 15;
            this.dataLabel4.Text = "dataLabel4";
            this.dataLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataLabel5
            // 
            this.dataLabel5.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.dataLabel5, 3);
            this.dataLabel5.DataFormatString = null;
            this.dataLabel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLabel5.LinkObject = "DataContext";
            this.dataLabel5.LinkProperty = "FileSyncInfo.MiniServerSyncFolder";
            this.dataLabel5.Location = new System.Drawing.Point(306, 169);
            this.dataLabel5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dataLabel5.MaxLength = 0;
            this.dataLabel5.Name = "dataLabel5";
            this.dataLabel5.PrimaryKey = false;
            this.dataLabel5.ReadOnly = true;
            this.dataLabel5.Size = new System.Drawing.Size(711, 83);
            this.dataLabel5.TabIndex = 16;
            this.dataLabel5.Text = "dataLabel5";
            this.dataLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataLabel6
            // 
            this.dataLabel6.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.dataLabel6, 3);
            this.dataLabel6.DataFormatString = null;
            this.dataLabel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLabel6.LinkObject = "DataContext";
            this.dataLabel6.LinkProperty = "FileSyncInfo.CurrentSyncInfo";
            this.dataLabel6.Location = new System.Drawing.Point(306, 253);
            this.dataLabel6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dataLabel6.MaxLength = 0;
            this.dataLabel6.Name = "dataLabel6";
            this.dataLabel6.PrimaryKey = false;
            this.dataLabel6.ReadOnly = true;
            this.dataLabel6.Size = new System.Drawing.Size(711, 83);
            this.dataLabel6.TabIndex = 17;
            this.dataLabel6.Text = "dataLabel6";
            this.dataLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataLabel7
            // 
            this.dataLabel7.AutoSize = true;
            this.dataLabel7.DataFormatString = null;
            this.dataLabel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLabel7.LinkObject = "DataContext";
            this.dataLabel7.LinkProperty = "FileSyncInfo.SyncCount";
            this.dataLabel7.Location = new System.Drawing.Point(816, 337);
            this.dataLabel7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dataLabel7.MaxLength = 0;
            this.dataLabel7.Name = "dataLabel7";
            this.dataLabel7.PrimaryKey = false;
            this.dataLabel7.ReadOnly = true;
            this.dataLabel7.Size = new System.Drawing.Size(201, 83);
            this.dataLabel7.SysTypeCode = System.TypeCode.Int32;
            this.dataLabel7.TabIndex = 18;
            this.dataLabel7.Text = "dataLabel7";
            this.dataLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel6.Controls.Add(this.lblDateTime);
            this.panel6.Controls.Add(this.btnStart);
            this.panel6.Controls.Add(this.ckbAuto);
            this.panel6.Controls.Add(this.txtSyncSpan);
            this.panel6.Controls.Add(this.label10);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(1022, 0);
            this.panel6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(450, 421);
            this.panel6.TabIndex = 17;
            // 
            // lblDateTime
            // 
            this.lblDateTime.AutoSize = true;
            this.lblDateTime.Location = new System.Drawing.Point(66, 363);
            this.lblDateTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(71, 18);
            this.lblDateTime.TabIndex = 15;
            this.lblDateTime.Text = "label12";
            // 
            // panel8
            // 
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Controls.Add(this.label5);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(1472, 107);
            this.panel8.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 530);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1474, 373);
            this.panel2.TabIndex = 17;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lstFilesInfo);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 82);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1472, 289);
            this.panel4.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblProgress);
            this.panel3.Controls.Add(this.progressBar1);
            this.panel3.Controls.Add(this.btnOpenLog);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1472, 82);
            this.panel3.TabIndex = 0;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.DataFormatString = null;
            this.lblProgress.LinkObject = "DataContext";
            this.lblProgress.LinkProperty = "FileSyncInfo.FolderProgress";
            this.lblProgress.Location = new System.Drawing.Point(165, 40);
            this.lblProgress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProgress.MaxLength = 0;
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.PrimaryKey = false;
            this.lblProgress.ReadOnly = true;
            this.lblProgress.Size = new System.Drawing.Size(26, 18);
            this.lblProgress.SysTypeCode = System.TypeCode.Int32;
            this.lblProgress.TabIndex = 14;
            this.lblProgress.Text = "0%";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(244, 33);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(776, 34);
            this.progressBar1.TabIndex = 13;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1474, 903);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "MainForm";
            this.Text = "File Sync GUI";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstFilesInfo;
        private PWMIS.Windows.Controls.DataCheckBox ckbAuto;
        private PWMIS.Windows.Controls.CommandButon btnStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnOpenLog;
        private System.Windows.Forms.Label label10;
        private PWMIS.Windows.Controls.DataTextBox txtSyncSpan;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private PWMIS.Windows.Controls.DataLabel dataLabel1;
        private PWMIS.Windows.Controls.DataLabel dataLabel2;
        private PWMIS.Windows.Controls.DataLabel dataLabel3;
        private PWMIS.Windows.Controls.DataLabel dataLabel4;
        private PWMIS.Windows.Controls.DataLabel dataLabel5;
        private PWMIS.Windows.Controls.DataLabel dataLabel6;
        private PWMIS.Windows.Controls.DataLabel dataLabel7;
        private System.Windows.Forms.Label lblDateTime;
        private PWMIS.Windows.Controls.DataLabel lblProgress;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timer1;
        private PWMIS.Windows.Controls.DataLabel dataLabel8;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.BindingSource bindingSource1;
    }
}

