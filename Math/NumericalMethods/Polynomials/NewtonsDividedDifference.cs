using System;
using System.Linq;
namespace MathLibrary.NumericalMethods.Polynomials
{
	public class NewtonsDividedDifference
	{
		double[] x_i;
		double[] f_x_i;
		public NewtonsDividedDifference (double[] x_i, double[] f_x_i)
		{
			this.x_i = x_i;
			this.f_x_i = f_x_i;
		}

		public Func<double, double> Solve()
		{
			double[,] F = new double[f_x_i.Length, f_x_i.Length];
			for (int i = 0; i < f_x_i.Length; i++) {
				F [i, 0] = f_x_i[i];
			}
			for (int i = 1; i < f_x_i.Length; i++) {
				for (int j = 1; j <= i; j++) {
					F [i, j] = (F [i, j - 1] - F [i - 1, j - 1]) / (x_i [i] - x_i [i - j]);
				}
			}

			Func<double, double> f = delegate(double arg) {
				return 0;
			};

			for (int i = 0; i < f_x_i.Length; i++) {
				Func<double, double> product = delegate (double arg1){
					return 1;
				};
				for (int j = 0; j <= i - 1; j++) {
					product = delegate(double arg2) {
						return product (arg2) * (arg2 - x_i [j]);
					};
				}
				f = delegate(double arg3)
				{
					return f(arg3) + (F[i,i] * product(arg3));
				};
			}

			return f;
		}
	}
}

