using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Star_MVVM.Model.Enums
{
    public enum EnumNodeState
    {
        StartNode,
        EndNode,
        Open,
        Closed,
        Barrier,
        Path
    }
}
