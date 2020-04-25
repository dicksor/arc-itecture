/*
 * ARC-Itecture
 * Romain Capocasale, Vincent Moulin and Jonas Freiburghaus
 * He-Arc, INF3dlm-a
 * 2019-2020
 * .NET Course
 */


using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ARC_Itecture.Utils;
using ARC_Itecture.Geometry;
using System.Windows.Input;
using System.Linq;

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
        private Stack<Point> _doorPoints;
        private List<Line> _currentWalls;
        private List<Rect> _windowAvailableWalls;
        private List<Point> _doorAvailablePoints;
        private Plan _plan;
        private Shape _lastShape;
        private ViewModel _viewModel;

        private const int COMPONENT_OFFSET = 10;
        private const int DOOR_MINIMUM_DISTANCE = 10;
        private const int DOOR_MAXIMUM_DISTANCE = 80;

        public Receiver(ViewModel viewModel)
        {
            this._canvas = viewModel._mainWindow.canvas;
            this._strokeBrush = new SolidColorBrush(Colors.White);
            this._fillBrush = new SolidColorBrush(ImageUtil.RandomColor());
            this._wallPoints = new Queue<Point>();
            this._areaPoints = new Stack<Point>();
            this._windowPoints = new Stack<Point>();
            this._doorPoints = new Stack<Point>();
            this._currentWalls = new List<Line>();
            this._windowAvailableWalls = new List<Rect>();
            this._doorAvailablePoints = new List<Point>();
            this._plan = viewModel._plan;
            this._viewModel = viewModel;
        }

        public async void DrawArea(Point p, string areaTypeName)
        {
            _areaPoints.Push(p);

            if(_areaPoints.Count == 2)
            {
                Point p2 = _areaPoints.Pop();
                Point p1 = _areaPoints.Pop();

                this._fillBrush = new SolidColorBrush(ImageUtil.RandomColor());

                if(areaTypeName == "")
                {
                    areaTypeName = await _viewModel.ShowAreaDialog();
                }
                
                Area area = _plan.AddArea(p1, p2, areaTypeName);

                TextBlock textBlock = new TextBlock
                {
                    Text = areaTypeName,
                    Foreground = Brushes.White,
                    Width = 70,
                    Height = 20,
                    TextAlignment = TextAlignment.Center,
                    LayoutTransform = new ScaleTransform(1, -1)
                };

                Canvas.SetLeft(textBlock, Canvas.GetLeft(_lastShape) + (_lastShape.Width / 2) - (textBlock.Width / 2));
                Canvas.SetTop(textBlock, Canvas.GetTop(_lastShape) + (_lastShape.Height / 2) - (textBlock.Height / 2));
                _canvas.Children.Add(textBlock);

                MainWindow.main.History = "Area";
                _viewModel._stackHistory.Push(new Tuple<Object, Object, string>(textBlock, area, "Area"));

                _lastShape = null;
            }
        }

        public void DrawAreaPreview(Point p)
        {

            if (_lastShape is Rectangle lastRectangle)
                _canvas.Children.Remove(lastRectangle);

            if (_areaPoints.Count > 0)
                _lastShape = DrawRectangle(_areaPoints.Peek(), p);
        }

        public void DrawCamera(Point p)
        {
            System.Drawing.Bitmap cameraBitmap = Properties.Resources.camera_icon;
            cameraBitmap.MakeTransparent(cameraBitmap.GetPixel(1, 1));

            Image cameraImage = new Image
            {
                Source = ImageUtil.ImageSourceFromBitmap(cameraBitmap),
                LayoutTransform = new ScaleTransform(1, -1)
            };

            Canvas.SetLeft(cameraImage, p.X);
            Canvas.SetTop(cameraImage, p.Y);
            _canvas.Children.Add(cameraImage);

            MainWindow.main.History = "Camera";
            _viewModel._stackHistory.Push(new Tuple<Object, Object, string>(cameraImage, cameraImage, "Camera"));

            _plan.AddCamera(p);
        }

        public void DrawDoor(Point p)
        {
            _doorPoints.Push(p);

            if (_doorPoints.Count == 2)
            {
                Point p2 = _doorPoints.Pop();
                Point p1 = _doorPoints.Pop();

                this._fillBrush = new SolidColorBrush(ImageUtil.RandomColor());

                Rect rect = new Rect(Canvas.GetLeft(_lastShape), Canvas.GetTop(_lastShape), _lastShape.Width, _lastShape.Height);

                List<Point> doorAnchorPoints = new List<Point>();

                foreach(Point doorPoint in _doorAvailablePoints)
                {
                    if(rect.Contains(doorPoint))
                    {
                        doorAnchorPoints.Add(doorPoint);
                    }
                }

                if (doorAnchorPoints.Count >= 2 )
                {
                    double pointsDoorDistance = MathUtil.DistanceBetweenTwoPoints(doorAnchorPoints[0], doorAnchorPoints[1]);

                    if (pointsDoorDistance > DOOR_MINIMUM_DISTANCE && pointsDoorDistance < DOOR_MAXIMUM_DISTANCE && !IsDoorOnWall(doorAnchorPoints[0], doorAnchorPoints[1]))
                    {

                        Rectangle rectangle = new Rectangle
                        {
                            Fill = Application.Current.TryFindResource("PrimaryHueLightBrush") as SolidColorBrush
                        };

                        doorAnchorPoints = doorAnchorPoints.OrderBy(point => point.X).ToList();
                        Canvas.SetLeft(rectangle, doorAnchorPoints[0].X);

                        doorAnchorPoints = doorAnchorPoints.OrderBy(point => point.Y).ToList();
                        Canvas.SetTop(rectangle, doorAnchorPoints[0].Y);

                        double width = Math.Abs(doorAnchorPoints[1].X - doorAnchorPoints[0].X);
                        double height = Math.Abs(doorAnchorPoints[1].Y - doorAnchorPoints[0].Y);

                        if (width <= COMPONENT_OFFSET)
                        {
                            width = COMPONENT_OFFSET;
                        }
                        if (height <= COMPONENT_OFFSET)
                        {
                            height = COMPONENT_OFFSET;
                        }

                        rectangle.Width = width;
                        rectangle.Height = height;

                        _canvas.Children.Add(rectangle);

                        Door door = _plan.AddDoor(doorAnchorPoints[0], doorAnchorPoints[1]);
                        MainWindow.main.History = "Door";
                        _viewModel._stackHistory.Push(new Tuple<object, object, string>(rectangle, door, "Door"));
                    }
                }    


                _canvas.Children.Remove(_lastShape);
                _lastShape = null;
            }
        }

        public void DrawDoorPreview(Point p)
        {
            _fillBrush = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));

            if (_lastShape is Rectangle lastRectangle)
                _canvas.Children.Remove(lastRectangle);

            if (_doorPoints.Count > 0)
                _lastShape = DrawRectangle(_doorPoints.Peek(), p);
        }

        public void DrawWall(Point p)
        {
            this._wallPoints.Enqueue(p);

            if (this._wallPoints.Count % 2 == 0)
            {
                Point clickedP1 = _wallPoints.Dequeue();
                Point clickedP2 = _wallPoints.Dequeue();

                if (clickedP1 != clickedP2)
                {
                    Line line = DrawSegment(clickedP1, clickedP2);

                    Point realP1 = new Point(line.X1, line.Y1);
                    Point realP2 = new Point(line.X2, line.Y2);

                    _windowAvailableWalls.Add(new Rect(realP1, realP2));

                    _doorAvailablePoints.Add(realP1);
                    _doorAvailablePoints.Add(realP2);

                    Segment segment = _plan.AddWall(realP1, realP2);

                    Intersection intersection = MathUtil.LineIntersect(line, _currentWalls);
                    if (intersection.IntersectionPoint != null)
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

                    MainWindow.main.History = "Wall";
                    _viewModel._stackHistory.Push(new Tuple<Object, Object, string>(line, segment, "Wall"));

                    line.MouseEnter += (s, e) => Mouse.OverrideCursor = Cursors.Cross;
                    line.MouseLeave += (s, e) => Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
        }

        public void DrawWallPreview(Point p)
        {

            if (_lastShape is Line lastWall)
                _canvas.Children.Remove(lastWall);

            if (_wallPoints.Count > 0)
                _lastShape = DrawSegment(_wallPoints.Peek(), p);
        }

        public void DrawWindow(Point p)
        {
            _windowPoints.Push(p);

            if (_windowPoints.Count == 2)
            {
                Point p2 = _windowPoints.Pop();
                Point p1 = _windowPoints.Pop();

                this._fillBrush = new SolidColorBrush(ImageUtil.RandomColor());

                Rect rect = new Rect(Canvas.GetLeft(_lastShape), Canvas.GetTop(_lastShape), _lastShape.Width, _lastShape.Height);

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

                            Canvas.SetLeft(rectangle, intersect.X - (COMPONENT_OFFSET / 2));
                            Canvas.SetTop(rectangle, intersect.Y - (COMPONENT_OFFSET / 2));

                            rectangle.Width = intersect.Width + COMPONENT_OFFSET;
                            rectangle.Height = intersect.Height + COMPONENT_OFFSET;

                            _canvas.Children.Add(rectangle);

                            HouseWindow houseWindow = _plan.AddWindow(new Point(intersect.X, intersect.Y),
                               new Point(intersect.X + intersect.Width, intersect.Y + intersect.Height),
                               new Point(wall.X, wall.Y),
                               new Point(wall.X + wall.Width, wall.Y + wall.Height));

                            if (houseWindow != null)
                            {
                                MainWindow.main.History = "Window";
                                _viewModel._stackHistory.Push(new Tuple<Object, Object, string>(rectangle, houseWindow, "Window"));

                                _windowAvailableWalls.Remove(wall);

                                break; // Allows not to draw 2 windows on parallel walls
                            }
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

            if (_lastShape is Rectangle lastRectangle)
                _canvas.Children.Remove(lastRectangle);

            if (_windowPoints.Count > 0)
                _lastShape = DrawRectangle(_windowPoints.Peek(), p);
        }

        public void RemoveLastPreview()
        {
            _canvas.Children.Remove(_lastShape);
        }

        public void StartNewWall()
        {
            if (_lastShape is Line lastWall)
                _canvas.Children.Remove(lastWall);

            _wallPoints.Clear();
            _currentWalls.Clear();
        }

        public void UpdateAvailableWindowList(Rectangle rect)
        {
            Line line = FindSegmentByWindow(rect);

            Point realP1 = new Point(line.X1, line.Y1);
            Point realP2 = new Point(line.X2, line.Y2);

            _windowAvailableWalls.Add(new Rect(realP1, realP2));

        }

        private bool IsDoorOnWall(Point p1, Point p2)
        {
            bool isDoorOnWall = false;

            List<double> doorPointsX = new List<double>() { Math.Floor(p1.X), Math.Floor(p2.X) };
            List<double> doorPointsY = new List<double>() { Math.Floor(p1.Y), Math.Floor(p2.Y) };
            doorPointsX.Sort();
            doorPointsY.Sort();

            foreach (UIElement element in _canvas.Children)
            {
                if (element is Line line)
                {
                    List<double> segmentPointsX = new List<double>() { Math.Floor(line.X1), Math.Floor(line.X2) };
                    segmentPointsX.Sort();
                    if (doorPointsX[0] == segmentPointsX[0] && doorPointsX[1] == segmentPointsX[1])
                    {
                        isDoorOnWall = true;
                    }

                    List<double> segmentPointsY = new List<double>() { Math.Floor(line.Y1), Math.Floor(line.Y2) };
                    segmentPointsY.Sort();
                    if (doorPointsY[0] == segmentPointsY[0] && doorPointsY[1] == segmentPointsY[1])
                    {
                        isDoorOnWall = true;
                    }
                }
            }
            return isDoorOnWall;
        }

        private Rectangle DrawRectangle(Point p1, Point p2)
        {
            Rectangle rectangle = new Rectangle
            {
                Fill = _fillBrush,
                Width = Math.Abs(p2.X - p1.X),
                Height = Math.Abs(p2.Y - p1.Y)
            };

            double leftMostX = p2.X > p1.X ? p1.X : p2.X;
            double topMostY = p2.Y > p1.Y ? p1.Y : p2.Y;
            Canvas.SetLeft(rectangle, leftMostX);
            Canvas.SetTop(rectangle, topMostY);

            _canvas.Children.Add(rectangle);

            return rectangle;
        }

        private Line DrawSegment(Point p1, Point p2)
        {
            Line line = new Line
            {
                StrokeThickness = 1,
                Stroke = this._strokeBrush
            };

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

        private Line FindSegmentByWindow(Rectangle rect)
        {
            double x = Canvas.GetLeft(rect);
            double y = Canvas.GetTop(rect);
            double width = rect.Width;
            double height = rect.Height;

            Line wall = null;

            foreach(UIElement element in _canvas.Children)
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
