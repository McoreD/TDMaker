using System.Runtime.InteropServices;

namespace TDMakerLib
{
    public static class Kernel32Helper
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDllDirectory(string lpPathName);
    }
}