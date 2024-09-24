
namespace Statistics.Logistic
{
    internal class logisticMethods
    {
        internal static Func<ProbabilityValue, ProbabilityValue> OddsRatio(Tuple<OddsRatio, ProbabilityValue>[] tuple)
        {
            if (tuple.Select(x => x.Item2.Value).Sum() < 0 || tuple.Select(x => x.Item2.Value).Sum() > 1)
                throw new ArgumentOutOfRangeException("Given probabilities outside the valid range");
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

        internal static Decimal OddsRatioMeasure(Tuple<OddsRatio, ProbabilityValue>[] tuple, ProbabilityValue x)
        {
            return System.Math.Abs(OddsRatio(tuple).Invoke(x).Value-x.Value);
        }
    }
}
