using System.Windows;

namespace ARC_Itecture.DrawCommand.Commands
{
    class CameraCommand : IDrawCommand
    {
        private Receiver _receiver;
        private const ComponentType _componentType = ComponentType.Camera;
        private static bool _isAlreadyUsed = false;

        public CameraCommand(Receiver receiver)
        {
            this._receiver = receiver;
        }

        public void Execute(Point point)
        {
            if (!_isAlreadyUsed)
            {
                this._receiver.DrawCamera(point, _componentType);
                _isAlreadyUsed = !_isAlreadyUsed;
            }
        }
    }
}
