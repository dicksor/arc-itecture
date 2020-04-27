/*
 * ARC-Itecture
 * Romain Capocasale, Vincent Moulin and Jonas Freiburghaus
 * He-Arc, INF3dlm-a
 * 2019-2020
 * .NET Course
 */


using ARC_Itecture.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ARC_Itecture.DrawCommand.Drawers
{

    /// <summary>
    /// Performs all the windows drawing
    /// </summary>
    class WindowDrawer : Drawer
    {
        private List<Rect> _windowAvailableWalls;
        private Stack<Point> _windowPoints;

        public WindowDrawer(Receiver receiver, ref List<Rect> windowAvailableWalls)
            : base(receiver)
        {
            this._windowAvailableWalls = windowAvailableWalls;
            this._windowPoints = new Stack<Point>();
        }

        public override void Draw(Point p)
        {
            _windowPoints.Push(p);

            if (_windowPoints.Count == 2)
            {
                Point p2 = _windowPoints.Pop();
                Point p1 = _windowPoints.Pop();

                this._fillBrush = new SolidColorBrush(ImageUtil.RandomColor());

                Rect rect = new Rect(InkCanvas.GetLeft(_receiver.LastShape), InkCanvas.GetTop(_receiver.LastShape), _receiver.LastShape.Width, _receiver.LastShape.Height);

                foreach (Rect wall in _windowAvailableWalls)
                {
                    Rect intersect = Rect.Intersect(rect, wall);

                    if (!intersect.IsEmpty)
                    {
                        if ((intersect.X > wall.X && intersect.X + intersect.Width < wall.X + wall.Width) ||
                            (intersect.Y > wall.Y && intersect.Y + intersect.Height < wall.Y + wall.Height))
                        {
                            Rectangle rectangle = new Rectangle
                            {
                                Fill = Application.Current.TryFindResource("PrimaryHueDarkBrush") as SolidColorBrush
                            };

                            InkCanvas.SetLeft(rectangle, intersect.X - (COMPONENT_OFFSET / 2));
                            InkCanvas.SetTop(rectangle, intersect.Y - (COMPONENT_OFFSET / 2));

                            rectangle.Width = intersect.Width + COMPONENT_OFFSET;
                            rectangle.Height = intersect.Height + COMPONENT_OFFSET;

                            _receiver.ViewModel._mainWindow.canvas.Children.Add(rectangle);

                            HouseWindow houseWindow = _receiver.ViewModel._plan.AddWindow(new Point(intersect.X, intersect.Y),
                               new Point(intersect.X + intersect.Width, intersect.Y + intersect.Height),
                               new Point(wall.X, wall.Y),
                               new Point(wall.X + wall.Width, wall.Y + wall.Height));

                            if (houseWindow != null)
                            {
                                MainWindow.main.History = "Window";
                                _receiver.ViewModel._stackHistory.Push(new Tuple<Object, Object, string>(rectangle, houseWindow, "Window"));

                                _windowAvailableWalls.Remove(wall);

                                break; // Allows not to draw 2 windows on parallel walls
                            }
                        }
                    }
                }
                _receiver.ViewModel._mainWindow.canvas.Children.Remove(_receiver.LastShape);
                _receiver.LastShape = null;
            }
        }

        public override void DrawPreview(Point p)
        {
            _fillBrush = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));

            if (_receiver.LastShape is Rectangle lastRectangle)
                _receiver.ViewModel._mainWindow.canvas.Children.Remove(lastRectangle);

            if (_windowPoints.Count > 0)
                _receiver.LastShape = DrawRectangle(_windowPoints.Peek(), p);
        }

        public void UpdateAvailableWindowList(Rectangle rect)
        {
            Line line = FindSegmentByWindow(rect);

            Point realP1 = new Point(line.X1, line.Y1);
            Point realP2 = new Point(line.X2, line.Y2);

            _windowAvailableWalls.Add(new Rect(realP1, realP2));
        }

        private Line FindSegmentByWindow(Rectangle rect)
        {
            double x = InkCanvas.GetLeft(rect);
            double y = InkCanvas.GetTop(rect);
            double width = rect.Width;
            double height = rect.Height;

            Line wall = null;

            foreach (UIElement element in _receiver.ViewModel._mainWindow.canvas.Children)
            {
                if (element is Line line)
                {
                    if (x > line.X1 && x + width < line.X2 ||
                       x > line.X2 && x + width < line.X1 ||
                       y > line.Y1 && y + height < line.Y2 ||
                       y > line.Y2 && y + height < line.Y1)
                    {
                        wall = line;
                    }
                }
            }
            return wall;
        }
    }
}
