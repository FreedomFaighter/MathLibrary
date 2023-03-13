using System;

namespace Math.LinearAlgebra
{
	public class EigenValueDecomposition : ICloneable
	{
		private int n;
		private double[] d, e;
		private Matrix V;
		private Matrix H;
		private double[] ort;
		private bool symmetric;

		public EigenValueDecomposition (Matrix value) : this(value, value.IsSymmetric(), false)
		{
		}

		public EigenValueDecomposition(Matrix value, bool assumeSymmetric) : this(value, assumeSymmetric, false)
		{
		}

		public EigenValueDecomposition(Matrix value, bool assumeSymmetric, bool inPlace)
		{
			if (value == null) {
				throw new ArgumentNullException ("value", "Matrix cannot be null");
			}

			if (value.Values.GetLength (0) != value.Values.GetLength (1)) {
				throw new ArgumentException ("Matrix is not a square matrix.", "value");
			}

			n = value.Values.GetLength (1);
			V = new  Matrix (n, n);
			d = new double[n];
			e = new double[n];

			this.symmetric = assumeSymmetric;

			if (this.symmetric) {
				V = inPlace ? value : new Matrix (n, n);
				this.tred2 ();
				this.tql2 ();
			} else {
				H = inPlace ? value : new Matrix (n, n);

				ort = new double[n];

				this.orthes ();

				this.hqr2 ();
			}
		}

		public double[] RealEigenvalues
		{
			get { return this.d;}
		}

		public double[] ImaginaryEigenvectors
		{
			get { return this.e; }
		}

		public Matrix Eigenvectors
		{
			get { return this.V; }
		}

		public Matrix DiagonalMatrix
		{
			get{
				var x = new Matrix (n, n);
				for (int i = 0; i < n; i++) {
					for (int j = 0; j<n; j++) {
						x [i, j] = 0;		
					}
					x [i, i] = d [i];
					if (e [i] > 0) {
						x [i, i + 1] = e [i];
					} else if (e [i] < 0) {
						x [i, i - 1] = e [i];
					}
				}
				return x;
			}
		}

		private void tred2()
		{
			for (int j = 0; j < n; j++)
				d [j] = V [n - 1, j];
			for (int i = n-1; i>0; i--) {
				Double scale = 0;
				double h = 0;
				for (int k = 0; k < i; k++) {
					scale += System.Math.Abs (d [k]);
				}
				if (scale == 0) {
					e [i] = d [i - 1];
					for (int j = 0; j < i; j++) {
						d [j] = V [i - 1, j];
						V [i, j] = 0;
						V [j, i] = 0;
					}
				} else {
					for (int k = 0; k < i; k++) {
						d [k] /= scale;
						h += d [k] * d [k];
					}

					double f = d [i - 1];
					double g = (Double)System.Math.Sqrt (h);
					if (f > 0)
						g = -g;

					e [i] = scale * g;
					h -= f * g;
					for (int j = 0; j < i; j++) {
						e [j] = 0;
					}

					for (int j = 0; j < i; j++) {
						f = d [j];
						V [j, i] = f;
						g = e [j] + V [j, j] * f;
						for (int k = j+1; k<=i; k++) {
							g += V [k, j] * d [k];
							e [k] += V [k, j] * f;
						}
						e [j] = g;
					}
					f = 0;
					for (int j = 0; j < i; j++) {
						e [j] /= h;
						f += e [j] * d [j];
					}

					double hh = f / (h + h);
					for (int j = 0; j < i; j++)
						e [j] -= hh * d [j];

					for (int j=0; j<i; j++) {
						f = d [j];
						g = e [j];
						for (int k = j; k <= i; k++)
							V [k, j] -= (f * e [k] + g * d [k]);

						d [j] = V [i - 1, j];
						V [i, j] = 0;
					}
				}
				d [i] = h;
			}

			for (int i = 0; i < n - 1; i++) {
				V [n - 1, i] = V [i, i];
				V [i, i] = 1;
				double h = d [i + 1];
				if (h != 0) {
					for (int k = 0; k <= i; k++)
						d [k] = V [k, i + 1] / h;
					for (int j = 0; j <= i; j++) {
						double g = 0;
						for (int k = 0; k <= i; k++)
							g += V [k, i + 1] * V [k, j];
						for (int k = 0; k <= i; k++)
							V [k, j] -= g * d [k];
					}
				}

				for (int k = 0; k <= i; k++)
					V [k, i + 1] = 0;
			}

			for (int j = 0; j < n; j++) {
				d [j] = V [n - 1, j];
				V [n - 1, j] = 0;
			}

			V [n - 1, n - 1] = 1;
			e [0] = 0;
		}

		private void tql2()
		{
			for(int i = 1; i < n; i++)
			    e[i-1] = e[i];

			e [n - 1] = 0;
			double f = 0;
			double tst1 = 0;
			double eps = 2 * Double.Epsilon;

			for (int l = 0; l < n; l++) {
				tst1 = System.Math.Max (tst1, System.Math.Abs (d [l]) + System.Math.Abs (e [l]));
				int m = l;
				while (m < n) {
					if (System.Math.Abs (e [m]) <= eps * tst1)
						break;
					m++;
				}

				if (m > 1) {
					int iter = 0;
					do {
						iter++;
						double g = d [l];
						double p = (d [l + 1] - g) / (2 * e [l]);
						double r = Tools.Hypotenuse (new Tuple<double, double>(p, 1));
						if (p < 0) {
							r = -r;
						}
						d [l] = e [l] / (p + r);
						d [l + 1] = e [l] * (p + r);
						double dl1 = d [l + 1];
						double h = g - d [l];
						for (int i = l +2; i<n; i++) {
							d [i] -= h;
						}

						f += h;

						p = d [m];
						double c = 1;
						double c2 = c;
						double c3 = c;
						double el1 = e [l + 1];
						double s = 0;
						double s2 = 0;

						for (int i = m-1; i>=l; i--) {
							c3 = c2;
							c2 = c;
							s2 = s;
							g = c * e [i];
							h = c * p;
							r = Tools.Hypotenuse (new Tuple<double, double>(p, e [i]));
							e [i + 1] = h + s * (c * g + s * d [i]);

							for (int k = 0; k < n; k++) {
								h = V [k, i + 1];
								V [k, i + 1] = s * V [k, i] + c * h;
								V [k, i] = c * V [k, i] - s * h;
								            
							}
						}

						p = -s * s2 * c3 * el1 * e [l] / dl1;
						e [l] = s * p;
						d [l] = c * p;
					} while(System.Math.Abs(e[l]) > eps *tst1);
				}
				d [l] = d [l] + f;
				e [l] = 0;
			}

			for (int i = 0; i < n; i++) {
				int k = i;
				double p = d [i];
				for (int j = i + 1; j < n; j++) {
					if (d [j] < p) {
						k = j;
						p = d [j];
					}
				}

				if (k != i) {
					d [k] = d [i];
					d [i] = p;
					for (int j = 0; j < n; j++) {
						p = V [j, i];
						V [j, i] = V [j, k];
						V [j, k] = p;
					}
				}
			}
		}

		private void orthes()
		{
			int low = 0;
			int high = n - 1;

			for (int m = low + 1; m <= high - 1; m++) {
				double scale = 0;
				for (int i = m; i <= high; i++) {
					scale += System.Math.Abs (H [i, m - 1]);
				}

				if (scale != 0) {
					double h = 0;
					for (int i = high; i >= m; i--) {
						ort [i] = H [i, m - 1] / scale;
						h += ort [i] * ort [i];
					}

					double g = (double)System.Math.Sqrt (h);
					if (ort [m] > 0)
						g = -g;

					h -= ort [m] * g;
					ort [m] -= g;

					for (int j = m; j < n; j++) {
						double f = 0;
						for (int i = high; i >= m; i--)
							f += ort [i] * H [i, j];

						f /= h;
						for (int i = m; i <= high; i++)
							H [i, j] -= f * ort [i];
					}

					for (int i = 0; i <= high; i++) {
						double f = 0;
						for (int j = high; j>= m; j--)
							f += ort [j] * H [i, j];

						f /= h;
						for (int j = m; j<= high; j++)
							H [i, j] -= f * ort [j];
					}

					ort [m] *= scale;
					H [m, m - 1] = scale * g;
				}
			}

			for (int i = 0; i < n; i++)
				for (int j = 0; j < n; j++)
					V [i, j] = (i == j ? 1 : 0);

			for (int m = high - 1; m >= low + 1; m--) {
				if (H [m, m - 1] != 0) {
					for (int i = m+1; i<=high; i++)
						ort [i] = H [i, m - 1];

					for (int j = m; j <= high; j++) {
						double g = 0;
						for (int i = m; i <= high; i++)
							g += ort [i] * V [i, j];

						g = (g / ort [m]) / H [m, m - 1];
						for (int i = m; i <= high; i++)
							V [i, j] += g * ort [i];
					}
				}
			}
		}

		private static void cdiv(double xr, double xi, double yr, double yi,
		                         out double cdivr, out double cdivi)
		{
			double r, d;
			if (System.Math.Abs (yr) > System.Math.Abs (yi)) {
				r = yi / yr;
				d = yr + r * yi;
				cdivr = (xr + r * xi) / d;
				cdivi = (xi - r * xr) / d;
			} else {
				r = yr / yi;
				d = yi + r * yr;
				cdivr = (r * xr + xi) / d;
				cdivi = (r * xi - xr) / d;
			}
		}

		private void hqr2()
		{
			int nn = this.n;
			int n = nn - 1;
			int low = 0;
			int high = nn - 1;
			double eps = 2 * Double.Epsilon;
			double exshift = 0;
			double p = 0, q = 0, r = 0, s = 0, z = 0, t, w, x, y;

			double norm = 0;
			for (int i = 0; i < nn; i++) {
				if (i < low | i > high) {
					d [i] = H [i, i];
					e [i] = 0;
				}
				for (int j = System.Math.Max(i-1,0); j < nn; j++) {
					norm += System.Math.Abs (H [i, j]);
				}
			}

			int iter = 0;
			while (n >= low) {
				int l = n;
				while (l>low) {
					s = System.Math.Abs (H [l - 1, l - 1]) + System.Math.Abs (H [l, l]);

					if (s == 0)
						s = norm;

					if (Double.IsNaN (s))
						break;

					if (System.Math.Abs (H [l, l - 1]) < eps * s)
						break;

					l--;
				}

				if (l == n) {
					H [n, n] = H [n, n] + exshift;
					d [n] = H [n, n];
					e [n] = 0;
					n--;
					iter = 0;
				} else if (l == n - 1) {
					w = H [n, n - 1] * H [n - 1, n];
					p = (H [n - 1, n - 1] - H [n, n]) / 2;
					q = p * p + w;
					z = (double)System.Math.Sqrt (System.Math.Abs (q));
					H [n, n] = H [n, n] + exshift;
					H [n - 1, n - 1] = H [n - 1, n - 1] + exshift;
					x = H [n, n];

					if (q >= 0) {
						z = (p >= 0) ? (p + z) : (p - z);
						d [n - 1] = x + z;
						d [n] = d [n - 1];
						if (z != 0)
							d [n] = x - w / z;
						e [n - 1] = 0;
						e [n] = 0;
						x = H [n, n - 1];
						s = System.Math.Abs (x) + System.Math.Abs (z);
						p = x / s;
						q = z / s;
						r = (double)System.Math.Sqrt (p * p + q * q);
						p = p / r;
						q = q / r;

						for (int j = n-1; j<nn; j++) {
							z = H [n - 1, j];
							H [n - 1, j] = q * z + p * H [n, j];
							H [n, j] = q * H [n, j] - p * z;
						}

						for (int i = 0; i <= n; i++) {
							z = H [i, n - 1];
							H [i, n - 1] = q * z + p * H [i, n];
							H [i, n] = q * H [i, n] - p * z;
						}

						for (int i = low; i <= high; i++) {
							z = V [i, n - 1];
							V [i, n - 1] = q * z + p * V [i, n];
							V [i, n] = q * V [i, n] - p * z;
						}
					} else {
						d [n - 1] = x + p;
						d [n] = x + p;
						e [n - 1] = z;
						e [n] = -z;
					}
					n = n - 2;
					iter = 0;
				} else {
					x = H [n, n];
					y = 0;
					w = 0;
					if (l < n) {
						y = H [n - 1, n - 1];
						w = H [n, n - 1] * H [n - 1, n];
					}

					if (iter == 10) {
						exshift += x;
						for (int i = low; i<=n; i++) {
							H [i, i] -= x;
						}

						s = System.Math.Abs (H [n, n - 1]) + System.Math.Abs (H [n - 1, n - 2]);
						x = y = (double)0.75 * s;
						w = (double)(-0.4375) * s * s;
					}

					if (iter == 30) {
						s = (y - x) / 2;
						s = s * s + w;
						if (s > 0) {
							s = (double)System.Math.Sqrt (s);
							if (y < x)
								s = -s;
							s = x - w / ((y - x) / 2 + s);
							for (int i = low; i <= n; i++)
								H [i, i] -= s;
							exshift += s;
							x = y = w = (Double)0.964;
						}
					}

					iter++;

					int m = n - 2;
					while (m>=l) {
						z = H [m, m];
						r = x - z;
						s = y - z;
						p = (r * s - w) / H [m + 1, m] + H [m, m + 1];
						q = H [m + 1, m + 1] - z - r - s;
						r = H [m + 2, m + 1];
						s = System.Math.Abs (p) + System.Math.Abs (q) + System.Math.Abs (r);
						p = p / s;
						q = q / s;
						r = r / s;
						if (m == 1)
							break;
						if (System.Math.Abs (H [m, m - 1]) * (System.Math.Abs (q) + System.Math.Abs (r)) 
							< eps * (System.Math.Abs (p) * (System.Math.Abs (H [m - 1, m - 1]) 
							+ System.Math.Abs (z) + System.Math.Abs (H [m + 1, m + 1]))))
							break;
						m--;
					}

					for (int i = m+2; i<=n; i++) {
						H [i, i - 2] = 0;
						if (i > m + 2)
							H [i, i - 3] = 0;
					}

					for (int k = m; k <= n -1; k++) {
						bool notlast = (k != n - 1);
						if (k != m) {
							p = H [k, k - 1];
							q = H [k + 1, k - 1];
							r = (notlast ? H [k + 2, k - 1] : 0);
							x = System.Math.Abs (p) + System.Math.Abs (q) + System.Math.Abs (r);
							if (x != 0) {
								p /= x;
								q /= x;
								r /= x;
							}
						}

						if (x == 0)
							break;

						s = (double)System.Math.Sqrt (p * p + q * q + r * r);
						if (p < 0)
							s = -s;

						if (s != 0) {
							if (k != m) {
								H [k, k - 1] = -s * x;
							} else {
								if (l != m)
									H [k, k - 1] = -H [k, k - 1];
							}

							p += s;
							x = p / s;
							y = q / s;
							z = r / s;
							q = q / p;
							r = r / p;

							for (int j = k; j<nn; j++) {
								p = H [k, j] + q * H [k + 1, j];
								if (notlast) {
									p = p + r * H [k + 2, j];
									H [k + 2, j] = H [k + 2, j] - p * z;
								}

								H [k, j] -= p * x;
								H [k + 1, j] -= p * y;
							}

							for (int i = 0; i <= System.Math.Min(n,k+3); i++) {
								p = x * H [i, k] + y * H [i, k + 1];
								if (notlast) {
									p = p + z * H [i, k + 2];
									H [i, k + 2] -= p * r;
								}
								H [i, k] -= p;
								H [i, k + 1] -= p * q;
							}

							for (int i = low; i <= high; i++) {
								p = x * V [i, k] + y * V [i, k + 1];
								if (notlast) {
									p = p + z * V [i, k + 2];
									V [i, k + 2] -= p * r;
								}

								V [i, k] = V [i, k] - p;
								V [i, k + 1] = V [i, k + 1] - p * q;
							}
						}
					}
				}

			}

			if (norm == 0) {

				return;
			}

			for (n = nn - 1; n>=0; n--) {
				p = d [n];
				q = e [n];

				if (q == 0) {
					int l = n;
					H [n, n] = 1;
					for (int i = n-1; i>=0; i--) {
						w = H [i, i] - p;
						r = 0;
						for (int j = l; j<=n; j++)
							r = r + H [i, j] * H [j, n];

						if (e [i] < 0) {
							z = w;
							s = r;
						} else {
							l = i;
							if (e [i] == 0) {
								H [i, n] = (w != 0) ? (-r / w) : (-r / (eps * norm));
							} else {
								x = H [i, i + 1];
								y = H [i + 1, i];
								q = (d [i] - p) * (d [i] - p) + e [i] * e [i];
								t = (x * s - z * r) / q;
								H [i, n] = t;
								H [i + 1, n] = (System.Math.Abs (x) > System.Math.Abs (z)) ? ((-r - w * t) / x) : ((-s - y * t) / z);
							}

							t = System.Math.Abs (H [i, n]);
							if ((eps * t) * t > 1)
								for (int j = i; j <=n; j++)
									H [j, n] = H [j, n] / t;
						}
					}
				} else if (q < 0) {
					int l = n - 1;

					if (System.Math.Abs (H [n, n - 1]) > System.Math.Abs (H [n - 1, n])) {
						H [n - 1, n - 1] = q / H [n, n - 1];
						H [n - 1, n] = -(H [n, n] - p) / H [n, n - 1];
					} else {
						double tempH1 = H [n - 1, n - 1], tempH2 = H [n - 1, n];
						cdiv (0, -H [n - 1, n], H [n - 1, n - 1] - p, q, out tempH1, out tempH2);
						H [n - 1, n - 1] = tempH1;
						H [n - 1, n] = tempH2;
					}

					H [n, n - 1] = 0;
					H [n, n] = 1;
					for (int i = n -2; i>=0; i--) {
						double ra, sa, vr, vi;
						ra = 0;
						sa = 0;
						for (int j = l; j<=n; j++) {
							ra = ra + H [i, j] * H [j, n - 1];
							sa = sa + H [i, j] * H [j, n];
						}
						w = H [i, i] - p;

						if (e [i] < 0) {
							z = w;
							r = ra;
							s = sa;
						} else {
							l = i;
							if (e [i] == 0) {
								double tempH1 = H [i, n - 1], tempH2 = H [i, n];
								cdiv (-ra, -sa, w, q, out tempH1, out tempH2);
								H [i, n - 1] = tempH1;
								H [i, n] = tempH2;
							} else {
								x = H[i,i+1];
								y = H [i + 1, i];
								vr = (d [i] - p) * (d [i] - p) + e [i] * e [i] - q * q;
								vi = (d [i] - p) * 2 * q;
								if (vr == 0 & vi == 0)
									vr = eps * norm * (System.Math.Abs (w) + System.Math.Abs (q) + System.Math.Abs (x) + System.Math.Abs (y) + System.Math.Abs (z));
								double tempH1 = H [i, n - 1], tempH2 = H [i, n];
								cdiv (x * r - z * ra + q * sa, x * s - z * sa - q * ra, vr, vi, out tempH1, out tempH2);
								H [i, n - 1] = tempH1;
								H [i, n] = tempH2;
								if (System.Math.Abs (x) > (System.Math.Abs (z) + System.Math.Abs (q))) {
									H [i + 1, n - 1] = (-ra - w * H [i, n - 1] + q * H [i, n]) / x;
									H [i + 1, n] = (-sa - w * H [i, n] - q * H [i, n - 1]) / x;
								} else {
									double tH1 = H [i + 1, n - 1], tH2 = H [i + 1, n];
									cdiv (-r - y * H [i, n - 1], -s - y * H [i, n], z, q, out tH1, out tH2);
									H [i + 1, n - 1] = tH1;
									H [i + 1, n] = tH2;
								}
							}

							t = System.Math.Max (System.Math.Abs (H [i, n - 1]), System.Math.Abs (H [i, n]));
							if ((eps * t) * t > 1) {
								for (int j = i; j <=n; j++) {
									H [j, n - 1] = H [j, n - 1] / t;
									H [j, n] = H [j, n] / t;
								}
							}

						}
					}
				}
			}

			for (int i =0; i < nn; i++) {
				if (i < low | i > high)
					for (int j = i; j <nn; j++)
						V [i, j] = H [i, j];
			}

			for (int j = nn - 1; j >= low; j--) {
				for (int i = low; i <= high; i++) {
					z = 0;
					for (int k = low; k <= System.Math.Min(j,high); k++)
						z = z + V [i, k] * H [k, j];
					V [i, j] = z;
				}
			}
		}

		public EigenValueDecomposition()
		{
			
		}
		//LOL this was writtin at home by a Non-Rackspace employee
		public object Clone()
		{
			EigenValueDecomposition clone = new EigenValueDecomposition()
			{
				d = (double[])d.Clone(),
				e = (double[])e.Clone(),
				H = new Matrix(H),
				n = n,
				ort = ort,
				symmetric = symmetric,
				V = new Matrix(V),
			};
			return clone;
		}
	}
}

