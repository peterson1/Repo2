using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace Repo2.SDK.WPF45.ImagingTools
{
    public static class BitmapTools
    {
        //https://stackoverflow.com/a/1069509/3973863
        public static BitmapImage ConvertToImage(this Bitmap bmp)
        {
            var img = new BitmapImage();
            using (var mem = new MemoryStream())
            {
                bmp.Save(mem, ImageFormat.Bmp);
                mem.Position = 0;
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.StreamSource = mem;
                img.EndInit();
            }
            return img;
        }


        public static string ConvertToBase64(this Bitmap bitmap)
        {
            var imgCnv = new ImageConverter();
            var byts = (byte[])imgCnv.ConvertTo(bitmap, typeof(byte[]));
            return Convert.ToBase64String(byts);
        }
    }


    public static class CreateBitmap
    {
        public static Bitmap FromScreenRegion(int x, int y, int width, int height)
        {
            var bmp = new Bitmap(width, height);
            var gpx = Graphics.FromImage(bmp);
            gpx.CopyFromScreen(x, y, 0, 0, bmp.Size);
            return bmp;
        }


        public static Bitmap FromBase64(string b64EncodedBitmap)
        {
            var byts = Convert.FromBase64String(b64EncodedBitmap);
            using (var ms = new MemoryStream(byts))
            {
                return new Bitmap(ms);
            }
        }
    }
}
