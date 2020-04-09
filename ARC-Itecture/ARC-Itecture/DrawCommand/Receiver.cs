using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ARC_Itecture.Utils;
using ARC_Itecture.Geometry;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Interop;
using MaterialDesignThemes.Wpf;

namespace ARC_Itecture.DrawCommand
{
    public class Receiver
    {
        private Canvas _canvas;
        private Brush _strokeBrush;
        private Brush _fillBrush;
        private Queue<Point> _wallPoints;
        private Stack<Point> _areaPoints;
        private Stack<Point> _windowPoints;
        private List<Line> _currentWalls;
        private List<Rect> _walls;
        private Plan _plan;
        private Shape _lastShape;
        private ViewModel _viewModel;

        private const int WINDOW_OFFSET = 10;

        public Receiver(ViewModel viewModel)
        {
            this._canvas = viewModel._mainWindow.canvas;
            this._strokeBrush = new SolidColorBrush(Colors.White);
            this._fillBrush = new SolidColorBrush(ImageUtil.RandomColor());
            this._wallPoints = new Queue<Point>();
            this._areaPoints = new Stack<Point>();
            this._windowPoints = new Stack<Point>();
            this._currentWalls = new List<Line>();
            this._walls = new List<Rect>();
            this._plan = viewModel.plan;
            this._viewModel = viewModel;
        }

        public void DrawWindow(Point p)
        {
            _windowPoints.Push(p);

            if (_windowPoints.Count == 2)
            {
                Point p2 = _windowPoints.Pop();
                Point p1 = _windowPoints.Pop();

                this._fillBrush = new SolidColorBrush(ImageUtil.RandomColor());
                //_plan.AddComponent(p1, p2, componentType);

                Rect rect = new Rect(Canvas.GetLeft(_lastShape), Canvas.GetTop(_lastShape), _lastShape.Width, _lastShape.Height);

                foreach(Rect wall in _walls)
                {
                    Rect intersect = Rect.Intersect(rect, wall);

                    if (!intersect.IsEmpty)
                    {        
                        if((intersect.X > wall.X && intersect.X + intersect.Width < wall.X + wall.Width) || 
                            (intersect.Y > wall.Y && intersect.Y + intersect.Height < wall.Y + wall.Height))
                        {
                            Rectangle rectangle = new Rectangle();
                            rectangle.Fill = Application.Current.TryFindResource("PrimaryHueDarkBrush") as SolidColorBrush;

                            Canvas.SetLeft(rectangle, intersect.X-(WINDOW_OFFSET/2));
                            Canvas.SetTop(rectangle, intersect.Y-(WINDOW_OFFSET/2));

                            rectangle.Width = intersect.Width + WINDOW_OFFSET;
                            rectangle.Height = intersect.Height + WINDOW_OFFSET;
                            
                            _canvas.Children.Add(rectangle);

                           _plan.AddWindow(new Point(intersect.X, intersect.Y), 
                               new Point(intersect.X + intersect.Width, intersect.Y + intersect.Height),
                               new Point(wall.X, wall.Y),
                               new Point(wall.X + wall.Width, wall.Y + wall.Height));

                            _walls.Remove(wall);

                            break; // Allows not to draw 2 windows on parallel walls
                        }
                    }
                }
                _canvas.Children.Remove(_lastShape);
                _lastShape = null;
            }
        }

        public void DrawWindowPreview(Point p)
        {
            _fillBrush = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
            Rectangle lastRectangle = _lastShape as Rectangle;

            if (lastRectangle != null)
                _canvas.Children.Remove(lastRectangle);

            if (_windowPoints.Count > 0)
                _lastShape = DrawRectangle(_windowPoints.Peek(), p);
        }

        public async void DrawArea(Point p)
        {
            _areaPoints.Push(p);

            if(_areaPoints.Count == 2)
            {
                Point p2 = _areaPoints.Pop();
                Point p1 = _areaPoints.Pop();

                this._fillBrush = new SolidColorBrush(ImageUtil.RandomColor());

                string areaTypeName = await _viewModel.ShowAreaDialog();
                _plan.AddArea(p1, p2, areaTypeName);

                TextBlock tb = new TextBlock();
                tb.Text = areaTypeName;
                tb.Foreground = Brushes.White;
                tb.Width = 70;
                tb.Height = 20;
                tb.TextAlignment = TextAlignment.Center;

                Canvas.SetLeft(tb, Canvas.GetLeft(_lastShape) + (_lastShape.Width / 2) - (tb.Width/2));
                Canvas.SetTop(tb, Canvas.GetTop(_lastShape) + (_lastShape.Height / 2) - (tb.Height / 2));
                _canvas.Children.Add(tb);

                _lastShape = null;
            }
        }

        public void DrawAreaPreview(Point p)
        {
            Rectangle lastRectangle = _lastShape as Rectangle;

            if (lastRectangle != null)
                _canvas.Children.Remove(lastRectangle);
            
            if(_areaPoints.Count > 0)
                _lastShape = DrawRectangle(_areaPoints.Peek(), p);
        }

        public void DrawCamera(Point p)
        {
            System.Drawing.Bitmap cameraBitmap = Properties.Resources.camera_icon;
            cameraBitmap.MakeTransparent(cameraBitmap.GetPixel(1, 1));

            Image cameraImage = new Image();
            cameraImage.Source = ImageUtil.ImageSourceFromBitmap(cameraBitmap);

            Canvas.SetLeft(cameraImage, p.X);
            Canvas.SetTop(cameraImage, p.Y);
            _canvas.Children.Add(cameraImage);
            _plan.AddCamera(p);
        }

        public void DrawDoor(Point p)
        {
            throw new NotImplementedException();
        }

        public void DrawWall(Point p)
        {
            this._wallPoints.Enqueue(p);

            if (this._wallPoints.Count % 2 == 0)
            {
                Point clickedP1 = _wallPoints.Dequeue();
                Point clickedP2 = _wallPoints.Dequeue();
                Line line = DrawSegment(clickedP1, clickedP2);

                Point realP1 = new Point(line.X1, line.Y1);
                Point realP2 = new Point(line.X2, line.Y2);

                _walls.Add(new Rect(realP1, realP2));
                _plan.AddWall(realP1, realP2);

                Intersection intersection = MathUtil.LineIntersect(line, _currentWalls);
                if(intersection.IntersectionPoint != null)
                {
                    if (intersection.L2.Equals(this._currentWalls[0]))
                    {
                        line.X2 = intersection.IntersectionPoint.Value.X;
                        line.Y2 = intersection.IntersectionPoint.Value.Y;

                        intersection.L2.X1 = line.X2;
                        intersection.L2.Y1 = line.Y2;

                        _currentWalls.Clear();
                    }
                    else
                    {
                        _wallPoints.Enqueue(new Point(line.X2, line.Y2));
                    }
                }
                else
                {
                    _wallPoints.Enqueue(new Point(line.X2, line.Y2));
                }

                _canvas.Children.Remove(_lastShape as Line);
                _currentWalls.Add(line);

                line.MouseEnter += (s, e) => Mouse.OverrideCursor = Cursors.Cross;
                line.MouseLeave += (s, e) => Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void DrawWallPreview(Point p)
        {
            Line lastWall = _lastShape as Line;

            if (lastWall != null)
                _canvas.Children.Remove(lastWall);

            if(_wallPoints.Count > 0)
                _lastShape = DrawSegment(_wallPoints.Peek(), p);
        }

        public void StartNewWall()
        {
            _wallPoints.Clear();
            _currentWalls.Clear();
        }

        private Line DrawSegment(Point p1, Point p2)
        {
            Line line = new Line();
            line.StrokeThickness = 1;
            line.Stroke = this._strokeBrush;

            double dX = Math.Abs(p2.X - p1.X);
            double dY = Math.Abs(p2.Y - p1.Y);

            line.X1 = p1.X;
            line.Y1 = p1.Y;

            if (dX > dY)
            {
                line.Y2 = p1.Y;
                line.X2 = p2.X;
            }
            else
            {
                line.X2 = p1.X;
                line.Y2 = p2.Y;
            }
            _canvas.Children.Add(line);

            return line;
        }

        private Rectangle DrawRectangle(Point p1, Point p2)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Fill = _fillBrush;

            rectangle.Width = Math.Abs(p2.X - p1.X);
            rectangle.Height = Math.Abs(p2.Y - p1.Y);

            double leftMostX = p2.X > p1.X ? p1.X : p2.X;
            double topMostY = p2.Y > p1.Y ? p1.Y : p2.Y;
            Canvas.SetLeft(rectangle, leftMostX);
            Canvas.SetTop(rectangle, topMostY);

            _canvas.Children.Add(rectangle);

            return rectangle;
        }

    }
}
