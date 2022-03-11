using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NABLA.sim
{
    /// <summary>
    /// A linear resistor
    /// </summary>
    class Resistor : Connector
    {

        /// <summary>
        /// Create a new resistor
        /// </summary>
        /// <param name="Name">The name of the connector</param>
        /// <param name="Value">The value for resistance</param>
        /// <param name="Nodes">A list of nodes the resistor is connected between</param>
        public Resistor(string Name, double Value, List<int> Nodes)
        {
            _type = "Resistor";
            _name = Name;
            Parameters = new ParameterSet(_type);
            Parameters.SetParameter("Resistance", Value);
            this.ConnectNodes(Nodes);
        }

    }
}
