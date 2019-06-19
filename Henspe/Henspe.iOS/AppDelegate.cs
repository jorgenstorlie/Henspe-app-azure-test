using System;
using UIKit;
using Foundation;
using Henspe.Core.Communication;
using Henspe.iOS.Const;
using Henspe.Core.Model.Dto;
using Henspe.Core.Services;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using SNLA.Core.Util;
using SNLA.iOS.Util;
using SNLA.Core.Services;

namespace Henspe.iOS
{
    // AppStore: no.norskluftambulanse.henspe
    // Enterprise: no.snla-system.henspe
    // Debug: no.norskluftambulanse.testapp
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        public bool appActicatedOccured;

        private string noNetworkString;
        public bool servcicesInitialized = false;
        public CoordinateService coordinateService;
        public RegEmailSMSService regEmailSMSService;

		// Main reference
		public MainViewController mainViewController = null;

        // Location manager
        public LocationManager locationManager;

        public StructureDto structure;

        // class-level declarations
        UIWindow window;
        public override UIWindow Window
        {
            get;
            set;
        }

        public static AppDelegate current { get; private set; }
        public ApplicationService ApplicationService;

        public static UIViewController initialViewController;

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            current = this;

            window = new UIWindow(UIScreen.MainScreen.Bounds);

			ApplicationService = new IOSApplicationService();

			SetupCustomNavigationBar();
            SetupSectionsWithElements();

            if (UserUtil.Current.format == CoordinateFormat.Undefined)
                UserUtil.Current.format = CoordinateFormat.DDM;

            SetupServicesIfNeeded();

            var textAttributes = new UITextAttributes();
            textAttributes.TextColor = ColorConst.snlaBlue;
            textAttributes.Font = FontConst.fontMedium;
            UIBarButtonItem.Appearance.SetTitleTextAttributes(textAttributes, UIControlState.Normal);
            UINavigationBar.Appearance.SetTitleTextAttributes(textAttributes);

            StartAppCenter();

            return true;
        }

		public void SetupCoordinates()
		{
			
		}

        private void StartAppCenter()
        {
            AppCenter.Start("48ab726a-46ee-4762-ad50-51a2dcc928b4", typeof(Analytics), typeof(Crashes));
        }

        private void SetupServicesIfNeeded()
        {
            if (servcicesInitialized == false)
            {
                noNetworkString = LangUtil.Get("Error.NoResponse");

                regEmailSMSService = new RegEmailSMSService();

                coordinateService = new CoordinateService();
                coordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_North,	LangUtil.Get("Element.North.Text"));
                coordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_South,	LangUtil.Get("Element.South.Text"));
                coordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_East,	LangUtil.Get("Element.East.Text"));
                coordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_West,	LangUtil.Get("Element.West.Text"));
                coordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_Degrees, LangUtil.Get("Element.Degrees.Text"));
                coordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_Minutes, LangUtil.Get("Element.Minutes.Text"));
                coordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_Seconds, LangUtil.Get("Element.Seconds.Text"));
                servcicesInitialized = true;
            }
        }

        void SetupSectionsWithElements()
        {
            // Structure that all will be added to
            structure = new StructureDto();

            // Hendelse
            StructureSectionDto structureHendelse = structure.AddStructureSection(LangUtil.Get("Structure.Hendelse.Header"));
            structureHendelse.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Hendelse.Trafikk"), "ic_trafikk.svg");
            structureHendelse.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Hendelse.Brann"), "ic_brann.svg");

            // Eksakt posisjon
            StructureSectionDto structureEksaktPosisjon = structure.AddStructureSection(LangUtil.Get("Structure.EksaktPosisjon.Header"));
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Position, LangUtil.Get("Structure.EksaktPosisjon.Posisjon"), "ic_posisjon.svg");
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Address, LangUtil.Get("Structure.EksaktPosisjon.Adresse"), "ic_adresse.svg");
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.EksaktPosisjon.Oppmotested"), "ic_oppmotested.svg");
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.EksaktPosisjon.Ankomst"), "ic_ankomst.svg");
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.EksaktPosisjon.Avreise"), "ic_avreise.svg");

            // Nivå
            StructureSectionDto structureNiva = structure.AddStructureSection(LangUtil.Get("Structure.Niva.Header"));
            structureNiva.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Niva.1"), "ic_1.svg");
            structureNiva.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Niva.2"), "ic_2.svg");
            structureNiva.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Niva.3"), "ic_3.svg");
            structureNiva.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Niva.QuattroVarsling"), "ic_quattro.svg");

            // Sikkerhet
            StructureSectionDto structureSikkerhet = structure.AddStructureSection(LangUtil.Get("Structure.Sikkerhet.Header"));
            structureSikkerhet.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Sikkerhet.Farer"), "ic_farer.svg");
            structureSikkerhet.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Sikkerhet.Brann"), "ic_brann.svg");
            structureSikkerhet.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Sikkerhet.Sikkerhet"), "ic_sikkerhet.svg");

            // Pasienter
            StructureSectionDto structurePasienter = structure.AddStructureSection(LangUtil.Get("Structure.Pasienter.Header"));
            structurePasienter.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Pasienter.Antall"), "ic_pasienter.svg");
            structurePasienter.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Pasienter.Type"), "ic_skader.svg");
            structurePasienter.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Pasienter.Omfang"), "ic_skademekanikk.svg");

            // Evakuering
            StructureSectionDto structureEvakuering = structure.AddStructureSection(LangUtil.Get("Structure.Evakuering.Header"));
            structureEvakuering.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Evakuering.Flaskehalser"), "ic_flaskehalser.svg");
            structureEvakuering.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Evakuering.Kjeder"), "ic_evakuering.svg");
            structureEvakuering.AddStructureElement(StructureElementDto.ElementType.Normal, LangUtil.Get("Structure.Evakuering.Rett"), "ic_rett.svg");

            structure.currentStructureSectionId = 0;
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
            //Onboarding needed if location rights is needed
            return !(locationManager.HasLocationPermission());
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