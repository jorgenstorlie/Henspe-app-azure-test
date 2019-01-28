using System;
using UIKit;
using CoreLocation;
using Foundation;
using Henspe.Core.Communication;
using Henspe.Core;
using Henspe.iOS.Const;
using Henspe.iOS.AppModel;
using Henspe.Core.Model.Dto;
using Henspe.Core.Service;
using Henspe.Core.Storage;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using SNLA.Core.Util;
using SNLA.iOS.Util;
using SNLA.Core.Const;
using System.IO;

namespace Henspe.iOS
{
    // AppStore: no.norskluftambulanse.henspe
    // Enterprise: no.snla-system.henspe
    // Debug: no.norskluftambulanse.testapp
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        public string os = SettingsConst.ios;
        public string version;

        public string mode = ModeConst.test;

        public string prodUrlString = "https://snla-apps.no/apps/henspe/";
        public string testUrlTest = "https://snla-apps.no/apps/henspetest/";
        public string plistFile = "Henspe.plist";

        public bool appActicatedOccured;

        // Services
        private bool deleteDatabaseOnStartup = false;
        private string noNetworkString;
        public bool servcicesInitialized = false;
        public HjertestarterService hjertestarterService;
        public CoordinateService coordinateService;
        public RegEmailSMSService regEmailSMSService;

        // Main reference
        public MainViewController mainViewController = null;

        // Location manager
        public LocationManager locationManager;

        public StructureDto structure;

        public Repository repository { get; set; }

        // class-level declarations
        UIWindow window;
        public override UIWindow Window
        {
            get;
            set;
        }

        private SQLite.SQLiteConnection conn;

        public static AppDelegate current { get; private set; }

        //public Repository repository { get; set; }
        public CxHttpClient client { get; private set; }

        //Connection conn;

        public static UIViewController initialViewController;

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            current = this;

            window = new UIWindow(UIScreen.MainScreen.Bounds);
            client = new CxHttpClient();
            SetupCustomNavigationBar();
            SetupLocalData();

            SetupDatabase();

            if (UserUtil.settings.format == CoordinateFormat.Undefined)
                UserUtil.settings.format = CoordinateFormat.DDM;

            if (deleteDatabaseOnStartup)
            {
                repository.DeleteAllTables();
            }

            GetVersion();
            SetupServicesIfNeeded();

            var textAttributes = new UITextAttributes();
            textAttributes.TextColor = ColorConst.snlaBlue;
            textAttributes.Font = FontConst.fontMedium;
            UIBarButtonItem.Appearance.SetTitleTextAttributes(textAttributes, UIControlState.Normal);
            UINavigationBar.Appearance.SetTitleTextAttributes(textAttributes);

            StartAppCenter();

            return true;
        }

        private void StartAppCenter()
        {
            AppCenter.Start("48ab726a-46ee-4762-ad50-51a2dcc928b4", typeof(Analytics), typeof(Crashes));
        }

        private void GetVersion()
        {
            NSObject thisVersionObject = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString");
            version = thisVersionObject.ToString();
        }

        private void SetupServicesIfNeeded()
        {
            if (servcicesInitialized == false)
            {
                noNetworkString = LangUtil.Get("Error.NoResponse");

                hjertestarterService = new HjertestarterService(client, repository, UserUtil.settings, version, os);

                regEmailSMSService = new RegEmailSMSService(client, repository, UserUtil.settings, version, os);

                coordinateService = new CoordinateService();
                coordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_North, LangUtil.Get("Element.North.Text"));
                coordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_South, LangUtil.Get("Element.South.Text"));
                coordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_East, LangUtil.Get("Element.East.Text"));
                coordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_West, LangUtil.Get("Element.West.Text"));
                coordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_Degrees, LangUtil.Get("Element.Degrees.Text"));
                coordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_Minutes, LangUtil.Get("Element.Minutes.Text"));
                coordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_Seconds, LangUtil.Get("Element.Seconds.Text"));

                servcicesInitialized = true;
            }
        }

        void SetupLocalData()
        {
            SetupSectionsWithElements();

            structure.currentStructureSectionId = 0;
        }

        private void SetupDatabase()
        {
            var sqliteFilename = "HenspeDB.db";
            // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
            // (they don't want non-user-generated data in Documents)
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "../Library/"); // Library folder
            string path = Path.Combine(libraryPath, sqliteFilename);
            conn = new SQLite.SQLiteConnection(path);
            repository = new Repository(conn);
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
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Position, LangUtil.Get("Structure.EksaktPosisjon.Posisjon"), "ic_e_posisjon.svg", 0.8f);
			structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Address, LangUtil.Get("Structure.EksaktPosisjon.Adresse"), "ic_e_adresse.svg", 0.8f);
			structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.EksaktPosisjon.Oppmotested"), "ic_e_oppmotested.svg", 1.0f);
			structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.EksaktPosisjon.Ankomst"), "ic_e_ankomst.svg", 0.6f);
			structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.EksaktPosisjon.Avreise"), "ic_e_avreise.svg", 0.6f);

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
                Font = FontConst.fontHeadingTable,
                TextColor = ColorConst.snlaText
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

			InvokeOnMainThread(delegate
            {
        	    NSNotificationCenter.DefaultCenter.PostNotificationName(EventConst.appActivated, this);
            });
        }

        #region permissions
        public bool IsOnboardingNeeded()
        {
            bool isLocationRightsNeeded = IsLocationRightsNeeded();

            if (isLocationRightsNeeded)
                return true;
            else
                return false;
        }

        public bool IsLocationRightsNeeded()
        {
            if (locationManager.HasLocationPermission())
                return false;
            else
                return true;
        }
        #endregion

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