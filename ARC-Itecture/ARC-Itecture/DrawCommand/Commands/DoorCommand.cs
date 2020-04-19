/*
 * ARC-Itecture
 * Romain Capocasale, Vincent Moulin and Jonas Freiburghaus
 * He-Arc, INF3dlm-a
 * 2019-2020
 * .NET Course
 */

using System;
using System.Windows;

namespace ARC_Itecture.DrawCommand.Commands
{
    class DoorCommand : IDrawCommand
    {
        private Receiver _receiver;

        public DoorCommand(Receiver receiver)
        {
            this._receiver = receiver;
        }

        public void Execute(Point point)
        {
            _receiver.DrawDoor(point);
        }
    }
}
