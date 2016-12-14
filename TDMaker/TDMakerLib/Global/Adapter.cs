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

using ShareX.HelpersLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDMakerLib
{
    /// <summary>
    /// Description of Adapter.
    /// </summary>
    public static class Adapter
    {
        public static string GetDurationString(double dura)
        {
            int mins = (int)dura / 1000 / 60;
            int secsLeft = (int)dura / 1000 - mins * 60;
            return string.Format("{0}:{1}", mins.ToString(), secsLeft.ToString("00"));
        }

        /// <summary>
        /// Remove HTML from string with Regex.
        /// </summary>
        public static string StripImg(string source)
        {
            return Regex.Replace(source, @"\[img\].*?\[/img\]", string.Empty);
        }

        public static bool MediaIsAudio(string dir)
        {
            List<string> fileColl = new List<string>();
            if (Directory.Exists(dir))
            {
                foreach (string ext in App.Settings.SupportedFileExtAudio)
                {
                    fileColl.AddRange(Directory.GetFiles(dir, "*" + ext, SearchOption.AllDirectories));
                }
                foreach (string ext in App.Settings.SupportedFileExtVideo)
                {
                    fileColl.AddRange(Directory.GetFiles(dir, "*" + ext, SearchOption.AllDirectories));
                }
            }
            return MediaIsAudio(fileColl);
        }

        public static bool MediaIsAudio(List<string> fileColl)
        {
            bool allAudio = true;
            List<MediaFile> tempMediaFiles = new List<MediaFile>();
            foreach (string fp in fileColl)
            {
                tempMediaFiles.Add(new MediaFile(fp, ""));
            }

            foreach (MediaFile mf in tempMediaFiles)
            {
                allAudio = mf.IsAudioFile() && allAudio;
            }
            return allAudio;
        }

        public static bool MediaIsDisc(string p)
        {
            bool dir = Directory.Exists(p);

            if (dir)
            {
                string[] ifo = Directory.GetFiles(p, "VTS_01_0.IFO", SearchOption.AllDirectories);
                string[] vob = Directory.GetFiles(p, "*.VOB", SearchOption.AllDirectories);
                dir = ifo.Length > 0 && vob.Length > 0;
            }
            else if (File.Exists(p))
            {
                dir = Path.GetExtension(p).ToLower() == "iso";
            }

            return dir;
        }

        public static MediaWizardOptions GetMediaType(List<string> FileOrDirPaths, bool silent = false)
        {
            MediaWizardOptions mwo = new MediaWizardOptions() { MediaTypeChoice = MediaType.MediaIndiv };

            if (FileOrDirPaths.Count == 1 && File.Exists(FileOrDirPaths[0]))
            {
                mwo.MediaTypeChoice = MediaType.MediaIndiv;
            }
            else
            {
                bool bDirFound = false;
                int dirCount = 0;

                foreach (string fd in FileOrDirPaths)
                {
                    if (Directory.Exists(fd))
                    {
                        dirCount++;
                        bDirFound = true;
                    }
                    if (dirCount > 1) break;
                }
                if (bDirFound && dirCount == 1)
                {
                    string dir = FileOrDirPaths[0];
                    if (MediaIsDisc(dir))
                    {
                        mwo.MediaTypeChoice = MediaType.MediaDisc;
                    }
                    else if (MediaIsAudio(dir))
                    {
                        mwo.MediaTypeChoice = MediaType.MusicAudioAlbum;
                    }
                    else if (!silent)
                    {
                        mwo.ShowWizard = true;
                    }
                }
                else if (!silent) // no dir found
                {
                    mwo.ShowWizard = true;
                }
            }

            DebugHelper.WriteLine("Determined media type as: " + mwo.MediaTypeChoice.ToString());
            return mwo;
        }

        /// <summary>
        /// Function to determine DVD-5 or DVD-9
        /// </summary>
        /// <returns>DVD-5 or DVD-9</returns>
        public static string GetDVDString(string p)
        {
            string ss = "DVD";
            double size = 0.0; // size in Bytes
            if (MediaIsDisc(p))
            {
                string[] files = Directory.GetFiles(p, "*.*", SearchOption.AllDirectories);
                foreach (string f in files)
                {
                    FileInfo fi = new FileInfo(f);
                    size += fi.Length;
                }
                if (size > 0.0)
                {
                    ss = (size > 4.7 * 1000.0 * 1000.0 * 1000.0 ? "DVD-9" : "DVD-5");
                }
            }
            return ss;
        }

        public static void ScheduleFileForDeletion(string ssPath)
        {
            new Thread(() =>
            {
                if (!App.Settings.ProfileActive.KeepScreenshots)
                {
                    try
                    {
                        File.Delete(ssPath);
                    }
                    catch (Exception ex)
                    {
                        DebugHelper.WriteException(ex, "Error deleting file.");
                    }
                }
            }).Start();
        }

        #region Publish

        public static string ToPublishString(TaskSettings ts, PublishOptions pop)
        {
            string pt = "";

            switch (pop.PublishInfoTypeChoice)
            {
                case PublishInfoType.ExternalTemplate:
                    if (Directory.Exists(pop.TemplateLocation))
                    {
                        pt = TorrentInfo.ToStringPublishExternal(pop, new TemplateReader(pop.TemplateLocation, ts));
                    }
                    else if (Directory.Exists(ts.Media.TemplateLocation))
                    {
                        pt = TorrentInfo.ToStringPublishExternal(pop, new TemplateReader(ts.Media.TemplateLocation, ts));
                    }
                    else
                    {
                        pt = TorrentInfo.ToStringPublishInternal(ts);
                    }
                    break;

                case PublishInfoType.InternalTemplate:
                    pt = TorrentInfo.ToStringPublishInternal(ts);
                    break;

                case PublishInfoType.MediaInfo:
                    pt = TorrentInfo.ToStringPublishMediaInfo(ts);
                    break;
            }

            ts.Media.ReleaseDescription = Adapter.StripImg(pt).Trim();
            ;

            return pt;
        }

        #endregion Publish
    }
}