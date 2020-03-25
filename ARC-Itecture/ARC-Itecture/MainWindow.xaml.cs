using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ARC_Itecture
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(canvas);

            Ellipse el = new Ellipse();
            el.Width = 10;
            el.Height = 10;
            el.Fill = new SolidColorBrush(Colors.White);
            Canvas.SetLeft(el, p.X);
            Canvas.SetTop(el, p.Y);

            canvas.Children.Add(el);
        }
    }
}
