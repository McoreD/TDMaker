using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace TDMakerLib.Templates
{
    public class MIFieldValue
    {
        public string FieldWithoutPadding { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }

        private char paddingChar = '%';

        public MIFieldValue(string field, string value, string prefix)
        {
            Value = value;
            TextInfo ti = new CultureInfo("en-US", false).TextInfo;
            string tempName = string.Empty;

            foreach (char c in field)
            {
                if (Char.IsLetterOrDigit(c))
                {
                    tempName += c;
                }
                else if (c == '(' || c == ')')
                {
                    continue;
                }
                else
                {
                    tempName += " ";
                }
            }
            tempName = ti.ToTitleCase(tempName.Trim()).Replace(" ", "");
            FieldWithoutPadding = string.Format("{0}_{1}", prefix, tempName);
            Field = string.Format("{0}{1}{2}", paddingChar, FieldWithoutPadding, paddingChar);
        }

        public override string ToString()
        {
            return string.Format("{0} = {1}", Field, Value);
        }
    }
}