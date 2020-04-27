/*
 * ARC-Itecture
 * Romain Capocasale, Vincent Moulin and Jonas Freiburghaus
 * He-Arc, INF3dlm-a
 * 2019-2020
 * .NET Course
 */


using ARC_Itecture.Geometry;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace ARC_Itecture.Utils
{
    class MathUtil
    {
        /// <summary>
        /// Compute the angle between two points
        /// </summary>
        /// <param name="p1">First point</param>
        /// <param name="p2">Second point</param>
        /// <returns>angle between the points</returns>
        public static double ComputeAngle(Point p1, Point p2)
        {
            double dX = Math.Abs(p2.X - p1.X);
            double dY = Math.Abs(p2.Y - p1.Y);
            double opp = dX < dY ? dX : dY;
            double adj = dX > dY ? dX : dY;

            return Math.Atan2(opp, adj);
        }

        /// <summary>
        /// Compute the euclidian distance between two points
        /// </summary>
        /// <param name="p1">First point</param>
        /// <param name="p2">Second point</param>
        /// <returns>Euclidian distance between two point</returns>
        public static double DistanceBetweenTwoPoints(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        /// <summary>
        /// Comupte the intersetion between a line and a list of line
        /// </summary>
        /// <param name="line">Line to check the intersect</param>
        /// <param name="lines">All lines to check intersections</param>
        /// <returns>Intersection object between lines</returns>
        public static Intersection LineIntersect(Line line, List<Line> lines)
        {
            Intersection intersection = new Intersection();

            double A1 = line.Y2 - line.Y1;
            double B1 = line.X1 - line.X2;
            double C1 = A1 * line.X1 + B1 * line.Y1;

            foreach (Line l in lines)
            {
                double A2 = l.Y2 - l.Y1;
                double B2 = l.X1 - l.X2;
                double C2 = A2 * l.X1 + B2 * l.Y1;

                double det = A1 * B2 - A2 * B1;

                if (det != 0)
                {
                    double x = (B2 * C1 - B1 * C2) / det;
                    double y = (A1 * C2 - A2 * C1) / det;

                    if (Math.Floor(x) != Math.Floor(line.X1) || Math.Floor(y) != Math.Floor(line.Y1))
                    {
                        if(IsThroughHorizontalLine(line, l) || IsThroughVerticalLine(line, l))
                        {
                            intersection.IntersectionPoint = new Point(x, y);
                            intersection.L1 = line;
                            intersection.L2 = l;
                        }
                    }
                }
            }

            return intersection;
        }

        /// <summary>
        /// Checks if intersection is through a vertical line
        /// </summary>
        /// <param name="l1">First line</param>
        /// <param name="l2">Second line</param>
        /// <returns>True if intersection is through a vertical line</returns>
        private static Boolean IsThroughVerticalLine(Line l1, Line l2)
        {
            Boolean isVerticalCross = l1.Y1 < l2.Y1 && l1.Y2 > l2.Y1 || l1.Y1 > l2.Y1 && l1.Y2 < l2.Y1;
            Boolean isBetweenX = l1.X1 > l2.X1 && l1.X1 < l2.X2 || l1.X1 < l2.X1 && l1.X1 > l2.X2;
            return isVerticalCross && isBetweenX;
        }

        /// <summary>
        /// Checks if intersection is through a horizontal line
        /// </summary>
        /// <param name="l1">First line</param>
        /// <param name="l2">Second line</param>
        /// <returns>True if intersection is through a horizontal line</returns>
        private static Boolean IsThroughHorizontalLine(Line l1, Line l2)
        {
            Boolean isHorizontalCross = l1.X1 < l2.X1 && l1.X2 > l2.X1 || l1.X1 > l2.X1 && l1.X2 < l2.X1;
            Boolean isBetweenY = l1.Y1 > l2.Y1 && l1.Y1 < l2.Y2 || l1.Y1 < l2.Y1 && l1.Y1 > l2.Y2;
            return isHorizontalCross && isBetweenY;
        }

    }
}
