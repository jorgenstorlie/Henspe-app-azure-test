using System;

namespace Henspe.Core.Util
{
    public class NumberUtil
	{
		public NumberUtil ()
		{
		}

        static public bool isEven(int number)
        {
            if (number % 2 == 0)
                return true;
            else
                return false;
        }
	}
}