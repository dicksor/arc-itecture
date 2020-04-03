using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ARC_Itecture.Utils
{
    class MathUtil
    {

        public static double ComputeAngle(Point p1, Point p2)
        {
            double dX = Math.Abs(p2.X - p1.X);
            double dY = Math.Abs(p2.Y - p1.Y);
            double opp = dX < dY ? dX : dY;
            double adj = dX > dY ? dX : dY;

            return Math.Atan2(opp, adj);
        }

    }
}
