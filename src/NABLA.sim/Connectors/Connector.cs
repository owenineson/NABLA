using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NABLA.sim
{
    abstract public class Connector
    {
        /// <summary>
        /// The name of the connector, as referenced in the netlist
        /// </summary>
        protected string _name;

        /// <summary>
        /// The type of the connector (i.e. resistor, capacitor)
        /// </summary>
        protected string _type;

        /// <summary>
        /// The given parameters specific to the type
        /// </summary>
        protected ParameterSet Parameters { get; set; }

        /// <summary>
        /// A list of nodes the connector is connected between
        /// </summary>
        protected List<int> _nodes;

        /// <summary>
        /// Get the type of the connector (i.e. resistor, capacitor)
        /// </summary>
        /// <returns>A string specfiying the type</returns>
        public string GetConnectorType()
        {
            return _type;
        }

        /// <summary>
        /// Connect the connector between the nodes specfied
        /// </summary>
        /// <param name="Nodes">A list of node names</param>
        /// <returns>True if succsesful</returns>
        public bool ConnectNodes(List<int> Nodes)
        {
            _nodes = new List<int>();
            foreach (int node in Nodes)
            {
                _nodes.Add(node);
            }
            return true;
        }

        /// <summary>
        /// Get a value for a specific parameter
        /// </summary>
        /// <param name="Parameter">A string specifying the parameter</param>
        /// <returns>A double with the value of the parameter</returns>
        public double GetParameter(string Parameter)
        {
            if (Parameters.TryGetParameter(Parameter))
            {
                return Parameters.GetParameter(Parameter);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Get the parameter set for a connector
        /// </summary>
        /// <returns>The ParameterSet for the connector</returns>
        public ParameterSet GetParameterSet()
        {
            return Parameters;
        }

        /// <summary>
        /// Gets this list of nodes the connector is between
        /// </summary>
        /// <returns>A list of strings of each node name</returns>
        public List<int> GetNodes()
        {
            return _nodes;
        }

        /// <summary>
        /// Get the name of the connector as referenced by the netlist
        /// </summary>
        /// <returns>A string of the connector name</returns>
        public string GetName()
        {
            return _name;
        }
    }
}
