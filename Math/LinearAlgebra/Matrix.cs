using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Math.LinearAlgebra
{
	public class Matrix
	{
		public double[,] Values;

		public int Rows {
			get{ return Values.GetLength (0);}
		}

		public int Columns {
			get { return  Values.GetLength (1);}
		}

		public void SwapRows (int row1, int row2)
		{
			double temp;
			for (int i = 0; i < this.Columns; i++) {
				temp = Values [row1, i];
				Values [row1, i] = Values [row2, i];
				Values [row2, i] = temp;
			}
		}

		public Matrix (ColumnVector[] cVectors)
		{
			if (cVectors.Length > 0) {
				if (cVectors [0].Values.Length > 0) {
					this.Values = new double[cVectors [0].Values.Length, cVectors.Length];
					for (int i =0; i< cVectors.Length; i++) {
						for (int j = 0; j < cVectors[0].Values.Length; j++) {
							this.Values [i, j] = cVectors [j] [i];
						}
					}
				}
			}
		}

		public Matrix (RowVector[] rVectors)
		{
			if (rVectors.Length > 0) {
				if (rVectors [0].Values.Length > 0) {
					this.Values = new double[rVectors.Length, rVectors [0].Values.Length];
					for (int i =0; i < rVectors.Length; i++) {
						for (int j = 0; j < rVectors[0].Values.Length; j++) {
							this.Values [j, i] = rVectors [j] [i];
						}
					}
				}
			}
		}

		private Matrix ()
		{

		}

		public Matrix (int rows, int columns)
		{
			this.Values = new double[rows, columns];
		}

		public Matrix (double[,] Values)
		{
			this.Values = Values;
		}

		public Matrix (Matrix m)
		{
			this.Values = m.Values;
		}

		public static Matrix Multiply (Matrix lhs, Matrix rhs)
		{
			Matrix result;

			if (lhs.Values.GetLength (1) != rhs.Values.GetLength (0)) {
				//MathLog.MathLog ml = new MathLog.MathLog ();
				//ml.Write (string.Format ("lhs columns: {0} does not match rhs rows: {1}", lhs.Values.GetLength (1), rhs.Values.GetLength (0)),
				//          System.Reflection.MethodBase.GetCurrentMethod ().Name);
				//ml = null;
				return new Matrix ();
			}
			result = new Matrix (lhs.Values.GetLength (0), rhs.Values.GetLength (1));


			Parallel.For (0, lhs.Values.GetLength (0), delegate(int i) {
				for (int j = 0; j < result.Values.GetLength(1); j++) {
					for (int k = 0; k < rhs.Values.GetLength(0); k++)
						result [i, j] += lhs [i, k] * rhs [k, j];
				}
			});

			return result;
		}

		public static Matrix Add (Matrix lhs, Matrix rhs)
		{
			Matrix result;
			if (lhs.Values.GetLength (0) != rhs.Values.GetLength (0) || lhs.Values.GetLength (1) != rhs.Values.GetLength (1)) {
				//MathLog.MathLog ml = new MathLog.MathLog ();
				//ml.Write (string.Format ("lhs: {0}x{1} unequal to rhs: {2}x{3}", lhs.Values.GetLength (0), lhs.Values.GetLength (1),
				                         //rhs.Values.GetLength (0), rhs.Values.GetLength (1))
				          //, System.Reflection.MethodBase.GetCurrentMethod ().Name);
				//ml = null;
				return new Matrix (0, 0);
			}

			result = new Matrix (lhs.Values.GetLength (0), lhs.Values.GetLength (1));

			Parallel.For (0, lhs.Values.GetLength (0), delegate(int i) {
				for (int j = 0; j < lhs.Values.GetLength(1); j++) {
					result [i, j] = lhs [i, j] + rhs [i, j];
				}
			});
			return result;
		}

		public static Matrix Subtract (Matrix lhs, Matrix rhs)
		{
			Matrix result;
			if (lhs.Values.GetLength (0) != rhs.Values.GetLength (0) || lhs.Values.GetLength (1) != rhs.Values.GetLength (1)) {
				//MathLog.MathLog ml = new MathLog.MathLog ();
				//ml.Write (string.Format ("lhs: {0}x{1} unequal to rhs: {2}x{3}", lhs.Values.GetLength (0), lhs.Values.GetLength (1),
				//                         rhs.Values.GetLength (0), rhs.Values.GetLength (1))
				//          , System.Reflection.MethodBase.GetCurrentMethod ().Name);
				//ml = null;
				return new Matrix (0, 0);
			}

			result = new Matrix (lhs.Values.GetLength (0), lhs.Values.GetLength (1));

			Parallel.For (0, lhs.Values.GetLength (0), delegate(int i) {
				for (int j = 0; j < lhs.Values.GetLength(1); j++) {
					result [i, j] = lhs [i, j] - rhs [i, j];
				}
			});
			return result;
		}

		public static Matrix Negate (Matrix m)
		{
			Matrix result = new Matrix (m);
			Parallel.For (0, m.Values.GetLength (0), delegate(int index) {
				for (int j = 0; j < m.Values.GetLength(1); j++) {
					result [index, j] *= -1;
				}
			});
			return result;
		}

		public Matrix Transpose ()
		{
			Matrix result = new Matrix (this.Values.GetLength (1), this.Values.GetLength (0));
			Parallel.For (0, this.Values.GetLength (0), delegate(int index) {
				for (int j = 0; j < this.Values.GetLength(1); j++) {
					result [j, index] = this [index, j];
				}
			});
			return result;
		}

		public static Matrix operator + (Matrix lhs, Matrix rhs)
		{
			return Matrix.Add (lhs, rhs);
		}

		public static Matrix operator - (Matrix lhs, Matrix rhs)
		{
			return Matrix.Subtract (lhs, rhs);
		}

		public static Matrix operator * (Matrix lhs, Matrix rhs)
		{
			return Matrix.Multiply (lhs, rhs);
		}

		public double this [int i, int j] {
			get {
				return this.Values [i, j];
			}
			set {
				this.Values [i, j] = value;
			}
		}

		public double? Determinant ()
		{
			if (!this.IsSquare) {
				//MathLog.MathLog ml = new MathLog.MathLog ();
				//ml.Write ("Matrix not square", System.Reflection.MethodBase.GetCurrentMethod ().Name);
				return null;
			}
			if (this.Values.GetLength (0) == 1 && this.Values.GetLength (1) == 1) {
				return this.Values [0, 0];
			}
			if (this.Values.GetLength (0) == 2 && this.Values.GetLength (1) == 2) {
				return (this [0, 0] * this [1, 1]) - (this [0, 1] * this [1, 0]);
			}
			double sum = 0.0;
			for (int i = 0; i < Values.GetLength(0); i++) {
				sum += i % 2 == 0 ? 1 : -1 * Values [0, i] * createSubMatrix (0, i).Determinant ().Value;
			}
			return sum;
		}

		public Matrix createSubMatrix (int excludingRow, int excludingColumn)
		{
			Matrix result = new Matrix (this.Values.GetLength (0) - 1, this.Values.GetLength (1) - 1);
			int r = -1;
			for (int i = 0; i < this.Values.GetLength(0); i++) {
				if (i == excludingRow)
					continue;
				r++;
				int c = -1;
				for (int j = 0; j < this.Values.GetLength(1); j++) {
					if (j == excludingColumn)
						continue;
					result [r, ++c] = this [i, j];
				}
			}
			return result;
		}

		public Matrix Cofactor ()
		{
			Matrix result = new Matrix (this.Values.GetLength (0), this.Values.GetLength (1));
			for (int i = 0; i < this.Values.GetLength(0); i++) {
				for (int j = 0; j<this.Values.GetLength(1); j++) {
					result [i, j] = (i % 2 == 0 ? 1 : -1) * (j % 2 == 0 ? 1 : -1) * createSubMatrix (i, j).Determinant ().Value;
				}
			}
			return result;
		}

		public Matrix MultiplyByConstant (double constant)
		{
			Matrix result = new Matrix (this);
			Parallel.For (0, this.Values.GetLength (0), delegate(int index) {
				for (int j = 0; j < this.Values.GetLength(1); j++) {
					result [index, j] *= constant;
				}
			});
			return result;
		}

		public Matrix Inverse ()
		{

			double? determinant = this.Determinant ();
			if (determinant.HasValue) {
				if (determinant.Value != 0) {
					return this.Cofactor ().Transpose ().MultiplyByConstant (1.0 / determinant.Value);
				} else
					throw new DivideByZeroException ("determinant is equal to zero");
			} else
				throw new Exception ("Problem inverting matrix");
		}

		public bool IsSquare {
			get {
				return this.Values.GetLength (0) == this.Values.GetLength (1);
			}
		}

		public static ColumnVector operator * (Matrix lhs, ColumnVector rhs)
		{
			ColumnVector result;

			if (lhs.Values.GetLength (1) != rhs.Values.Length) {
				MathLog.MathLog ml = new MathLog.MathLog ();
				ml.Write (string.Format ("{0} != {1}", lhs.Values.GetLength (0), rhs.Values.Length),
				          System.Reflection.MethodBase.GetCurrentMethod ().Name);
				return null;
			}
			result = new ColumnVector (new double[lhs.Values.GetLength (0)]);
			for (int i = 0; i < lhs.Values.GetLength(0); i++) {
				double sum = 0;
				for (int j = 0; j < lhs.Values.GetLength(1); j++) {
					sum += lhs [i, j] * rhs [j];
				}
				result [i] = sum;
			}

			return result;
		}

		public static RowVector operator * (RowVector lhs, Matrix rhs)
		{
			RowVector result;
			if (lhs.Values.Length != rhs.Values.GetLength (0)) {
				MathLog.MathLog ml = new MathLog.MathLog ();
				ml.Write (string.Format ("{0} != {1}", lhs.Values.Length, rhs.Values.GetLength (0)), 
				          System.Reflection.MethodBase.GetCurrentMethod ().Name);
				return null;
			}
			result = new RowVector (new double[lhs.Values.GetLength (1)]);
			for (int i = 0; i < lhs.Values.Length; i++) {
				double sum = 0;
				for (int j = 0; j < rhs.Values.GetLength(1); j++) {
					sum += lhs [i] * rhs [i, j];
				}
				result [i] = sum;
			}
			return result;
		}

		static public Matrix Identity (int n)
		{
			Matrix result = new Matrix (n, n);
			for (int i = 0; i < n; i++) {
				result [i, i] = 1;
			}
			return result;
		}

		static public Matrix Identity (int n, double d)
		{
			Matrix result = new Matrix (n, n);
			for (int i =0; i <n; i++)
				result [i, i] = d;
			return result;
		}

		static public Matrix Diagonal (double[] diags)
		{
			Matrix result = new Matrix (diags.Length, diags.Length);
			for (int i = 0; i < diags.Length; i++) {
				result [i, i] = diags [i];
			}
			return result;
		}

		public ColumnVector GetColumnVector (int n)
		{
			if (n < this.Columns) {
				double[] c = new double[this.Rows];
				for (int i = 0; i < this.Rows; i++) {
					c [i] = this [i, n];
				}
				return new ColumnVector (c);
			} else
				throw new ArgumentOutOfRangeException ("n larger than number of columns");
		}

		public RowVector GetRowVector (int n)
		{
			if (n < this.Rows) {
				double[] c = new double[this.Columns];
				for (int i = 0; i < this.Columns; i++) {
					c [i] = this [n, i];
				}
				return new RowVector (c);
			} else
				throw new ArgumentOutOfRangeException ("n larger than number of rows");
		}

		public void InsertMinor (Matrix minor, int n, int m)
		{
			if (minor.Rows + n + 1 < this.Rows && minor.Columns + m + 1 < this.Columns) {
				for (int i = n; i - n < minor.Rows; i++) {
					for (int j = m; j - m < minor.Columns; j++) {
						this [i, j] = minor [i - n, j - m];
					}
				}
			}
		}

		public static bool operator < (Matrix lhs, double rhs)
		{
			for (int i = 0; i < lhs.Rows; i++) {
				for (int j = 0; j < lhs.Columns; j++) {
					if (lhs [i, j] >= rhs)
						return false;
				}
			}
			return true;
		}

		public static bool operator <= (Matrix lhs, double rhs)
		{
			for (int i = 0; i < lhs.Rows; i++) {
				for (int j = 0; j < lhs.Columns; j++) {
					if (lhs [i, j] > rhs)
						return false;
				}
			}
			return true;
		}

		public static bool operator < (double lhs, Matrix rhs)
		{
			for (int i = 0; i < rhs.Rows; i++) {
				for (int j = 0; j < rhs.Columns; j++) {
					if (lhs >= rhs [i, j])
						return false;
				}
			}
			return true;
		}

		public static bool operator <= (double lhs, Matrix rhs)
		{
			for (int i = 0; i < rhs.Rows; i++) {
				for (int j = 0; j < rhs.Columns; j++) {
					if (lhs > rhs [i, j])
						return false;
				}
			}
			return true;
		}

		public static bool operator > (double lhs, Matrix rhs)
		{
			return rhs < lhs;
		}

		public static bool operator >= (double lhs, Matrix rhs)
		{
			return rhs <= lhs;
		}

		public static bool operator > (Matrix lhs, double rhs)
		{
			return rhs < lhs;
		}

		public static bool operator >= (Matrix lhs, double rhs)
		{
			return rhs <= lhs;
		}
	}

	public static class MatrixExtensions
	{
		public static bool IsSymmetric (this Matrix matrix)
		{
			if (matrix == null)
				throw new ArgumentNullException ("matrix");

			if (matrix.Values.GetLength (0) == matrix.Values.GetLength (1)) {
				for (int i = 0; i < matrix.Values.GetLength(0); i++) {
					for (int j = 0; j <= i; j++) {
						if (matrix [i, j] != matrix [j, i])
							return false;
					}
				}
				return true;
			}
			return false;

		}

		public static Matrix Covariance (this Matrix matrix)
		{
			int n = matrix.Values.GetLength (1);
			Matrix result = new Matrix (n, n);

			double[] meanOfColumns = new double[n];
			for (int i = 0; i < n; i++) {
				meanOfColumns [i] = 0;
			}
			int numOfRows = matrix.Values.GetLength (0);
			for (int i = 0; i < numOfRows; i++) {
				for (int j =0; j < n; j++) {
					meanOfColumns [i] += matrix [i, j] / numOfRows;
				}
			}
			double firstTerm, secondTerm;
			for (int i = 0; i < n; i++) {
				for (int j = 0; j <= i; j++) {
					result [i, j] = 0;
					for (int k = 0; k < numOfRows; k++) {
						firstTerm = matrix [k, i] - meanOfColumns [i];
						secondTerm = matrix [k, j] - meanOfColumns [j];
						result [i, j] += (firstTerm * secondTerm) / (n - 1);
					}
					if (i != j) {
						result [j, i] = result [i, j];
					}
				}
			}
			return result;
		}

		public static Matrix Correlation (this Matrix matrix)
		{
			Matrix covarianceMatrix = matrix.Covariance ();
			Matrix diagsSigmaNegativeSquareRoot = new Matrix (covarianceMatrix.Values.GetLength (0), covarianceMatrix.Values.GetLength (0));

			for (int i = 0; i < covarianceMatrix.Rows; i++) {
				diagsSigmaNegativeSquareRoot [i, i] = (1 / System.Math.Sqrt (covarianceMatrix [i, i]));
			}

			return diagsSigmaNegativeSquareRoot * matrix * diagsSigmaNegativeSquareRoot;
		}

		public static QR QR (this Matrix A)
		{
			int m = A.Rows, n = A.Columns;
			Matrix Q, R = new Matrix (n, n);
			ColumnVector[] qs = new ColumnVector[n], xs = new ColumnVector[n];
			for (int i = 0; i < n; i++) {
				xs [i] = A.GetColumnVector (i);
			}

			for (int i = 0; i < n; i++) {
				R [i, i] = System.Math.Sqrt (ColumnVector.InnerProduct (A.GetColumnVector (i), A.GetColumnVector (i)));
				qs [i] = (1 / R [i, i]) * xs [i];
				for (int j = i + 1; j < n; j++) {
					R [i, j] = ColumnVector.InnerProduct (A.GetColumnVector (j), qs [i]);
					xs [j] = xs [j] - R [i, j] * qs [i];
				}
			}

			Q = new Matrix (qs);

			return new Math.LinearAlgebra.QR (Q, R);
		}

		public static Matrix Parition (this Matrix matrix, int startRow, int startColumn, int endRow, int endColumn)
		{
			if (startRow <= endRow && startColumn <= endColumn && endRow < matrix.Rows && endColumn < matrix.Columns) {
				Matrix results = new Matrix (endRow - startRow + 1, endColumn - startColumn + 1);
				for (int i = startRow; i <= endRow; i++) {
					for (int j = startColumn; j <= endColumn; j++) {
						results [i - startRow, j - startColumn] = matrix [i, j];
					}
				}
				return results;
			}
			throw new ArgumentOutOfRangeException ("Unable to partition matrix with given parameters");
		}

		public static Eigen Eigen (this Matrix matrix, double TOL = 0.0001)
		{
			if (matrix.IsSquare) {
				int n = matrix.Rows;
				Matrix A = new Matrix (matrix);


				Matrix X = A + A.Transpose (), oldX;

				Matrix pQ = Matrix.Identity (n);

				bool RUN = true;
				while (RUN) {
					QR d = X.QR ();
					pQ = pQ * d.Q;
					oldX = X;
					X = d.R * d.Q;
					if (MatrixMath.Abs (X - oldX) < TOL) {
						bool traceTest = false, determinantTest = false;
						if (System.Math.Abs ((X.Trace () - A.Trace ())) < TOL) {
							traceTest = true;
						}
						if(System.Math.Abs(X.Diagonals().Product() - A.Determinant().Value) < TOL)
						{
							determinantTest = true;
						}
						return new Math.LinearAlgebra.Eigen (pQ, X, new Matrix (n, n), traceTest, determinantTest, TOL);
					}
				}
				return null;

			} else {
				throw new ArgumentOutOfRangeException ("Not a Square matrix, cannot compute eigen values");
			}
		}

		public static Matrix Pow(this Matrix matrix, int n)
		{
			if (matrix.IsSquare) {
				if (n == 0) {
					return Matrix.Identity (matrix.Rows);
				} else if (n == 1) {
					return matrix;
				} else {
					Matrix product = Matrix.Identity (matrix.Rows);
					for (int i = 0; i < System.Math.Abs(n); i++) {
						product = product * matrix;
					}
					if (n < 0) {
						return product.Inverse ();
					} else
						return product;
				}
			} else {
				throw new NoSquareException (matrix);
			}
		}

		public static Matrix Minor (this Matrix matrix, int n, int m)
		{
			if (n < matrix.Rows && m < matrix.Columns) {
				Matrix result = new Matrix (matrix.Rows - n, matrix.Columns - m);
				for (int i = n; i < matrix.Rows; i++) {
					for (int j = m; j < matrix.Columns; j++) {
						result [i - n, j - m] = matrix [i, j];
					}
				}
				return result;
			} else {
				throw new ArgumentOutOfRangeException ("n or m is too large");
			}
		}

		public static Matrix Product (this List<Matrix> matricies)
		{
			Matrix result = Matrix.Identity (matricies [0].Rows);
			for (int i =0; i < matricies.Count; i++) {
				result = result * matricies [i];
			}
			return result;
		}

		public static Cholesky Cholesky (this Matrix A)
		{
			int n = A.Rows;
			Matrix L = new Matrix (n, n), U = new Matrix (n, n);

			L [0, 0] = System.Math.Sqrt (A [0, 0]);
			for (int i = 1; i < n; i++) {
				L [i, 0] = A [i, 0] / L [0, 0];
			}
			for (int i = 1; i < n-1; i++) {
				double sum = 0;
				for (int k = 0; k <= i - 1; k++) {
					sum += L [i, k] * L [i, k];
				}
				L [i, i] = System.Math.Sqrt (A [i, i] - sum);
				for (int j = i; j < n; j++) {
					sum = 0;
					for (int k = 0; k <= i - 1; k++) {
						sum += L [j, k] * L [i, k];
					}
					L [j, i] = (A [j, i] - sum) / L [i, i];
				}
			}
			double sum1 = 0;
			for (int k = 0; k < n -1; k++) {
				sum1 += L [n - 1, k] * L [n - 1, k];
			}
			L [n - 1, n - 1] = System.Math.Sqrt (A [n - 1, n - 1] - sum1);

			for (int i = 0; i < n; i ++) {
				for (int j = 0; j <= i; i++) {
					U [j, i] = L [i, j];
				}
			}

			return new Math.LinearAlgebra.Cholesky (U, L);
		}

		public static double Trace(this Matrix matrix)
		{
			if (matrix.IsSquare) {
				double result = 0.0;
				for (int i =0; i < matrix.Rows; i++) {
					result += matrix [i, i];
				}
				return result;
			} else
				throw new ArgumentOutOfRangeException ("Matrix not square cannot compute Trace.");
		}

		public static double[] Diagonals(this Matrix matrix)
		{
			if (matrix.IsSquare) {
				int n = matrix.Rows;
				double[] result = new double[n]; 
				for (int i = 0; i < n; i++) {
					result [i] = matrix [i, i];
				}
				return result;
			} else
				throw new ArgumentException ("Not a square matrix");
		}


	}
}

