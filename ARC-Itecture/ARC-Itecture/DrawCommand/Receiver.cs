using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ARC_Itecture.Command
{
    class Receiver
    {
        private Canvas _canvas;

        public Receiver(Canvas canvas)
        {
            this._canvas = canvas;
        }

        public void DrawCamera(Point p)
        {
            Ellipse el = new Ellipse();
            el.Width = 10;
            el.Height = 10;
            el.Fill = new SolidColorBrush(Colors.White);
            Canvas.SetLeft(el, p.X);
            Canvas.SetTop(el, p.Y);

            this._canvas.Children.Add(el);
        }
    }
}
