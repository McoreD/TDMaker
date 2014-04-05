#region License Information (GPL v2)

/*
    TDMaker - A program that allows you to upload screenshots in one keystroke.
    Copyright (C) 2008-2009  Brandon Zimmerman

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v2)

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TDMakerLib
{
    public static class Extensions
    {
        public static int ToInt(this string str)
        {
            return Convert.ToInt32(str);
        }

        public static int Mid(this int number, int min, int max)
        {
            return Math.Min(Math.Max(number, min), max);
        }

        public static Point Intersect(this Point point, Rectangle rect)
        {
            if (point.X < rect.X)
            {
                point.X = rect.X;
            }
            else if (point.X > rect.Right)
            {
                point.X = rect.Right;
            }
            if (point.Y < rect.Y)
            {
                point.Y = rect.Y;
            }
            else if (point.Y > rect.Bottom)
            {
                point.Y = rect.Bottom;
            }
            return point;
        }

        public static string ToSpecialString(this Keys key)
        {
            string[] split = key.ToString().Split(new[] { ", " }, StringSplitOptions.None).Reverse().ToArray();
            return string.Join(" + ", split);
        }
    }
}