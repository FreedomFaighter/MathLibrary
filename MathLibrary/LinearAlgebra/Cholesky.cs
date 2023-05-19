using System;

namespace MathLibrary.LinearAlgebra
{
	public class Cholesky
	{
		Matrix _U, _L;

		public Matrix L{
			get{ return _L;}
			set{ _L = value;}
		}

		public Matrix U{
			get{ return _U;}
			set{ _U = value;}
		}

		public Cholesky (Matrix U, Matrix L)
		{
			this._L = L;
			this._U = U;
		}
	}
}

