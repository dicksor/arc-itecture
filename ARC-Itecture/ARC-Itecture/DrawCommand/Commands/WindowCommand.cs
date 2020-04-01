using System;
using System.Windows;

namespace ARC_Itecture.DrawCommand.Commands
{
    class WindowCommand : IDrawCommand
    {
        private Receiver _receiver;
        private const ComponentType _componentType = ComponentType.Window;

        public WindowCommand(Receiver receiver)
        {
            this._receiver = receiver;
        }

        public void Execute(Point point)
        {
            throw new NotImplementedException();
        }
    }
}
