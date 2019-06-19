using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using SNLA.Core.Util;
using SNLA.Core.Communication;
using SNLA.Core.Storage;
using SNLA.Core.Services;
using Henspe.Core.Communication;

namespace Henspe.Core.Services
{
	public class ApplicationService : ApplicationServiceBase
	{
		public CoordinateService CoordinateService = null;

		public ApplicationService(string databaseName = "snladata") : base(databaseName)
		{
			versionMode = NewVersionUpdateMode.NoVersionCheck;
		}

		protected override void PreInitialize() { }

		protected override void PostInitialize()
		{
			SetupServicesIfNeeded();
		}

		private void SetupServicesIfNeeded()
		{
			CoordinateService = new CoordinateService();
		}
	}
}
