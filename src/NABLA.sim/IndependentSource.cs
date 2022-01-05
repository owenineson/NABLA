using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NABLA.sim
{
    class IndependentSource : Connector
    {
        /// <summary>
        /// Source name
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
        /// Value for Independent Source
        /// </summary>
        private float _value;
        public float Value
        {
            get { return _value; }
            set
            {
                if (!(value >= 0))
                {
                    throw new ArgumentException("Source value must be a non negative float");
                }
                else
                {
                    _value = value;
                }
            }
        }

        /// <summary>
        /// Instantiate a new IndependentSource object
        /// </summary>
        /// <param name="ElementName">Name of element</param>
        public IndependentSource(string ElementName)
        {
            Element = ElementName;
        }

        /// <summary>
        /// Instantiate a new IndependentSource object
        /// </summary>
        /// <param name="ElementName">Name of element</param>
        /// <param name="PNode">Positive node of the element</param>
        /// <param name="NNode">Negative noe of the element</param>
        /// <param name="Value">Value of the element</param>
        public IndependentSource(string ElementName, int PNode, int NNode, float Value)
        {
            this.Element = ElementName;
            this.PNode = PNode;
            this.NNode = NNode;
            this.Value = Value;

        }
    }
}
