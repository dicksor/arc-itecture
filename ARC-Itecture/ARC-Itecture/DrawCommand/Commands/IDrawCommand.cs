using System.Windows;

namespace ARC_Itecture.DrawCommand.Commands
{
    interface IDrawCommand
    {
        void Execute(Point point);
    }
}
