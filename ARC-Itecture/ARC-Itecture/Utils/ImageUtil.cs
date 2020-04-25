/*
 * ARC-Itecture
 * Romain Capocasale, Vincent Moulin and Jonas Freiburghaus
 * He-Arc, INF3dlm-a
 * 2019-2020
 * .NET Course
 */

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ARC_Itecture.Utils
{
    class ImageUtil
    {
        private static Random rand = new Random();

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        /// <summary>
        /// Makes an image source from a given bitmap image
        /// </summary>
        /// <param name="bmp">Bitmap image</param>
        /// <returns>ImageSource</returns>
        public static ImageSource ImageSourceFromBitmap(System.Drawing.Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(handle);
            }
        }

        /// <summary>
        /// Compute a random Color
        /// </summary>
        /// <returns>Random color</returns>
        public static Color RandomColor()
        {
            
            byte R = (byte)rand.Next(0, 255);
            byte G = (byte)rand.Next(0, 255);
            byte B = (byte)rand.Next(0, 255);

            return Color.FromArgb(50, R, G, B);
        }
    }
}
