using System;
using UIKit;
using CoreLocation;
using Henspe.Core.Storage;
using Foundation;
using System.IO;
using Henspe.Core.Communication;
using System.Net;
using Henspe.Core.Const;
using Henspe.iOS.Util;
using System.Timers;
using Henspe.Core.Model;
using System.Linq;
using Henspe.Core.Util;
using Henspe.Core;
using System.Threading.Tasks;
using Henspe.iOS.Const;
using Henspe.iOS.Communication;
using Henspe.iOS.AppModel;
using System.Collections.Generic;
using Henspe.Core.Model.Dto;
using static Henspe.Core.Model.Dto.StructureSectionDto;

namespace Henspe.iOS
{
    // Enterprise: no.snla-system.estroke
    // Debug: com.norskluftambulanse.testapp
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        public string mode = ModeConst.test;

        public string prodUrlString = "https://snla-apps.no/apps/sms/";
        public string testUrlTest = "https://snla-apps.no/apps/smstest/";
        public string plistFile = "Henspe.plist";

        private AppVersionData appVersionData = new AppVersionData();

        private float version;

        public bool askedIfUserWantNewVersion = false;
        public bool appResetOccured = false;
        public bool appActicatedOccured = false;
        public bool syncInProgress = false;
        public int networkState = NetworkStateConst.noNetwork;

		// Format
        public int coordinateFormat = CoordinateUtil.ddm; // Default coordinate format

		// Flash text
        private string lastNorthText = "";
        private string lastEastText = "";
        private string lastAccuracyLargeText = "";
        private string lastAccuracySmallText = "";

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
        public Repository repository { get; set; }
        public CxHttpClient client { get; private set; }

        Connection conn;

        public static UIViewController initialViewController;

        public StructureDto structure;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // Background update interval in seconds
            //UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval (60.0f * 60.0f); // One hour

            current = this;
            window = new UIWindow(UIScreen.MainScreen.Bounds);

            setupDatabase();
            client = new CxHttpClient();

            NSNotificationCenter.DefaultCenter.AddObserver(new NSString("registerDone"), HandleRegisterDone);
            NSNotificationCenter.DefaultCenter.AddObserver(new NSString("doSync"), HandleDoSync);

            //WaitBeforeDoSync (); // Is called from Activated
            SetupCustomNavigationBar();
            GetVersion();

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
			StructureSectionDto structureHendelse = structure.AddStructureSection(Foundation.NSBundle.MainBundle.LocalizedString("Structure.Hendelse.Header", null), "ic_h.png");
			structureHendelse.AddStructureElement(StructureElementDto.ElementType.Normal, Foundation.NSBundle.MainBundle.LocalizedString("Structure.Hendelse.Trafikk", null), "ic_h_trafikk.png", 1.0f);
			structureHendelse.AddStructureElement(StructureElementDto.ElementType.Normal, Foundation.NSBundle.MainBundle.LocalizedString("Structure.Hendelse.Brann", null), "ic_h_brann.png", 0.8f);
            
			// Eksakt posisjon
			StructureSectionDto structureEksaktPosisjon = structure.AddStructureSection(Foundation.NSBundle.MainBundle.LocalizedString("Structure.EksaktPosisjon.Header", null), "ic_e.png");
			structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Position, Foundation.NSBundle.MainBundle.LocalizedString("Structure.EksaktPosisjon.Posisjon", null), "ic_e_posisjon.png", 0.8f);
			structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Address, Foundation.NSBundle.MainBundle.LocalizedString("Structure.EksaktPosisjon.Adresse", null), "ic_e_adresse.png", 0.8f);
			structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, Foundation.NSBundle.MainBundle.LocalizedString("Structure.EksaktPosisjon.Oppmotested", null), "ic_e_oppmotested.png", 1.0f);
			structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, Foundation.NSBundle.MainBundle.LocalizedString("Structure.EksaktPosisjon.Ankomst", null), "ic_e_ankomst.png", 0.6f);
			structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, Foundation.NSBundle.MainBundle.LocalizedString("Structure.EksaktPosisjon.Avreise", null), "ic_e_avreise.png", 0.6f);

            // Nivå
			StructureSectionDto structureNiva = structure.AddStructureSection(Foundation.NSBundle.MainBundle.LocalizedString("Structure.Niva.Header", null), "ic_n.png");
			structureNiva.AddStructureElement(StructureElementDto.ElementType.Normal, Foundation.NSBundle.MainBundle.LocalizedString("Structure.Niva.Type", null), "ic_n_begrenset.png", 0.5f);
            
			// Sikkerhet
			StructureSectionDto structureSikkerhet = structure.AddStructureSection(Foundation.NSBundle.MainBundle.LocalizedString("Structure.Sikkerhet.Header", null), "ic_s.png");
			structureSikkerhet.AddStructureElement(StructureElementDto.ElementType.Normal, Foundation.NSBundle.MainBundle.LocalizedString("Structure.Sikkerhet.Farer", null), "ic_s_farer.png", 0.8f);
			structureSikkerhet.AddStructureElement(StructureElementDto.ElementType.Normal, Foundation.NSBundle.MainBundle.LocalizedString("Structure.Sikkerhet.Brann", null), "ic_s_brann.png", 0.8f);
			structureSikkerhet.AddStructureElement(StructureElementDto.ElementType.Normal, Foundation.NSBundle.MainBundle.LocalizedString("Structure.Sikkerhet.Sikkerhet", null), "ic_s_sikkerhet.png", 0.8f);

            // Pasienter
			StructureSectionDto structurePasienter = structure.AddStructureSection(Foundation.NSBundle.MainBundle.LocalizedString("Structure.Pasienter.Header", null), "ic_p.png");
			structurePasienter.AddStructureElement(StructureElementDto.ElementType.Normal, Foundation.NSBundle.MainBundle.LocalizedString("Structure.Pasienter.Antall", null), "ic_p_pasienter.png", 0.9f);
			structurePasienter.AddStructureElement(StructureElementDto.ElementType.Normal, Foundation.NSBundle.MainBundle.LocalizedString("Structure.Pasienter.Type", null), "ic_p_type.png", 0.7f);
			structurePasienter.AddStructureElement(StructureElementDto.ElementType.Normal, Foundation.NSBundle.MainBundle.LocalizedString("Structure.Pasienter.Omfang", null), "ic_p_skademekanikk.png", 0.8f);

            // Evakuering
			StructureSectionDto structureEvakuering = structure.AddStructureSection(Foundation.NSBundle.MainBundle.LocalizedString("Structure.Evakuering.Header", null), "ic_e.png");
			structureEvakuering.AddStructureElement(StructureElementDto.ElementType.Normal, Foundation.NSBundle.MainBundle.LocalizedString("Structure.Evakuering.Flaskehalser", null), "ic_e_flaskehalser.png", 0.7f);
			structureEvakuering.AddStructureElement(StructureElementDto.ElementType.Normal, Foundation.NSBundle.MainBundle.LocalizedString("Structure.Evakuering.Kjeder", null), "ic_e_evakuering.png", 0.7f);
			structureEvakuering.AddStructureElement(StructureElementDto.ElementType.Normal, Foundation.NSBundle.MainBundle.LocalizedString("Structure.Evakuering.Rett", null), "ic_e_rett.png", 0.7f);
        }

        private void GetVersion()
        {
            NSObject thisVersionObject = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString");
            string thisVersionString = thisVersionObject.ToString();
            version = ConvertUtil.ConvertStringToFloat(thisVersionString);
            Console.WriteLine("UpdateMechanism-GetVersion. version: " + version);
        }

        public void HandleRegisterDone(NSNotification notification)
        {
            DoSyncIfLocalDataOld(false);
        }

        public void HandleDoSync(NSNotification notification)
        {
            DoSyncIfLocalDataOld(false);
        }

        private void SetUpGoogleAnalytics()
        {
            /*
            // We use NSUserDefaults to store a bool value if we are tracking the user or not 
            var optionsDict = NSDictionary.FromObjectAndKey (new NSString ("YES"), new NSString (AllowTrackingKey));
            NSUserDefaults.StandardUserDefaults.RegisterDefaults (optionsDict);

            // User must be able to opt out of tracking
            GAI.SharedInstance.OptOut = !NSUserDefaults.StandardUserDefaults.BoolForKey (AllowTrackingKey);

            // Initialize Google Analytics with a 5-second dispatch interval (Use a higher value when in production). There is a
            // tradeoff between battery usage and timely dispatch.
            GAI.SharedInstance.DispatchInterval = 5;
            GAI.SharedInstance.TrackUncaughtExceptions = true;

            //Tracker = GAI.SharedInstance.GetTracker ("CuteAnimals", TrackingId);
            //Tracker = GAI.SharedInstance.GetTracker ("LATSamband", TrackingId);
            //Tracker = GAI.SharedInstance.GetTracker (TrackingId);
            Tracker = GAI.SharedInstance.GetTracker ("Computas", TrackingId);
            */
        }

        private void SetupCustomNavigationBar()
        {
            UINavigationBar.Appearance.TintColor = ColorConst.textColor;

            UITextAttributes attributes = new UITextAttributes
            {
                Font = UIFont.SystemFontOfSize(16, UIFontWeight.Medium),
                TextColor = ColorConst.textColor
            }; // ForegroundColor

            // White top
            //UIApplication.SharedApplication.SetStatusBarStyle (UIStatusBarStyle.LightContent, false);

            UINavigationBar.Appearance.SetTitleTextAttributes(attributes);
        }

        private void setupDatabase()
        {
            var sqliteFilename = "Henspe.db3";
            // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
            // (they don't want non-user-generated data in Documents)
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "../Library/"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);
            conn = new Connection(path);
            repository = new Repository(conn);
        }

        // Standard methods

        public override void OnActivated(UIApplication application)
        {
            askedIfUserWantNewVersion = false;

            Console.WriteLine("UpdateMechanism-OnActivated. Not waiting for new version install");
            // Sync after sleep
            appActicatedOccured = true;

            NSNotificationCenter.DefaultCenter.PostNotificationName("appActivated", this);
			CheckForAppSettings();
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

        private void SetupServicesIfNeeded()
        {
            /*
            if (syncHallo == null && UserUtil.Credentials.IsAuthenticated && UserUtil.Credentials.Key != 0)
            {
                syncHallo = new SyncHallo(client, repository, UserUtil.Credentials);
                syncBasis = new SyncBasis(client, repository, UserUtil.Credentials);
                syncTelefonliste = new SyncTelefonliste(client, repository, UserUtil.Credentials);
            }
            */
        }

        /*
         * Server communication
         */
        public void DoSyncIfLocalDataOld(bool isInBackgroundMode)
        {
            // Dont sync if not authenticated
            //if (UserUtil.credentials.IsAuthenticated == false)
            //    return;

            // Dont sync if sync already in progress
            if (AppDelegate.current.syncInProgress)
                return;

            SetupServicesIfNeeded();

            /*
            // Check if there is an hour since last time
            if (AppDelegate.current.repository.GetItemList<Telefon> ().ToList().Count > 0)
            {
                if (UserUtil.Credentials.Server_Local_Millisec_Diff != 0)
                {
                    int updateIntervarMinutes = UserUtil.Credentials.Intervall;
                    updateIntervarMinutes = updateIntervarMinutes - 5;

                    // Dont sync if local data is less than interval - 5 minutes old
                    double updateInterval = 1000 * 60 * updateIntervarMinutes;

                    DateTime lastSyncServerDateTime = DateUtil.ConvertDateStringToDateTime (String.Format("{0:00000000}", UserUtil.Credentials.Server_dato) + " " + String.Format("{0:000000}", UserUtil.Credentials.Server_kl), "yyyyMMdd HHmmss");
                    DateTime nowSmartphoneDateTime = DateTime.Now;
                    TimeSpan timeSpan = lastSyncServerDateTime - nowSmartphoneDateTime;

                    double millisecondsAdjusted = timeSpan.TotalMilliseconds + UserUtil.Credentials.Server_Local_Millisec_Diff;
                    double millisecondsAdjustedAbs = Math.Abs (millisecondsAdjusted);
                    if(millisecondsAdjustedAbs < updateInterval)
                    {
                        return;
                    }
                }
            }
            */

            SyncInProgress(true);
            PerformServerCalls(isInBackgroundMode);
            //if (isInBackgroundMode == false)
            //    WaitBeforeDoSync();
        }

        private void SyncInProgress(bool inProgress)
        {
            AppDelegate.current.syncInProgress = inProgress;
            InvokeOnMainThread(delegate
            {
                NSNotificationCenter.DefaultCenter.PostNotificationName("syncStateChanged", this);
            });
        }

        public async void PerformServerCalls(bool isInBackroundMode)
        {
            NetworkStatus internetStatus = NetUtil.InternetConnectionStatus();
            if (NetUtil.IsHostReachable())
            {
                // Put alternative content/message here
                if (internetStatus == NetworkStatus.ReachableViaWiFiNetwork)
                    AppDelegate.current.networkState = NetworkStateConst.wifiNetworkHostReached;
                else if (internetStatus == NetworkStatus.ReachableViaCarrierDataNetwork)
                    AppDelegate.current.networkState = NetworkStateConst.mobileNetworkOnlyHostReached;
                else
                    AppDelegate.current.networkState = NetworkStateConst.noNetwork;

                if (isInBackroundMode == false)
                {
                    InvokeOnMainThread(delegate
                    {
                        NSNotificationCenter.DefaultCenter.PostNotificationName("syncStateChanged", this);
                    });
                }
            }
            else
            {
                // Put Internet Required Code here
                if (internetStatus == NetworkStatus.ReachableViaWiFiNetwork)
                    AppDelegate.current.networkState = NetworkStateConst.wifiNetworkButCouldNotReachHost;
                else if (internetStatus == NetworkStatus.ReachableViaCarrierDataNetwork)
                    AppDelegate.current.networkState = NetworkStateConst.mobileNetworkOnlyButCouldNotReachHost;
                else
                    AppDelegate.current.networkState = NetworkStateConst.noNetwork;

                if (isInBackroundMode == false)
                {
                    InvokeOnMainThread(delegate
                    {
                        NSNotificationCenter.DefaultCenter.PostNotificationName("syncStateChanged", this);
                    });
                }

                return;
            }

			if (UserUtil.credentials.instructionsFinished == false)
            {
                // Not authenticated
                SyncInProgress(false);
                return;
            }

            try
            {
                //DebugUtil.ShowDebugTime("Calling syncHallo");
                //Task<bool> halloResultTask = syncHallo.Hallo();
                //bool halloResult = await halloResultTask.ConfigureAwait(false);

                //// Basis
                //DateTime prevBasisSyncDateTime = DateUtil.ConvertDateStringToDateTime(String.Format("{0:00000000}", UserUtil.Credentials.LastSync_basis_dato) + " " + String.Format("{0:000000}", UserUtil.Credentials.LastSync_basis_kl), "yyyyMMdd HHmmss");
                //DateTime basisDateTime = DateUtil.ConvertDateStringToDateTime(String.Format("{0:00000000}", UserUtil.Credentials.Endr_basis_dato) + " " + String.Format("{0:000000}", UserUtil.Credentials.Endr_basis_kl), "yyyyMMdd HHmmss");
                //TimeSpan timeSinceBasisAndLastSync = basisDateTime - prevBasisSyncDateTime;
                //if (timeSinceBasisAndLastSync.TotalSeconds > 0)
                //{
                //    DebugUtil.ShowDebugTime("Calling syncBasis");
                //    Task<bool> basisResultTask = syncBasis.Basis();
                //    bool basisResult = await basisResultTask.ConfigureAwait(false);
                //    UserUtil.Credentials.LastSync_basis_dato = UserUtil.Credentials.Endr_basis_dato;
                //    UserUtil.Credentials.LastSync_basis_kl = UserUtil.Credentials.Endr_basis_kl;
                //}

                //// Tlf liste (ansatte and telefonliste)
                //DateTime prevAnsSyncDateTime = DateUtil.ConvertDateStringToDateTime(String.Format("{0:00000000}", UserUtil.Credentials.LastSync_ans_dato) + " " + String.Format("{0:000000}", UserUtil.Credentials.LastSync_ans_kl), "yyyyMMdd HHmmss");
                //DateTime ansDateTime = DateUtil.ConvertDateStringToDateTime(String.Format("{0:00000000}", UserUtil.Credentials.Endr_ans_dato) + " " + String.Format("{0:000000}", UserUtil.Credentials.Endr_ans_kl), "yyyyMMdd HHmmss");
                //TimeSpan timeSinceAnsAndLastSync = ansDateTime - prevAnsSyncDateTime;

                //DateTime prevTlfSyncDateTime = DateUtil.ConvertDateStringToDateTime(String.Format("{0:00000000}", UserUtil.Credentials.LastSync_tlf_dato) + " " + String.Format("{0:000000}", UserUtil.Credentials.LastSync_tlf_kl), "yyyyMMdd HHmmss");
                //DateTime tlfDateTime = DateUtil.ConvertDateStringToDateTime(String.Format("{0:00000000}", UserUtil.Credentials.Endr_tlf_dato) + " " + String.Format("{0:000000}", UserUtil.Credentials.Endr_tlf_kl), "yyyyMMdd HHmmss");
                //TimeSpan timeSinceTlfAndLastSync = tlfDateTime - prevTlfSyncDateTime;

                //string action = null;

                //if (timeSinceAnsAndLastSync.TotalSeconds > 0 && timeSinceTlfAndLastSync.TotalSeconds > 0)
                //    action = "alle";
                //else if (timeSinceAnsAndLastSync.TotalSeconds > 0)
                //    action = "ans";
                //else if (timeSinceTlfAndLastSync.TotalSeconds > 0)
                //    action = "tlf";

                //if (action != null)
                //{
                //    DebugUtil.ShowDebugTime("Calling syncTelefonliste");
                //    Task<bool> telefonlisteResultTask = syncTelefonliste.Telefonliste(action);
                //    bool telefonlisteResult = await telefonlisteResultTask.ConfigureAwait(false);

                //    if (action == "alle" || action == "ans")
                //    {
                //        UserUtil.Credentials.LastSync_ans_dato = UserUtil.Credentials.Endr_ans_dato;
                //        UserUtil.Credentials.LastSync_ans_kl = UserUtil.Credentials.Endr_ans_kl;
                //    }

                //    if (action == "alle" || action == "tlf")
                //    {
                //        UserUtil.Credentials.LastSync_tlf_dato = UserUtil.Credentials.Endr_tlf_dato;
                //        UserUtil.Credentials.LastSync_tlf_kl = UserUtil.Credentials.Endr_tlf_kl;
                //    }
                //}

                //DebugUtil.ShowDebugTime("Finished");
                //SyncInProgress(false);

                //InvokeOnMainThread(delegate
                //{
                //    NSNotificationCenter.DefaultCenter.PostNotificationName("syncStateChanged", this);
                //});
            }
            catch (TaskCanceledException e)
            {
                BugtrackUtil.SendBugtrack("AppDelegate TaskCanceledException", e, "No json in this case", client, version, "Henspe app user");
                return;
            }
            catch (Exception e)
            {
                SyncInProgress(false);
                if (e.Message != "Error: NameResolutionFailure")
                {
                    if (isInBackroundMode == false)
                    {
                        InvokeOnMainThread(delegate
                        {
                            ErrorUtil.ShowError(e.Message);
                        });
                    }

                    BugtrackUtil.SendBugtrack("AppDelegate error", e, "No json in this case", client, version, "Henspe app user");
                }
            }
        }

        /*
         * New app version
         */
        private void CheckNewAppVersionAvailable()
        {
            if (askedIfUserWantNewVersion == false)
            {
                Console.WriteLine("UpdateMechanism-CheckNewAppVersionAvailable. Getting appversion");
                appVersionData.GetIphoneVersion(OnGetCurrentAppVersionSuccess, OnGetCurrentAppVersionFault);
            }
        }

        public void OnGetCurrentAppVersionSuccess(String appVersion)
        {
            string serverVersionNumberString = appVersion;
            float serverVersionNumberFloat = ConvertUtil.ConvertStringToFloat(serverVersionNumberString);

            if (serverVersionNumberFloat > version)
            {
                askedIfUserWantNewVersion = true;
                Console.WriteLine("UpdateMechanism-CheckNewAppVersionAvailable. New version found");

                InvokeOnMainThread(delegate
                {
                    UIAlertView alert = new UIAlertView(Foundation.NSBundle.MainBundle.LocalizedString("Alert.NewVersion.Title", null),
                        Foundation.NSBundle.MainBundle.LocalizedString("Alert.NewVersion.Message1", null) + " " +
                        serverVersionNumberFloat + " " + Foundation.NSBundle.MainBundle.LocalizedString("Alert.NewVersion.Message2", null) + " " +
                        version + ". " + Foundation.NSBundle.MainBundle.LocalizedString("Alert.NewVersion.Message3", null),
                        null,
                        Foundation.NSBundle.MainBundle.LocalizedString("Alert.Yes", null),
                        new string[] { Foundation.NSBundle.MainBundle.LocalizedString("Alert.No", null) });

                    alert.Clicked += (s, b) =>
                    {
                        if (b.ButtonIndex == 0)
                        {
                            Console.WriteLine("UpdateMechanism-CheckNewAppVersionAvailable. New version found and user want to install it");

                            // Ja chosen
                            string urlString = "";

                            /*
                            if(mode == ModeConst.prod)
                                urlString = "itms-services://?action=download-manifest&url=https://snla-apps.no/apps/sms/Henspe.plist";
                            else
                                urlString = "itms-services://?action=download-manifest&url=https://snla-apps.no/apps/smstest/Henspe.plist";
                            */

                            if (mode == ModeConst.prod)
                                urlString = prodUrlString;
                            else
                                urlString = testUrlTest;

                            var url = NSUrl.FromString(urlString);
                            UIApplication.SharedApplication.OpenUrl(url);
                        }
                        else
                        {
                            Console.WriteLine("UpdateMechanism-CheckNewAppVersionAvailable. New version found but user does not want to install it");
                        }
                    };

                    alert.Show();
                });
            }
        }

        public void OnGetCurrentAppVersionFault(String reason)
        {
            // We accept that it does not work
        }
    }
}