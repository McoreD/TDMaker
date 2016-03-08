using System.IO;

namespace TDMakerLib
{
    public static class MediaHelper
    {
        public static string GetMediaName(string p)
        {
            string name = "";

            if (File.Exists(p))
            {
                string ext = Path.GetExtension(p).ToLower();
                if (ext == ".vob" && Path.GetFileName(Path.GetDirectoryName(p)) == "VIDEO_TS")
                {
                    name = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(p)));
                }
                else
                {
                    name = Path.GetFileNameWithoutExtension(p);
                }
            }
            else if (Directory.Exists(p))
            {
                name = Path.GetFileName(p);
                if (name.ToUpper().Equals("VIDEO_TS"))
                    name = Path.GetFileName(Path.GetDirectoryName(p));
            }

            return name;
        }

        public static SourceType GetSourceType(string fd)
        {
            SourceType st = SourceType.Rip;

            if (Directory.Exists(fd))
            {
                string[] d = Directory.GetDirectories(fd, "VIDEO_TS", SearchOption.AllDirectories);
                if (d.Length > 0)
                    return SourceType.DVD;

                d = Directory.GetDirectories(fd, "BDMV", SearchOption.AllDirectories);
                if (d.Length > 0)
                    return SourceType.Bluray;
            }
            return st;
        }
    }
}