/*
 * ARC-Itecture
 * Romain Capocasale, Vincent Moulin and Jonas Freiburghaus
 * He-Arc, INF3dlm-a
 * 2019-2020
 * .NET Course
 */


using ARC_Itecture.Geometry;
using ARC_Itecture.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace ARC_Itecture.DrawCommand.Drawers
{

    /// <summary>
    /// Performs all the walls drawing
    /// </summary>
    class WallDrawer : Drawer
    {
        private List<Line> _currentWalls;
        private List<Point> _doorAvailablePoints;
        private List<Rect> _windowAvailableWalls;
        private Queue<Point> _wallPoints;

        public WallDrawer(Receiver receiver, ref List<Point> doorAvailablePoints, ref List<Rect> windowAvailableWalls)
            : base(receiver)
        {
            this._currentWalls = new List<Line>();
            this._doorAvailablePoints = doorAvailablePoints;
            this._windowAvailableWalls = windowAvailableWalls;
            this._wallPoints = new Queue<Point>();  
        }

        public override void Draw(Point p)
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

                    Segment segment = _receiver.ViewModel._plan.AddWall(realP1, realP2);

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

                    _receiver.ViewModel._mainWindow.canvas.Children.Remove(_receiver.LastShape as Line);
                    _currentWalls.Add(line);

                    MainWindow.main.History = "Wall";
                    _receiver.ViewModel._stackHistory.Push(new Tuple<Object, Object, string>(line, segment, "Wall"));

                    line.MouseEnter += (s, e) => Mouse.OverrideCursor = Cursors.Cross;
                    line.MouseLeave += (s, e) => Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
        }

        public override void DrawPreview(Point p)
        {
            if (_receiver.LastShape is Line lastWall)
                _receiver.ViewModel._mainWindow.canvas.Children.Remove(lastWall);

            if (_wallPoints.Count > 0)
                _receiver.LastShape = DrawSegment(_wallPoints.Peek(), p);
        }

        /// <summary>
        /// Allows to start a wall from a new point
        /// </summary>
        public void StartNewWall()
        {
            if (_receiver.LastShape is Line lastWall)
                _receiver.ViewModel._mainWindow.canvas.Children.Remove(lastWall);

            _wallPoints.Clear();
            _currentWalls.Clear();
        }
    }
}
