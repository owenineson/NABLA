using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NABLA.sim
{
    class VoltageControlledSource : Connector
    {
        protected float Value;
        protected int Node1;
        protected int Node2;
    }
}
