namespace WinFormMvvm
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
            this.dataTextBox1 = new PWMIS.Windows.Controls.DataTextBox();
            this.dataLabel1 = new PWMIS.Windows.Controls.DataLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new PWMIS.Windows.Controls.DataListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dataTextBox1
            // 
            this.dataTextBox1.ClientValidationFunctionString = null;
            this.dataTextBox1.CompareSymbol = null;
            this.dataTextBox1.DataFormatString = null;
            this.dataTextBox1.ErrorMessage = null;
            this.dataTextBox1.LinkObject = "DataContext";
            this.dataTextBox1.LinkProperty = "CurrentUser.Name";
            this.dataTextBox1.Location = new System.Drawing.Point(106, 89);
            this.dataTextBox1.MessageType = PWMIS.Windows.Validate.EnumMessageType.层;
            this.dataTextBox1.Name = "dataTextBox1";
            this.dataTextBox1.OftenType = "无";
            this.dataTextBox1.PrimaryKey = false;
            this.dataTextBox1.QueryFormatString = null;
            this.dataTextBox1.RegexName = null;
            this.dataTextBox1.RegexString = null;
            this.dataTextBox1.Size = new System.Drawing.Size(262, 21);
            this.dataTextBox1.SysTypeCode = System.TypeCode.String;
            this.dataTextBox1.TabIndex = 0;
            this.dataTextBox1.Type = PWMIS.Windows.ValidationDataType.String;
            // 
            // dataLabel1
            // 
            this.dataLabel1.AutoSize = true;
            this.dataLabel1.DataFormatString = null;
            this.dataLabel1.LinkObject = "DataContext";
            this.dataLabel1.LinkProperty = "CurrentUser.ID";
            this.dataLabel1.Location = new System.Drawing.Point(104, 57);
            this.dataLabel1.MaxLength = 0;
            this.dataLabel1.Name = "dataLabel1";
            this.dataLabel1.PrimaryKey = false;
            this.dataLabel1.ReadOnly = true;
            this.dataLabel1.Size = new System.Drawing.Size(65, 12);
            this.dataLabel1.TabIndex = 1;
            this.dataLabel1.Text = "dataLabel1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "ID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Name:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(106, 143);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "添加";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.CompareSymbol = null;
            this.listBox1.DisplayMember = "Name";
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.LinkObject = "DataContext";
            this.listBox1.LinkProperty = "SelectedUserID";
            this.listBox1.Location = new System.Drawing.Point(106, 199);
            this.listBox1.Name = "listBox1";
            this.listBox1.PrimaryKey = false;
            this.listBox1.QueryFormatString = null;
            this.listBox1.ReadOnly = true;
            this.listBox1.Size = new System.Drawing.Size(262, 136);
            this.listBox1.SysTypeCode = System.TypeCode.Empty;
            this.listBox1.TabIndex = 5;
            this.listBox1.ValueMember = "ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 199);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Submited List:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(199, 143);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "更新";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(293, 143);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "删除";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 362);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataLabel1);
            this.Controls.Add(this.dataTextBox1);
            this.Name = "Form1";
            this.Text = "SOD MVVM Test Form";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PWMIS.Windows.Controls.DataTextBox dataTextBox1;
        private PWMIS.Windows.Controls.DataLabel dataLabel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private PWMIS.Windows.Controls.DataListBox listBox1;
        private System.Windows.Forms.Button button3;
    }
}

