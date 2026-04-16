using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using TDMakerLib;

public class StringListConverter : TypeConverter
{
    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
        return true; // display drop
    }

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
        return true; // drop-down vs combo
    }

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
        string[] dirs = Directory.GetDirectories(App.TemplatesDir);
        string[] templateNames = new string[dirs.Length];
        for (int i = 0; i < templateNames.Length; i++)
        {
            templateNames[i] = Path.GetFileName(dirs[i]);
        }

        return new StandardValuesCollection(templateNames);
    }
}