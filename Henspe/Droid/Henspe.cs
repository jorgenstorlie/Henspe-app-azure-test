using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Content;
using Android.Content.PM;
using Android.Support.V4.Content;
using Henspe.Core;
using Henspe.Core.Model.Dto;
using System.Threading;
using Android.Locations;
using Henspe.Droid.Const;
using Android.Gms.Location;

namespace Henspe.Droid
{
	#if DEBUG
    [Application(Label = "@string/app_name", Theme = "@style/SplashTheme", Debuggable = true, LargeHeap = true)]
#else
    [Application(Label = "@string/app_name", Theme = "@style/SplashTheme", Debuggable = false, LargeHeap = true)]
#endif
    class Henspe : Application, Application.IActivityLifecycleCallbacks
    {
		public string mode = ModeConst.test;

		protected const int RequestPermissionsRequestCode = 34;

		public static Henspe Current { get; private set; }
        public float version { get; private set; }
        public bool askedIfUserWantNewVersion = false;
        public bool fetchNewVersion = false;

		// GPS
        public int coordinateFormat = CoordinateUtil.ddm; // Default coordinate format
		public FusedLocationProviderClient mFusedLocationClient;
		public Location myLocation;
		public string unknownCoordinates = "";
		public string coordinatesText = "";
		public string unknownAddress = "";
		public string addressText = "";

		public StructureDto structure;

		public int screenHeight { get; private set; }
        public int screenWidth { get; private set; }
        public int screenHeightPercentage { get; private set; }
        public int screenWidthPercentage { get; private set; }

		public Henspe(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
            Current = this;
        }

		public override void OnCreate()
		{
			base.OnCreate();

			mFusedLocationClient = LocationServices.GetFusedLocationProviderClient(this);
			GetVersion();
			InitializeLocationText();

			SetupLocalData();
			CheckNewAppVersionAvailable();
		}

		private void InitializeLocationText()
		{
			unknownCoordinates = Resources.GetString(Resource.String.Location_unknown_coordinates_row1) + "\n" + Resources.GetString(Resource.String.Location_unknown_coordinates_row2);
			coordinatesText = unknownCoordinates;
			unknownAddress = Resources.GetString(Resource.String.Location_unknown_address_row1) + "\n" + Resources.GetString(Resource.String.Location_unknown_address_row2);
			addressText = unknownAddress;
		}

		private void GetVersion()
        {
            PackageInfo pInfo = PackageManager.GetPackageInfo(PackageName, 0);
            version = ConvertUtil.ConvertStringToFloat(pInfo.VersionName);
        }

		public void SetScreenSize(int height, int width)
        {
            screenHeight = height;
            screenWidth = width;
            screenHeightPercentage = (int)(0.01 * screenHeight);
            screenWidthPercentage = (int)(0.01 * screenWidth);
        }

		void SetupLocalData()
        {
            SetupSectionsWithElements();

            structure.currentStructureSectionId = 0;
        }

        void SetupSectionsWithElements()
        {
            // Structure that all will be added to
            structure = new StructureDto();

            // Hendelse
			StructureSectionDto structureHendelse = structure.AddStructureSection(Resources.GetString(Resource.String.Structure_Hendelse_Header), "ic_h.png");
            structureHendelse.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Hendelse_Trafikk), "ic_h_trafikk.png", 1.0f);
            structureHendelse.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Hendelse_Brann), "ic_h_brann.png", 0.8f);

            // Eksakt posisjon
            StructureSectionDto structureEksaktPosisjon = structure.AddStructureSection(Resources.GetString(Resource.String.Structure_EksaktPosisjon_Header), "ic_e.png");
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Position, Resources.GetString(Resource.String.Structure_EksaktPosisjon_Posisjon), "ic_e_posisjon.png", 0.8f);
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Address, Resources.GetString(Resource.String.Structure_EksaktPosisjon_Adresse), "ic_e_adresse.png", 0.8f);
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_EksaktPosisjon_Oppmotested), "ic_e_oppmotested.png", 1.0f);
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_EksaktPosisjon_Ankomst), "ic_e_ankomst.png", 0.6f);
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_EksaktPosisjon_Avreise), "ic_e_avreise.png", 0.6f);

            // Nivå
            StructureSectionDto structureNiva = structure.AddStructureSection(Resources.GetString(Resource.String.Structure_Niva_Header), "ic_n.png");
            structureNiva.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Niva_Type), "ic_n_begrenset.png", 0.5f);
            structureNiva.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Niva_QuatroVarsling), "ic_n_quartro.png", 0.7f);

            // Sikkerhet
            StructureSectionDto structureSikkerhet = structure.AddStructureSection(Resources.GetString(Resource.String.Structure_Sikkerhet_Header), "ic_s.png");
            structureSikkerhet.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Sikkerhet_Farer), "ic_s_farer.png", 0.8f);
            structureSikkerhet.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Sikkerhet_Brann), "ic_s_brann.png", 0.8f);
            structureSikkerhet.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Sikkerhet_Sikkerhet), "ic_s_sikkerhet.png", 0.8f);

            // Pasienter
            StructureSectionDto structurePasienter = structure.AddStructureSection(Resources.GetString(Resource.String.Structure_Pasienter_Header), "ic_p.png");
            structurePasienter.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Pasienter_Antall), "ic_p_pasienter.png", 0.9f);
            structurePasienter.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Pasienter_Type), "ic_p_type.png", 0.7f);
            structurePasienter.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Pasienter_Omfang), "ic_p_skademekanikk.png", 0.8f);

            // Evakuering
            StructureSectionDto structureEvakuering = structure.AddStructureSection(Resources.GetString(Resource.String.Structure_Evakuering_Header), "ic_e.png");
            structureEvakuering.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Evakuering_Flaskehalser), "ic_e_flaskehalser.png", 0.7f);
            structureEvakuering.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Evakuering_Kjeder), "ic_e_evakuering.png", 0.7f);
            structureEvakuering.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Evakuering_Rett), "ic_e_rett.png", 0.7f);
        }

		#region check new version
		public void CheckNewAppVersionAvailable()
        {
			string testLink = EventsConst.versionXML;
            ThreadPool.QueueUserWorkItem(o => GetResponseFromServerAppVersion(testLink));
        }

        public async void GetResponseFromServerAppVersion(string givenLink)
        {
			System.IO.Stream istream = null;
            Java.Net.HttpURLConnection urlConnection = null;
            try
            {
                var url = new Java.Net.URL((string)givenLink);
                urlConnection = (Java.Net.HttpURLConnection)url.OpenConnection();
                urlConnection.Connect();

                istream = urlConnection.InputStream;// as Java.IO.InputStream;
                Java.IO.BufferedReader br = new Java.IO.BufferedReader(new Java.IO.InputStreamReader(istream));
                Java.Lang.StringBuffer sb = new Java.Lang.StringBuffer();
                String line;
                while ((line = (string)br.ReadLine()) != null)
                {
                    sb.Append(line);
                }
                string data = (string)sb.ToString();
                br.Close();
                string newVersion = GetAppVersionFromResponse(data);
                if (ShouldDownloadNewVersion(newVersion))
                {
                    var intent = new Intent(EventsConst.newVersionAvailable);
                    LocalBroadcastManager.GetInstance(ApplicationContext).SendBroadcast(intent);
                    fetchNewVersion = true;
                }
            }
            catch (Exception)
            {
            }
        }

        string GetAppVersionFromResponse(string data)
        {
            string versionTempString = StringUtil.FindStringBetween(data, "<key>bundle-version</key>", "</string>");
            string versionString = StringUtil.FindStringAfter(versionTempString, "<string>");
            versionString = versionString.Trim();
            return versionString;
        }

        bool ShouldDownloadNewVersion(string newVersion)
        {
            var newVersionFloat = ConvertUtil.ConvertStringToFloat(newVersion);
            if (version < newVersionFloat)
                return true;
            else
                return false;
        }

        public void OnNewVersionDownloaded()
        {
            try
            {
                Java.IO.File dir = new Java.IO.File(
                                global::Android.OS.Environment.GetExternalStoragePublicDirectory
                                    (global::Android.OS.Environment.DirectoryDownloads), EventsConst.newApkName);
                var uri = global::Android.Net.Uri.FromFile(dir);
                Intent i = new Intent();
                i.SetAction(Intent.ActionView);
                i.SetFlags(ActivityFlags.GrantReadUriPermission);
                i.SetDataAndType(uri, "application/vnd.android.package-archive");
                i.SetFlags(ActivityFlags.NewTask);
                StartActivity(i);
            }
            catch (Exception)
            {

            }
        }
		#endregion

		public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
		{
			throw new NotImplementedException();
		}

		public void OnActivityDestroyed(Activity activity)
		{
			throw new NotImplementedException();
		}

		public void OnActivityPaused(Activity activity)
		{
			throw new NotImplementedException();
		}

		public void OnActivityResumed(Activity activity)
		{
			throw new NotImplementedException();
		}

		public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
		{
			throw new NotImplementedException();
		}

		public void OnActivityStarted(Activity activity)
		{
			throw new NotImplementedException();
		}

		public void OnActivityStopped(Activity activity)
		{
			throw new NotImplementedException();
		}
	}
}