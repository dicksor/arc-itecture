using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using ARC_Itecture.Utils;
using ARC_Itecture.Geometry;

namespace ARC_Itecture.DrawCommand
{
    public class Receiver
    {
        private Canvas _canvas;
        private Brush _brush;
        private Queue<Point> _wallPoints;
        private Stack<Point> _areaPoints;
        private List<Line> _walls;
        private Plan _plan;

        public Receiver(Canvas canvas, Plan plan)
        {
            this._canvas = canvas;
            this._brush = new SolidColorBrush(Colors.White);
            this._wallPoints = new Queue<Point>();
            this._areaPoints = new Stack<Point>();
            this._walls = new List<Line>();
            this._plan = plan;
        }

        public void DrawArea(Point p, ComponentType componentType)
        {
            this._areaPoints.Push(p);
            if(this._areaPoints.Count == 2)
            {
                Rectangle rectangle = new Rectangle();
                rectangle.Stroke = this._brush;

                Point p2 = this._areaPoints.Pop();
                Point p1 = this._areaPoints.Pop();

                _plan.addComponent(p1, p2, componentType);

                rectangle.Width = Math.Abs(p2.X - p1.X);
                rectangle.Height = Math.Abs(p2.Y - p1.Y);

                double leftMostX = p2.X > p1.X ? p1.X : p2.X;
                double topMostY = p2.Y > p1.Y ? p1.Y : p2.Y;
                Canvas.SetLeft(rectangle, leftMostX);
                Canvas.SetTop(rectangle, topMostY);

                this._canvas.Children.Add(rectangle);
            }
        }

        public void DrawCamera(Point p, ComponentType componentType)
        {
            System.Drawing.Bitmap cameraBitmap = Properties.Resources.camera_icon;

            Image cameraImage = new Image();
            cameraImage.Source = ImageUtil.ImageSourceFromBitmap(cameraBitmap);

            Canvas.SetLeft(cameraImage, p.X);
            Canvas.SetTop(cameraImage, p.Y);
            this._canvas.Children.Add(cameraImage);
        }

        public void DrawWall(Point p, ComponentType componentType)
        {
            this._wallPoints.Enqueue(p);

            if (this._wallPoints.Count % 2 == 0)
            {
                Line line = new Line();
                line.StrokeThickness = 1;
                line.Stroke = this._brush;

                Point p1 = this._wallPoints.Dequeue();
                Point p2 = this._wallPoints.Dequeue();

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

                Intersection intersection = MathUtil.LineIntersect(line, this._walls);
                if(intersection.IntersectionPoint != null)
                {
                    line.X2 = intersection.IntersectionPoint.Value.X;
                    line.Y2 = intersection.IntersectionPoint.Value.Y;

                    if (intersection.L2.Equals(this._walls[0]))
                    {
                        intersection.L2.X1 = line.X2;
                        intersection.L2.Y1 = line.Y2;
                    }
                }
                else
                {
                    this._wallPoints.Enqueue(new Point(line.X2, line.Y2));
                }
                
                this._canvas.Children.Add(line);
                this._walls.Add(line);
            }
        }

    }
}
