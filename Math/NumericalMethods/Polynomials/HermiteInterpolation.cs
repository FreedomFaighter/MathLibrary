using System;

namespace Math
{
	public class HermiteInterpolation
	{
		double[] f_x_i, f_prime_x_i, x_i;


		public HermiteInterpolation (double[] x_i, double[] f_x_i, double[] f_prime_x_i)
		{
			this.x_i = x_i;
			this.f_x_i = f_x_i;
			this.f_prime_x_i = f_prime_x_i;
		}

		public Func<double, double> Solve()
		{
			double[] z = new double[this.x_i.Length * 2 + 1];
			double[,] Q = new double[this.x_i.Length * 2 + 1, this.x_i.Length * 2 + 1];
			for (int i = 0; i < this.x_i.Length; i++) {
				z [2 * i] = x_i [i];
				z [2 * i + 1] = x_i [i];
				Q [2 * i, 0] = f_x_i [i];
				Q [2 * i + 1, 0] = f_x_i [i];
				Q [2 * i + 1, 1] = f_prime_x_i [i];
				if (i != 0)
					Q [2 * i, 1] = (Q [2 * i, 0] - Q [2 * i - 1, 0]) / (z [2 * i] - z [2 * i - 1]);
			}
			for (int i = 2; i < 2*x_i.Length+1; i++) {
				for (int j = 2; j <= i; j++) {
					Q [i, j] = (Q [i, j - 1] - Q [i - 1, j - 1]) / (z [i] - z [i - j]);
				}
			}

			Func<double, double> H = delegate(double x){
				return 0;
			};

			return H;
		}
	}
}

