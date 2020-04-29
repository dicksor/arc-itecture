/*
 * ARC-Itecture
 * Romain Capocasale, Vincent Moulin and Jonas Freiburghaus
 * He-Arc, INF3dlm-a
 * 2019-2020
 * .NET Course
 */


using System;
using System.Windows;
using System.Windows.Shapes;

namespace ARC_Itecture.Geometry
{
    /// <summary>
    /// Contains the necessary information for the intersection of two segments
    /// </summary>    
    public class Intersection
    {
        public Point? IntersectionPoint { get; set; }
        public Line L1 { get; set; }
        public Line L2 { get; set; }

       public Intersection()
        {

        }

        public Intersection(Intersection intersection)
        {
            IntersectionPoint = intersection.IntersectionPoint;

            L1 = new Line();
            L1.X1 = intersection.L1.X1;
            L1.X2 = intersection.L1.X2;
            L1.Y1 = intersection.L1.Y1;
            L1.Y2 = intersection.L1.Y2;

            L2 = new Line();
            L2.X1 = intersection.L2.X1;
            L2.X2 = intersection.L2.X2;
            L2.Y1 = intersection.L2.Y1;
            L2.Y2 = intersection.L2.Y2;
        }
    }
}
