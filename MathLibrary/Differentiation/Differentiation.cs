namespace MathLibrary.Differentiation
{
	static public class Differentiation
	{
		static public decimal Differentate (this Func<decimal, decimal, decimal> source
		                                                                          , Func<decimal, decimal> source2, decimal p, decimal h)
		{
			decimal r1 = (source (p + h, source2 (p + h)) - source (p, source2 (p))) / h;
			decimal r2 = ((source2 (p + h) - source2 (p)) / h);
			return  r1 * r2;
		}
	}
}

