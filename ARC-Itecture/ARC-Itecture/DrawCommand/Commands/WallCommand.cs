using System;
using System.Windows;

namespace ARC_Itecture.DrawCommand.Commands
{
    class WallCommand : IDrawCommand
    {

        private Receiver _receiver;
        private const ComponentType _componentType = ComponentType.Wall;

        public WallCommand(Receiver receiver)
        {
            this._receiver = receiver;
        }

        public void Execute(Point point)
        {
            _receiver.DrawWall(point, _componentType);
        }
    }
}
