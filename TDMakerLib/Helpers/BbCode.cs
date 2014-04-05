using System;
using System.Collections.Generic;
using System.Text;

namespace TDMakerLib
{
    public abstract class BbCode
    {
        public static string Img(string url)
        {
            return string.Format("[img]{0}[/img]", url);
        }

        public static string Bold(string txt)
        {
            return string.Format("[b]{0}[/b]", txt);
        }

        public static string BoldItalic(string txt)
        {
            return string.Format("[b]{0}[/b]", Italic(txt));
        }

        public static string Pre(string txt)
        {
            return string.Format("[pre]{0}[/pre]", txt);
        }

        public static string Italic(string txt)
        {
            return string.Format("[i]{0}[/i]", txt);
        }

        public static string Underline(string txt)
        {
            return string.Format("[u]{0}[/u]", txt);
        }

        public static string Size(int size, string txt)
        {
            return string.Format("[size={0}]{1}[/size]", size, txt);
        }

        public static string AlignCenter(string txt)
        {
            return string.Format("[align=center]{0}[/align]", txt);
        }
    }
}
