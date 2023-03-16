using System;
using System.Linq;

namespace Statistics
{
	static public class Expectation
	{
		static public double Mean(this double[] source)
		{
			if(source.Length > 0)
				return source.AsParallel().Sum () / source.Length;
			throw new DivideByZeroException("number of measurements too small (<1)");
		}

		static public double Variance(this double[] source)
		{
			double mean = source.Mean ();
			if (source.Length > 1)
				return source.Select(x => (x - mean) * (x - mean)).AsParallel().Sum() / (source.Length - 1);
			else
				throw new DivideByZeroException("number of measurements too small (<2)");
		}
  
  static public double StandardDeviation(this double[] values)
  {
    return System.Math.Sqrt(values.Variance());
  }
	}
}