/*
 * ARC-Itecture
 * Romain Capocasale, Vincent Moulin and Jonas Freiburghaus
 * He-Arc, INF3dlm-a
 * 2019-2020
 * .NET Course
 */


using ARC_Itecture.DrawCommand.Commands;
using System.Collections.Generic;
using System.Windows;

namespace ARC_Itecture.DrawCommand
{

    /// <summary>
    /// Manages the current selected tools
    /// There is one tool for the shape's preview
    /// and one tool to draw the final shape
    /// </summary>
    public class Invoker
    {
        private IDrawCommand _drawCommand;
        private IDrawCommand _previewCommand;

        public IDrawCommand DrawCommand{
            get
            {
                return this._drawCommand;
            }
            set
            {
                this._drawCommand = value;
            }
        }

        public IDrawCommand PreviewCommand
        {
            get
            {
                return this._drawCommand;
            }
            set
            {
                this._previewCommand = value;
            }
        }

        /// <summary>
        /// Used to draw the final shape
        /// </summary>
        /// <param name="point">Clicked point</param>
        public void InvokeClick(Point point)
        {
            if(this._drawCommand != null)
            {
                _drawCommand.Execute(point);
            }
            else
            {
                MessageBox.Show("Select a tool to draw");
            }
        }
        
        /// <summary>
        /// Used to draw the shape preview
        /// </summary>
        /// <param name="point">Current mouse point</param>
        public void InvokeMouseMove(Point point)
        {
            if (this._previewCommand != null)
                _previewCommand.Execute(point);
        }
    }
}
