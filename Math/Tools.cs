using System;
using System.Linq;
namespace Math
{
	public class Tools
	{
		public static double Hypotenuse(double a, double b)
		{
			double r = 0.0;
			double absA = System.Math.Abs (a);
			double absB = System.Math.Abs (b);

			if (absA > absB) {
				r = b / a;
				r = absA * System.Math.Sqrt (1 + r * r);
			} else if (b != 0) {
				r = a / b;
				r = absB * System.Math.Sqrt (1 + r * r);
			}
			return r;
		}
	}

	public static class DoubleExtensions
	{
		public static double Product(this double[] value)
		{
			double product = 1;
			for (int i =0; i < value.Length; i++) {
				product *= value [i];
			}
			return product;
		}
	}
}

