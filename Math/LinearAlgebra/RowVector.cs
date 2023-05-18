using System;
using System.Linq;

namespace Math.LinearAlgebra
{
	public class RowVector : IVector
	{
		private RowVector ()
		{

		}

		public RowVector(double[] values)
		{
			this.Values = values;
		}

		#region IVector implementation

		public ColumnVector Transpose ()
		{
			return new ColumnVector (this.Values);
		}

		public double this[int i]
		{
			get{
				return this.Values [i];
			}
			set {
				this.Values [i] = value;
			}
		}

		public double[] Values {
			get;
			set;
		}

		public RowVector Add (RowVector rhs)
		{
			RowVector result;
			if (this.Values.Length != rhs.Values.Length) {
				return new RowVector ();
			}
			result = new RowVector (this.Values);
			for (int i =0; i < this.Values.Length; i++) {
				result [i] += rhs.Values [i];
			}
			return result;
		}

		public RowVector Subtract (RowVector rhs)
		{
			RowVector result;
			if (this.Values.Length != rhs.Values.Length) {
				return new RowVector ();
			}
			result = new RowVector (this.Values);
			for (int i =0; i < this.Values.Length; i++) {
				result [i] -= rhs.Values [i];
			}
			return result;
		}

		public RowVector MultiplyByScalar (double scalar)
		{
			RowVector result = new RowVector (this.Values);
			for (int i = 0; i < result.Values.Length; i++) {
				result [i] *= scalar;
			}
			return result;
		}

		#endregion

		public double l2Norm()
		{
			return l_n_norm(2);
		}

		public double l_n_norm(int n)
		{
			return System.Math.Pow(Values.Select(p=>System.Math.Pow(p, (double)n)).Sum(), 1/((double)n));
		}

		public static double operator *(RowVector lhs, ColumnVector rhs)
		{
			if (lhs.Values.Length != rhs.Values.Length) {
				throw new ArgumentException ("Mismatch of vector sizes");
			}
			double sum = 0;
			for (int i = 0; i < lhs.Values.Length; i++) {
				sum += lhs [i] * rhs [i];
			}
			return sum;
		}

		public static RowVector operator -(RowVector lhs, RowVector rhs)
		{
			return lhs.Subtract(rhs);
		}

		public static RowVector operator *(double scalar, RowVector rhs)
		{
			return rhs.MultiplyByScalar (scalar);
		}

		public static RowVector operator +(RowVector lhs, RowVector rhs)
		{
			return lhs.Add (rhs);
		}
	}
}

