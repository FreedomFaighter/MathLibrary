using System;

namespace Math.LinearAlgebra
{
	public enum EigenMatrix{
		Reals,
		Imaginaries,
		Lambda
	}
	public class Eigen
	{


		Matrix _Lambda, _Reals, _Imaginaries;
		double _ToleranceUsed;
		bool? _PassedTraceTest, _PassedDeterminantTest;

		public Matrix Imaginaries
		{
			get{ return _Imaginaries;}
			set{ _Imaginaries = value;}
		}

		public Matrix Lambda
		{
			get{ return _Lambda;}
			set { _Lambda = value;}
		}

		public Matrix Reals
		{
			get{return _Reals;}
			set{_Reals = value;}
		}

		public double ToleranceUsed{
			get{ return _ToleranceUsed;}
		}

		public bool? PassedTraceTest
		{
			get{ return _PassedTraceTest;}
		}

		public bool? PassedDeterminantTest
		{
			get{ return _PassedDeterminantTest;}
		}
	
		public Eigen (Matrix lambda, Matrix reals, Matrix imaginaries) : this(lambda, reals,imaginaries,null,null)
		{

		}

		public Eigen(Matrix lambda, Matrix reals, Matrix imaginaries, bool? passedTraceTest, bool? passedDeterminantTest, double toleranceUsed = 0.01)
		{
			this._Reals = reals;
			this._Imaginaries = imaginaries;
			this._Lambda = lambda;
			this._ToleranceUsed = toleranceUsed;
			if (passedTraceTest.HasValue) {
				this._PassedTraceTest = passedTraceTest.Value;
			}
			if (passedDeterminantTest.HasValue) {
				this._PassedDeterminantTest = passedDeterminantTest.Value;
			}
		}

		public Eigen(int n) : this(new Matrix (n, n), new Matrix (n, n),new Matrix (n, n), null, null)
		{
		}

		public double this[EigenMatrix em, int i, int j]
		{
			get{
				switch (em) {
				case EigenMatrix.Imaginaries:
					return this._Imaginaries[i,j];

				case EigenMatrix.Reals:
					return this._Reals[i,j];

				case EigenMatrix.Lambda:
					return this._Lambda[i,j];
				default:
					throw new ArgumentException ("unable to determine which value being sought");
				}
			}
			set{
				switch (em) {
				case EigenMatrix.Imaginaries:
					this._Imaginaries [i, j] = value;
					break;
				case EigenMatrix.Reals:
					this._Reals [i, j] = value;
					break;
				case EigenMatrix.Lambda:
					this._Lambda [i, j] = value;
					break;
				}
			}
		}
	}
}

