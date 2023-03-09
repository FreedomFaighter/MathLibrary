using System;
using System.Linq;

namespace MathLibrary.LinearAlgebra
{
    public class ColumnVector : IVector
    {
        public ColumnVector(int v)
        {
            double[] tempValues = new double[1];
            if (v > 1)
                Array.Resize<double>(ref tempValues, v);
            this.Values = tempValues;
        }

        public ColumnVector(double[] values)
        {
            values.CopyTo(this.Values, 0);
        }

        #region IVector implementation

        public RowVector Transpose()
        {
            return new RowVector(this.Values);
        }

        public double this[int i]
        {
            get
            {
                return this.Values[i];
            }
            set
            {
                this.Values[i] = value;
            }
        }

        #endregion

        #region IVector implementation

        public double[] Values
        {
            get;
            set;
        }

        #endregion

        #region IVector implementation

        public ColumnVector Add(ColumnVector rhs)
        {
            ColumnVector result = new ColumnVector(new double[rhs.Values.Length]);
            for (int i = 0; i < result.Values.Length; i++)
            {
                result[i] = this[i] + rhs.Values[i];
            }
            return result;
        }

        public ColumnVector Subtract(ColumnVector rhs)
        {
            ColumnVector result = new ColumnVector(new double[rhs.Values.Length]);
            for (int i = 0; i < result.Values.Length; i++)
            {
                result[i] = this[i] - rhs.Values[i];
            }
            return result;
        }

        public IVector MultiplyByScalar(double scalar)
        {
            ColumnVector result = new ColumnVector(new double[this.Values.Length]);
            for (int i = 0; i < result.Values.Length; i++)
            {
                result[i] = this[i] * scalar;
            }
            return result;
        }

        #endregion

        public static Matrix operator *(ColumnVector lhs, RowVector rhs)
        {
            Matrix result = new Matrix(lhs.Values.Length, rhs.Values.Length);
            for (int i = 0; i < lhs.Values.Length; i++)
            {
                for (int j = 0; j < rhs.Values.Length; j++)
                {
                    result[i, j] += lhs[i] * rhs[j];
                }
            }
            return result;
        }

        public static ColumnVector operator *(double lhs, ColumnVector rhs)
        {
            ColumnVector result = new ColumnVector(rhs.Values);
            for (int i = 0; i < result.Values.Length; i++)
            {
                result[i] *= lhs;
            }
            return result;
        }

        public static ColumnVector operator -(ColumnVector lhs, ColumnVector rhs)
        {
            if (lhs.Values.Length == rhs.Values.Length)
            {
                ColumnVector result = new ColumnVector(lhs.Values);
                for (int i = 0; i < result.Values.Length; i++)
                {
                    result[i] -= rhs[i];
                }
            }
            throw new ArgumentOutOfRangeException("vector lengths not equal");
        }

        public static ColumnVector operator +(ColumnVector lhs, ColumnVector rhs)
        {
            if (lhs.Values.Length == rhs.Values.Length)
            {
                ColumnVector result = new ColumnVector(lhs.Values);
                for (int i = 0; i < result.Values.Length; i++)
                {
                    result[i] += rhs[i];
                }
            }
            throw new ArgumentOutOfRangeException("vector lengths not equal");
        }

        public double l2Norm()
        {
            return System.Math.Sqrt(this.Values.Select(p => p * p).Sum());
        }

        public double InfinityNorm()
        {
            return this.Values.Select(p => System.Math.Abs(p)).Max();
        }

        public static double InnerProduct(ColumnVector lhs, ColumnVector rhs)
        {
            return lhs.Transpose() * rhs;
        }
    }
}