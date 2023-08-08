using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using PWMIS.Common;
using PWMIS.Core;
using PWMIS.DataMap.Entity;

namespace PWMIS.Windows
{
    public partial class PropertySelectorForm : Form
    {
        private readonly IDataControl currDataControl;
        private readonly Dictionary<string, string> dictNames = new Dictionary<string, string>();
        private string currentFieldName = "";
        private List<KeyValuePair<string, string>> dataList;
        private EntityFields ef;
        private bool entityCheck = false; //检测输入的类型是否是实体类

        public PropertySelectorForm(IDataControl control)
        {
            InitializeComponent();
            currDataControl = control;
            txtFullTypeName.Text = currDataControl.LinkObject;
        }

        private string ConfigFileName =>
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\PdfNetVSDataForm.ini";

        /// <summary>
        ///     数据控件选择的属性名称
        /// </summary>
        public string PropertyName
        {
            get
            {
                if (cmbProperty.SelectedIndex >= 0)
                {
                    var propName = cmbProperty.SelectedValue.ToString();
                    if (ckbEntity.Checked)
                        //string fieldName = ef.GetPropertyField(propName);
                        //return fieldName;
                        return currentFieldName;
                    return propName;
                }

                return "";
            }
        }

        private void PropertySelectorForm_Load(object sender, EventArgs e)
        {
            if (File.Exists(ConfigFileName)) txtAssembly.Text = File.ReadAllText(ConfigFileName);
            ckbEntity.Enabled = false;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbProperty.SelectedIndex >= 0)
            {
                if (ckbEntity.Checked)
                {
                    LocalLoader assLoader = null;
                    try
                    {
                        var fullName = txtAssembly.Text;
                        var basePath = Path.GetDirectoryName(fullName);
                        var propName = cmbProperty.SelectedValue.ToString();

                        assLoader = new LocalLoader(basePath);
                        assLoader.LoadAssembly(fullName);
                        var arr = assLoader.TableFieldName(txtFullTypeName.Text, propName);
                        if (arr != null)
                        {
                            currDataControl.LinkObject = arr[0];
                            currentFieldName = arr[1];
                        }
                        else
                        {
                            MessageBox.Show(assLoader.ErrorMessage);
                        }

                        DialogResult = DialogResult.OK;
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
                    DialogResult = DialogResult.OK;
                    currDataControl.LinkObject = txtFullTypeName.Text;
                }

                //不能使用SelectText 在不是只读的下拉框的情况下
                var typeCodeName = cmbProperty.Text.Split('|')[1];
                currDataControl.SysTypeCode = (TypeCode)Enum.Parse(typeof(TypeCode), typeCodeName);

                File.WriteAllText(ConfigFileName, txtAssembly.Text);
                Close();
            }
            else
            {
                MessageBox.Show("请选择属性！");
            }
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = ".NET程序集|*.dll|.NET可执行程序|*.exe";
            openFileDialog1.ShowDialog();
            txtAssembly.Text = openFileDialog1.FileName;
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
                txtFullTypeName.Text = dictNames[cmbClassName.Text];
        }

        private void cmbProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
        }


        private void cmbClassName_Enter(object sender, EventArgs e)
        {
            if (cmbClassName.Items.Count == 0)
            {
                if (txtAssembly.Text == "")
                {
                    MessageBox.Show("请输入程序集文件路径！");
                    return;
                }

                LoadClassNames(txtAssembly.Text);
            }
        }

        private void cmbProperty_Enter(object sender, EventArgs e)
        {
            if (dataList == null)
            {
                if (txtAssembly.Text == "")
                {
                    MessageBox.Show("请输入程序集文件路径！");
                    return;
                }

                LocalLoader assLoader = null;
                try
                {
                    var fullName = txtAssembly.Text;
                    var basePath = Path.GetDirectoryName(fullName);
                    var coreFile = Path.Combine(basePath, "PWMIS.Core.dll");
                    if (!File.Exists(coreFile))
                        throw new Exception("未找到当前指定程序集目录下面的PDF.NET核心类库文件 PWMIS.Core.dll");

                    assLoader = new LocalLoader(basePath);
                    assLoader.LoadAssembly(fullName);
                    dataList = assLoader.GetAllPropertyNames(txtFullTypeName.Text);
                    if (dataList != null)
                    {
                        cmbProperty.DisplayMember = "Key";
                        cmbProperty.ValueMember = "Value";
                        cmbProperty.DataSource = dataList;

                        ckbEntity.Enabled = assLoader.IsEntityClass;
                    }
                    else
                    {
                        MessageBox.Show(assLoader.ErrorMessage);
                        ckbEntity.Enabled = false;
                    }
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
        }


        /// <summary>
        ///     加载程序集中的全部类型信息到字典中，FullClassName为Key，ClassName为Value
        /// </summary>
        /// <param name="fullName">程序集的名称</param>
        private void LoadClassNames(string fullName)
        {
            var basePath = Path.GetDirectoryName(fullName);
            var coreFile = Path.Combine(basePath, "PWMIS.Core.dll");
            if (!File.Exists(coreFile))
                throw new Exception("未找到当前指定程序集目录下面的PDF.NET核心类库文件 PWMIS.Core.dll");

            LocalLoader assLoader = null;
            try
            {
                assLoader = new LocalLoader(basePath);
                assLoader.LoadAssembly(fullName);
                var types = assLoader.GetAllTypeNames;

                dictNames.Clear();
                var names = new List<string>();
                foreach (var t in types)
                {
                    var arr = t.Split(',');
                    names.Add(arr[0]);
                    dictNames.Add(arr[0], arr[1]);
                }

                names.Sort();
                cmbClassName.DataSource = names; //类型的全名称
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
    }
}