/*
 * ARC-Itecture
 * Romain Capocasale, Vincent Moulin and Jonas Freiburghaus
 * He-Arc, INF3dlm-a
 * 2019-2020
 * .NET Course
 */

using System.Windows;

namespace ARC_Itecture.DrawCommand.Commands
{
    class PreviewWallCommand : IDrawCommand
    {
        private Receiver _receiver;

        public PreviewWallCommand(Receiver receiver)
        {
            this._receiver = receiver;
        }

        public void Execute(Point point)
        {
            this._receiver.DrawWallPreview(point);
        }
    }
}
