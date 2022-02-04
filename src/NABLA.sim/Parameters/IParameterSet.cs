using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NABLA.sim
{
    interface IParameterSet
    {

        public bool TryGetParameter(string Parameter);

        public double GetParameter(string Parameter);

        public bool TrySetParameter(string Parameter, double Value);

        public bool SetParameter(string Parameter, double Value);

        public bool LoadType(string Type);

    }
}
