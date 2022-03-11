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
        protected int _pNode
        {
            get => _pNode;
            set => _pNode = value;
        }
        protected int _nNode
        {
            get => _nNode;
            set => _nNode = value;
        }
    }
}
