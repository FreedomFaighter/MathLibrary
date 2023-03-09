using MathLibrary.NumericalMethods;

namespace Math.NumericalMethods
{
	public class Eulers : INumericalMethod<decimal>
	{
		Func<decimal, decimal, decimal> f_of_t_y;
		decimal alpha, a, b;
		int N;
		public Eulers (Func<decimal, decimal, decimal> f, decimal alpha, decimal a, decimal b, int N)
		{
			this.f_of_t_y = f;
			this.alpha = alpha;
			this.a = a;
			this.b = b;
			this.N = N;
		}

		public IEnumerable<Tuple<decimal, decimal>> Solve()
		{
			decimal h = (b - a) / N, t = a, w = alpha;
			yield return new Tuple<decimal, decimal> (t, w);
			int i = 1;
			while (i<=N) {
				w = w + h * this.f_of_t_y (t, w);
				t = a + i * h;
				yield return new Tuple<decimal, decimal> (t, w);
			}
		}
	}
}

