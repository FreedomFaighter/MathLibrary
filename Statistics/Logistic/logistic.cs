
namespace Statistics.Logistic
{
    /// <summary>
    /// logistic methods holds static methods for usage of an open form of checking the numerical error after a logistic regression has calculated the odds ratios
    /// </summary>
    internal class logisticMethods
    {
        /// <summary>
        /// static method for usage of odds ratios and probability associated to compute value of the total probability response from a logistic regression
        /// </summary>
        /// <param name="tuple"></param>
        /// <returns>returns a Func class type constaining the method for calculating the value of the sum of the probabilities</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal static Func<ProbabilityValue, ProbabilityValue> OddsRatio(Tuple<OddsRatio, ProbabilityValue>[] tuple)
        {
            if (tuple.Count() < 1)
                throw new ArgumentException(nameof(tuple), "Does not contain any members in the array");
            if (tuple.Select(x => x.Item2.Value).Sum() < 0 || tuple.Select(x => x.Item2.Value).Sum() > 1)
                throw new ArgumentOutOfRangeException(nameof(tuple), "Given probabilities outside the valid range");
            Func<ProbabilityValue, ProbabilityValue> func = (x) =>
            {
                Func<ProbabilityValue, ProbabilityValue> innerFunc = null;
                foreach (var pair in tuple)
                {
                    if (innerFunc == null)
                        innerFunc = (q) => { return new ProbabilityValue(q.Value * pair.Item1.Value * pair.Item2.Value / (Decimal.One - q.Value * (Decimal.One + pair.Item1.Value))); };
                    else
                        innerFunc += (q) => { return new ProbabilityValue(q.Value * pair.Item1.Value * pair.Item2.Value / (Decimal.One - q.Value * (Decimal.One + pair.Item1.Value))); };
                }

                return innerFunc == null ? default : innerFunc(x);
            };
            return func;
        }
        /// <summary>
        /// Measure for calculating the differing in the Func from OddsRatio and the value given
        /// </summary>
        /// <param name="tuple"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        internal static Decimal OddsRatioMeasure(Tuple<OddsRatio, ProbabilityValue>[] tuple)
        {
            return System.Math.Abs(OddsRatio(tuple).Invoke(new ProbabilityValue(tuple.Select(x => x.Item2.Value).Sum())).Value-tuple.Select(x => x.Item2.Value).Sum());
        }
    }
}
