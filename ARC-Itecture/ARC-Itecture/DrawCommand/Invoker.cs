using ARC_Itecture.DrawCommand.Commands;
using System.Collections.Generic;
using System.Windows;

namespace ARC_Itecture.DrawCommand
{
    public class Invoker
    {
        private IDrawCommand _drawCommand;
        private IDrawCommand _previewCommand;
        public List<IDrawCommand> history = new List<IDrawCommand>();

        public IDrawCommand DrawCommand{
            set
            {
                this._drawCommand = value;
            }
        }

        public IDrawCommand PreviewCommand
        {
            set
            {
                this._previewCommand = value;
            }
        }

        public void InvokeClick(Point point)
        {
            if(this._drawCommand != null)
            {
                history.Add(this._drawCommand);
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
