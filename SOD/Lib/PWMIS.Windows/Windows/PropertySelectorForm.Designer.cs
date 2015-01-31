namespace PWMIS.Windows
{
    partial class PropertySelectorForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtAssembly = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFullTypeName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbProperty = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancle = new System.Windows.Forms.Button();
            this.btnFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cmbClassName = new System.Windows.Forms.ComboBox();
            this.ckbEntity = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(209, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "程序集（请选择程序使用的程序集）：";
            // 
            // txtAssembly
            // 
            this.txtAssembly.Location = new System.Drawing.Point(17, 47);
            this.txtAssembly.Name = "txtAssembly";
            this.txtAssembly.Size = new System.Drawing.Size(320, 21);
            this.txtAssembly.TabIndex = 1;
            this.txtAssembly.TextChanged += new System.EventHandler(this.txtAssembly_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "类型名称：";
            // 
            // txtFullTypeName
            // 
            this.txtFullTypeName.Location = new System.Drawing.Point(89, 84);
            this.txtFullTypeName.Name = "txtFullTypeName";
            this.txtFullTypeName.Size = new System.Drawing.Size(248, 21);
            this.txtFullTypeName.TabIndex = 3;
            this.txtFullTypeName.TextChanged += new System.EventHandler(this.txtFullTypeName_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "属性名称：";
            // 
            // cmbProperty
            // 
            this.cmbProperty.FormattingEnabled = true;
            this.cmbProperty.Location = new System.Drawing.Point(17, 169);
            this.cmbProperty.Name = "cmbProperty";
            this.cmbProperty.Size = new System.Drawing.Size(320, 20);
            this.cmbProperty.TabIndex = 5;
            this.cmbProperty.SelectedIndexChanged += new System.EventHandler(this.cmbProperty_SelectedIndexChanged);
            this.cmbProperty.Enter += new System.EventHandler(this.cmbProperty_Enter);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(50, 237);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancle.Location = new System.Drawing.Point(226, 237);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 23);
            this.btnCancle.TabIndex = 7;
            this.btnCancle.Text = "取消";
            this.btnCancle.UseVisualStyleBackColor = true;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(264, 18);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(75, 23);
            this.btnFile.TabIndex = 8;
            this.btnFile.Text = "浏览";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // cmbClassName
            // 
            this.cmbClassName.FormattingEnabled = true;
            this.cmbClassName.Location = new System.Drawing.Point(17, 112);
            this.cmbClassName.Name = "cmbClassName";
            this.cmbClassName.Size = new System.Drawing.Size(320, 20);
            this.cmbClassName.TabIndex = 9;
            this.cmbClassName.SelectedIndexChanged += new System.EventHandler(this.cmbClassName_SelectedIndexChanged);
            this.cmbClassName.Enter += new System.EventHandler(this.cmbClassName_Enter);
            // 
            // ckbEntity
            // 
            this.ckbEntity.AutoSize = true;
            this.ckbEntity.Location = new System.Drawing.Point(15, 200);
            this.ckbEntity.Name = "ckbEntity";
            this.ckbEntity.Size = new System.Drawing.Size(216, 16);
            this.ckbEntity.TabIndex = 10;
            this.ckbEntity.Text = "使用实体类的数据映射（表和字段）";
            this.ckbEntity.UseVisualStyleBackColor = true;
            // 
            // PropertySelectorForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancle;
            this.ClientSize = new System.Drawing.Size(366, 288);
            this.Controls.Add(this.ckbEntity);
            this.Controls.Add(this.cmbClassName);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cmbProperty);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtFullTypeName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAssembly);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PropertySelectorForm";
            this.Text = "PDF.NET WinForm 数据控件属性选择器";
            this.Load += new System.EventHandler(this.PropertySelectorForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAssembly;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFullTypeName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbProperty;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox cmbClassName;
        private System.Windows.Forms.CheckBox ckbEntity;
    }
}