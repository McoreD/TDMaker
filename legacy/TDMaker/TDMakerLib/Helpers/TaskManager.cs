using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace TDMakerLib.Helpers
{
    public class TaskManager
    {
        private WorkerTask mTask = null;

        public TaskManager(WorkerTask task)
        {
            this.mTask = task;
        }
    }
}
