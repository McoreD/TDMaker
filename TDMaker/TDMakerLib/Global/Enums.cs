﻿using System.ComponentModel;

namespace TDMakerLib
{
    public enum MediaType
    {
        [Description("Media disc e.g. DVD or BD")]
        MediaDisc,
        [Description("Single media file")]
        MediaIndiv,
        [Description("Media files collection")]
        MediaCollection,
        [Description("Music audio album")]
        MusicAudioAlbum
    }

    public enum SourceType
    {
        [Description("Rip")]
        Rip,
        [Description("Blu-ray")]
        Bluray,
        [Description("DVD")]
        DVD
    }

    public enum LocationType
    {
        [Description("Parent folder of the media file")]
        ParentFolder,
        [Description("Default folder")]
        KnownFolder,
        [Description("Custom folder")]
        CustomFolder,
    }

    public enum PublishInfoType
    {
        [Description("Internal Template")]
        InternalTemplate,
        [Description("External Template")]
        ExternalTemplate,
        [Description("MediaInfo")]
        MediaInfo
    }

    public enum TakeScreenshotsType
    {
        NONE,
        TAKE_ALL_SCREENSHOTS,
        TAKE_ONE_SCREENSHOT
    }

    public enum ProgressType
    {
        INCREMENT_PROGRESS_WITH_MSG,
        PREVIEW_SCREENSHOT,
        REPORT_MEDIAINFO_SUMMARY,
        REPORT_TORRENTINFO,
        UPDATE_PROGRESSBAR_MAX,
        UPDATE_PROGRESSBAR_Cumulative,
        UPDATE_PROGRESSBAR_ProgressManager,
        UPDATE_STATUSBAR_DEBUG
    }

    public enum TaskType
    {
        ANALYZE_MEDIA,
        CREATE_TORRENT
    }

    public enum TaskStatus
    {
        InQueue,
        Preparing,
        Working,
        Stopping,
        Completed
    }

    public enum ThumbnailerType
    {
        [Description("FFmpeg")]
        FFmpeg,
        [Description("MPlayer")]
        MPlayer
    }

    public enum ThumbnailExtension
    {
        [Description("png")]
        png,
        [Description("jpg")]
        jpg
    }
}