using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NABLA.sim
{
    /// <summary>
    /// A collection of parameters specific to a type
    /// </summary>
    public class ParameterSet 
    {
        /// <summary>
        /// A dictionary with string keys to refrence parameters and double values
        /// </summary>
        private Dictionary<string, double> _parameters;

        /// <summary>
        /// Create a new ParameterSet
        /// </summary>
        /// <param name="Type">A string specifying the type of the connector</param>
        public ParameterSet(string Type)
        {
            _parameters = new Dictionary<string, double>();
            this.LoadType(Type);
        }

        /// <summary>
        /// Get a specific parameter value
        /// </summary>
        /// <param name="Parameter">A string naming the parameter</param>
        /// <returns>A double of the parameter value</returns>
        public double GetParameter(string Parameter)
        {
            double value = _parameters[Parameter];
            return value;
        }

        /// <summary>
        /// Try get a parameter value
        /// </summary>
        /// <param name="Parameter">A string naming the parameter</param>
        /// <returns>True if succsesful</returns>
        public bool TryGetParameter(string Parameter)
        {
            if(_parameters.ContainsKey(Parameter))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set the value of a parameter
        /// </summary>
        /// <param name="Parameter">A string naming the parameter</param>
        /// <param name="Value">The value to set the parameter too</param>
        /// <returns>True if succsesful</returns>
        public bool SetParameter(string Parameter, double Value)
        {
            _parameters[Parameter] = Value;
            return true;
        }

        /// <summary>
        /// Try set a parameter value
        /// </summary>
        /// <param name="Parameter">A string naming the parameter</param>
        /// <param name="Value">The value to set the parameter too</param>
        /// <returns>True if succseful</returns>
        public bool TrySetParameter(string Parameter, double Value)
        {
            if (_parameters.ContainsKey(Parameter))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Load the correct parameters to the set
        /// </summary>
        /// <param name="Type">The type of connector</param>
        /// <returns>True if succsesful</returns>
        public bool LoadType(string Type)
        {
            if (Type == "Resistor")
            {
                _parameters.Add("Resistance", -1);
            }
            else if (Type == "IndependentSource")
            {
                _parameters.Add("Voltage", -1);
            }
            return true;
        }

    }
}
