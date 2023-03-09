using System;
using System.Collections.Generic;


namespace Math.NumericalMethods
{
	public interface INumericalMethod<T>
	{
		IEnumerable<Tuple<T,T>> Solve();
	}
}

