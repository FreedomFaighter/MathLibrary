using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Threading.Tasks;
using System.Collections;
using MathLibrary.NumericalMethods;
using MathLibrary.NumericalMethods.OridinaryDifferentialEquations;

namespace MathLibrary
{
    public static class DecimalMath
    {
		private static Decimal ToleranceSqrt = 0.0001M;

        public static Decimal Sqrt(this decimal a)
		{
            
			//var ourGuess = guess.GetValueOrDefault(x / 2m);
			//var result = x / ourGuess;
			//var average = (ourGuess + result) / 2m;
			//if (average == ourGuess)
			//    return average;
			//else
			//    return DecimalMath.Sqrt(x, average);
			decimal ERR;
			Func<Decimal, Decimal> g = (y) => {
				return 0.5M * (y + a / y);
			};
            
            
			Decimal x0 = a / 2, x1;
			do {
				x1 = g (x0);
				ERR = x1 - x0;
				x0 = x1;
			} while(System.Math.Abs(ERR) > DecimalMath.ToleranceSqrt);

			return x0;
		}

        static uint _LOOPS = 15;

        public static decimal Pow(this decimal x, int y)
		{
			if (y == 1)
				return x;
			decimal A = x;

			for (int i = 0; i < y; i++) {
				A *= A;
			}
			if (y < 0)
				return 1 / A;
			return A;
		}

        public static decimal Ln(this decimal a)
		{
			/*
            ln(a) = log(1-x) = - x - x^2/2 - x^3/3 - ...   (where |x| < 1)
                x: a = 1-x    =>   x = 1-a = 1 - 1.004 = -.004
            */
			if (a == 1)
				return 0;
			else if (a < 1) {
				decimal x = 1 - a;
				if (System.Math.Abs (x) >= 1)
					throw new Exception ("must be 0 < a < 2");

				decimal result = 0;
				uint iteration = _LOOPS;
				while (iteration > 0) {
					result -= x.Pow (iteration) / iteration;
					iteration--;
				}
				return result;
			} else {
				Func<decimal, decimal, decimal> f_of_t_y = delegate(decimal t, decimal y) {
					return 1 / t;
				};
				INumericalMethod<decimal> rk = new RungeKutta (f_of_t_y, 0, 1, a);
				List<Tuple<decimal, decimal>> list = new List<Tuple<decimal, decimal>> ();
				list = (List<Tuple<decimal, decimal>>)rk.Solve ();

				return list [list.Count - 1].Item2;
			}
		}

        public static ulong[] Fact = new ulong[] {
    1L,
    1L * 2,
    1L * 2 * 3,
    1L * 2 * 3 * 4,
    1L * 2 * 3 * 4 * 5,
    1L * 2 * 3 * 4 * 5 * 6,
    1L * 2 * 3 * 4 * 5 * 6 * 7,
    1L * 2 * 3 * 4 * 5 * 6 * 7 * 8,
    1L * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9,
    1L * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10,
    1L * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11,
    1L * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12,
    1L * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13,
    1L * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14,
    1L * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15,
    1L * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16,
    1L * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16 * 17,
    1L * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16 * 17 * 18,
    1L * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16 * 17 * 18 * 19,
    1L * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16 * 17 * 18 * 19 * 20,
    14197454024290336768L, //1L * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16 * 17 * 18 * 19 * 20 * 21,        // NOTE: Overflow during compilation
    17196083355034583040L, //1L * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16 * 17 * 18 * 19 * 20 * 21 * 22    // NOTE: Overflow during compilation
		};

        public static decimal Exp(this decimal b)
		{
			/*
            exp(y) = 1 + y + y^2/2 + y^3/3! + y^4/4! + y^5/5! + ...
            */
			if (b == 0)
				return 1.0M;
			Func<decimal, decimal, decimal> y = delegate(decimal t, decimal w) {
				return w;
			};
			INumericalMethod<decimal> rk = new RungeKutta (y, 1, 0, System.Math.Abs (b));
			List<Tuple<decimal, decimal>> list = (List<Tuple<decimal, decimal>>)rk.Solve ();
			Decimal result = ((List<Tuple<decimal, decimal>>)list) [list.Count - 1].Item2;
			if (b < 0)
				return 1.0M / result;
			else
				return result;
		}

        public static Decimal Pow(this Decimal a, Decimal b)
		{
			return Exp (b * Ln (a));
		}
    }
}
