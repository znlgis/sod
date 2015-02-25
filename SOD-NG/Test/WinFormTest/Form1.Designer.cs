namespace WinFormTest
{
    partial class Form1
    {
        private System.Windows.Forms.RichTextBox sqlRichTextBox;
        private System.Windows.Forms.Button button1;
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
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.sqlRichTextBox = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // sqlRichTextBox
            // 
            this.sqlRichTextBox.Location = new System.Drawing.Point(56, 32);
            this.sqlRichTextBox.Name = "sqlRichTextBox";
            this.sqlRichTextBox.Size = new System.Drawing.Size(569, 346);
            this.sqlRichTextBox.TabIndex = 0;
            this.sqlRichTextBox.Text = "select * from table1";
            this.sqlRichTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sqlRichTextBox_KeyDown);
            this.sqlRichTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.sqlRichTextBox_KeyPress);
            this.sqlRichTextBox.TextChanged += new System.EventHandler(this.sqlRichTextBox_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(677, 32);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(677, 89);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "异步写文件";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(830, 416);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.sqlRichTextBox);
            this.Name = "Form1";
            this.Text = "Form4";
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button button2;

    }
}

