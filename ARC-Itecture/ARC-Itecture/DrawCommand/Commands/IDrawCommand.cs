using System.Windows;

namespace ARC_Itecture.DrawCommand.Commands
{
    public interface IDrawCommand
    {
        void Execute(Point point);
    }
}
