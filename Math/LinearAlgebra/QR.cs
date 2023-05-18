using System;

namespace MathLibrary.LinearAlgebra
{
	public class QR
	{
		private Matrix _q;
		private Matrix _r;
		public Matrix Q
		{
			get{ return _q;}
			set{ _q = value;}
		}

		public Matrix R
		{
			get{ return _r;}
			set { _r = value;}
		}
		public QR (Matrix Q, Matrix R)
		{
			this._q = Q;
			this._r = R;
		}
	}
}

