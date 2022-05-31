using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NABLA.sim
{
    public class Circuit
    {
        
        // ***** Properties *****

        /// <summary>
        /// The base data structure within which each entity can be stored
        /// </summary>
        private Dictionary<string, Connector> _entities;

        //these are counts of all of the components in the circuit
        //TODO: this needs to reference the type descriptions so as to be in line with it
        /// <summary>
        /// Counts of each connector type
        /// </summary>
        private Dictionary<string, int> _connectorCounts = new Dictionary<string, int>() { { "Resistor", 0 }, {"IndependentSource", 0 } };

        /// <summary>
        /// A list of all the nodes in the circuit
        /// </summary>
        private List<int> _nodes = new List<int>();     


        // ***** Constructors *****

        /// <summary>
        /// Create a new instance of a circuit, used for holding connectors
        /// </summary>
        public Circuit()
        {
            _entities = new Dictionary<string, Connector>();
        }

        /// <summary>
        /// Create a new instance of a circuit, used for holding connectors
        /// </summary>
        /// <param name="Elements">A list of connectors in the circuit</param>
        public Circuit(List<Connector> Elements)
        {
            _entities = new Dictionary<string, Connector>();
            foreach (Connector item in Elements)
            {
                this.Add(item);
;           }
        }


        // ***** Element Modifying *****

        /// <summary>
        /// Add a new connector to the circuit
        /// </summary>
        /// <param name="item">The connector to add</param>
        /// <returns>True if the operation is succsesful</returns>
        public bool Add(Connector item)
        {
            try
            {
                _entities.Add(item.GetName(), item);
            }
            catch (ArgumentException)
            {
                return false;
            }
            
            //Update the connector count for the type
            //TODO: this should refrence a fucntion instead to handle potential unknown types
            _connectorCounts[item.GetConnectorType()] += 1;

            //Go through each node for the connector and add it if its a new one
            foreach (int node in item.GetNodes())
            {
                if (_nodes.Contains(node) == false)
                {
                    _nodes.Add(node);
                }
            }
            return true;
        }

        /// <summary>
        /// Load elements into a circuit from a netlsit
        /// </summary>
        /// <param name="netlist">The netlist to load from</param>
        /// <returns>True if succseful</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool LoadFromNetlist(Netlist netlist)
        {
            if (netlist == null || netlist.ElementArray == null)
            {
                throw new ArgumentNullException("Netlist is invalid");
                return false;
            }
            
            
            foreach (string[] item in netlist.ElementArray)
            {
                //we're swtiching on the first char as this signifies the type
                switch (item[0].Substring(0, 1))
                {
                    case "R":
                        Add(
                            new Resistor(item[0],
                            Convert.ToDouble(item[3]),
                            new List<int>()
                            {
                                Int32.Parse(item[1]),
                                Int32.Parse(item[2])
                            }));
                        break;     
                    case "V":
                        Add(
                            new IndependentSource(item[0],
                            Convert.ToDouble(item[3]),
                            new List<int>()
                            {
                                Int32.Parse(item[1]),
                                Int32.Parse(item[2])
                            }));
                        break;
                    
                    //no fall through at the moment, shouldn't be nessecary for safe operation
                    default:
                        break;
                }
            }
            return true;
        }


        // ***** Utility Accessors *****

        /// <summary>
        /// Get the list of nodes in the circuit
        /// </summary>
        /// <returns>A List of strings naming each node</returns>
        public List<int> GetNodes()
        {
            return _nodes;
        }

        /// <summary>
        /// Gets the count of a specific type of component in a circuit
        /// </summary>
        /// <param name="Type">The type of connector to get count of</param>
        /// <returns>An integer specifying the count</returns>
        public int GetCount(string Type)
        {
            return _connectorCounts[Type];
        }

        /// <summary>
        /// Get the count of nodes in the circuit
        /// </summary>
        /// <returns>An integer value for nodes</returns>
        public int GetNodeCount()
        {
            return _nodes.Count - 1;
        }


        // ***** Utilities *****

        /// <summary>
        /// Return an enumerable dictionary of connectors
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Connector>.Enumerator GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

    }
}
