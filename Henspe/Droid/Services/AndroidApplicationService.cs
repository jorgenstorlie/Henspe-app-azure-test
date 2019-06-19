using SNLA.Core.Services;
using Henspe.Core.Services;
using System;
using Android.App;
using Android.Runtime;
using Henspe.Core.Model.Dto;
using Android.Locations;
using Henspe.Droid.Services;
using SNLA.Core.Const;

namespace Henspe.Droid.Services
{
    public class AndroidApplicationService : ApplicationService
    {
		public AndroidApplicationService(string databaseName = "snladata") : base()
        {
			CoordinateService = new CoordinateService();
        }
    }
}
