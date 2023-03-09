using System;

namespace Math.LinearAlgebra
{
	public class MatrixMath
	{

		public MatrixMath ()
		{
		}

		public static Matrix Abs(Matrix matrix)
		{
			Matrix result = new Matrix (matrix);
			Parallel.Parallel.For (0, matrix.Rows, (i) =>
			{
				for(int j = 0; j < matrix.Columns;j++)
				{
					result[i,j] = System.Math.Abs(result[i,j]);
				}
			});

			return result;
		}

	}
}

