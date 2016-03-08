using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace TDMakerLib
{
    internal class FontDialogEditor : FileNameEditor
    {
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || provider == null)
            {
                return base.EditValue(context, provider, value);
            }
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                value = fd.Font.Name;
            }
            return value;
        }
    }
}