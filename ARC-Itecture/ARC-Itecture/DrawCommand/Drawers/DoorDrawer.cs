using ARC_Itecture.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ARC_Itecture.DrawCommand.Drawers
{
    internal class DoorDrawer : Drawer
    {
        private List<Point> _doorAvailablePoints;
        private Stack<Point> _doorPoints;

        private const int DOOR_MAXIMUM_DISTANCE = 80;
        private const int DOOR_MINIMUM_DISTANCE = 10;
        
        public DoorDrawer(Receiver receiver, ref List<Point> doorAvailablePoints) 
            : base(receiver)
        {
            this._doorAvailablePoints = doorAvailablePoints;
            this._doorPoints = new Stack<Point>();
        }

        public override void Draw(Point p)
        {
            _doorPoints.Push(p);

            if (_doorPoints.Count == 2)
            {
                Point p2 = _doorPoints.Pop();
                Point p1 = _doorPoints.Pop();

                this._fillBrush = new SolidColorBrush(ImageUtil.RandomColor());

                Rect rect = new Rect(Canvas.GetLeft(_receiver.LastShape), Canvas.GetTop(_receiver.LastShape), _receiver.LastShape.Width, _receiver.LastShape.Height);

                List<Point> doorAnchorPoints = new List<Point>();

                foreach (Point doorPoint in _doorAvailablePoints)
                {
                    if (rect.Contains(doorPoint))
                    {
                        doorAnchorPoints.Add(doorPoint);
                    }
                }

                if (doorAnchorPoints.Count >= 2)
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

                        _receiver.ViewModel._mainWindow.canvas.Children.Add(rectangle);

                        Door door = _receiver.ViewModel._plan.AddDoor(doorAnchorPoints[0], doorAnchorPoints[1]);
                        MainWindow.main.History = "Door";
                        _receiver.ViewModel._stackHistory.Push(new Tuple<object, object, string>(rectangle, door, "Door"));
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

            if (_doorPoints.Count > 0)
                _receiver.LastShape = DrawRectangle(_doorPoints.Peek(), p);
        }

        private bool IsDoorOnWall(Point p1, Point p2)
        {
            bool isDoorOnWall = false;

            List<double> doorPointsX = new List<double>() { Math.Floor(p1.X), Math.Floor(p2.X) };
            List<double> doorPointsY = new List<double>() { Math.Floor(p1.Y), Math.Floor(p2.Y) };
            doorPointsX.Sort();
            doorPointsY.Sort();

            foreach (UIElement element in _receiver.ViewModel._mainWindow.canvas.Children)
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
    }
}
