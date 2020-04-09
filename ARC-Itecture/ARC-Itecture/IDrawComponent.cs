using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARC_Itecture
{
    public interface IDrawComponent
    {
        void RemoveComponent();
        string GetName();
    }
}
