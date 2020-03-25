using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ARC_Itecture.Command
{
    interface IDrawCommand
    {
        void Execute(Point point);
    }
}
