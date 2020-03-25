using System;
using System.Windows;

namespace ARC_Itecture.DrawCommand.Commands
{
    class DoorCommand : IDrawCommand
    {
        private Receiver _receiver;

        public DoorCommand(Receiver receiver)
        {
            this._receiver = receiver;
        }

        public void Execute(Point point)
        {
            throw new NotImplementedException();
        }
    }
}
