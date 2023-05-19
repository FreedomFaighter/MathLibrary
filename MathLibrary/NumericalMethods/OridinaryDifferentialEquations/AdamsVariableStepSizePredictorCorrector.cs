using System;
using System.Collections.Generic;

namespace MathLibrary.NumericalMethods.OridinaryDifferentialEquations
{
	public class AdamsVariableStepSizePredictorCorrector
	{
		Func<decimal, decimal, decimal> f_of_t_y;
		decimal a, b, alpha, TOL, hmax, hmin;
		public AdamsVariableStepSizePredictorCorrector (Func<decimal, decimal, decimal> f, decimal a, decimal b, decimal alpha
		                                                ,decimal TOL, decimal hmax, decimal hmin)
		{
			this.f_of_t_y = f;
			this.a = a;
			this.b = b;
			this.alpha = alpha;
			this.TOL = TOL;
			this.hmax = hmax;
			this.hmin = hmin;
		}

		public IEnumerator<Tuple<decimal, decimal>> Solve()
		{
			List<Tuple<decimal,decimal>> tw = new List<Tuple<decimal, decimal>> (1);
			decimal h = hmax, T = this.a, W = this.alpha, t, WP, WC, Sigma;
			bool FLAG = true, LAST = false;
			tw [0] = new Tuple<decimal, decimal> (T, W);
			yield return tw [0];
			tw.AddRange ((List<Tuple<decimal, decimal>>)RK4 (h, tw [0].Item1, tw [0].Item2));
			bool NFLAG = true;
			int i = 4;
			t = tw [3].Item1 + h;
			while (FLAG) {
				WP = tw [i - 1].Item2 + h / 24.0M * (55.0M * this.f_of_t_y (tw [i - 1].Item1, tw [i - 1].Item2) 
					- 59.0M * this.f_of_t_y (tw [i - 2].Item1, tw [i - 2].Item2)
					+ 37.0M * this.f_of_t_y (tw [i - 3].Item1, tw [i - 3].Item2) 
					- 9.0M * this.f_of_t_y (tw [i - 4].Item1, tw [i - 4].Item2));
				WC = tw [i - 1].Item2 + h / 24.0M * (9.0M * this.f_of_t_y (t, WP) 
					+ 19.0M * this.f_of_t_y (tw [i - 1].Item1, tw [i - 1].Item2)
					- 5.0M * this.f_of_t_y (tw [i - 2].Item1, tw [i - 2].Item2) 
					+ this.f_of_t_y (tw [i - 3].Item1, tw [i - 3].Item2));
				Sigma = (19.0M * System.Math.Abs (WC - WP)) / (270.0M * h);
				if (Sigma <= TOL) {
					tw.Add (new Tuple<decimal, decimal> (t, WC));
					if (NFLAG == true) {
						for (int j = i - 3; j <= i; j++) {
							yield return tw [j];
						}
					} else {
						yield return tw [i];
					}
					if (LAST == true) {
						FLAG = false;
					} else {
						i++;
						NFLAG = false;
						if (Sigma <= 0.1M * this.TOL || tw [i - 1].Item1 + h > b) {
							Decimal q = (this.TOL / (2.0M * Sigma)).Pow (1.0M / 4.0M);
							if (q > 4.0M) {
								h = 4.0M * h;
							} else {
								h = q * h;
							}
						}
						if (h > hmax) {
							h = hmax;
						}
						if (tw [i - 1].Item1 + 4.0M * h > b) {
							h = (b - tw [i - 1].Item1) / 4.0M;
							LAST = true;
						}
						tw.AddRange ((List<Tuple<decimal, decimal>>)RK4 (h, tw [i - 1].Item1, tw [i - 1].Item2));
						NFLAG = true;
						i += 3;
					}
				} else {
					Decimal q = (this.TOL / (2.0M * Sigma)).Pow (0.25M);
					if (q < 0.1M) {
						h *= 0.1M;
					} else {
						h *= q;
					}
					if (h < hmin) {
						FLAG = false;
						throw new Exception ("hmin exceeded");
					} else {
						if (NFLAG == true) {
							i -= 3;
							List<Tuple<decimal, decimal>> temp = (List<Tuple<decimal, decimal>>)RK4 (h, tw [i - 1].Item1, tw [i - 1].Item2);
							for (int j = i; j <= i+2; j++) {
								tw [j] = temp [j - i];
							}
							i += 3;
							NFLAG = true;
						}
					}
				}
				t = tw[i - 1].Item1 + h;
			}
		}

		private IEnumerator<Tuple<decimal, decimal>> RK4(decimal h, decimal x0, decimal v0)
		{
			int j = 1;
			decimal K1, K2, K3, K4;
			decimal[] v = new decimal[4], x = new decimal[4];
			v [0] = v0;
			x [0] = x0;
			while (j<=3) {
				K1 = h * this.f_of_t_y (x [j - 1], v [j - 1]);
				K2 = h * this.f_of_t_y (x [j - 1] + h / 2.0M, v [j - 1] + K1 / 2.0M);
				K3 = h * this.f_of_t_y (x [j - 1] + h / 2.0M, v [j - 1] + K2 / 2.0M);
				K4 = h * this.f_of_t_y (x [j - 1] + h, v [j - 1] + K3);
				v [j] = v [j - 1] + (K1 + 2.0M * K2 + 2.0M * K3 + K4) / 6.0M;
				x [j] = x [0] + j * h;
				yield return new Tuple<decimal, decimal> (x[j], v[j]);
			}
		}
	}
}

