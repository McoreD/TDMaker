using ShareX.HelpersLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDMakerLib
{
    public class TaskManager
    {
        private static readonly List<WorkerTask> Tasks = new List<WorkerTask>();

        public static bool IsBusy
        {
            get
            {
                return Tasks.Count > 0 && Tasks.Any(task => task.IsBusy);
            }
        }

        public static void Start(WorkerTask task)
        {
            if (task != null)
            {
                Tasks.Add(task);
                StartTasks();
            }
        }

        private static void StartTasks()
        {
            int workingTasksCount = Tasks.Count(x => x.IsWorking);
            WorkerTask[] inQueueTasks = Tasks.Where(x => x.Status == TaskStatus.InQueue).ToArray();

            if (inQueueTasks.Length > 0)
            {
                int len;

                int UploadLimit = 4;
                if (UploadLimit == 0)
                {
                    len = inQueueTasks.Length;
                }
                else
                {
                    len = (UploadLimit - workingTasksCount).Between(0, inQueueTasks.Length);
                }

                for (int i = 0; i < len; i++)
                {
                    inQueueTasks[i].Start();
                }
            }
        }
    }
}