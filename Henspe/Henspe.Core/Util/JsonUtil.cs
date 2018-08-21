using System;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Henspe.Core.Util
{
	public class JsonUtil
	{
		public JsonUtil () {
		}

        static public string CheckForError(string jsonString)
        {
            bool containsError = StringUtil.StringContains(jsonString, "error_message");
            if (containsError)
            {
                var jsonObject = JObject.Parse(jsonString);
                string errorMessage = (string)jsonObject.SelectToken("error_message");
                if (errorMessage != null)
                {
                    return errorMessage;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
	}
}