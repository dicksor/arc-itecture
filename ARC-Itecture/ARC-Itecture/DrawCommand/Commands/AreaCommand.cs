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
    class AreaCommand : IDrawCommand
    {
        private Receiver _receiver;
        private string _areaTypeName;

        public AreaCommand(Receiver receiver, string areaTypeName = "")
        {
            this._areaTypeName = areaTypeName;
            this._receiver = receiver;
        }

        public void Execute(Point point)
        {
            this._receiver.DrawArea(point, _areaTypeName);
        }
    }
}
