using PWMIS.Common;
using PWMIS.DataForms.Adapter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormMvvm
{
    public partial class MvvmForm : Form
    {
        private Dictionary<object, CommandMethod> dictCommand;
        public delegate void CommandMethod();

        public MvvmForm()
        {
            InitializeComponent();
            dictCommand = new Dictionary<object, CommandMethod>();
        }

        public void BindDataControls(Control.ControlCollection controls)
        {
            var dataControls = MyWinForm.GetIBControls(controls);
            foreach (IDataControl control in dataControls)
            {
                //control.LinkObject 这里都是 "DataContext"
                object dataSource = GetInstanceByMemberName(control.LinkObject);
                if (control is TextBox)
                {
                    ((TextBox)control).DataBindings.Add("Text", dataSource, control.LinkProperty);
                }
                if (control is Label)
                {
                    ((Label)control).DataBindings.Add("Text", dataSource, control.LinkProperty);
                }
                if (control is ListBox)
                {
                    ((ListBox)control).DataBindings.Add("SelectedValue", dataSource, control.LinkProperty, false, DataSourceUpdateMode.OnPropertyChanged);
                }
            }
        }
       

        public void BindCommandControls(Control control,CommandMethod command)
        {
            if (control is Button)
            {
                dictCommand.Add(control, command);
                ((Button)control).Click += (sender, e) => {
                    dictCommand[sender](); 
                };
            }
        }


        protected virtual object GetInstanceByMemberName(string memberName)
        {
           var obj= this.GetType().InvokeMember(memberName, 
                System.Reflection.BindingFlags.Instance | 
                System.Reflection.BindingFlags.GetField |  
                System.Reflection.BindingFlags.GetProperty |
                System.Reflection.BindingFlags.NonPublic | 
                System.Reflection.BindingFlags.Public , 
                null, this, null);
           if (obj == null)
               throw new Exception("当前窗体未找到名称为 " + memberName + " 的实例成员，请检查数据绑定控件的 LinkObject 属性。");

           return obj;
            
        }
    }
}
