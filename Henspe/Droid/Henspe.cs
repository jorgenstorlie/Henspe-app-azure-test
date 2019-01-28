using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Content;
using Android.Content.PM;
using Android.Support.V4.Content;
using Henspe.Core;
using Henspe.Core.Service;
using Henspe.Core.Model.Dto;
using System.Threading;
using Android.Locations;
using Henspe.Droid.Const;
using Android.Gms.Location;
using SNLA.Core.Const;

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
   //     public int coordinateFormat = CoordinateUtil.ddm; // Default coordinate format
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
		public position_fragment PositionFragment;

		public CoordinateService CoordinateService = null;

		public Henspe(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
            Current = this;
        }

		public override void OnCreate()
		{
			base.OnCreate();

			CoordinateService = new CoordinateService();

			mFusedLocationClient = LocationServices.GetFusedLocationProviderClient(this);
			InitializeLocationText();
			SetupLocalData();
		}

		private void InitializeLocationText()
		{
			unknownCoordinates = Resources.GetString(Resource.String.Location_unknown_coordinates_row1) + "\n" + Resources.GetString(Resource.String.Location_unknown_coordinates_row2);
			coordinatesText = unknownCoordinates;
			unknownAddress = Resources.GetString(Resource.String.Location_unknown_address_row1) + "\n" + Resources.GetString(Resource.String.Location_unknown_address_row2);
			addressText = unknownAddress;
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
			StructureSectionDto structureHendelse = structure.AddStructureSection(Resources.GetString(Resource.String.Structure_Hendelse_Header), "");
            structureHendelse.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Hendelse_Trafikk), "ic_trafikk", 1.0f);
            structureHendelse.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Hendelse_Brann), "ic_brann", 0.8f);

            // Eksakt posisjon
            StructureSectionDto structureEksaktPosisjon = structure.AddStructureSection(Resources.GetString(Resource.String.Structure_EksaktPosisjon_Header), "");
         //   structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Position, Resources.GetString(Resource.String.Structure_EksaktPosisjon_Posisjon), "ic_posisjon", 0.8f);
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Address, Resources.GetString(Resource.String.Structure_EksaktPosisjon_Adresse), "ic_adresse", 0.8f);
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_EksaktPosisjon_Oppmotested), "ic_oppmotested", 1.0f);
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_EksaktPosisjon_Ankomst), "ic_ankomst", 0.6f);
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_EksaktPosisjon_Avreise), "ic_avreise", 0.6f);

            // Nivå
            StructureSectionDto structureNiva = structure.AddStructureSection(Resources.GetString(Resource.String.Structure_Niva_Header), "");
            structureNiva.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Niva_Type), "ic_begrenset", 0.5f);
            structureNiva.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Niva_QuatroVarsling), "ic_quartro", 0.7f);

            // Sikkerhet
            StructureSectionDto structureSikkerhet = structure.AddStructureSection(Resources.GetString(Resource.String.Structure_Sikkerhet_Header), "");
            structureSikkerhet.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Sikkerhet_Farer), "ic_farer", 0.8f);
            structureSikkerhet.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Sikkerhet_Brann), "ic_brann", 0.8f);
            structureSikkerhet.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Sikkerhet_Sikkerhet), "ic_sikkerhet", 0.8f);

            // Pasienter
            StructureSectionDto structurePasienter = structure.AddStructureSection(Resources.GetString(Resource.String.Structure_Pasienter_Header), "");
            structurePasienter.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Pasienter_Antall), "ic_pasienter", 0.9f);
            structurePasienter.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Pasienter_Type), "ic_type", 0.7f);
            structurePasienter.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Pasienter_Omfang), "ic_skademekanikk", 0.8f);

            // Evakuering
            StructureSectionDto structureEvakuering = structure.AddStructureSection(Resources.GetString(Resource.String.Structure_Evakuering_Header), "");
            structureEvakuering.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Evakuering_Flaskehalser), "ic_flaskehalser", 0.7f);
            structureEvakuering.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Evakuering_Kjeder), "ic_evakuering", 0.7f);
            structureEvakuering.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Evakuering_Rett), "ic_rett", 0.7f);
        }



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