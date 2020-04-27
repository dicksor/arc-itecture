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
    /// Performs all the areas drawing
    /// </summary>
    class AreaDrawer : Drawer
    {

        private Stack<Point> _areaPoints;
        private String _areaTypeName;

        public String AreaTypeName
        {
            set
            {
                _areaTypeName = value;
            }
        }

        public AreaDrawer(Receiver receiver): base(receiver)
        {
            this._areaPoints = new Stack<Point>();
            this._areaTypeName = "";
        }

        /// <summary>
        /// Draws the area and open a dialog to pick up a name for the area
        /// </summary>
        /// <param name="p"></param>
        public override async void Draw(Point p)
        {
            _areaPoints.Push(p);

            if (_areaPoints.Count == 2)
            {
                Point p2 = _areaPoints.Pop();
                Point p1 = _areaPoints.Pop();

                this._fillBrush = new SolidColorBrush(ImageUtil.RandomColor());

                if (_areaTypeName == "")
                {
                    _areaTypeName = await _receiver.ViewModel.ShowAreaDialog();
                }

                Area area = _receiver.ViewModel._plan.AddArea(p1, p2, _areaTypeName);

                TextBlock textBlock = new TextBlock
                {
                    Text = _areaTypeName,
                    Foreground = Brushes.White,
                    Width = 70,
                    Height = 20,
                    TextAlignment = TextAlignment.Center,
                    LayoutTransform = new ScaleTransform(1, -1)
                };

                InkCanvas.SetLeft(textBlock, InkCanvas.GetLeft(_receiver.LastShape) + (_receiver.LastShape.Width / 2) - (textBlock.Width / 2));
                InkCanvas.SetTop(textBlock, InkCanvas.GetTop(_receiver.LastShape) + (_receiver.LastShape.Height / 2) - (textBlock.Height / 2));
                _receiver.ViewModel._mainWindow.canvas.Children.Add(textBlock);

                MainWindow.main.History = "Area";
                _receiver.ViewModel._stackHistory.Push(new Tuple<Object, Object, string>(textBlock, area, "Area"));

                _receiver.LastShape = null;
            }
        }

        public override void DrawPreview(Point p)
        {
            if (_receiver.LastShape is Rectangle lastRectangle)
                _receiver.ViewModel._mainWindow.canvas.Children.Remove(lastRectangle);

            if (_areaPoints.Count > 0)
                _receiver.LastShape = DrawRectangle(_areaPoints.Peek(), p);
        }
    }
}
