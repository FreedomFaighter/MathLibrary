namespace Statistics
{
    /// <summary>
    /// Odds ratio with Decimal type and bounds checking
    /// Value is readonly after constructor use
    /// </summary>
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
