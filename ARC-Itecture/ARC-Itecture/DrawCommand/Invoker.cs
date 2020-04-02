using ARC_Itecture.DrawCommand.Commands;
using System.Windows;
using System.Windows.Controls;

namespace ARC_Itecture.DrawCommand
{
    public class Invoker
    {
        private IDrawCommand _command;

        public IDrawCommand Command{
            set
            {
                this._command = value;
            }
        }

        public void Invoke(Point point)
        {
            if(this._command != null)
                this._command.Execute(point);
            // TODO : Else toast -> pick tool
        }
    }
}
