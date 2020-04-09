using System.Windows;

namespace ARC_Itecture.DrawCommand.Commands
{
    class PreviewWindowCommand:IDrawCommand
    {
        private Receiver _receiver;

        public PreviewWindowCommand(Receiver receiver)
        {
            this._receiver = receiver;
        }

        public void Execute(Point point)
        {
            this._receiver.DrawWindowPreview(point);
        }
    }
}
