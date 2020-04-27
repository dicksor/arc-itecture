/*
 * ARC-Itecture
 * Romain Capocasale, Vincent Moulin and Jonas Freiburghaus
 * He-Arc, INF3dlm-a
 * 2019-2020
 * .NET Course
 */

using ARC_Itecture.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ARC_Itecture.DrawCommand.Drawers
{

    /// <summary>
    /// Performs the camera drawing
    /// </summary>
    class CameraDrawer : Drawer
    {

        public CameraDrawer(Receiver receiver) : base(receiver) { }

        public override void Draw(Point p)
        {
            System.Drawing.Bitmap cameraBitmap = Properties.Resources.camera_icon;
            cameraBitmap.MakeTransparent(cameraBitmap.GetPixel(1, 1));

            Image cameraImage = new Image
            {
                Source = ImageUtil.ImageSourceFromBitmap(cameraBitmap),
                LayoutTransform = new ScaleTransform(1, -1)
            };

            InkCanvas.SetLeft(cameraImage, p.X);
            InkCanvas.SetTop(cameraImage, p.Y);
            _receiver.ViewModel._mainWindow.canvas.Children.Add(cameraImage);

            MainWindow.main.History = "Camera";
            _receiver.ViewModel._stackHistory.Push(new Tuple<Object, Object, string>(cameraImage, cameraImage, "Camera"));

            _receiver.ViewModel._plan.AddCamera(p);
        }

        public override void DrawPreview(Point p)
        {
            throw new NotImplementedException();
        }
    }
}
