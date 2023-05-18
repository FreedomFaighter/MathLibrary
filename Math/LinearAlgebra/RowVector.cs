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
#if DEBUG
				MathLog.MathLog ml = new MathLog.MathLog ();
				ml.Write (string.Format ("lhs: {0} rhs: {1}", this.Values.Length, rhs.Values.Length)
				          , System.Reflection.MethodBase.GetCurrentMethod ().Name);
				ml = null;
#endif
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
				MathLog.MathLog ml = new MathLog.MathLog ();
				ml.Write (string.Format ("lhs: {0} rhs: {1}", this.Values.Length, rhs.Values.Length)
				          , System.Reflection.MethodBase.GetCurrentMethod ().Name);
				ml = null;
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
			return System.Math.Sqrt (Values.Select (p => p * p).Sum ());
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

