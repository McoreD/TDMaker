using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace TDMakerLib
{
    class ColorDialogEditor : FileNameEditor
    {
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || provider == null)
            {
                return base.EditValue(context, provider, value);
            }
            string hexColor = "";
            ColorDialog cd = new ColorDialog();
            cd.FullOpen = true;
            cd.AnyColor = true;

            if (cd.ShowDialog() == DialogResult.OK)
            {
                hexColor = string.Format("0x{0:X8}", cd.Color.ToArgb());
                hexColor = hexColor.Substring(hexColor.Length - 6, 6);
                value = hexColor;
            }
            return value;
        }
    }
}
