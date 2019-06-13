using SNLA.Core.Service;
using Henspe.Core.Service;
using System;
using Android.App;
using Android.Runtime;
using Henspe.Core.Service;
using Henspe.Core.Model.Dto;
using Android.Locations;
using SNLA.Core.Service;
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
