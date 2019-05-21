namespace WinFormTest
{
    partial class Form2
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
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form2
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(10, 21);
            this.ClientSize = new System.Drawing.Size(1226, 466);
            this.Name = "Form2";
            this.Text = "Form4";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);

        }
        #endregion

    }
}

