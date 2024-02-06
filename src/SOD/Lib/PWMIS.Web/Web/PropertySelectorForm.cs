﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PWMIS.Common;
using System.Reflection;
using PWMIS.DataMap.Entity;
using PWMIS.Core;

namespace PWMIS.Windows
{
    public partial class PropertySelectorForm : Form
    {
        List<KeyValuePair<string, string>> dataList ;
        IDataControl currDataControl;
        EntityFields ef;
        Dictionary<string, string> dictNames = new Dictionary<string, string>();
        bool entityCheck = false;//检测输入的类型是否是实体类
        string currentFieldName = "";

        private string ConfigFileName
        {
            get { 
                return  Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+"\\PdfNetVSDataForm.ini";
            }
        }

        public PropertySelectorForm(IDataControl control) 
        {
            InitializeComponent();
            currDataControl = control;
            this.txtFullTypeName.Text = currDataControl.LinkObject;
        }

        /// <summary>
        /// 数据控件选择的属性名称
        /// </summary>
        public string PropertyName
        {
            get {
                if (cmbProperty.SelectedIndex >= 0)
                {
                    string propName = cmbProperty.SelectedValue.ToString();
                    if (ckbEntity.Checked)
                    {
                        //string fieldName = ef.GetPropertyField(propName);
                        //return fieldName;
                        return currentFieldName;
                    }
                    return propName;
                }
                else
                    return "";
            }
        }

        private void PropertySelectorForm_Load(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(this.ConfigFileName))
            {
                this.txtAssembly.Text = System.IO.File.ReadAllText(this.ConfigFileName);
            }
            ckbEntity.Enabled = false;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbProperty.SelectedIndex >= 0)
            {
                if (ckbEntity.Checked)
                {
                    LocalLoader assLoader =null;
                    try
                    {
                        string fullName = this.txtAssembly.Text;
                        string basePath = System.IO.Path.GetDirectoryName(fullName);
                        string propName = cmbProperty.SelectedValue.ToString();

                        assLoader = new LocalLoader(basePath);
                        assLoader.LoadAssembly(fullName);
                        var arr = assLoader.TableFieldName(this.txtFullTypeName.Text, propName);
                        if (arr != null)
                        {
                            currDataControl.LinkObject = arr[0];
                            currentFieldName = arr[1];
                        }
                        else
                        {
                            MessageBox.Show(assLoader.ErrorMessage);
                        }

                        this.DialogResult = DialogResult.OK;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        if (assLoader != null)
                            assLoader.Unload();
                    }
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    currDataControl.LinkObject = this.txtFullTypeName.Text;
                }
               
                //不能使用SelectText 在不是只读的下拉框的情况下
                string typeCodeName = cmbProperty.Text.ToString().Split('|')[1];
                currDataControl.SysTypeCode = (TypeCode)Enum.Parse(typeof(TypeCode), typeCodeName);

                System.IO.File.WriteAllText(ConfigFileName, txtAssembly.Text);
                this.Close();
            }
            else
            {
                MessageBox.Show("请选择属性！");   
            }
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.FileName = "";
            this.openFileDialog1.Filter = ".NET程序集|*.dll|.NET可执行程序|*.exe";
            this.openFileDialog1.ShowDialog();
            this.txtAssembly.Text = this.openFileDialog1.FileName;
        }


        private void txtAssembly_TextChanged(object sender, EventArgs e)
        {
            dataList = null;
            cmbClassName.Items.Clear();
        }

        private void txtFullTypeName_TextChanged(object sender, EventArgs e)
        {
            dataList = null;
        }

        private void cmbClassName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dictNames.ContainsKey(cmbClassName.Text))
                this.txtFullTypeName.Text = dictNames[ cmbClassName.Text];
        }

        private void cmbProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }


        private void cmbClassName_Enter(object sender, EventArgs e)
        {
            if (cmbClassName.Items.Count == 0)
            {
                if (this.txtAssembly.Text == "")
                {
                    MessageBox.Show("请输入程序集文件路径！");
                    return;
                }
                else
                {
                    LoadClassNames(this.txtAssembly.Text);
                }
            }
        }

        private void cmbProperty_Enter(object sender, EventArgs e)
        {
            if (dataList == null)
            {
                if (this.txtAssembly.Text == "")
                {
                    MessageBox.Show("请输入程序集文件路径！");
                    return;
                }
                else
                {
                    LocalLoader assLoader = null;
                    try
                    {
                        string fullName = this.txtAssembly.Text;
                        string basePath = System.IO.Path.GetDirectoryName(fullName);
                        string coreFile = System.IO.Path.Combine(basePath, "PWMIS.Core.dll");
                        if (!System.IO.File.Exists(coreFile))
                            throw new Exception("未找到当前指定程序集目录下面的PDF.NET核心类库文件 PWMIS.Core.dll");

                        assLoader = new LocalLoader(basePath);
                        assLoader.LoadAssembly(fullName);
                        dataList = assLoader.GetAllPropertyNames(this.txtFullTypeName.Text);
                        if (dataList != null)
                        {
                            cmbProperty.DisplayMember = "Key";
                            cmbProperty.ValueMember = "Value";
                            cmbProperty.DataSource = dataList;

                            this.ckbEntity.Enabled = assLoader.IsEntityClass;
                        }
                        else
                        {
                            MessageBox.Show(assLoader.ErrorMessage);
                            this.ckbEntity.Enabled = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    finally
                    {
                        if (assLoader != null)
                            assLoader.Unload();
                    }
                }
            }
        }
      

        /// <summary>
        /// 加载程序集中的全部类型信息到字典中，FullClassName为Key，ClassName为Value
        /// </summary>
        /// <param name="fullName">程序集的名称</param>
        private void LoadClassNames(string fullName)
        {
            string basePath = System.IO.Path.GetDirectoryName(fullName);
            string coreFile = System.IO.Path.Combine(basePath, "PWMIS.Core.dll");
            if (!System.IO.File.Exists(coreFile))
                throw new Exception("未找到当前指定程序集目录下面的PDF.NET核心类库文件 PWMIS.Core.dll");

            LocalLoader assLoader = null;
            try
            {
                assLoader = new LocalLoader(basePath);
                assLoader.LoadAssembly(fullName);
                string[] types = assLoader.GetAllTypeNames;

                dictNames.Clear();
                List<string> names = new List<string>();
                foreach (string t in types)
                {
                    string[] arr = t.Split(',');
                    names.Add(arr[0]);
                    dictNames.Add(arr[0], arr[1]);
                }
                names.Sort();
                cmbClassName.DataSource = names;//类型的全名称
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                if (assLoader != null)
                    assLoader.Unload();
            }
        }

    }
}
