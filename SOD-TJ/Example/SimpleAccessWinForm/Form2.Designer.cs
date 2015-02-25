namespace SimpleAccessWinForm
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.dataTextBox1 = new PWMIS.Windows.Controls.DataTextBox();
            this.dataCalendar1 = new PWMIS.Windows.Controls.DataCalendar();
            this.ddlUserType = new PWMIS.Windows.Controls.DataDropDownList();
            this.userTypeInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dtbUserName = new PWMIS.Windows.Controls.DataTextBox();
            this.dlbUID = new PWMIS.Windows.Controls.DataLabel();
            ((System.ComponentModel.ISupportInitialize)(this.userTypeInfoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "*用户名(唯一)：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "用户标识：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "用户类型：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "注册时间：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(44, 191);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "消费金额：";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(168, 255);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 5;
            this.btnSubmit.Text = "确定";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // dataTextBox1
            // 
            this.dataTextBox1.ClientValidationFunctionString = null;
            this.dataTextBox1.CompareSymbol = null;
            this.dataTextBox1.DataFormatString = null;
            this.dataTextBox1.ErrorMessage = null;
            this.dataTextBox1.LinkObject = "会员用户表";
            this.dataTextBox1.LinkProperty = "消费金额";
            this.dataTextBox1.Location = new System.Drawing.Point(168, 181);
            this.dataTextBox1.MessageType = PWMIS.Windows.Validate.EnumMessageType.层;
            this.dataTextBox1.Name = "dataTextBox1";
            this.dataTextBox1.OftenType = "无";
            this.dataTextBox1.PrimaryKey = false;
            this.dataTextBox1.QueryFormatString = null;
            this.dataTextBox1.RegexName = null;
            this.dataTextBox1.RegexString = null;
            this.dataTextBox1.Size = new System.Drawing.Size(100, 21);
            this.dataTextBox1.SysTypeCode = System.TypeCode.Single;
            this.dataTextBox1.TabIndex = 10;
            this.dataTextBox1.Type = PWMIS.Windows.ValidationDataType.String;
            // 
            // dataCalendar1
            // 
            this.dataCalendar1.CompareSymbol = null;
            this.dataCalendar1.LinkObject = "会员用户表";
            this.dataCalendar1.LinkProperty = "注册时间";
            this.dataCalendar1.Location = new System.Drawing.Point(168, 144);
            this.dataCalendar1.Name = "dataCalendar1";
            this.dataCalendar1.PrimaryKey = false;
            this.dataCalendar1.QueryFormatString = null;
            this.dataCalendar1.ReadOnly = false;
            this.dataCalendar1.Size = new System.Drawing.Size(200, 21);
            this.dataCalendar1.SysTypeCode = System.TypeCode.DateTime;
            this.dataCalendar1.TabIndex = 9;
            // 
            // ddlUserType
            // 
            this.ddlUserType.CompareSymbol = null;
            this.ddlUserType.DataSource = this.userTypeInfoBindingSource;
            this.ddlUserType.DisplayMember = "UserTypeName";
            this.ddlUserType.FormattingEnabled = true;
            this.ddlUserType.LinkObject = "会员用户表";
            this.ddlUserType.LinkProperty = "用户类型";
            this.ddlUserType.Location = new System.Drawing.Point(168, 112);
            this.ddlUserType.Name = "ddlUserType";
            this.ddlUserType.PrimaryKey = false;
            this.ddlUserType.QueryFormatString = null;
            this.ddlUserType.ReadOnly = false;
            this.ddlUserType.Size = new System.Drawing.Size(121, 20);
            this.ddlUserType.SysTypeCode = System.TypeCode.Int32;
            this.ddlUserType.TabIndex = 8;
            this.ddlUserType.ValueMember = "UserTypeCode";
            // 
            // userTypeInfoBindingSource
            // 
            this.userTypeInfoBindingSource.DataSource = typeof(SimpleAccessWinForm.UserTypeInfo);
            // 
            // dtbUserName
            // 
            this.dtbUserName.ClientValidationFunctionString = null;
            this.dtbUserName.CompareSymbol = null;
            this.dtbUserName.DataFormatString = null;
            this.dtbUserName.ErrorMessage = "要求内容为中文字符";
            this.dtbUserName.IsNull = false;
            this.dtbUserName.LinkObject = "会员用户表";
            this.dtbUserName.LinkProperty = "用户名";
            this.dtbUserName.Location = new System.Drawing.Point(168, 76);
            this.dtbUserName.MaxLength = 50;
            this.dtbUserName.MessageType = PWMIS.Windows.Validate.EnumMessageType.提示框;
            this.dtbUserName.Name = "dtbUserName";
            this.dtbUserName.OftenType = "中文字符";
            this.dtbUserName.PrimaryKey = false;
            this.dtbUserName.QueryFormatString = null;
            this.dtbUserName.RegexName = "用户名";
            this.dtbUserName.RegexString = "^[\\u4e00-\\u9fa5](\\s*[\\u4e00-\\u9fa5])*$$";
            this.dtbUserName.Size = new System.Drawing.Size(201, 21);
            this.dtbUserName.SysTypeCode = System.TypeCode.String;
            this.dtbUserName.TabIndex = 7;
            this.dtbUserName.Type = PWMIS.Windows.ValidationDataType.String;
            // 
            // dlbUID
            // 
            this.dlbUID.AutoSize = true;
            this.dlbUID.DataFormatString = null;
            this.dlbUID.LinkObject = "会员用户表";
            this.dlbUID.LinkProperty = "标识";
            this.dlbUID.Location = new System.Drawing.Point(171, 42);
            this.dlbUID.MaxLength = 0;
            this.dlbUID.Name = "dlbUID";
            this.dlbUID.PrimaryKey = true;
            this.dlbUID.ReadOnly = true;
            this.dlbUID.Size = new System.Drawing.Size(11, 12);
            this.dlbUID.SysTypeCode = System.TypeCode.Int32;
            this.dlbUID.TabIndex = 6;
            this.dlbUID.Text = "0";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 338);
            this.Controls.Add(this.dataTextBox1);
            this.Controls.Add(this.dataCalendar1);
            this.Controls.Add(this.ddlUserType);
            this.Controls.Add(this.dtbUserName);
            this.Controls.Add(this.dlbUID);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form2";
            this.Text = "会员信息";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.userTypeInfoBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSubmit;
        private PWMIS.Windows.Controls.DataLabel dlbUID;
        private PWMIS.Windows.Controls.DataTextBox dtbUserName;
        private PWMIS.Windows.Controls.DataDropDownList ddlUserType;
        private PWMIS.Windows.Controls.DataCalendar dataCalendar1;
        private PWMIS.Windows.Controls.DataTextBox dataTextBox1;
        private System.Windows.Forms.BindingSource userTypeInfoBindingSource;
    }
}