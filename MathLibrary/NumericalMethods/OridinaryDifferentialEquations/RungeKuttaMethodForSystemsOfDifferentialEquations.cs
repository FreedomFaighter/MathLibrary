using System;
using System.Collections.Generic;
using System.Linq;

namespace MathLibrary.NumericalMethods.OridinaryDifferentialEquations
{
	public class RungeKuttaMethodForSystemsOfDifferentialEquations
	{
		List<Func<decimal, decimal[], decimal>> u_i;
		decimal a;
		decimal b;
		decimal[] alpha_j;
		int N;

		public RungeKuttaMethodForSystemsOfDifferentialEquations (List<Func<decimal, decimal[], decimal>> u_i, decimal a, decimal b
		                                                          , decimal[] alpha, int N)
		{
			this.u_i = u_i;
			this.a = a;
			this.b = b;
			this.alpha_j = alpha;
			this.N = N;
		}

		public IEnumerable<Tuple<decimal, decimal[]>> Solve()
		{
			decimal h = (b - a) / N;
			decimal t = a;
			decimal[] w = alpha_j;
			decimal[,] k = new decimal[4, this.u_i.Count];
			yield return new Tuple<decimal, decimal[]> (t, w);
			for (int i = 1; i <=N; i++) {
				for (int j = 0; j<this.u_i.Count; j++) {
					k [0, j] = h * this.u_i [j] (t, w);
				}
				for (int j = 0; j<this.u_i.Count; j++) {
					decimal[] wTemp = w.Select ((x,y) => {
						return x + k [0, y] / 2.0M; }).ToArray();
					k [1, j] = h * this.u_i [j] (t + h / 2.0M, wTemp);
				}
				for (int j = 0; j<this.u_i.Count; j++) {
					decimal[] wTemp = w.Select ((x,y) => {
						return x + k [1, y] / 2.0M;}).ToArray();
					k [2, j] = h * this.u_i [j] (t, wTemp);
				}
				for (int j = 0; j<this.u_i.Count; j++) {
					decimal[] wTemp = w.Select ((x,y) => {
						return x + k [2, y]; }).ToArray();
					k [3, j] = h * this.u_i [j] (t, wTemp);
				}
				w = w.Select ((x,y) => {
					return x + (k [0, y] + 2.0M * k [1, y] + 2.0M * k [2, y] + k [3, y]) / 6.0M; }).ToArray();
				t = a + i * h;
				yield return new Tuple<decimal, decimal[]> (t, w);
			}
		}
	}
}

