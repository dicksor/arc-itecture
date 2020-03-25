using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ARC_Itecture.Command
{
    class Invoker
    {
        private IDrawCommand _command;
        private Receiver _receiver;

        public Invoker(Canvas canvas)
        {
            this._receiver = new Receiver(canvas);
            this._command = new CameraCommand(this._receiver);
        }

        public void SelectCommand()
        {
            // TODO when tool from toolbar is selected
        }

        public void Invoke(Point point)
        {
            this._command.Execute(point);
        }
    }
}
