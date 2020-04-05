﻿using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ARC_Itecture.Utils;
using ARC_Itecture.Geometry;

namespace ARC_Itecture.DrawCommand
{
    public class Receiver
    {
        private Canvas _canvas;
        private Brush _stroleBrush;
        private Brush _fillBrush;
        private Queue<Point> _wallPoints;
        private Stack<Point> _areaPoints;
        private List<Line> _walls;
        private Plan _plan;

        public Receiver(Canvas canvas, Plan plan)
        {
            this._canvas = canvas;
            this._stroleBrush = new SolidColorBrush(Colors.White);
            this._fillBrush = new SolidColorBrush(Color.FromArgb(100, 50, 50, 50));
            this._wallPoints = new Queue<Point>();
            this._areaPoints = new Stack<Point>();
            this._walls = new List<Line>();
            this._plan = plan;
        }

        public void DrawArea(Point p, ComponentType componentType)
        {
            _areaPoints.Push(p);
            if(_areaPoints.Count == 2)
            {
                Rectangle rectangle = new Rectangle();
                rectangle.Stroke = _stroleBrush;
                rectangle.Fill = _fillBrush;

                Point p2 = _areaPoints.Pop();
                Point p1 = _areaPoints.Pop();

                _plan.AddComponent(p1, p2, componentType);

                rectangle.Width = Math.Abs(p2.X - p1.X);
                rectangle.Height = Math.Abs(p2.Y - p1.Y);

                double leftMostX = p2.X > p1.X ? p1.X : p2.X;
                double topMostY = p2.Y > p1.Y ? p1.Y : p2.Y;
                Canvas.SetLeft(rectangle, leftMostX);
                Canvas.SetTop(rectangle, topMostY);

                _canvas.Children.Add(rectangle);
            }
        }

        public void DrawCamera(Point p, ComponentType componentType)
        {
            System.Drawing.Bitmap cameraBitmap = Properties.Resources.camera_icon;

            Image cameraImage = new Image();
            cameraImage.Source = ImageUtil.ImageSourceFromBitmap(cameraBitmap);

            Canvas.SetLeft(cameraImage, p.X);
            Canvas.SetTop(cameraImage, p.Y);
            _canvas.Children.Add(cameraImage);
        }

        public void DrawDoor(Point p, ComponentType componentType)
        {
            throw new NotImplementedException();
        }

        public void DrawWindow(Point p, ComponentType componentType)
        {
            throw new NotImplementedException();
        }

        public void DrawWall(Point p, ComponentType componentType)
        {
            this._wallPoints.Enqueue(p);

            if (this._wallPoints.Count % 2 == 0)
            {
                Line line = new Line();
                line.StrokeThickness = 1;
                line.Stroke = this._stroleBrush;

                Point p1 = _wallPoints.Dequeue();
                Point p2 = _wallPoints.Dequeue();

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

                Intersection intersection = MathUtil.LineIntersect(line, _walls);
                if(intersection.IntersectionPoint != null)
                {
                    if (intersection.L2.Equals(this._walls[0]))
                    {
                        line.X2 = intersection.IntersectionPoint.Value.X;
                        line.Y2 = intersection.IntersectionPoint.Value.Y;

                    
                        intersection.L2.X1 = line.X2;
                        intersection.L2.Y1 = line.Y2;
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
                
                _canvas.Children.Add(line);
                _walls.Add(line);
            }
        }



    }
}
