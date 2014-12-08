using System;
using System.Collections.Generic;
using System.Linq;

namespace TDMakerLib
{
    public static class FileSystemHelper
    {
        public static string GetLargestFilePathFromDir(string startFolder)
        {
            // Take a snapshot of the file system.
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);

            // This method assumes that the application has discovery permissions
            // for all folders under the specified path.
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            //Return the size of the largest file
            long maxSize =
                (from file in fileList
                 let len = GetFileLength(file)
                 select len)
                 .Max();

            Console.WriteLine("The length of the largest file under {0} is {1}", startFolder, maxSize);

            // Return the FileInfo object for the largest file
            // by sorting and selecting from beginning of list
            System.IO.FileInfo longestFile =
                (from file in fileList
                 let len = GetFileLength(file)
                 where len > 0
                 orderby len descending
                 select file)
                .First();

            Console.WriteLine("The largest file under {0} is {1} with a length of {2} bytes",
                                startFolder, longestFile.FullName, longestFile.Length);

            return longestFile.FullName;
        }

        public static string GetSmallestFilePathFromDir(string startFolder)
        {
            // Take a snapshot of the file system.
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);

            // This method assumes that the application has discovery permissions
            // for all folders under the specified path.
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            //Return the FileInfo of the smallest file
            System.IO.FileInfo smallestFile =
                (from file in fileList
                 let len = GetFileLength(file)
                 where len > 0
                 orderby len ascending
                 select file).First();

            Console.WriteLine("The smallest file under {0} is {1} with a length of {2} bytes",
                                startFolder, smallestFile.FullName, smallestFile.Length);

            return smallestFile.FullName;
        }

        // This method is used to swallow the possible exception
        // that can be raised when accessing the FileInfo.Length property.
        // In this particular case, it is safe to swallow the exception.
        private static long GetFileLength(System.IO.FileInfo fi)
        {
            long retval;
            try
            {
                retval = fi.Length;
            }
            catch (System.IO.FileNotFoundException)
            {
                // If a file is no longer present,
                // just add zero bytes to the total.
                retval = 0;
            }
            return retval;
        }
    }
}