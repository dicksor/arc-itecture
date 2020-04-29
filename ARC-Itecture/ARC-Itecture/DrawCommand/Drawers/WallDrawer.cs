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
        private List<Point> _doorAvailablePoints; // Contains all the points available for the design of a door
        private List<Rect> _windowAvailableWalls; // Contains all the points available for drawing a window
        private Queue<Point> _wallPoints;

        public WallDrawer(Receiver receiver, ref List<Point> doorAvailablePoints, ref List<Rect> windowAvailableWalls)
            : base(receiver)
        {
            this._currentWalls = new List<Line>();
            this._doorAvailablePoints = doorAvailablePoints;
            this._windowAvailableWalls = windowAvailableWalls;
            this._wallPoints = new Queue<Point>();  
        }

        /// <summary>
        /// Draw a wall to the canvas 
        /// </summary>
        /// <param name="p">Wall point</param>
        public override void Draw(Point p)
        {
            this._wallPoints.Enqueue(p);

            //Count two point to create the rectangle area
            if (this._wallPoints.Count % 2 == 0)
            {
                Point clickedP1 = _wallPoints.Dequeue();
                Point clickedP2 = _wallPoints.Dequeue();

                if (clickedP1 != clickedP2)
                {
                    Line line = DrawSegment(clickedP1, clickedP2);

                    //Contain the line points
                    Point realP1 = new Point(line.X1, line.Y1);
                    Point realP2 = new Point(line.X2, line.Y2);

                    _windowAvailableWalls.Add(new Rect(realP1, realP2));  // Add the wall in the available wall list

                    // Add this point to available door points
                    _doorAvailablePoints.Add(realP1);
                    _doorAvailablePoints.Add(realP2);

                    Segment segment = _receiver.ViewModel._plan.AddWall(realP1, realP2); // Add the wall to the plan

                    //Search for a intersection between two line
                    Intersection intersection = MathUtil.LineIntersect(line, _currentWalls);
                    if (intersection.IntersectionPoint != null)
                    {
                        //If two walls intersect, they're cut off
                        if (intersection.L2.Equals(this._currentWalls[0]))
                        {
                            _receiver.ViewModel._plan.UpdateWall(intersection); //If there is an intersection and the wall is shortened it has to be updated in the plan

                            Intersection oldIntersection = new Intersection(intersection);

                            // Shortcut the 2 intersecting walls
                            line.X2 = intersection.IntersectionPoint.Value.X;
                            line.Y2 = intersection.IntersectionPoint.Value.Y;

                            intersection.L2.X1 = line.X2;
                            intersection.L2.Y1 = line.Y2;

                            UpdateDoorsAndWindowsList(oldIntersection, intersection);

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

                    // Add the wall to the history
                    MainWindow.main.History = "Wall";
                    _receiver.ViewModel._stackHistory.Push(new Tuple<Object, Object, string>(line, segment, "Wall"));

                    line.MouseEnter += (s, e) => Mouse.OverrideCursor = Cursors.Cross;
                    line.MouseLeave += (s, e) => Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
        }

        /// <summary>
        /// Update the door anchor point list and the windows wall anchor list
        /// </summary>
        /// <param name="oldI">Old intersection object</param>
        /// <param name="newI">New intersection object</param>
        public void UpdateDoorsAndWindowsList(Intersection oldI, Intersection newI)
         {
            //Crate the old and new line  
            Line oldL1 = oldI.L1;
            Line oldL2 = oldI.L2;
            Line newL1 = newI.L1;
            Line newL2 = newI.L2;

            //Delete old door points
            _doorAvailablePoints.Remove(new Point(oldL1.X1, oldL1.Y1));
            _doorAvailablePoints.Remove(new Point(oldL1.X2, oldL1.Y2));
            _doorAvailablePoints.Remove(new Point(oldL2.X1, oldL2.Y1));
            _doorAvailablePoints.Remove(new Point(oldL2.X2, oldL2.Y2));

            // Create new door points
            _doorAvailablePoints.Add(new Point(newL1.X1, newL1.Y1));
            _doorAvailablePoints.Add(new Point(newL1.X2, newL1.Y2));
            _doorAvailablePoints.Add(new Point(newL2.X1, newL2.Y1));
            _doorAvailablePoints.Add(new Point(newL2.X2, newL2.Y2));

            //Delete old window anchor walls
            for(int i = _windowAvailableWalls.Count -1; i >= 0; i--)
            {
                if(_windowAvailableWalls[i].Contains(new Point(oldL1.X1, oldL1.Y1)) && _windowAvailableWalls[i].Contains(new Point(oldL1.X2, oldL1.Y2))
                    || _windowAvailableWalls[i].Contains(new Point(oldL2.X1, oldL2.Y1)) && _windowAvailableWalls[i].Contains(new Point(oldL2.X2, oldL2.Y2)))
                {
                    _windowAvailableWalls.RemoveAt(i);
                }
            }

            //Create new window anchor walls
            _windowAvailableWalls.Add(new Rect(new Point(newL1.X1, newL1.Y1), new Point(newL1.X2, newL1.Y2)));
            _windowAvailableWalls.Add(new Rect(new Point(newL2.X1, newL2.Y1), new Point(newL2.X2, newL2.Y2)));
        }

        /// <summary>
        /// Draw the wall preview
        /// </summary>
        /// <param name="p"></param>
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
