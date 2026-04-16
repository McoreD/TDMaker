using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace TDMakerLib
{
    internal class ExeFileNameEditor : FileNameEditor
    {
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || provider == null)
            {
                return base.EditValue(context, provider, value);
            }
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Browse for Thumbnailer executable...";
            dlg.Filter = "Applications (*.exe)|*.exe";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                value = dlg.FileName;
            }
            return value;
        }
    }
}