using System;

namespace Henspe.Core.Util
{
	public class MathUtil
	{
		public MathUtil ()
		{
		}

		static public bool IsOdd(int value)
		{
			return value % 2 != 0;
		}

		static public bool IsEven(int value)
		{
			return !IsOdd(value);
		}

		static public double Round(double value)
		{
			return Math.Round(value, 0, MidpointRounding.AwayFromZero);
		}
	}
}