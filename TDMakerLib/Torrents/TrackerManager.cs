using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TDMakerLib;
using System.Diagnostics;

namespace TDMakerLib
{
   public class TrackerManager
    {
        public List<Tracker> Trackers { get; set; }

        public string TrackersXML { get; private set; }

        public TrackerManager()
        {
            Trackers = new List<Tracker>();
            this.TrackersXML = Path.Combine(Program.SettingsDir, "trackers.xml");
        }

        public void Read()
        {
            if (File.Exists(TrackersXML))
            {
                object obj = new object();
                try
                {
                    using (FileStream fs = new FileStream(TrackersXML, FileMode.Open, FileAccess.Read))
                    {
                        XmlSerializer xs = new XmlSerializer(Trackers.GetType());
                        obj = xs.Deserialize(fs);
                        fs.Close();
                    }

                    if (obj != null)
                    {
                        Trackers = (List<Tracker>)obj;
                    }

                    Debug.WriteLine("Done reading trackers.xml");

                }
                catch (Exception e)
                {
                    Debug.WriteLine("Failed to deserialize. Reason: " + e.Message);
                }
            }            
        }

        public void Write()
        {
            try
            {
                if (Trackers.Count > 0)
                {
                    using (FileStream fs = new FileStream(TrackersXML, FileMode.Create))
                    {
                        XmlSerializer xs = new XmlSerializer(Trackers.GetType());
                        xs.Serialize(fs, Trackers);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
               // fs.Close();
            }
          
            
        }

    }
}
