using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Statistics
{
    internal class OddsRatio
    {
        readonly Decimal value;

        internal OddsRatio(Decimal value)
        {

            this.value = value;

            if (Validate())
                throw new ArgumentException("Value is less then zero for Odds Ratio");
        }

        internal Decimal Value { get { return value; } }

        internal bool Validate()
        {
            if (value < 0)
                return false;
            else return true;
        }
    }
}
