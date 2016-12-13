using ShareX.HelpersLib;
using ShareX.UploadersLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TDMakerLib
{
    public class WorkerTask
    {
        public delegate void TaskEventHandler(WorkerTask task);
        public delegate void ScreenshotInfoEventHandler(ScreenshotInfo si);
        public delegate void UploaderServiceEventHandler(IUploaderService uploaderService);

        public event TaskEventHandler StatusChanged, UploadStarted, UploadProgressChanged, UploadCompleted, TaskCompleted;
        public event TaskEventHandler MediaLoaded;
        public event ScreenshotInfoEventHandler ScreenshotUploaded;
        public event UploaderServiceEventHandler UploadersConfigWindowRequested;

        public TaskInfo Info { get; set; }

        public TaskStatus Status { get; private set; }

        public bool IsBusy
        {
            get
            {
                return Status == TaskStatus.InQueue || IsWorking;
            }
        }

        public bool IsWorking
        {
            get
            {
                return Status == TaskStatus.Preparing || Status == TaskStatus.Working || Status == TaskStatus.Stopping;
            }
        }

        public bool StopRequested { get; private set; }
        public bool RequestSettingUpdate { get; private set; }

        public TaskType Task { get; private set; }
        public List<TorrentInfo> MediaList { get; set; }

        public List<TorrentCreateInfo> TorrentPackets { get; set; }

        public ThreadWorker threadWorker { get; private set; }
        private GenericUploader uploader;
        private TaskReferenceHelper taskReferenceHelper;

        public bool Success { get; private set; }

        public WorkerTask(TaskType task, BackgroundWorker worker = null)
        {
            Task = task;
        }

        public WorkerTask(TaskSettings taskSettings)
        {
            Status = TaskStatus.InQueue;
            Info = new TaskInfo(taskSettings);
        }

        public void Start()
        {
            if (Status == TaskStatus.InQueue && !StopRequested)
            {
                Info.TaskStartTime = DateTime.UtcNow;

                threadWorker = new ThreadWorker();
                Prepare();
                threadWorker.DoWork += ThreadDoWork;
                threadWorker.Completed += ThreadCompleted;
                threadWorker.Start(ApartmentState.STA);
            }
        }

        private void ThreadDoWork()
        {
            Info.TaskSettings.Media.ReadMedia();
            OnMediaLoaded();

            if (Info.TaskSettings.MediaOptions.UploadScreenshots)
            {
                CreateScreenshots();
                UploadScreenshots();
            }
            else if (Info.TaskSettings.MediaOptions.CreateScreenshots)
            {
                CreateScreenshots();
            }

            OnUploadCompleted();
        }

        private void ThreadCompleted()
        {
            OnTaskCompleted();
        }

        private void Prepare()
        {
            Status = TaskStatus.Preparing;
        }

        public static WorkerTask CreateTask(TaskSettings ts)
        {
            WorkerTask task = new WorkerTask(ts);
            return task;
        }

        public void CreateScreenshots()
        {
            switch (Info.TaskSettings.Media.Options.MediaTypeChoice)
            {
                case MediaType.MediaDisc:
                    TakeScreenshot(Info.TaskSettings.Media.Overall, FileSystem.GetScreenShotsDir(Info.TaskSettings.Media.Overall.FilePath));
                    break;

                default:
                    foreach (MediaFile mf in Info.TaskSettings.Media.MediaFiles)
                    {
                        TakeScreenshot(mf, FileSystem.GetScreenShotsDir(mf.FilePath));
                    }
                    break;
            }
        }

        public void CreateScreenshots(string ssDir)
        {
            switch (Info.TaskSettings.Media.Options.MediaTypeChoice)
            {
                case MediaType.MediaCollection:
                case MediaType.MediaIndiv:
                    Parallel.ForEach<MediaFile>(Info.TaskSettings.Media.MediaFiles, mf => { TakeScreenshot(mf, ssDir); });
                    break;

                case MediaType.MediaDisc:
                    TakeScreenshot(Info.TaskSettings.Media.Overall, ssDir);
                    break;
            }
        }

        private bool TakeScreenshot(MediaFile mf, string ssDir)
        {
            String mediaFilePath = mf.FilePath;

            Thumbnailer thumb = new Thumbnailer(mf, ssDir, App.Settings.ProfileActive);

            try
            {
                thumb.TakeScreenshots(threadWorker);
                ReportProgress("Done taking Screenshot for " + Path.GetFileName(mediaFilePath));
            }
            catch (Exception ex)
            {
                Success = false;
                Debug.WriteLine(ex.ToString());
                ReportProgress(ex.Message + " for " + Path.GetFileName(mediaFilePath));
            }

            return Success;
        }

        private void ReportProgress(string msg)
        {
            Info.Status = msg;
            OnStatusChanged();
        }

        public void UploadScreenshots()
        {
            if (Info.TaskSettings.Media.Options.UploadScreenshots)
            {
                switch (Info.TaskSettings.Media.Options.MediaTypeChoice)
                {
                    case MediaType.MediaDisc:
                        UploadScreenshots(Info.TaskSettings.Media.Overall);
                        break;

                    default:
                        foreach (MediaFile mf in Info.TaskSettings.Media.MediaFiles)
                        {
                            UploadScreenshots(mf);
                        }
                        break;
                }
            }
        }

        private void UploadScreenshots(MediaFile mf)
        {
            if (Info.TaskSettings.Media.Options.UploadScreenshots)
            {
                int i = 0;
                Parallel.ForEach<ScreenshotInfo>(mf.Screenshots, si =>
                {
                    if (si != null)
                    {
                        ReportProgress(string.Format("Uploading {0} ({1} of {2})", Path.GetFileName(si.LocalPath), ++i, mf.Screenshots.Count));
                        UploadResult ur = UploadScreenshot(si.LocalPath);

                        if (ur != null && !string.IsNullOrEmpty(ur.URL))
                        {
                            si.FullImageLink = ur.URL;
                            si.LinkedThumbnail = ur.ThumbnailURL;
                            OnScreenshotUploaded(si);
                        }
                    }
                    else
                    {
                        Success = false;
                    }
                });
            }
        }

        private UploadResult UploadScreenshot(string ssPath)
        {
            UploadResult ur = null;

            if (File.Exists(ssPath))
            {
                using (FileStream fs = new FileStream(ssPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    ur = UploadImage(fs, Path.GetFileName(ssPath));
                }

                if (ur != null)
                {
                    if (!string.IsNullOrEmpty(ur.URL))
                    {
                        ReportProgress(string.Format("Uploaded {0}.", Path.GetFileName(ssPath)));
                        Adapter.ScheduleFileForDeletion(ssPath);
                    }
                }
                else
                {
                    ReportProgress(string.Format("Failed uploading {0}. Try again later.", Path.GetFileName(ssPath)));
                    Success = false;
                }
            }

            return ur;
        }

        public UploadResult UploadImage(Stream stream, string fileName)
        {
            ImageUploaderService service = UploaderFactory.ImageUploaderServices[App.Settings.ProfileActive.ImageUploaderType];

            return UploadData(service, stream, fileName);
        }

        public UploadResult UploadData(IGenericUploaderService service, Stream stream, string fileName)
        {
            if (!service.CheckConfig(App.UploadersConfig))
            {
                return GetInvalidConfigResult(service);
            }

            uploader = service.CreateUploader(App.UploadersConfig, taskReferenceHelper);

            if (uploader != null)
            {
                uploader.BufferSize = (int)Math.Pow(2, App.Settings.BufferSizePower) * 1024;
                uploader.ProgressChanged += uploader_ProgressChanged;

                Info.UploadDuration = Stopwatch.StartNew();

                UploadResult result = uploader.Upload(stream, fileName);

                Info.UploadDuration.Stop();

                return result;
            }

            return null;
        }

        private UploadResult GetInvalidConfigResult(IUploaderService uploaderService)
        {
            UploadResult ur = new UploadResult();

            string message = string.Format("Configuration is invalid. Please check Destination Settings window and configurue it.",
                uploaderService.ServiceName);
            DebugHelper.WriteLine(message);
            ur.Errors.Add(message);

            OnUploadersConfigWindowRequested(uploaderService);

            return ur;
        }

        private void OnUploadersConfigWindowRequested(IUploaderService uploaderService)
        {
            if (UploadersConfigWindowRequested != null)
            {
                threadWorker.InvokeAsync(() => UploadersConfigWindowRequested(uploaderService));
            }
        }

        private void uploader_ProgressChanged(ProgressManager progress)
        {
            if (progress != null)
            {
                Info.Progress = progress;

                OnUploadProgressChanged();
            }
        }

        private void OnStatusChanged()
        {
            if (StatusChanged != null)
            {
                threadWorker.InvokeAsync(() => StatusChanged(this));
            }
        }

        private void OnUploadStarted()
        {
            if (UploadStarted != null)
            {
                threadWorker.InvokeAsync(() => UploadStarted(this));
            }
        }

        private void OnUploadCompleted()
        {
            if (UploadCompleted != null)
            {
                threadWorker.InvokeAsync(() => UploadCompleted(this));
            }
        }

        private void OnUploadProgressChanged()
        {
            if (UploadProgressChanged != null)
            {
                threadWorker.InvokeAsync(() => UploadProgressChanged(this));
            }
        }

        private void OnMediaLoaded()
        {
            if (MediaLoaded != null)
            {
                threadWorker.InvokeAsync(() => MediaLoaded(this));
            }
        }

        private void OnScreenshotUploaded(ScreenshotInfo si)
        {
            if (ScreenshotUploaded != null)
            {
                threadWorker.InvokeAsync(() => ScreenshotUploaded(si));
            }
        }

        private void OnTaskCompleted()
        {
            Info.TaskEndTime = DateTime.UtcNow;

            Status = TaskStatus.Completed;

            if (StopRequested)
            {
                Info.Status = "Stopped.";
            }
            else
            {
                Info.Status = "Done.";
            }

            if (TaskCompleted != null)
            {
                TaskCompleted(this);
            }

            Dispose();
        }

        public void Dispose()
        {
        }
    }
}