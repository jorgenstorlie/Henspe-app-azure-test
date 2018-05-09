using System;
using UIKit;
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

        //private SyncHallo syncHallo;
        //private SyncBasis syncBasis;
        //private SyncTelefonliste syncTelefonliste;
        private Timer syncTimer = null;

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

            // 1a - Bevissthet
            StructureSectionDto structure1a = structure.AddStructureSection(StepType.schemeStep, "1a", Foundation.NSBundle.MainBundle.LocalizedString("Structure.1a.Name", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.1a.Description", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.1a.AlternateDescription", null), "video1a.mp4");
            structure1a.AddStructureElement(StructureElementDto.ElementType.Single, "0", 0, "Bilde 6-0.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.1a.0.Description", null), null);
            structure1a.AddStructureElement(StructureElementDto.ElementType.Single, "1", 1, "Bilde 6-1.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.1a.1.Description", null), null);
            structure1a.AddStructureElement(StructureElementDto.ElementType.Single, "2", 2, "Bilde 6-2.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.1a.2.Description", null), null);
            structure1a.AddStructureElement(StructureElementDto.ElementType.Single, "3", 3, "Bilde 6-3.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.1a.3.Description", null), null);

            // Iktus
            StructureSectionDto iktus = structure.AddStructureSection(StepType.iktus, null, null, null, null, null);

            // Medicines
            StructureSectionDto medicines = structure.AddStructureSection(StepType.medicines, null, null, null, null, null);

            // 1ba - Orientering Måned
            StructureSectionDto structure1ba = structure.AddStructureSection(StepType.schemeStep, "1ba", Foundation.NSBundle.MainBundle.LocalizedString("Structure.1ba.Name", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.1ba.Description", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.1ba.AlternateDescription", null), "video1ba.mp4");
            structure1ba.AddStructureElement(StructureElementDto.ElementType.Single, "0", 0, "Bilde 9-0.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.1ba.0.Description", null), null);
            structure1ba.AddStructureElement(StructureElementDto.ElementType.Single, "1", 1, "Bilde 9-1.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.1ba.1.Description", null), null);

            // 1bb - Orientering Alder
            StructureSectionDto structure1bb = structure.AddStructureSection(StepType.schemeStep, "1bb", Foundation.NSBundle.MainBundle.LocalizedString("Structure.1bb.Name", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.1bb.Description", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.1bb.AlternateDescription", null), "video1bb.mp4");
            structure1bb.AddStructureElement(StructureElementDto.ElementType.Single, "0", 0, "Bilde 10-0.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.1bb.0.Description", null), null);
            structure1bb.AddStructureElement(StructureElementDto.ElementType.Single, "1", 1, "Bilde 10-1.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.1bb.1.Description", null), null);

            // 9 - Språk og tale
            StructureSectionDto structure9 = structure.AddStructureSection(StepType.schemeStep, "9", Foundation.NSBundle.MainBundle.LocalizedString("Structure.9.Name", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.9.Description", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.9.AlternateDescription", null), "video9.mp4");
            structure9.AddStructureElement(StructureElementDto.ElementType.Single, "0", 0, "Bilde 11-0.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.9.0.Description", null), null);
            structure9.AddStructureElement(StructureElementDto.ElementType.Single, "1", 1, "Bilde 11-1.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.9.1.Description", null), null);
            structure9.AddStructureElement(StructureElementDto.ElementType.Single, "2", 2, "Bilde 11-2.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.9.2.Description", null), null);
            structure9.AddStructureElement(StructureElementDto.ElementType.Single, "3", 3, "Bilde 11-3.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.9.3.Description", null), null);

            // 10 - Snøvlete/utydelig tale
            StructureSectionDto structure10 = structure.AddStructureSection(StepType.schemeStep, "10", Foundation.NSBundle.MainBundle.LocalizedString("Structure.10.Name", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.10.Description", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.10.AlternateDescription", null), "video10.mp4");
            structure10.AddStructureElement(StructureElementDto.ElementType.Single, "0", 0, "Bilde 12-0.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.10.0.Description", null), null);
            structure10.AddStructureElement(StructureElementDto.ElementType.Single, "1", 1, "Bilde 12-1.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.10.1.Description", null), null);
            structure10.AddStructureElement(StructureElementDto.ElementType.Single, "2", 2, "Bilde 12-2.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.10.2.Description", null), null);

            // 1c - Respons på kommando
            StructureSectionDto structure1c = structure.AddStructureSection(StepType.schemeStep, "1c", Foundation.NSBundle.MainBundle.LocalizedString("Structure.1c.Name", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.1c.Description", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.1c.AlternateDescription", null), "video1c.mp4");
            structure1c.AddStructureElement(StructureElementDto.ElementType.Single, "0", 0, "Bilde 13-0.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.1c.0.Description", null), null);
            structure1c.AddStructureElement(StructureElementDto.ElementType.Single, "1a", 1, "Bilde 13-1a.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.1c.1a.Description", null), null);
            structure1c.AddStructureElement(StructureElementDto.ElementType.Single, "1b", 1, "Bilde 13-1b.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.1c.1b.Description", null), null);
            structure1c.AddStructureElement(StructureElementDto.ElementType.Single, "2", 2, "Bilde 13-2.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.1c.2.Description", null), null);

            // N1 - Lyse
            StructureSectionDto structureN1 = structure.AddStructureSection(StepType.schemeStep, "N1", Foundation.NSBundle.MainBundle.LocalizedString("Structure.N1.Name", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.N1.Description", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.N1.AlternateDescription", null), "videoN1.mp4");
            structureN1.AddStructureElement(StructureElementDto.ElementType.Single, "0a", 0, "Bilde 14-0a.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.N1.0a.Description", null), null);
            structureN1.AddStructureElement(StructureElementDto.ElementType.Single, "0b", 0, "Bilde 14-0b.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.N1.0b.Description", null), null);
            structureN1.AddStructureElement(StructureElementDto.ElementType.Single, "0c", 0, "Bilde 14-0c.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.N1.0c.Description", null), null);
            structureN1.AddStructureElement(StructureElementDto.ElementType.Single, "0d", 0, "Bilde 14-0d.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.N1.0d.Description", null), null);

            // 2 - Blikkbevegelse
            StructureSectionDto structure2 = structure.AddStructureSection(StepType.schemeStep, "2", Foundation.NSBundle.MainBundle.LocalizedString("Structure.2.Name", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.2.Description", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.2.AlternateDescription", null), "video2.mp4");
            structure2.AddStructureElement(StructureElementDto.ElementType.Single, "0", 0, "Bilde 15-0.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.2.0.Description", null), null);
            structure2.AddStructureElement(StructureElementDto.ElementType.LeftRight, "1a", 1, "Bilde 15-1ar.png", "Bilde 15-1al.png", Foundation.NSBundle.MainBundle.LocalizedString("Structure.2.1a.Description", null), null);
            structure2.AddStructureElement(StructureElementDto.ElementType.Single, "1b", 1, "Bilde 15-1b.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.2.1b.Description", null), null);
            structure2.AddStructureElement(StructureElementDto.ElementType.Single, "2", 2, "Bilde 15-2.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.2.2.Description", null), null);

            // 3 - Synsfelt
            StructureSectionDto structure3 = structure.AddStructureSection(StepType.schemeStep, "3", Foundation.NSBundle.MainBundle.LocalizedString("Structure.3.Name", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.3.Description", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.3.AlternateDescription", null), "video3.mp4");
            structure3.AddStructureElement(StructureElementDto.ElementType.Single, "0", 0, "Bilde 16-0.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.3.0.Description", null), null);
            structure3.AddStructureElement(StructureElementDto.ElementType.LeftRight, "1", 1, "Bilde 16-1r.png", "Bilde 16-1l.png", Foundation.NSBundle.MainBundle.LocalizedString("Structure.3.1.Description", null), null);
            structure3.AddStructureElement(StructureElementDto.ElementType.LeftRight, "2", 2, "Bilde 16-2r.png", "Bilde 16-2l.png", Foundation.NSBundle.MainBundle.LocalizedString("Structure.3.2.Description", null), null);
            structure3.AddStructureElement(StructureElementDto.ElementType.Single, "3", 3, "Bilde 16-3.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.3.3.Description", null), null);

            // 4 - Ansikt
            StructureSectionDto structure4 = structure.AddStructureSection(StepType.schemeStep, "4", Foundation.NSBundle.MainBundle.LocalizedString("Structure.4.Name", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.4.Description", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.4.AlternateDescription", null), "video4.mp4");
            structure4.AddStructureElement(StructureElementDto.ElementType.Single, "0", 0, "Bilde 17-0.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.4.0.Description", null), null);
            structure4.AddStructureElement(StructureElementDto.ElementType.LeftRight, "1", 1, "Bilde 17-1r.png", "Bilde 17-1l.png", Foundation.NSBundle.MainBundle.LocalizedString("Structure.4.1.Description", null), null);
            structure4.AddStructureElement(StructureElementDto.ElementType.LeftRight, "2", 2, "Bilde 17-2r.png", "Bilde 17-2l.png", Foundation.NSBundle.MainBundle.LocalizedString("Structure.4.2.Description", null), null);
            structure4.AddStructureElement(StructureElementDto.ElementType.LeftRight, "3", 3, "Bilde 17-3r.png", "Bilde 17-3l.png", Foundation.NSBundle.MainBundle.LocalizedString("Structure.4.3.Description", null), null);

            // 5 - Kraft i arm
            StructureSectionDto structure5 = structure.AddStructureSection(StepType.schemeStep, "5", Foundation.NSBundle.MainBundle.LocalizedString("Structure.5.Name", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.5.Description", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.5.AlternateDescription", null), "video5.mp4");
            structure5.AddStructureElement(StructureElementDto.ElementType.LeftRight, "0", 0, "Bilde 18-0r.png", "Bilde 18-0l.png", Foundation.NSBundle.MainBundle.LocalizedString("Structure.5.0.Description", null), null);
            structure5.AddStructureElement(StructureElementDto.ElementType.LeftRight, "1", 1, "Bilde 18-1r.png", "Bilde 18-1l.png", Foundation.NSBundle.MainBundle.LocalizedString("Structure.5.1.Description", null), null);
            structure5.AddStructureElement(StructureElementDto.ElementType.LeftRight, "2", 2, "Bilde 18-2r.png", "Bilde 18-2l.png", Foundation.NSBundle.MainBundle.LocalizedString("Structure.5.2.Description", null), null);
            structure5.AddStructureElement(StructureElementDto.ElementType.LeftRight, "3", 3, "Bilde 18-3r.png", "Bilde 18-3l.png", Foundation.NSBundle.MainBundle.LocalizedString("Structure.5.3.Description", null), null);
            structure5.AddStructureElement(StructureElementDto.ElementType.Single, "4", 4, "Bilde 18-4.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.5.4.Description", null), null);

            // 6 - Kraft i ben
            StructureSectionDto structure6 = structure.AddStructureSection(StepType.schemeStep, "6", Foundation.NSBundle.MainBundle.LocalizedString("Structure.6.Name", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.6.Description", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.6.AlternateDescription", null), "video6.mp4");
            structure6.AddStructureElement(StructureElementDto.ElementType.LeftRight, "0", 0, "Bilde 19-0r.png", "Bilde 19-0l.png", Foundation.NSBundle.MainBundle.LocalizedString("Structure.6.0.Description", null), null);
            structure6.AddStructureElement(StructureElementDto.ElementType.LeftRight, "1", 1, "Bilde 19-1r.png", "Bilde 19-1l.png", Foundation.NSBundle.MainBundle.LocalizedString("Structure.6.1.Description", null), null);
            structure6.AddStructureElement(StructureElementDto.ElementType.LeftRight, "2", 2, "Bilde 19-2r.png", "Bilde 19-2l.png", Foundation.NSBundle.MainBundle.LocalizedString("Structure.6.2.Description", null), null);
            structure6.AddStructureElement(StructureElementDto.ElementType.LeftRight, "3", 3, "Bilde 19-3r.png", "Bilde 19-3l.png", Foundation.NSBundle.MainBundle.LocalizedString("Structure.6.3.Description", null), null);
            structure6.AddStructureElement(StructureElementDto.ElementType.Single, "4", 4, "Bilde 19-4.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.6.4.Description", null), null);

            // 8a - Hudfølelse arm
            StructureSectionDto structure8a = structure.AddStructureSection(StepType.schemeStep, "8a", Foundation.NSBundle.MainBundle.LocalizedString("Structure.8a.Name", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.8a.Description", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.8a.AlternateDescription", null), "video8a.mp4");
            structure8a.AddStructureElement(StructureElementDto.ElementType.Single, "0", 0, "Bilde 20-0.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.8a.0.Description", null), null);
            structure8a.AddStructureElement(StructureElementDto.ElementType.Single, "1", 1, "Bilde 20-1.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.8a.1.Description", null), null);

            // 8b - Hudfølelse ben
            StructureSectionDto structure8b = structure.AddStructureSection(StepType.schemeStep, "8b", Foundation.NSBundle.MainBundle.LocalizedString("Structure.8b.Name", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.8b.Description", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.8b.AlternateDescription", null), "video8b.mp4");
            structure8b.AddStructureElement(StructureElementDto.ElementType.Single, "0", 0, "Bilde 21-0.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.8b.0.Description", null), null);
            structure8b.AddStructureElement(StructureElementDto.ElementType.Single, "1", 1, "Bilde 21-1.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.8b.1.Description", null), null);

            // 7a - Ataksi / hakkete bevegelse - arm
            StructureSectionDto structure7a = structure.AddStructureSection(StepType.schemeStep, "7a", Foundation.NSBundle.MainBundle.LocalizedString("Structure.7a.Name", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.7a.Description", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.7a.AlternateDescription", null), "video7a.mp4");
            structure7a.AddStructureElement(StructureElementDto.ElementType.Single, "0", 0, "Bilde 22-0.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.7a.0.Description", null), null);
            structure7a.AddStructureElement(StructureElementDto.ElementType.Single, "1", 1, "Bilde 22-1.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.7a.1.Description", null), null);

            // 7b - Ataksi / hakkete bevegelse - ben
            StructureSectionDto structure7b = structure.AddStructureSection(StepType.schemeStep, "7b", Foundation.NSBundle.MainBundle.LocalizedString("Structure.7b.Name", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.7b.Description", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.7b.AlternateDescription", null), "video7b.mp4");
            structure7b.AddStructureElement(StructureElementDto.ElementType.Single, "0", 0, "Bilde 23-0.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.7b.0.Description", null), null);
            structure7b.AddStructureElement(StructureElementDto.ElementType.Single, "1", 1, "Bilde 23-1.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.7b.1.Description", null), null);

            // 11a - Oppmerksomhet/Neglekt - syn
            StructureSectionDto structure11a = structure.AddStructureSection(StepType.schemeStep, "11a", Foundation.NSBundle.MainBundle.LocalizedString("Structure.11a.Name", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.11a.Description", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.11a.AlternateDescription", null), "video11a.mp4");
            structure11a.AddStructureElement(StructureElementDto.ElementType.Single, "0", 0, "Bilde 24-0.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.11a.0.Description", null), null);
            structure11a.AddStructureElement(StructureElementDto.ElementType.Single, "1", 1, "Bilde 24-1.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.11a.1.Description", null), null);

            // 11b - Oppmerksomhet/Neglekt - følelse
            StructureSectionDto structure11b = structure.AddStructureSection(StepType.schemeStep, "11b", Foundation.NSBundle.MainBundle.LocalizedString("Structure.11b.Name", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.11b.Description", null), Foundation.NSBundle.MainBundle.LocalizedString("Structure.11b.AlternateDescription", null), "video11b.mp4");
            structure11b.AddStructureElement(StructureElementDto.ElementType.Single, "0", 0, "Bilde 25-0.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.11b.0.Description", null), null);
            structure11b.AddStructureElement(StructureElementDto.ElementType.Single, "1", 1, "Bilde 25-1.png", null, Foundation.NSBundle.MainBundle.LocalizedString("Structure.11b.1.Description", null), null);

            // Tilstand
            StructureSectionDto tilstand = structure.AddStructureSection(StepType.state, null, null, null, null, null);
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
            CheckForDeleteAllSetting();
            //WaitShortTimeBeforeDoSync();
        }

        private void CheckForDeleteAllSetting()
        {
            bool deleteAllDataValue = NSUserDefaults.StandardUserDefaults.BoolForKey("settingsDeleteAllData");
            if (deleteAllDataValue)
            {
                // Delete all settings here
                UIAlertView alert = new UIAlertView(Foundation.NSBundle.MainBundle.LocalizedString("AppDelegate.Alert.DeleteAll.Title", null),
                    Foundation.NSBundle.MainBundle.LocalizedString("AppDelegate.Alert.DeleteAll.Message", null),
                    null,
                    Foundation.NSBundle.MainBundle.LocalizedString("Alert.Yes", null),
                    new string[] { Foundation.NSBundle.MainBundle.LocalizedString("Alert.No", null) });

                alert.Clicked += (s, b) =>
                {
                    if (b.ButtonIndex == 0)
                    {
                        // Yes chosen
                        AppDelegate.current.repository.DeleteAllTables(); // Deletes all data in database
                        NSUserDefaults.StandardUserDefaults.SetBool(false, "settingsDeleteAllData");
                        NSUserDefaults.StandardUserDefaults.Synchronize();
                        UserUtil.Reset();

                        AppDelegate.current.appResetOccured = true;
                    }
                };

                alert.Show();
            }
        }

        /*private void WaitShortTimeBeforeDoSync()
        {
            CheckNewAppVersionAvailable();

            if (syncTimer != null)
                syncTimer.Stop();

            double delayInterval = 1000 * 3; // Seconds

            syncTimer = new Timer(delayInterval);
            syncTimer.Elapsed += OnTimerElapsed;
            syncTimer.Start();
        }

        private void WaitBeforeDoSync()
        {
            CheckNewAppVersionAvailable();

            if (syncTimer != null)
                syncTimer.Stop();

            int intervalMinutes = UserUtil.Credentials.Intervall;
            if (intervalMinutes == 0)
                intervalMinutes = 60;

            double delayInterval = 1000 * 60 * intervalMinutes; // One hour

            syncTimer = new Timer(delayInterval);
            syncTimer.Elapsed += OnTimerElapsed;
            syncTimer.Start();
        }

        private void OnTimerElapsed(object o, EventArgs e)
        {
            DoSyncIfLocalDataOld(false);
        }*/

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
            // DOnt sync if not authenticated
            if (UserUtil.Credentials.IsAuthenticated == false || UserUtil.Credentials.Key == 0)
                return;

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

            if (UserUtil.Credentials.IsAuthenticated == false)
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
                BugtrackUtil.SendBugtrack("AppDelegate TaskCanceledException", e, "No json in this case", client, version, UserUtil.Credentials.Navn);
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

                    BugtrackUtil.SendBugtrack("AppDelegate error", e, "No json in this case", client, version, UserUtil.Credentials.Navn);
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