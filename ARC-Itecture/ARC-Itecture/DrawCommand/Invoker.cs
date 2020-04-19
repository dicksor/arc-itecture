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

        public void InvokeMouseMove(Point point)
        {
            if (this._previewCommand != null)
                _previewCommand.Execute(point);
        }
    }
}
