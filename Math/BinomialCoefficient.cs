using System;

namespace MathLibrary
{
	static public class BinomialCoefficient
	{
		static public double BC(int n, int k)
		{
			if (n > 0) {
				if (k < 0 || k > n)
					return 0;
				if (k == 0 || k == n)
					return 1;
				k = System.Math.Min (k, n - k);
				double c = 1;
				for (int i = 0; i < k; i++)
					c *= (n - i) / (i + 1);

				return c;
			} else if (n < 0) {
				return k % 2 == 0 ? 1 : -1 * BC (System.Math.Abs (n) + k - 1, k);
			} else {
				return 0;
			}
		}
	}
}

