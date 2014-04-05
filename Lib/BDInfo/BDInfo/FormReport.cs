//============================================================================
// BDInfo - Blu-ray Video and Audio Analysis Tool
// Copyright © 2010 Cinema Squid
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BDInfo
{
    public partial class FormReport : Form
    {
        private List<TSPlaylistFile> Playlists;
        public string report = "";

        public FormReport()
        {
            InitializeComponent();
        }

        public void Generate(
            BDROM BDROM,
            List<TSPlaylistFile> playlists,
            ScanBDROMResult scanResult)
        {
            Playlists = playlists;

            StreamWriter reportFile = null;
            if (BDInfoSettings.AutosaveReport)
            {
                string reportName = string.Format(
                    "BDINFO.{0}.txt",
                    BDROM.VolumeLabel);

                reportFile = File.CreateText(Path.Combine(Environment.CurrentDirectory, reportName));
            }
            textBoxReport.Text = "";

            string protection = (BDROM.IsBDPlus ? "BD+" : "AACS");
            string bdjava = (BDROM.IsBDJava ? "Yes" : "No");

            List<string> extraFeatures = new List<string>();
            if (BDROM.Is50Hz)
            {
                extraFeatures.Add("50Hz Content");
            }
            if (BDROM.Is3D)
            {
                extraFeatures.Add("Blu-ray 3D");
            }
            if (BDROM.IsDBOX)
            {
                extraFeatures.Add("D-BOX Motion Code");
            }
            if (BDROM.IsPSP)
            {
                extraFeatures.Add("PSP Digital Copy");
            }
            if (extraFeatures.Count > 0)
            {
                report += string.Format(
                    "{0,-16}{1}\r\n", "Extras:", string.Join(", ", extraFeatures.ToArray()));
            }

            if (scanResult.ScanException != null)
            {
                report += string.Format(
                    "WARNING: Report is incomplete because: {0}\r\n",
                    scanResult.ScanException.Message);
            }
            if (scanResult.FileExceptions.Count > 0)
            {
                report += "WARNING: File errors were encountered during scan:\r\n";
                foreach (string fileName in scanResult.FileExceptions.Keys)
                {
                    Exception fileException = scanResult.FileExceptions[fileName];
                    report += string.Format(
                        "\r\n{0}\t{1}\r\n", fileName, fileException.Message);
                    report += string.Format(
                        "{0}\r\n", fileException.StackTrace);
                }
            }

            foreach (TSPlaylistFile playlist in playlists)
            {
                string summary = "";

                comboBoxPlaylist.Items.Add(playlist);

                string title = playlist.Name;
                string discSize = string.Format(
                    "{0:N0}", BDROM.Size);

                TimeSpan playlistTotalLength =
                    new TimeSpan((long)(playlist.TotalLength * 10000000));
                string totalLength = string.Format(
                    "{0:D1}:{1:D2}:{2:D2}.{3:D3}",
                    playlistTotalLength.Hours,
                    playlistTotalLength.Minutes,
                    playlistTotalLength.Seconds,
                    playlistTotalLength.Milliseconds);
                string totalLengthShort = string.Format(
                    "{0:D1}:{1:D2}:{2:D2}",
                    playlistTotalLength.Hours,
                    playlistTotalLength.Minutes,
                    playlistTotalLength.Seconds);
                string totalSize = string.Format(
                    "{0:N0}", playlist.TotalSize);
                string totalBitrate = string.Format(
                    "{0:F2}",
                    Math.Round((double)playlist.TotalBitRate / 10000) / 100);

                TimeSpan playlistAngleLength =
                    new TimeSpan((long)(playlist.TotalAngleLength * 10000000));
                string totalAngleLength = string.Format(
                    "{0:D1}:{1:D2}:{2:D2}.{3:D3}",
                    playlistAngleLength.Hours,
                    playlistAngleLength.Minutes,
                    playlistAngleLength.Seconds,
                    playlistAngleLength.Milliseconds);
                string totalAngleSize = string.Format(
                    "{0:N0}", playlist.TotalAngleSize);
                string totalAngleBitrate = string.Format(
                    "{0:F2}",
                    Math.Round((double)playlist.TotalAngleBitRate / 10000) / 100);

                List<string> angleLengths = new List<string>();
                List<string> angleSizes = new List<string>();
                List<string> angleBitrates = new List<string>();
                List<string> angleTotalLengths = new List<string>();
                List<string> angleTotalSizes = new List<string>();
                List<string> angleTotalBitrates = new List<string>();
                if (playlist.AngleCount > 0)
                {
                    for (int angleIndex = 0; angleIndex < playlist.AngleCount; angleIndex++)
                    {
                        double angleLength = 0;
                        ulong angleSize = 0;
                        ulong angleTotalSize = 0;
                        if (angleIndex < playlist.AngleClips.Count &&
                            playlist.AngleClips[angleIndex] != null)
                        {
                            foreach (TSStreamClip clip in playlist.AngleClips[angleIndex].Values)
                            {
                                angleTotalSize += clip.PacketSize;
                                if (clip.AngleIndex == angleIndex + 1)
                                {
                                    angleSize += clip.PacketSize;
                                    angleLength += clip.Length;
                                }
                            }
                        }

                        angleSizes.Add(string.Format(
                            "{0:N0}", angleSize));

                        TimeSpan angleTimeSpan =
                            new TimeSpan((long)(angleLength * 10000000));

                        angleLengths.Add(string.Format(
                            "{0:D1}:{1:D2}:{2:D2}.{3:D3}",
                            angleTimeSpan.Hours,
                            angleTimeSpan.Minutes,
                            angleTimeSpan.Seconds,
                            angleTimeSpan.Milliseconds));

                        angleTotalSizes.Add(string.Format(
                            "{0:N0}", angleTotalSize));

                        angleTotalLengths.Add(totalLength);

                        double angleBitrate = 0;
                        if (angleLength > 0)
                        {
                            angleBitrate = Math.Round((double)(angleSize * 8) / angleLength / 10000) / 100;
                        }
                        angleBitrates.Add(string.Format("{0:F2}", angleBitrate));

                        double angleTotalBitrate = 0;
                        if (playlist.TotalLength > 0)
                        {
                            angleTotalBitrate = Math.Round((double)(angleTotalSize * 8) / playlist.TotalLength / 10000) / 100;
                        }
                        angleTotalBitrates.Add(string.Format(
                            "{0:F2}", angleTotalBitrate));
                    }
                }

                string videoCodec = "";
                string videoBitrate = "";
                if (playlist.VideoStreams.Count > 0)
                {
                    TSStream videoStream = playlist.VideoStreams[0];
                    videoCodec = videoStream.CodecAltName;
                    videoBitrate = string.Format(
                        "{0:F2}", Math.Round((double)videoStream.BitRate / 10000) / 100);
                }

                string audio1 = "";
                string languageCode1 = "";
                if (playlist.AudioStreams.Count > 0)
                {
                    TSAudioStream audioStream = playlist.AudioStreams[0];

                    languageCode1 = audioStream.LanguageCode;

                    audio1 = string.Format(
                        "{0} {1}",
                        audioStream.CodecAltName,
                        audioStream.ChannelDescription);

                    if (audioStream.BitRate > 0)
                    {
                        audio1 += string.Format(
                            " {0} Kbps",
                            (int)Math.Round((double)audioStream.BitRate / 1000));
                    }

                    if (audioStream.SampleRate > 0 &&
                        audioStream.BitDepth > 0)
                    {
                        audio1 += string.Format(
                            " ({0} kHz/{1}-bit)",
                            (int)Math.Round((double)audioStream.SampleRate / 1000),
                            audioStream.BitDepth);
                    }
                }

                string audio2 = "";
                if (playlist.AudioStreams.Count > 1)
                {
                    for (int i = 1; i < playlist.AudioStreams.Count; i++)
                    {
                        TSAudioStream audioStream = playlist.AudioStreams[i];

                        if (audioStream.LanguageCode == languageCode1 &&
                            audioStream.StreamType != TSStreamType.AC3_PLUS_SECONDARY_AUDIO &&
                            audioStream.StreamType != TSStreamType.DTS_HD_SECONDARY_AUDIO &&
                            !(audioStream.StreamType == TSStreamType.AC3_AUDIO &&
                              audioStream.ChannelCount == 2))
                        {
                            audio2 = string.Format(
                                "{0} {1}",
                                audioStream.CodecAltName, audioStream.ChannelDescription);

                            if (audioStream.BitRate > 0)
                            {
                                audio2 += string.Format(
                                    " {0} kbps",
                                    (int)Math.Round((double)audioStream.BitRate / 1000));
                            }

                            if (audioStream.SampleRate > 0 &&
                                audioStream.BitDepth > 0)
                            {
                                audio2 += string.Format(
                                    " ({0} kHz/{1}-bit)",
                                    (int)Math.Round((double)audioStream.SampleRate / 1000),
                                    audioStream.BitDepth);
                            }
                            break;
                        }
                    }
                }

                report += "DISC INFO:\r\n";
                report += "\r\n";

                report += string.Format(
                    "{0,-16}{1}\r\n", "Disc Title:", BDROM.VolumeLabel);
                report += string.Format(
                    "{0,-16}{1:N0} bytes\r\n", "Disc Size:", BDROM.Size);
                report += string.Format(
                    "{0,-16}{1}\r\n", "Protection:", protection);
                report += string.Format(
                    "{0,-16}{1}\r\n", "BD-Java:", bdjava);
                if (extraFeatures.Count > 0)
                {
                    report += string.Format(
                        "{0,-16}{1}\r\n", "Extras:", string.Join(", ", extraFeatures.ToArray()));
                }
                report += string.Format(
                    "{0,-16}{1}\r\n", "TDMaker:", Application.ProductVersion + " (BDInfo 0.5.8)");

                report += "\r\n";
                report += "PLAYLIST REPORT:\r\n";
                report += "\r\n";
                report += string.Format(
                    "{0,-24}{1}\r\n", "Name:", title);
                report += string.Format(
                    "{0,-24}{1} (h:m:s.ms)\r\n", "Length:", totalLength);
                report += string.Format(
                    "{0,-24}{1:N0} bytes\r\n", "Size:", totalSize);
                report += string.Format(
                    "{0,-24}{1} Mbps\r\n", "Total Bitrate:", totalBitrate);
                if (playlist.AngleCount > 0)
                {
                    for (int angleIndex = 0; angleIndex < playlist.AngleCount; angleIndex++)
                    {
                        report += "\r\n";
                        report += string.Format(
                            "{0,-24}{1} (h:m:s.ms) / {2} (h:m:s.ms)\r\n", string.Format("Angle {0} Length:", angleIndex + 1),
                            angleLengths[angleIndex], angleTotalLengths[angleIndex]);
                        report += string.Format(
                            "{0,-24}{1:N0} bytes / {2:N0} bytes\r\n", string.Format("Angle {0} Size:", angleIndex + 1),
                            angleSizes[angleIndex], angleTotalSizes[angleIndex]);
                        report += string.Format(
                            "{0,-24}{1} Mbps / {2} Mbps\r\n", string.Format("Angle {0} Total Bitrate:", angleIndex + 1),
                            angleBitrates[angleIndex], angleTotalBitrates[angleIndex], angleIndex);
                    }
                    report += "\r\n";
                    report += string.Format(
                        "{0,-24}{1} (h:m:s.ms)\r\n", "All Angles Length:", totalAngleLength);
                    report += string.Format(
                        "{0,-24}{1} bytes\r\n", "All Angles Size:", totalAngleSize);
                    report += string.Format(
                        "{0,-24}{1} Mbps\r\n", "All Angles Bitrate:", totalAngleBitrate);
                }
                /*
                report += string.Format(
                    "{0,-24}{1}\r\n", "Description:", "");
                 */

                summary += string.Format(
                    "Disc Title: {0}\r\n", BDROM.VolumeLabel);
                summary += string.Format(
                    "Disc Size: {0:N0} bytes\r\n", BDROM.Size);
                summary += string.Format(
                    "Protection: {0}\r\n", protection);
                summary += string.Format(
                    "BD-Java: {0}\r\n", bdjava);
                summary += string.Format(
                    "Playlist: {0}\r\n", title);
                summary += string.Format(
                    "Size: {0:N0} bytes\r\n", totalSize);
                summary += string.Format(
                    "Length: {0}\r\n", totalLength);
                summary += string.Format(
                    "Total Bitrate: {0} Mbps\r\n", totalBitrate);

                if (playlist.HasHiddenTracks)
                {
                    report += "\r\n(*) Indicates included stream hidden by this playlist.\r\n";
                }

                if (playlist.VideoStreams.Count > 0)
                {
                    report += "\r\n";
                    report += "VIDEO:\r\n";
                    report += "\r\n";
                    report += string.Format(
                        "{0,-24}{1,-20}{2,-16}\r\n",
                        "Codec",
                        "Bitrate",
                        "Description");
                    report += string.Format(
                        "{0,-24}{1,-20}{2,-16}\r\n",
                        "-----",
                        "-------",
                        "-----------");

                    foreach (TSStream stream in playlist.SortedStreams)
                    {
                        if (!stream.IsVideoStream) continue;

                        string streamName = stream.CodecName;
                        if (stream.AngleIndex > 0)
                        {
                            streamName += string.Format(
                                " ({0})", stream.AngleIndex);
                        }

                        string streamBitrate = string.Format(
                            "{0:D}",
                            (int)Math.Round((double)stream.BitRate / 1000));
                        if (stream.AngleIndex > 0)
                        {
                            streamBitrate += string.Format(
                                " ({0:D})",
                                (int)Math.Round((double)stream.ActiveBitRate / 1000));
                        }
                        streamBitrate += " kbps";

                        report += string.Format(
                            "{0,-24}{1,-20}{2,-16}\r\n",
                            (stream.IsHidden ? "* " : "") + streamName,
                            streamBitrate,
                            stream.Description);

                        summary += string.Format(
                            (stream.IsHidden ? "* " : "") + "Video: {0} / {1} / {2}\r\n",
                            streamName,
                            streamBitrate,
                            stream.Description);
                    }
                }

                if (playlist.AudioStreams.Count > 0)
                {
                    report += "\r\n";
                    report += "AUDIO:\r\n";
                    report += "\r\n";
                    report += string.Format(
                        "{0,-32}{1,-16}{2,-16}{3,-16}\r\n",
                        "Codec",
                        "Language",
                        "Bitrate",
                        "Description");
                    report += string.Format(
                       "{0,-32}{1,-16}{2,-16}{3,-16}\r\n",
                        "-----",
                        "--------",
                        "-------",
                        "-----------");

                    foreach (TSStream stream in playlist.SortedStreams)
                    {
                        if (!stream.IsAudioStream) continue;

                        string streamBitrate = string.Format(
                            "{0:D} kbps",
                            (int)Math.Round((double)stream.BitRate / 1000));

                        report += string.Format(
                            "{0,-32}{1,-16}{2,-16}{3,-16}\r\n",
                            (stream.IsHidden ? "* " : "") + stream.CodecName,
                            stream.LanguageName,
                            streamBitrate,
                            stream.Description);

                        summary += string.Format(
                            (stream.IsHidden ? "* " : "") + "Audio: {0} / {1} / {2}\r\n",
                            stream.LanguageName,
                            stream.CodecName,
                            stream.Description);
                    }
                }

                if (playlist.GraphicsStreams.Count > 0)
                {
                    report += "\r\n";
                    report += "SUBTITLES:\r\n";
                    report += "\r\n";
                    report += string.Format(
                        "{0,-32}{1,-16}{2,-16}{3,-16}\r\n",
                        "Codec",
                        "Language",
                        "Bitrate",
                        "Description");
                    report += string.Format(
                        "{0,-32}{1,-16}{2,-16}{3,-16}\r\n",
                        "-----",
                        "--------",
                        "-------",
                        "-----------");

                    foreach (TSStream stream in playlist.SortedStreams)
                    {
                        if (!stream.IsGraphicsStream) continue;

                        string streamBitrate = string.Format(
                            "{0:F3} kbps",
                            (double)stream.BitRate / 1000);

                        report += string.Format(
                            "{0,-32}{1,-16}{2,-16}{3,-16}\r\n",
                            (stream.IsHidden ? "* " : "") + stream.CodecName,
                            stream.LanguageName,
                            streamBitrate,
                            stream.Description);

                        summary += string.Format(
                            (stream.IsHidden ? "* " : "") + "Subtitle: {0} / {1}\r\n",
                            stream.LanguageName,
                            streamBitrate,
                            stream.Description);
                    }
                }

                if (playlist.TextStreams.Count > 0)
                {
                    report += "\r\n";
                    report += "TEXT:\r\n";
                    report += "\r\n";
                    report += string.Format(
                        "{0,-32}{1,-16}{2,-16}{3,-16}\r\n",
                        "Codec",
                        "Language",
                        "Bitrate",
                        "Description");
                    report += string.Format(
                        "{0,-32}{1,-16}{2,-16}{3,-16}\r\n",
                        "-----",
                        "--------",
                        "-------",
                        "-----------");

                    foreach (TSStream stream in playlist.SortedStreams)
                    {
                        if (!stream.IsTextStream) continue;

                        string streamBitrate = string.Format(
                            "{0:F3} kbps",
                            (double)stream.BitRate / 1000);

                        report += string.Format(
                            "{0,-32}{1,-16}{2,-16}{3,-16}\r\n",
                            (stream.IsHidden ? "* " : "") + stream.CodecName,
                            stream.LanguageName,
                            streamBitrate,
                            stream.Description);
                    }
                }

                report += "\r\n";

                if (BDInfoSettings.AutosaveReport && reportFile != null)
                {
                    try { reportFile.Write(report); }
                    catch { }
                }
                textBoxReport.Text += report;

                GC.Collect();
            }

            if (BDInfoSettings.AutosaveReport && reportFile != null)
            {
                try { reportFile.Write(report); }
                catch { }
            }
            textBoxReport.Text += report;

            if (reportFile != null)
            {
                reportFile.Close();
            }

            textBoxReport.Select(0, 0);
            comboBoxPlaylist.SelectedIndex = 0;
            comboBoxChartType.SelectedIndex = 0;
        }

        private void buttonCopy_Click(
            object sender,
            EventArgs e)
        {
            Clipboard.SetText(textBoxReport.Text);
        }

        private void textBoxReport_KeyDown(
            object sender,
            KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == System.Windows.Forms.Keys.A))
            {
                textBoxReport.SelectAll();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void comboBoxPlaylist_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSPlaylistFile playlist = (TSPlaylistFile)comboBoxPlaylist.SelectedItem;

            comboBoxAngle.Items.Clear();
            for (int i = 0; i <= playlist.AngleStreams.Count; i++)
            {
                comboBoxAngle.Items.Add(i);
            }
            if (comboBoxAngle.Items.Count > 0)
            {
                comboBoxAngle.SelectedIndex = 0;
            }

            comboBoxStream.Items.Clear();
            foreach (TSVideoStream videoStream in playlist.VideoStreams)
            {
                comboBoxStream.Items.Add(videoStream);
            }
            if (comboBoxStream.Items.Count > 0)
            {
                comboBoxStream.SelectedIndex = 0;
            }
        }

        private void buttonChart_Click(
            object sender,
            EventArgs e)
        {
            if (Playlists == null ||
                comboBoxPlaylist.SelectedItem == null ||
                comboBoxStream.SelectedItem == null ||
                comboBoxAngle.SelectedItem == null ||
                comboBoxChartType.SelectedItem == null)
            {
                return;
            }

            TSPlaylistFile playlist =
                (TSPlaylistFile)comboBoxPlaylist.SelectedItem;
            TSVideoStream videoStream =
                (TSVideoStream)comboBoxStream.SelectedItem;
            int angleIndex =
                (int)comboBoxAngle.SelectedItem;
            string chartType =
                comboBoxChartType.SelectedItem.ToString();

            FormChart chart = new FormChart();
            chart.Generate(
                chartType, playlist,
                videoStream.PID, angleIndex);
            chart.Show();
        }

        private void FormReport_FormClosed(
            object sender,
            FormClosedEventArgs e)
        {
            GC.Collect();
        }
    }
}