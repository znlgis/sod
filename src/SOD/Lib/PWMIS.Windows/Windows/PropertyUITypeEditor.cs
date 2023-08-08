using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using PWMIS.Common;

namespace PWMIS.Windows
{
    public class PropertyUITypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null) return UITypeEditorEditStyle.Modal;

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
                    var control = (IDataControl)context.Instance;
                    var dlg = new PropertySelectorForm(control);
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