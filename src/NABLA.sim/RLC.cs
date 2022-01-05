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
    class RLC : Connector
    {
        /// <summary>
        /// Name (and type) of the element
        /// </summary>
        private string _element;
        public string Element
        {
            get { return _element; }
            set
            {
                if (!Regex.IsMatch(value, "([1-z])"))
                {
                    throw new ArgumentException("Source name must only be an alphanumeric character sequence");
                }
                else
                {
                    _element = value;
                }
            }
        }

        /// <summary>
        /// Value for RLC component
        /// </summary>
        private float _value;
        public float Value
        {
            get { return _value; }
            set
            {
                if (! (value >= 0))
                {
                    throw new ArgumentException("Element value must be a non negative float");
                }
                else
                {
                    _value = value;
                }
            }
        }
        
        /// <summary>
        /// Instantiate a new RLC object
        /// </summary>
        /// <param name="ElementName">Name of element</param>
        public RLC(string ElementName)
        {
            Element = ElementName;
        }
        
        /// <summary>
        /// Instantiate a new RLC object
        /// </summary>
        /// <param name="ElementName">Name of element</param>
        /// <param name="PNode">Positive node of the element</param>
        /// <param name="NNode">Negative noe of the element</param>
        /// <param name="Value">Value of the element</param>
        public RLC(string ElementName, int PNode, int NNode, float Value)
        {
            this.Element = ElementName;
            this.PNode = PNode;
            this.NNode = NNode;
            this.Value = Value;

        }
    }
}
