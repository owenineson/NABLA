using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NABLA.sim
{
    /// <summary>
    /// A circuit element of type Resistor, Inductor or Capacitor
    /// </summary>
    class Resistor : Connector
    {

        /// <summary>
        /// Instantiate a new RLC object
        /// </summary>
        public Resistor(string Name, double Value, List<string> Nodes)
        {
            _type = "Resistor";
            _name = Name;
            Parameters = new ParameterSet(_type);
            Parameters.SetParameter("Resistance", Value);
            this.ConnectNodes(Nodes);
        }

    }
}
