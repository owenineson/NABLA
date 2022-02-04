using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NABLA.sim
{
    public class ParameterSet : IParameterSet
    {

        private Dictionary<string, double> _parameters;

        public ParameterSet(string Type)
        {
            _parameters = new Dictionary<string, double>();
            this.LoadType(Type);
        }

        public double GetParameter(string Parameter)
        {
            return _parameters[Parameter];
        }

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

        public bool SetParameter(string Parameter, double Value)
        {
            _parameters[Parameter] = Value;
            return true;
        }

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
