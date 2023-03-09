using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace Math.LinearAlgebra
{
	[ClassInterface(ClassInterfaceType.None), ComDefaultInterface(typeof(NoSquareException)),ComVisible(true)]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class NoSquareException : Exception
	{

		int m, n;

		public int M
		{
			get{ return m;}
		}

		public int N
		{
			get{ return n;}
		}

		public NoSquareException (Matrix problemMatrix) : base()
		{
			this.m = problemMatrix.Rows;
			this.n = problemMatrix.Columns;
		}

		public NoSquareException(string message, Matrix problemMatrix) : base(message)
		{
			this.m = problemMatrix.Rows;
			this.n = problemMatrix.Columns;
		}

		protected NoSquareException(SerializationInfo info, StreamingContext context, Matrix problemMatrix) : base(info, context)
		{
			this.m = problemMatrix.Rows;
			this.n = problemMatrix.Columns;
		}

		public NoSquareException(string message, Exception innerException, Matrix problemMatrix) : base(message, innerException)
		{
			this.m = problemMatrix.Rows;
			this.n = problemMatrix.Columns;
		}
	}
}

