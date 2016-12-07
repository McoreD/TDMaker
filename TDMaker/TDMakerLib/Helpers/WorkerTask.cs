using System.Collections.Generic;
using System.ComponentModel;

namespace TDMakerLib
{
    public class WorkerTask
    {
        public TaskType Task { get; private set; }
        public List<TorrentInfo> MediaList { get; set; }
        public BackgroundWorker MyWorker { get; private set; }
        public List<TorrentCreateInfo> TorrentPackets { get; set; }
        public List<string> FileOrDirPaths { get; set; }

        public MediaWizardOptions MediaOptions { get; set; }

        public WorkerTask(BackgroundWorker worker, TaskType task)
        {
            this.MyWorker = worker;
            this.Task = task;
            this.FileOrDirPaths = new List<string>();
            this.MediaOptions = new MediaWizardOptions();
        }

        public bool IsSingleTask()
        {
            return FileOrDirPaths.Count == 1;
        }
    }
}