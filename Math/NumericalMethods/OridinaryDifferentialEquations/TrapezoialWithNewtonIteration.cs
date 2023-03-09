using System;
using Math.NumericalMethods;
using System.Collections.Generic;
using MathLog;
namespace Math.NumericalMethods.OridinaryDifferentialEquations
{
	public class TrapezoialWithNewtonIteration : INumericalMethod<decimal>
	{
		Func<decimal, decimal, decimal> f_of_t_y;
		decimal a, b, alpha, TOL;
		int N, M;
		public TrapezoialWithNewtonIteration (Func<decimal, decimal, decimal> f, decimal a
		                                      , decimal b, decimal alpha
		                                      , decimal TOL, int N
		                                      ,int M)
		{
			this.f_of_t_y = f;
			this.a = a;
			this.b = b;
			this.alpha = alpha;
			this.TOL = TOL;
			this.N = N;
			this.M = M;
		}


		#region INumericalMethod implementation
		public IEnumerable<Tuple<decimal, decimal>> Solve ()
		{
			decimal h = (b - a) / N, t = a, w = alpha, k1, w_0;
			int j;
			bool FLAG = false;
			yield return new Tuple<decimal, decimal> (t, w);

			for (int i = 1; i <= N; i++) {
				k1 = w + (h / 2.0M) * this.f_of_t_y (t, w);
				w_0 = k1;
				j = 1;
				while (!FLAG) {
					w = w_0 - (w_0 - (h / 2.0M) * this.f_of_t_y (t + h, w_0) - k1) 
						/ (1 - (this.f_of_t_y (t + h, w_0 + h) - this.f_of_t_y (t + h, w_0)) / 2.0M);
					if (System.Math.Abs (w - w_0) < this.TOL) {
						FLAG = true;
					} else {
						j++;
						w_0 = w;
						if (j > M) {
							MathLog.MathLog ml = new MathLog.MathLog ();
							ml.Write ("The maximum number of iterations exceeded"
							          , System.Reflection.MethodInfo.GetCurrentMethod ().Name);
							yield break;
						}
					}
				}
				t = a + i * h;
				yield return new Tuple<decimal, decimal> (t, w);
			}
		}
		#endregion
	}
}

