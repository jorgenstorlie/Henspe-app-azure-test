using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Henspe.Core.Util;

namespace Henspe.Droid.Util
{
    public class ResourceUtil
    {
		public static int GetResourceIdForImagename(string name)
        {
			if(name != null && name.Length > 0)
			{
				string filenameWithoutExtension = StringUtil.FindStringBefore(name, ".");
				var resourceId = (int)typeof(Resource.Drawable).GetField(filenameWithoutExtension).GetValue(null);
				return resourceId;
			}

			return 0;
        }
    }
}