using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TDMakerLib
{
    public abstract class Thumbnailer
    {
        protected MediaFile MediaFile { get; set; }
        protected string ScreenshotDir { get; set; }
        public List<Screenshot> Screenshots = new List<Screenshot>();

        public string MediaSummary { get; protected set; }

        public abstract void TakeScreenshot();

        public static System.Drawing.Bitmap Combine(string[] files, int paddingY, string infoString)
        {
            //read all images into memory
            List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();
            System.Drawing.Bitmap finalImage = null;

            try
            {
                int INFOSTRINGHEIGHT = string.IsNullOrEmpty(infoString) ? 0 : 100; // for MTN string
                int width = 0;
                int height = INFOSTRINGHEIGHT; 

                foreach (string image in files)
                {
                    //create a Bitmap from the file and add it to the list
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(image);

                    //update the size of the final bitmap
                    height += bitmap.Height + paddingY;
                    width = bitmap.Width > width ? bitmap.Width : width;

                    images.Add(bitmap);
                }

                //create a bitmap to hold the combined image
                finalImage = new System.Drawing.Bitmap(width, height);

                //get a graphics object from the image so we can draw on it
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(System.Drawing.Color.Gray);

                    // fill mtn info if available
                    if (!string.IsNullOrEmpty(infoString))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawString(infoString,
                                new Font("Arial", 12, FontStyle.Bold),
                                SystemBrushes.WindowText, new Point(paddingY, paddingY));
                    }

                    //go through each image and draw it on the final image
                    int offset = INFOSTRINGHEIGHT;
                    foreach (System.Drawing.Bitmap image in images)
                    {
                        g.DrawImage(image,
                          new System.Drawing.Rectangle(0, offset, image.Width, image.Height));
                        offset += image.Height + paddingY;
                    }
                }



                return finalImage;
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();

                throw ex;
            }
            finally
            {
                //clean up memory
                foreach (System.Drawing.Bitmap image in images)
                {
                    image.Dispose();
                }
            }
        }
    }
}
