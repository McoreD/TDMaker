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
        public event TaskEventHandler MediaLoaded, TorrentInfoCreated;
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
            if (Info.TaskSettings.Media.DiscType != SourceType.Bluray)
            {
                ReportProgress("Reading " + Path.GetFileName(Info.TaskSettings.Media.Location) + " using MediaInfo...");
                Info.TaskSettings.Media.ReadMedia();
                OnMediaLoaded();
            }

            if (Info.TaskSettings.MediaOptions.UploadScreenshots)
            {
                TakeScreenshots();
                UploadScreenshots();
            }
            else if (Info.TaskSettings.MediaOptions.CreateScreenshots)
            {
                TakeScreenshots();
            }

            string PublishString = CreatePublishInitial(Info.TaskSettings);
            OnTorrentInfoCreated();

            // create textFiles of MediaInfo
            if (App.Settings.ProfileActive.WritePublish)
            {
                string txtPath = Path.Combine(Info.TaskSettings.Media.TorrentCreateInfo.TorrentFolder, Info.TaskSettings.Media.Overall.FileName) + ".txt";

                Helpers.CreateDirectoryFromDirectoryPath(Info.TaskSettings.Media.TorrentCreateInfo.TorrentFolder);

                using (StreamWriter sw = new StreamWriter(txtPath))
                {
                    sw.WriteLine(PublishString);
                }
            }

            // create torrent
            if (Info.TaskSettings.MediaOptions.CreateTorrent)
            {
                Info.TaskSettings.Media.TorrentCreateInfo.CreateTorrent();
            }

            // create xml info
            if (App.Settings.ProfileActive.XMLTorrentUploadCreate)
            {
                string fp = Path.Combine(Info.TaskSettings.Media.TorrentCreateInfo.TorrentFolder, MediaHelper.GetMediaName(Info.TaskSettings.Media.TorrentCreateInfo.MediaLocation)) + ".xml";
                FileSystem.GetXMLTorrentUpload(Info.TaskSettings).Write2(fp);
            }
        }

        private string CreatePublishInitial(TaskSettings ts)
        {
            PublishOptions pop = new PublishOptions();
            pop.AlignCenter = App.Settings.ProfileActive.AlignCenter;
            pop.FullPicture = ts.MediaOptions.UploadScreenshots && App.Settings.ProfileActive.UseFullPictureURL;
            pop.PreformattedText = App.Settings.ProfileActive.PreText;
            pop.PublishInfoTypeChoice = App.Settings.ProfileActive.Publisher;
            ts.PublishOptions = pop;

            return Adapter.CreatePublish(ts, pop);
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

        #region Take Screenshots

        public void TakeScreenshots()
        {
            switch (Info.TaskSettings.MediaOptions.MediaTypeChoice)
            {
                case MediaType.MediaDisc:
                    TakeScreenshots(Info.TaskSettings.Media.Overall, FileSystem.GetScreenShotsDir(Info.TaskSettings.Media.Overall.FilePath));
                    break;

                default:
                    foreach (MediaFile mf in Info.TaskSettings.Media.MediaFiles)
                    {
                        TakeScreenshots(mf, FileSystem.GetScreenShotsDir(mf.FilePath));
                    }
                    break;
            }
        }

        public void TakeScreenshots(string ssDir)
        {
            switch (Info.TaskSettings.MediaOptions.MediaTypeChoice)
            {
                case MediaType.MediaCollection:
                case MediaType.MediaIndiv:
                    Parallel.ForEach<MediaFile>(Info.TaskSettings.Media.MediaFiles, mf => { TakeScreenshots(mf, ssDir); });
                    break;

                case MediaType.MediaDisc:
                    TakeScreenshots(Info.TaskSettings.Media.Overall, ssDir);
                    break;
            }
        }

        private bool TakeScreenshots(MediaFile mf, string ssDir)
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

        #endregion Take Screenshots

        private void ReportProgress(string msg)
        {
            Info.Status = msg;
            OnStatusChanged();
        }

        #region Upload Screenshots

        public void UploadScreenshots()
        {
            if (Info.TaskSettings.MediaOptions.UploadScreenshots)
            {
                switch (Info.TaskSettings.MediaOptions.MediaTypeChoice)
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
            if (Info.TaskSettings.MediaOptions.UploadScreenshots)
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

        #endregion Upload Screenshots

        #region Task Events

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

        private void OnTorrentInfoCreated()
        {
            if (TorrentInfoCreated != null)
            {
                threadWorker.InvokeAsync(() => TorrentInfoCreated(this));
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

        #endregion Task Events

        public void Dispose()
        {
        }

        public override string ToString()
        {
            return Path.GetFileName(Info.TaskSettings.Media.Location);
        }
    }
}