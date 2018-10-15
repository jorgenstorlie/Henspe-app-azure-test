using System;
using UIKit;
using CoreLocation;
using Foundation;
using Henspe.Core.Communication;
using Henspe.Core.Const;
using Henspe.iOS.Util;
using Henspe.Core.Util;
using Henspe.Core;
using Henspe.iOS.Const;
using Henspe.iOS.AppModel;
using Henspe.Core.Model.Dto;

namespace Henspe.iOS
{
    // AppStore: no.norskluftambulanse.henspe
    // Enterprise: no.snla-system.henspe
    // Debug: no.norskluftambulanse.testapp
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        public string mode = ModeConst.test;

        public string prodUrlString = "https://snla-apps.no/apps/henspe/";
        public string testUrlTest = "https://snla-apps.no/apps/henspetest/";
        public string plistFile = "Henspe.plist";

        public bool appActicatedOccured;

		// Format
        // GPS
        public double highAndLowAccuracyDivider = 200;
        public double gpsAccuracyRequirement = 200;
        public double distanceToUpdateAddress = 200;
        public bool gpsEventOccured = false;
        public int gpsCoverage = GpsCoverageConst.none;
        public int lastPositionType = PositionTypeConst.off;
        public CLAuthorizationStatus gpsStatus;
        public bool gpsStarted = false;
        public CLLocationManager iPhoneLocationManager = null;
        public CLLocation currentLocation = null;
        public CLLocation lastLocation = null;
        public CLGeocoder geocoder = null;
        public bool gpsPosFound = false;
        public CLLocation lastAddressLocation = null;
        public GPSObject gpsCurrentPositionObject = new GPSObject();
        public GPSObject gpsStoredPositionObject = new GPSObject();
        public double desiredAccuracy = 10;
        public double distanceFilter = 10;
		public double roundedLatitude;
		public double roundedLongitude;

        // class-level declarations
        UIWindow window;
        public override UIWindow Window
        {
            get;
            set;
        }

        public static AppDelegate current { get; private set; }
        //public Repository repository { get; set; }
        public CxHttpClient client { get; private set; }

        //Connection conn;

        public static UIViewController initialViewController;

        public StructureDto structure;
        public int coordinateFormat;

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Background update interval in seconds
            //UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval (60.0f * 60.0f); // One hour

            current = this;
            window = new UIWindow(UIScreen.MainScreen.Bounds);

            client = new CxHttpClient();

            SetupCustomNavigationBar();

            SetupLocalData();

            return true;
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
			StructureSectionDto structureHendelse = structure.AddStructureSection(LangUtil.Get("Structure.Hendelse.Header"), string.Empty);
			structureHendelse.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Hendelse.Trafikk"), "ic_h_trafikk.svg", 1.0f);
			structureHendelse.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Hendelse.Brann"), "ic_h_brann.svg", 0.8f);
            
			// Eksakt posisjon
			StructureSectionDto structureEksaktPosisjon = structure.AddStructureSection(LangUtil.Get("Structure.EksaktPosisjon.Header"), "ic_e.svg");
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Selector, string.Empty, string.Empty, 0);
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Position, LangUtil.Get("Structure.EksaktPosisjon.Posisjon"), "ic_e_posisjon.svg", 0.8f);
			structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Address, LangUtil.Get("Structure.EksaktPosisjon.Adresse"), "ic_e_adresse.svg", 0.8f);
			structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.EksaktPosisjon.Oppmotested"), "ic_e_oppmotested.svg", 1.0f);
			structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.EksaktPosisjon.Ankomst"), "ic_e_ankomst.svg", 0.6f);
			structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.EksaktPosisjon.Avreise"), "ic_e_avreise.svg", 0.6f);
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Buttons, string.Empty, string.Empty, 0);

            // Nivå
            StructureSectionDto structureNiva = structure.AddStructureSection(LangUtil.Get("Structure.Niva.Header"), "ic_n.svg");
			structureNiva.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Niva.Type"), "ic_n_begrenset.svg", 0.5f);
			structureNiva.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Niva.QuatroVarsling"), "ic_n_quartro.svg", 0.7f);

			// Sikkerhet
			StructureSectionDto structureSikkerhet = structure.AddStructureSection(LangUtil.Get("Structure.Sikkerhet.Header"), "ic_s.svg");
			structureSikkerhet.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Sikkerhet.Farer"), "ic_s_farer.svg", 0.8f);
			structureSikkerhet.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Sikkerhet.Brann"), "ic_s_brann.svg", 0.8f);
			structureSikkerhet.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Sikkerhet.Sikkerhet"), "ic_s_sikkerhet.svg", 0.8f);

            // Pasienter
			StructureSectionDto structurePasienter = structure.AddStructureSection(LangUtil.Get("Structure.Pasienter.Header"), "ic_p.svg");
			structurePasienter.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Pasienter.Antall"), "ic_p_pasienter.svg", 0.9f);
			structurePasienter.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Pasienter.Type"), "ic_p_type.svg", 0.7f);
			structurePasienter.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Pasienter.Omfang"), "ic_p_skademekanikk.svg", 0.8f);

            // Evakuering
			StructureSectionDto structureEvakuering = structure.AddStructureSection(LangUtil.Get("Structure.Evakuering.Header"), "ic_e.svg");
			structureEvakuering.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Evakuering.Flaskehalser"), "ic_e_flaskehalser.svg", 0.7f);
			structureEvakuering.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Evakuering.Kjeder"), "ic_e_evakuering.svg", 0.7f);
			structureEvakuering.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Evakuering.Rett"), "ic_e_rett.svg", 0.7f);
        }

        private void SetupCustomNavigationBar()
        {
            UITextAttributes attributes = new UITextAttributes
            {
                Font = FontConst.fontNavbar,
                TextColor = ColorConst.textColor
            }; // ForegroundColor

            // White top
            //UIApplication.SharedApplication.SetStatusBarStyle (UIStatusBarStyle.LightContent, false);

            UINavigationBar.Appearance.SetTitleTextAttributes(attributes);
        }

        public override void OnActivated(UIApplication application)
        {
            Console.WriteLine("UpdateMechanism-OnActivated. Not waiting for new version install");
            // Sync after sleep
            appActicatedOccured = true;

			CheckForAppSettings();

			InvokeOnMainThread(delegate
            {
        	    NSNotificationCenter.DefaultCenter.PostNotificationName(EventConst.appActivated, this);
            });
        }

		private void CheckForAppSettings()
        {
            InvokeOnMainThread(delegate
            {
                // Coordinate format
                nint settingsUserDefinedFormat = NSUserDefaults.StandardUserDefaults.IntForKey("settings_user_defined_coordinate_format");
                if (settingsUserDefinedFormat == CoordinateUtil.undefinedFormat)
                    settingsUserDefinedFormat = CoordinateUtil.ddm; // Default coordinate format

                AppDelegate.current.coordinateFormat = Convert.ToInt32(settingsUserDefinedFormat);
            });
        }
        
        // This method is invoked when the application is about to move from active to inactive state.
        // OpenGL applications should use this method to pause.
        public override void OnResignActivation(UIApplication application)
        {
        }

        // This method should be used to release shared resources and it should store the application state.
        // If your application supports background exection this method is called instead of WillTerminate
        // when the user quits.
        public override void DidEnterBackground(UIApplication application)
        {
        }

        // This method is called as part of the transiton from background to active state.
        public override void WillEnterForeground(UIApplication application)
        {
        }

        // This method is called when the application is about to terminate. Save data, if needed.
        public override void WillTerminate(UIApplication application)
        {
        }

        public void OnGetCurrentAppVersionFault(String reason)
        {
            // We accept that it does not work
        }
    }
}