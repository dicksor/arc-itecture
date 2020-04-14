using System.Windows;

namespace ARC_Itecture.DrawCommand.Commands
{
    class CameraCommand : IDrawCommand
    {
        private Receiver _receiver;
        private static bool isAlreadyUsed = false;

        public CameraCommand(Receiver receiver)
        {
            this._receiver = receiver;
        }

        public static void ResetIsAlreadyUsed()
        {
            isAlreadyUsed = false;
        }

        public void Execute(Point point)
        {
            if (!isAlreadyUsed)
            {
                this._receiver.DrawCamera(point);
                isAlreadyUsed = !isAlreadyUsed;
            }
        }
    }
}
