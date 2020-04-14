using System.Windows;

namespace ARC_Itecture.DrawCommand.Commands
{
    class CameraCommand : IDrawCommand
    {
        private Receiver _receiver;
        private static bool _isAlreadyUsed = false;

        public CameraCommand(Receiver receiver)
        {
            this._receiver = receiver;
        }

        public static void ResetIsAlreadyUsed()
        {
            _isAlreadyUsed = false;
        }

        public void Execute(Point point)
        {
            if (!_isAlreadyUsed)
            {
                this._receiver.DrawCamera(point);
                _isAlreadyUsed = !_isAlreadyUsed;
            }
        }
    }
}
