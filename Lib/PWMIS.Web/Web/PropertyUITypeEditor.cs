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
                        return value;
                    }
                }
            }

            return value;
        }

    }
}
