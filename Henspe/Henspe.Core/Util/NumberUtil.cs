using System;

namespace Henspe.Core.Util
{
	public class NumberUtil
	{
        public NumberUtil()
		{
		}

        static public bool DoubleIsZero(double value)
        {
            if (System.Math.Abs(value) > double.Epsilon)
                return false;
            else
                return true;
        }

        static public bool FloatIsZero(float value)
        {
            if (System.Math.Abs(value) > float.Epsilon)
                return false;
            else
                return true;
        }
    }
}