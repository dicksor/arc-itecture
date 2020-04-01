using System.Windows;

namespace ARC_Itecture.DrawCommand.Commands
{
    class CameraCommand : IDrawCommand
    {
        private Receiver _receiver;
        private const ComponentType _componentType = ComponentType.Camera;

        public CameraCommand(Receiver receiver)
        {
            this._receiver = receiver;
        }

        public void Execute(Point point)
        {
            this._receiver.DrawCamera(point, _componentType);
        }
    }
}
