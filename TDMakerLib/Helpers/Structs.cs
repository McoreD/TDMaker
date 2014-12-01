namespace TDMakerLib
{
    /// <summary>
    /// Options regard Publish
    /// </summary>
    public struct PublishOptionsPacket
    {
        public bool AlignCenter { get; set; }
        public bool PreformattedText { get; set; }
        public bool FullPicture { get; set; }
        public PublishInfoType PublishInfoTypeChoice { get; set; }
        public string TemplateLocation { get; set; }
    }
}