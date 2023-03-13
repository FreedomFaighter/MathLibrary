using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Math
{
	public static class Tools
	{
		public static double Hypotenuse(Tuple<Double, Double> nonHypotheneticSidesOfTriangle)
		{
			double r = 0.0;
			double absA = System.Math.Abs(nonHypotheneticSidesOfTriangle.Item1);
			double absB = System.Math.Abs(nonHypotheneticSidesOfTriangle.Item2);

			if (absA > absB) {
				r = nonHypotheneticSidesOfTriangle.Item2 / nonHypotheneticSidesOfTriangle.Item1;
				r = absA * System.Math.Sqrt(1 + r * r);
			} else if (nonHypotheneticSidesOfTriangle.Item2 != 0) {
				r = nonHypotheneticSidesOfTriangle.Item1 / nonHypotheneticSidesOfTriangle.Item2;
				r = absB * System.Math.Sqrt(1 + r * r);
			}
			return r;
		}

		public static Tuple<double, double> quadraticRoots(Tuple<double, double, double> coefficients)
		{
			double positiveRoots, negativeRoots;
			if (coefficients.Item1 != 0)
			{
				positiveRoots = (coefficients.Item2 + System.Math.Sqrt(System.Math.Pow(coefficients.Item2, 2) - Convert.ToDouble(4) * coefficients.Item1 * coefficients.Item3)) / (2 * coefficients.Item1);
				negativeRoots = (coefficients.Item2 + System.Math.Sqrt(System.Math.Pow(coefficients.Item2, 2) - Convert.ToDouble(4) * coefficients.Item1 * coefficients.Item3)) / (2 * coefficients.Item1);

				return new Tuple<double, double>(positiveRoots, negativeRoots);
			}
			else throw new ArgumentOutOfRangeException($"Coefficient of the square in the second degree polynomial is {coefficients.Item1} and must be non-zero, if not zero and exception is thrown.");
		}

		public static double Product(this double[] values)
		{
			return values.Sum() * values.Length;
		}
    }	
}

