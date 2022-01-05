using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NABLA.sim
{
    class Connector
    {
        /// <summary>
        /// The positve node of the 2 pole circuit element
        /// </summary>
        protected int _pNode;
        public int PNode
        {
            get { return _pNode; }
            set
            {
                if (!(value >= 0))
                {
                    throw new ArgumentException("PNode value must be a non negative number");
                }
                else
                {
                    _pNode = value;
                }
            }
        }
        
        /// <summary>
        /// The negative node of the 2 pole circuit element
        /// </summary>
        protected int _nNode;
        public int NNode 
        {
            get { return _nNode; }
            set
            {
                if (!(value >= 0))
                {
                    throw new ArgumentException("NNode value must be a non negative number");
                }
                else
                {
                    _nNode = value;
                }
            }
        }
    }
}
