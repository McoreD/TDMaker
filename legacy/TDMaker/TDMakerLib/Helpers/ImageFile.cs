#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (C) 2012 ShareX Developers

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

#endregion License Information (GPL v3)

using System;

namespace UploadersLib.HelperClasses
{
    public class ImageFile : IComparable<ImageFile>
    {
        /// <summary>
        /// Name of the Screenshot with extension
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Local file path of the Screenshot
        /// </summary>
        public string LocalFilePath { get; set; }

        public string URI { get; set; }

        /// <summary>
        /// Size in Mebibytes (MiB) = 1024 KiB
        /// </summary>
        public decimal Size { get; set; }

        public DateTime DateModified { get; set; }

        public string Source { get; set; }

        public ImageFile()
        {
        }

        public ImageFile(string filePath)
        {
            this.LocalFilePath = filePath;
            this.Name = System.IO.Path.GetFileName(filePath);
            System.IO.FileInfo fi = new System.IO.FileInfo(filePath);
            this.DateModified = fi.LastWriteTime;
            this.Size = fi.Length / (decimal)(1024 * 1024);
        }

        #region IComparable<ImageFile> Members

        int IComparable<ImageFile>.CompareTo(ImageFile other)
        {
            int returnValue = -1;
            if (this.DateModified > other.DateModified)
            {
                returnValue = 1;
            }
            else if (other.DateModified == this.DateModified)
            {
                returnValue = other.Name.CompareTo(this.Name);
            }

            return returnValue;
        }

        #endregion IComparable<ImageFile> Members
    }
}