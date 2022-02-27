using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NABLA.sim
{
    /// <summary>
    /// A source that maintains a voltage irrespective of circuit conditions
    /// </summary>
    class IndependentSource : Connector
    {
        /// <summary>
        /// Create a new Independent Source
        /// </summary>
        public IndependentSource(string Name, double Value, List<int> Nodes)
        {
            _type = "IndependentSource";
            _name = Name;
            Parameters = new ParameterSet(_type);
            Parameters.SetParameter("Voltage", Value);
            this.ConnectNodes(Nodes);
        }
    }
}
