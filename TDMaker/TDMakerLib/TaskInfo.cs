using ShareX.UploadersLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TDMakerLib
{
    public class TaskInfo
    {
        public TaskSettings TaskSettings { get; set; }

        public string Status { get; set; }

        public DateTime TaskStartTime { get; set; }
        public DateTime TaskEndTime { get; set; }

        public Stopwatch UploadDuration { get; set; }
        public UploadResult Result { get; set; }
        public ProgressManager Progress { get; set; }
        public ProgressManager TorrentProgress { get; set; }

        public TaskInfo(TaskSettings taskSettings)
        {
            TaskSettings = taskSettings;
            Result = new UploadResult();
        }
    }
}