﻿/*
 * ARC-Itecture
 * Romain Capocasale, Vincent Moulin and Jonas Freiburghaus
 * He-Arc, INF3dlm-a
 * 2019-2020
 * .NET Course
 */

using System.Windows;

namespace ARC_Itecture.DrawCommand.Commands
{
    class WallCommand : IDrawCommand
    {
        private Receiver _receiver;

        public WallCommand(Receiver receiver)
        {
            this._receiver = receiver;    
        }

        public void Execute(Point point)
        {
            _receiver.DrawWall(point);
        }
    }
}
