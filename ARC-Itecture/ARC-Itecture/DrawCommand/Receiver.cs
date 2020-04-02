﻿using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ARC_Itecture.DrawCommand
{
    public class Receiver
    {
        private Canvas _canvas;
        private Brush _brush;
        private Queue<Point> _wallPoints;
        private Stack<Point> _areaPoints;
        private Plan _plan;

        public Receiver(Canvas canvas, Plan plan)
        {
            this._canvas = canvas;
            this._brush = new SolidColorBrush(Colors.White);
            this._wallPoints = new Queue<Point>();
            this._areaPoints = new Stack<Point>();
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
            Ellipse ellipse = new Ellipse();
            ellipse.Fill = this._brush;
            ellipse.Width = 10;
            ellipse.Height = 10;
            Canvas.SetLeft(ellipse, p.X);
            Canvas.SetTop(ellipse, p.Y);

            _plan.addComponent(p, componentType);

            this._canvas.Children.Add(ellipse);
        }

        public void DrawWall(Point p, ComponentType componentType)
        {
            this._wallPoints.Enqueue(p);

            if(this._wallPoints.Count % 2 == 0)
            {
                Line line = new Line();
                line.StrokeThickness = 1;
                line.Stroke = this._brush;

                Point p1 = this._wallPoints.Dequeue();
            
                line.X1 = p1.X;
                line.Y1 = p1.Y;

                Point p2 = this._wallPoints.Peek();

                line.X2 = p2.X;
                line.Y2 = p2.Y;

                this._canvas.Children.Add(line);
            }
        }


    }
}
