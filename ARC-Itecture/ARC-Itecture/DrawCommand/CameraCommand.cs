using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ARC_Itecture.Command
{
    class CameraCommand : IDrawCommand
    {
        private Receiver _receiver;

        public CameraCommand(Receiver receiver)
        {
            this._receiver = receiver;
        }

        public void Execute(Point point)
        {
            this._receiver.DrawCamera(point);
        }
    }
}
