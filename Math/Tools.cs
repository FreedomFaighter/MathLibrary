using System;
using System.Linq;

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
			double root1, root2;
			if (coefficients.Item1 != 0)
			{
				root1 = (coefficients.Item2 + System.Math.Sqrt(System.Math.Pow(coefficients.Item2, 2) - Convert.ToDouble(4) * coefficients.Item1 * coefficients.Item3)) / (2 * coefficients.Item1);
				root2 = (coefficients.Item2 - System.Math.Sqrt(System.Math.Pow(coefficients.Item2, 2) - Convert.ToDouble(4) * coefficients.Item1 * coefficients.Item3)) / (2 * coefficients.Item1);

				return new Tuple<double, double>(root1, root2);
			}
			else throw new ArgumentOutOfRangeException($"Coefficient of the square in the second degree polynomial is {coefficients.Item1} and must be non-zero, if not zero and exception is thrown.");
		}

		public static double Product(this double[] values)
		{
			return values.Aggregate((x1, x2) =>
			{
				return x1 * x2;
			});
		}
    }	
}
