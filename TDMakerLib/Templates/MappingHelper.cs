using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TDMakerLib.Templates;

namespace TDMakerLib
{
    public class MappingHelper
    {
        private Dictionary<string, string> MappingsGeneral { get; set; }

        private Dictionary<string, string> MappingsVideo { get; set; }

        private List<Dictionary<string, string>> MappingsAudio { get; set; }

        public MappingHelper(string summary)
        {
            MappingsGeneral = new Dictionary<string, string>();
            MappingsVideo = new Dictionary<string, string>();
            MappingsAudio = new List<Dictionary<string, string>>();

            if (!string.IsNullOrEmpty(summary))
            {
                int idAudio = 1;
                string prefix = string.Empty;
                string[] lines = summary.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                {
                    string[] temp = line.Split(new[] { " : " }, StringSplitOptions.None);

                    if (temp.Length == 2 && !string.IsNullOrEmpty(prefix))
                    {
                        MIFieldValue mifv = new MIFieldValue(temp[0], temp[1], prefix);
                        if (prefix == "General")
                        {
                            if (!this.MappingsGeneral.Keys.Contains(mifv.Field))
                                this.MappingsGeneral.Add(mifv.Field, mifv.Value);
                        }
                        else if (prefix == "Video")
                        {
                            if (!this.MappingsVideo.Keys.Contains(mifv.Field))
                                this.MappingsVideo.Add(mifv.Field, mifv.Value);
                        }
                        else if (prefix == "Audio")
                        {
                            if (!this.MappingsAudio[idAudio - 1].Keys.Contains(mifv.Field))
                                this.MappingsAudio[idAudio - 1].Add(mifv.Field, mifv.Value);
                        }
                    }
                    else if (temp.Length == 1)
                    {
                        // attempt to get audio id
                        string[] spTemp = Regex.Split(temp[0], " #");
                        if (spTemp.Length > 1)
                        {
                            // we have Audio #1, Text #1 etc.
                            prefix = spTemp[0].Trim();
                            if (prefix == "Audio")
                            {
                                int audioNum = 0; // compared with idAudio
                                int.TryParse(spTemp[1], out audioNum);
                                this.MappingsAudio.Add(new Dictionary<string, string>());
                                idAudio = audioNum;
                            }
                        }
                        else
                        {
                            prefix = temp[0].Trim(); // general, video or audio
                            if (prefix == "Audio" && this.MappingsAudio.Count == 0)
                            {
                                this.MappingsAudio.Add(new Dictionary<string, string>());
                            }
                        }
                    }
                    else
                    {
                        Debug.WriteLine(string.Format("Error TemplateReader2.cs: {0}, {1}", line, prefix));
                    }
                }
            }
        }

        public string ReplacePatternGeneral(string pattern)
        {
            return ReplacePattern(this.MappingsGeneral, pattern);
        }

        public string ReplacePatternVideo(string pattern)
        {
            return ReplacePattern(this.MappingsVideo, pattern);
        }

        public string ReplacePatternAudio(int id, string pattern)
        {
            return ReplacePattern(this.MappingsAudio[id], pattern);
        }

        private string ReplacePattern(Dictionary<string, string> dic, string pattern)
        {
            foreach (var pair in dic)
            {
                pattern = Regex.Replace(pattern, pair.Key, pair.Value, RegexOptions.IgnoreCase);
            }
            return pattern;
        }

        public string GetValueGeneral(string field)
        {
            return this.MappingsGeneral[field];
        }

        public void ListFieldsGeneral()
        {
            ListFields(MappingsGeneral);
        }

        public void ListFieldsAll()
        {
            ListFields(MappingsGeneral);
            if (MappingsAudio.Count > 0)
            {
                ListFields(MappingsAudio[0]);
            }
            ListFields(MappingsVideo);
        }

        public void ListFields(Dictionary<string, string> dic)
        {
            foreach (var pair in dic)
            {
                Debug.WriteLine(string.Format("{0} - {1}", pair.Key, pair.Value));
            }
        }

        /// <summary>
        /// Lists the available fields for a specified audio stream by its zero based index
        /// </summary>
        /// <param name="id">Zero based index of the audio stream</param>
        public void ListFieldsAudio(int id)
        {
            ListFields(this.MappingsAudio[id]);
        }
    }
}