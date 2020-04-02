using ARC_Itecture.DrawCommand.Commands;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ARC_Itecture.DrawCommand
{
    public class Invoker
    {
        private IDrawCommand _command;
        public List<IDrawCommand> history = new List<IDrawCommand>();

        public IDrawCommand Command{
            set
            {
                this._command = value;
            }
        }

        public void Invoke(Point point)
        {
            if(this._command != null)
            {
                history.Add(this._command);
                this._command.Execute(point);
            }
            else
            {
                MessageBox.Show("Select a tool to draw");
            }
                
            // TODO : Else toast -> pick tool
        }
    }
}
