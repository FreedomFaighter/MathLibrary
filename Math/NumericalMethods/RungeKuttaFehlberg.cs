using System;
using System.Collections.Generic;

namespace Math.NumericalMethods
{
	public class RungeKuttaFehlberg :INumericalMethod<decimal>
	{
		Func<decimal, decimal, decimal> f_of_t_y;
		decimal alpha, a, b, hmax, hmin, TOL;
		public RungeKuttaFehlberg (Func<decimal, decimal, decimal> f, decimal alpha
		                           , decimal a, decimal b, decimal hmax
		                           , decimal hmin, decimal TOL)
		{
			this.f_of_t_y = f;
			this.alpha = alpha;
			this.a = a;
			this.b = b;
			this.hmax = hmax;
			this.hmin = hmin;
			this.TOL = TOL;
		}

		public IEnumerable<Tuple<decimal, decimal>> Solve()
		{
			decimal t = a, w = alpha, h = hmax;
			bool FLAG = true;
			decimal K1, K2, K3, K4, K5, K6, R, delta;
			while (FLAG) {
				K1 = h * f_of_t_y (t, w);
				K2 = h * f_of_t_y (t + h / 4.0M, w + K1 / 4.0M);
				K3 = h * f_of_t_y (t + 3.0M / 8.0M * h, w + 3.0M / 32.0M * K1 + 9.0M / 32.0M * K2);
				K4 = h * f_of_t_y (t + 12.0M / 13.0M, w + 1932.0M / 2197.0M * K1 - 7200.0M / 2197.0M * K2 + 7296.0M / 2197.0M * K3);
				K5 = h * f_of_t_y (t + h, w + 439.0M / 216.0M * K1 - 8.0M * K2 + 3680.0M / 513.0M * K3 - 845.0M / 4104.0M * K4);
				K6 = h * f_of_t_y (t + h / 2.0M, w - 8.0M / 27.0M * K1 + 2.0M * K2 - 3544.0M / 2565.0M * K3 + 1859.0M / 4104.0M * K4 - 11.0M / 40.0M * K5);
				R = System.Math.Abs (K1 / 360.0M - 128.0M / 4275.0M * K3 - 2197.0M / 75240.0M * K4 + K5 / 50.0M + 2.0M / 55.0M * K6) / hmax;
				if (R <= this.TOL) {
					t += h;
					w += 25.0M / 216.0M * K1 + 1408.0M / 2565.0M * K3 + 2197.0M / 4104.0M * K4 - K5 / 5.0M;
					yield return new Tuple<decimal, decimal> (t, w);
				}
				delta = 0.84M * (TOL / R).Pow (1.0M / 4.0M);
				if (delta <= 0.1M) {
					h *= 0.1M;
				} else if (delta >= 4.0M) {
					h *= 4.0M;
				} else {
					h *= delta;
				}

				if (h > hmax) {
					h = hmax;
				}

				if (t >= b) {
					FLAG = false;
				} else if (t + h > b) {
					h = b - t;
				} else if (h < hmin) {
					FLAG = false;
					throw new Exception ("minimum h exceeded");
				}
			}
		}
	}
}

