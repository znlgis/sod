using PWMIS.Common;
using PWMIS.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PWMIS.Web
{
    public class PropertyUITypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }

            return base.GetEditStyle(context);
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorService = null;

            if (context != null && context.Instance != null && provider != null)
            {
                editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (editorService != null)
                {
                    IDataControl control = (IDataControl)context.Instance;
                    PropertySelectorForm dlg = new PropertySelectorForm(control);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        value = dlg.PropertyName;
                        control.LinkObject = control.LinkObject+" ";
                        return value;
                    }
                }
            }

            return value;
        }

    }

    public class LinkObjectUITypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }

            return base.GetEditStyle(context);
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorService = null;

            if (context != null && context.Instance != null && provider != null)
            {
                editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (editorService != null)
                {
                    IDataControl control = (IDataControl)context.Instance;
                    //PropertySelectorForm dlg = new PropertySelectorForm(control);
                    if (!string.IsNullOrEmpty( control.LinkObject))
                    {
                        System.TypeCode code = control.SysTypeCode;
                        if (code != TypeCode.Empty)
                            MessageBox.Show("请先选择SysTypeCode 属性并设置为" + code.ToString ());
                        else
                            MessageBox.Show("请先选择SysTypeCode 属性并设置合适的值" );

                        control.SysTypeCode = TypeCode.Empty;
                        return control.LinkObject.Trim();
                    }
                    else
                    {
                        MessageBox.Show("请先选择LinkProperty 属性设置值并设置合适的SysTypeCode");
                        control.SysTypeCode = TypeCode.Empty;
                        return "";
                    }
                }
            }

            return value;
        }

    }
}
