/*
 * ARC-Itecture
 * Romain Capocasale, Vincent Moulin and Jonas Freiburghaus
 * He-Arc, INF3dlm-a
 * 2019-2020
 * .NET Course
 */


using System.Windows;
using System.Windows.Shapes;

namespace ARC_Itecture.Geometry
{
    /// <summary>
    /// Contains the necessary information for the intersection of two segments
    /// </summary>    
    class Intersection
    {
        public Point? IntersectionPoint { get; set; }
        public Line L1 { get; set; }
        public Line L2 { get; set; }
    }
}
