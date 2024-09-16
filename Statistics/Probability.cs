using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics
{
    internal class ProbabilityValue
    {
        readonly private Decimal value;

        internal ProbabilityValue(Decimal value)
        {   
            this.value = value;
            if (!this.Validate())
                throw new ArgumentException("Not a valid probability.");
        }

        internal bool Validate()
        {
            return value <= 1 && value >= 0;
        }

        public override string ToString()
        {
            return this.value.ToString();
        }

        internal Decimal Value { get { return this.value; } }
    }

    internal static class ProbabilityValueExtensions
    {
        internal static bool IsValid(this ProbabilityValue value)
        {
            return value.Validate();
        }
    }
}
