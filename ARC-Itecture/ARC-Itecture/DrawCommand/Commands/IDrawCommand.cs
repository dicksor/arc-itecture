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
    public interface IDrawCommand
    {
        void Execute(Point point);
    }
}
