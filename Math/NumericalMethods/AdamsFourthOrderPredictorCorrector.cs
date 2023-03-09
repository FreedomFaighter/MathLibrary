using System;
using System.Collections.Generic;

namespace Math.NumericalMethods
{
	public class AdamsFourthOrderPredictorCorrector : INumericalMethod<decimal>
	{
		Func<decimal, decimal, decimal> f_of_t_y;
		decimal a, b, alpha;
		int N;
		public AdamsFourthOrderPredictorCorrector (Func<decimal, decimal, decimal> f, decimal a, decimal b, decimal alpha, int N)
		{
			this.f_of_t_y=f;
			this.a = a;
			this.b = b;
			this.alpha = alpha;
			this.N = N;
		}

		public IEnumerable<Tuple<decimal, decimal>> Solve(){
			decimal h = (b-a)/N, K1, K2, K3, K4, T, W;
			decimal[] w = new decimal[4], t = new decimal[4];
			t [0] = a;
			w [0] = alpha;
			int i = 1;
			while (i<=3) {
				K1 = h * f_of_t_y (t [i - 1], w [i - 1]);
				K2 = h * f_of_t_y (t [i - 1] + h / 2.0M, w [i - 1] + K1 / 2.0M);
				K3 = h * f_of_t_y (t [i - 1] + h / 2.0M, w [i - 1] + K2 / 2.0M);
				K4 = h * f_of_t_y (t [i - 1] + h, w [i - 1] + K3);
				w [i] = w [i - 1] + (K1 + 2.0M * K2 + 2.0M * K3 + K4) / 6;
				t [i] = a + i * h;
				yield return new Tuple<decimal, decimal>(t[i], w[i]);
				i++;
			}
			while (i<=N) {
				T = a + i * h;
				W = w [3] + h * (55.0M * f_of_t_y (t [3], w [3]) - 59.0M * f_of_t_y (t [2], w [2]) 
					+ 37.0M * f_of_t_y (t [1], w [1]) - 9.0M * f_of_t_y (t [0], w [0])) 
					/ 24.0M;
				W = w [3] + h * (9.0M * f_of_t_y (T, W) + 19.0M * f_of_t_y (t [3], w [3]) 
					- 5.0M * f_of_t_y (t [2], w [2]) + f_of_t_y (t [1], w [1])) 
					/ 24.0m;
				yield return new Tuple<decimal, decimal>(T,W);
				for (int j = 0; j<=2; j++) {
					t [j] = t [j + 1];
					w [j] = w [j + 1];
				}
				t [3] = T;
				w [3] = W;
			}
		}
	}
}

