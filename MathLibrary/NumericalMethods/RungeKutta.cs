using MathLibrary.NumericalMethods;

namespace Math.NumericalMethods
{
	public class RungeKutta : INumericalMethod<decimal>
	{
		private Func<decimal, decimal, decimal> f_of_t_y;
		private decimal alpha, a, b;
		public RungeKutta (Func<decimal, decimal, decimal> ftyt, decimal alpha, decimal a, decimal b)
		{
			this.f_of_t_y = ftyt;
			this.a = a;
			this.b = b;
			this.alpha = alpha;
		}

		private decimal tolerance = 0.001M;

		public IEnumerable<Tuple<decimal, decimal>> Solve()
		{
			int N = Convert.ToInt32 (1 / tolerance);
			decimal h = (b - a) / N;
			decimal t = a;
			decimal w = alpha, K1, K2, K3, K4;
			yield return new Tuple<decimal, decimal> (t, w);
			int i = 1;
			do {
				K1 = h * f_of_t_y (t, w);
				K2 = h * f_of_t_y (t + h / 2, w + K1 / 2);
				K3 = h * f_of_t_y (t + h / 2, w + K2 / 2);
				K4 = h * f_of_t_y (t + h, w + K3);
				w = w + (K1 + 2 * K2 + 2 * K3 + K4) / 6;
				t = a + i * h;
				yield return new Tuple<decimal, decimal> (t, w);
				i++;
			} while(i <= N);
		}
	}
}

