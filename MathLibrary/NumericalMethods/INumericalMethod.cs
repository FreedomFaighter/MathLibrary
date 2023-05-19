using System;
using System.Collections.Generic;


namespace MathLibrary.NumericalMethods
{
	public interface INumericalMethod<T>
	{
		IEnumerable<Tuple<T,T>> Solve();
	}
}

