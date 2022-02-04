using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NABLA.sim
{
    abstract public class Connector
    {
        protected string _name;

        protected string _type;

        protected ParameterSet Parameters { get; set; }

        protected List<string> _nodes;

        public string GetConnectorType()
        {
            return _type;
        }

        public bool ConnectNodes(List<string> Nodes)
        {
            _nodes = new List<string>();
            foreach (string node in Nodes)
            {
                _nodes.Add(node);
            }
            return true;
        }

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

        public List<string> GetNodes()
        {
            return _nodes;
        }

        public ParameterSet GetParameters()
        {
            return Parameters;
        }

        public string GetName()
        {
            return _name;
        }
    }
}
