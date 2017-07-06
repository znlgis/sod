namespace WinClient
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtA = new System.Windows.Forms.TextBox();
            this.txtB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSub = new System.Windows.Forms.Button();
            this.btnVoid = new System.Windows.Forms.Button();
            this.btnServerTime = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSerivceUri = new System.Windows.Forms.TextBox();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btnAlarmClock = new System.Windows.Forms.Button();
            this.btnServerText = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtA
            // 
            this.txtA.Location = new System.Drawing.Point(39, 83);
            this.txtA.Name = "txtA";
            this.txtA.Size = new System.Drawing.Size(100, 21);
            this.txtA.TabIndex = 0;
            this.txtA.Text = "1";
            // 
            // txtB
            // 
            this.txtB.Location = new System.Drawing.Point(179, 82);
            this.txtB.Name = "txtB";
            this.txtB.Size = new System.Drawing.Size(100, 21);
            this.txtB.TabIndex = 1;
            this.txtB.Text = "2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "a=";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(145, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = ",b=";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(312, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "服务的结果是：";
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(407, 92);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(59, 12);
            this.lblResult.TabIndex = 5;
            this.lblResult.Text = "lblResult";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(39, 127);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "计算 a+b";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSub
            // 
            this.btnSub.Location = new System.Drawing.Point(179, 127);
            this.btnSub.Name = "btnSub";
            this.btnSub.Size = new System.Drawing.Size(75, 23);
            this.btnSub.TabIndex = 7;
            this.btnSub.Text = "计算 a-b";
            this.btnSub.UseVisualStyleBackColor = true;
            this.btnSub.Click += new System.EventHandler(this.btnSub_Click);
            // 
            // btnVoid
            // 
            this.btnVoid.Location = new System.Drawing.Point(39, 171);
            this.btnVoid.Name = "btnVoid";
            this.btnVoid.Size = new System.Drawing.Size(137, 23);
            this.btnVoid.TabIndex = 8;
            this.btnVoid.Text = "无返回值的服务调用";
            this.btnVoid.UseVisualStyleBackColor = true;
            this.btnVoid.Click += new System.EventHandler(this.btnVoid_Click);
            // 
            // btnServerTime
            // 
            this.btnServerTime.Location = new System.Drawing.Point(314, 171);
            this.btnServerTime.Name = "btnServerTime";
            this.btnServerTime.Size = new System.Drawing.Size(152, 23);
            this.btnServerTime.TabIndex = 9;
            this.btnServerTime.Text = "订阅服务器时间";
            this.btnServerTime.UseVisualStyleBackColor = true;
            this.btnServerTime.Click += new System.EventHandler(this.btnServerTime_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(215, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "消息服务框架测试程序 ，服务器地址：";
            // 
            // txtSerivceUri
            // 
            this.txtSerivceUri.Location = new System.Drawing.Point(39, 47);
            this.txtSerivceUri.Name = "txtSerivceUri";
            this.txtSerivceUri.Size = new System.Drawing.Size(240, 21);
            this.txtSerivceUri.TabIndex = 11;
            this.txtSerivceUri.Text = "net.tcp://127.0.0.1:8888";
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(314, 47);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(152, 23);
            this.btnStartServer.TabIndex = 12;
            this.btnStartServer.Text = "启动服务器";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker1.Location = new System.Drawing.Point(39, 223);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowUpDown = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 21);
            this.dateTimePicker1.TabIndex = 13;
            // 
            // btnAlarmClock
            // 
            this.btnAlarmClock.Location = new System.Drawing.Point(311, 224);
            this.btnAlarmClock.Name = "btnAlarmClock";
            this.btnAlarmClock.Size = new System.Drawing.Size(154, 26);
            this.btnAlarmClock.TabIndex = 14;
            this.btnAlarmClock.Text = "订阅闹钟";
            this.btnAlarmClock.UseVisualStyleBackColor = true;
            this.btnAlarmClock.Click += new System.EventHandler(this.btnAlarmClock_Click);
            // 
            // btnServerText
            // 
            this.btnServerText.Location = new System.Drawing.Point(313, 127);
            this.btnServerText.Name = "btnServerText";
            this.btnServerText.Size = new System.Drawing.Size(152, 23);
            this.btnServerText.TabIndex = 15;
            this.btnServerText.Text = "订阅服务器文本";
            this.btnServerText.UseVisualStyleBackColor = true;
            this.btnServerText.Click += new System.EventHandler(this.btnServerText_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 271);
            this.Controls.Add(this.btnServerText);
            this.Controls.Add(this.btnAlarmClock);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.btnStartServer);
            this.Controls.Add(this.txtSerivceUri);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnServerTime);
            this.Controls.Add(this.btnVoid);
            this.Controls.Add(this.btnSub);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtB);
            this.Controls.Add(this.txtA);
            this.Name = "Form1";
            this.Text = "PDF.NET(http://www.pwmis.com/sqlmap)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtA;
        private System.Windows.Forms.TextBox txtB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnSub;
        private System.Windows.Forms.Button btnVoid;
        private System.Windows.Forms.Button btnServerTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSerivceUri;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button btnAlarmClock;
        private System.Windows.Forms.Button btnServerText;
    }
}

