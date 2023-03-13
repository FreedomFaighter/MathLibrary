using System;
using System.Collections.Generic;

namespace Math.NumericalMethods.OridinaryDifferentialEquations
{
	public class Extrapolation : INumericalMethod<decimal>
	{
		Func<decimal, decimal, decimal> f_of_t_y;
		decimal a, b, alpha, TOL, hmax, hmin;

		public Extrapolation (Func<decimal, decimal, decimal> f, decimal a, decimal b, decimal alpha, decimal TOL, decimal hmax, decimal hmin)
		{
			this.f_of_t_y = f;
			this.a = a;
			this.b = b;
			this.alpha = alpha;
			this.TOL = TOL;
			this.hmax = hmax;
			this.hmin = hmin;
		}


		public IEnumerable<Tuple<decimal, decimal>> Solve()
		{
			decimal[] NK = new decimal[] { 2, 4, 6, 8, 12, 16, 24, 32 }, y = new decimal[8];
			decimal T0 = a, T;
			decimal W0 = alpha, W1, W2, W3, hk, h = hmax;
			bool FLAG = true, NFLAG;
			int k;
			decimal[][] Q = new decimal[7][];
			for (int i = 1; i <= 7; i++) {
				Q [i - 1] = new decimal[i];
			}
			for (int i = 0; i < 7; i++) {
				for (int j = 0; j <= i; j++) {
					Q [i] [j] = ((NK [i + 1] * NK [i + 1]) / (NK [j] * NK [j]));
				}
			}
			while (FLAG) {
				k = 1;
				NFLAG = false;
				while (k<=8 && NFLAG == false) {
					hk = hmax / NK [k - 1];
					T = T0;
					W2 = W0;
					W3 = W2 + hk * this.f_of_t_y (T, W2);
					T = T0 + hk;
					for (int j = 1; j <= NK[k-1]-1; j++) {
						W1 = W2;
						W2 = W3;
						W3 = W1 + 2.0M * hk * this.f_of_t_y (T, W2);
						T = T0 + (j + 1) * hk;
					}
					y [k-1] = (W3 + W2 + hk * this.f_of_t_y (T, W3)) / 2.0M;
					if (k >= 2) {
						int j = k;
						decimal v = y [0];
						while (j>=2) {
							y [j - 2] = y [j - 1] + ((y [j - 1] - y [j - 2]) / (Q [k - 2] [j - 2] - 1));
							j--;
						}
						if (System.Math.Abs (y [0] - v) <= TOL) {
							NFLAG = true;
						}
					}
					k++;
				}
				k--;
				if (NFLAG == false) {
					h /= 2.0M;
					if (h < hmin) {
						MathLog.MathLog log = new MathLog.MathLog ();
						log.Write ("hmin exceeded", System.Reflection.MethodBase.GetCurrentMethod ().Name);
					}
				} else {
					W0 = y [0];
					T0 += h;
					yield return new Tuple<decimal, decimal> (T0, W0);
					if (T0 >= b) {
						FLAG = false;
					} else if (T0 + h > b) {
						h = b - T0;
					} else if (k <= 3 && h < 0.5M * hmax) {
						h *= 2.0M;
					}
				}
			}
		}
	}
}

