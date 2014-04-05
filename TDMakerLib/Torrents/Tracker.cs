using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace TDMakerLib
{
    [Serializable]
    public class TrackerGroup
    {
        public TrackerGroup() { }

        public TrackerGroup(string name)
        {            
            this.Name = name;
        }

        [Category("Settings"), Description("Descriptive name for the group of Trackers e.g. Movie Trackers")]
        public string Name { get; set; }
        public List<Tracker> Trackers = new List<Tracker>();

        public override string ToString()
        {
            return this.Name;
        }
    }

    [Serializable]
    public class Tracker
    {
        [Category("Settings"), Description("Descriptive name tracker")]
        public string Name { get; set; }
        [Category("Settings"), Description("Announce URL usually shown in the upload page")]
        public string AnnounceURL { get; set; }

        public Tracker() { }

        public Tracker(string name, string url)
        {
            this.Name = name;
            this.AnnounceURL = url;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
