using System;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Diagnostics;

namespace Henspe.Core.Util
{
	public class ConvertUtil
	{
		public ConvertUtil ()
		{
		}

		static public string Base64Encode(string data)
		{
			try
			{
				byte[] encData_byte = new byte[data.Length];
				encData_byte = System.Text.Encoding.UTF8.GetBytes(data);    
				string encodedData = Convert.ToBase64String(encData_byte);
				return encodedData;
			}
			catch(Exception e)
			{
				throw new Exception("Error in base64Encode" + e.Message);
			}
		}

		static public string Base64Decode(string data)
		{
			try
			{
				System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();  
				System.Text.Decoder utf8Decode = encoder.GetDecoder();

				byte[] todecode_byte = Convert.FromBase64String(data);
				int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);    
				char[] decoded_char = new char[charCount];
				utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);                   
				string result = new String(decoded_char);
				return result;
			}
			catch(Exception e)
			{
				throw new Exception("Error in base64Decode" + e.Message);
			}
		}

		static public float ConvertStringToFloat(string value) 
		{
			float result = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
			return result;
		}

		static public int ConvertStringToInt(string value) 
		{
			if (value == null || value == "")
				return 0;

			int result = int.Parse(value);
			return result;
		}

		static public double ConvertStringToDouble(string value) 
		{
			if(value == null || value.Length == 0)
				return 0;

			Regex rgx = new Regex("[^0-9.,]");
			value = rgx.Replace(value, "");
			// Try with ,
			value = StringUtil.ReplaceStringInStringWithString (value, ",", ".");
			double result = Double.Parse (value, CultureInfo.InvariantCulture);

			return result;
		}

		static public double ConvertFloatToDouble(float value) 
		{
			return Convert.ToDouble (value);
		}

        static public double MetersToDecimalDegrees(double meters, double latitude)
        {
            try
            {
                double result = meters / (111.32 * 1000 * Math.Cos(latitude * (Math.PI / 180)));
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("MetersToDecimalDegrees exception: " + e.ToString());
            }

            return 0;
        }
    }
}
