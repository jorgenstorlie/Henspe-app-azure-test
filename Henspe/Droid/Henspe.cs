using System;
using Android.App;
using Android.Runtime;
using Henspe.Core.Service;
using Henspe.Core.Model.Dto;
using Android.Locations;
using SNLA.Core.Service;
using Henspe.Droid.Services;

namespace Henspe.Droid
{
#if DEBUG
    [Application(Label = "@string/app_name", Theme = "@style/SplashTheme", Debuggable = true, LargeHeap = true)]
#else
    [Application(Label = "@string/app_name", Theme = "@style/SplashTheme", Debuggable = false, LargeHeap = true)]
#endif
    class Henspe : Application
    {
        public static Henspe Current { get; private set; }
        public Location myLocation;
        public string unknownCoordinates = "";
        public string coordinatesText = "";
        public string unknownAddress = "";
        public string addressText = "";
        public StructureDto structure;
        public PositionFragment PositionFragment;
        public ApplicationService ApplicationService { get; private set; }
        public CoordinateService CoordinateService = null;

        public Henspe(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
            Current = this;
        }

        public override void OnCreate()
        {
            base.OnCreate();

            ApplicationService = new AndroidApplicationService("https://snla-apps.no/apps/henspe/");
            CoordinateService = new CoordinateService();

            CoordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_North, Resources.GetString(Resource.String.Location_Element_North_Text));
            CoordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_South, Resources.GetString(Resource.String.Location_Element_South_Text));
            CoordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_East, Resources.GetString(Resource.String.Location_Element_East_Text));
            CoordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_West, Resources.GetString(Resource.String.Location_Element_West_Text));
            CoordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_Degrees, Resources.GetString(Resource.String.Location_Element_Degrees_Text));
            CoordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_Minutes, Resources.GetString(Resource.String.Location_Element_Minutes_Text));
            CoordinateService.AddLanguageValue(CoordinateServiceLanguageKey.Coordinate_Seconds, Resources.GetString(Resource.String.Location_Element_Seconds_Text));

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
            structureHendelse.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Hendelse_Trafikk), "ic_trafikk");
            structureHendelse.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Hendelse_Brann), "ic_brann");

            // Eksakt posisjon
            StructureSectionDto structureEksaktPosisjon = structure.AddStructureSection(Resources.GetString(Resource.String.Structure_EksaktPosisjon_Header), "");
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Position, Resources.GetString(Resource.String.Structure_EksaktPosisjon_Posisjon), "ic_posisjon");
            //  structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Address, Resources.GetString(Resource.String.Structure_EksaktPosisjon_Adresse), "ic_adresse");
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_EksaktPosisjon_Oppmotested), "ic_oppmotested");
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_EksaktPosisjon_Ankomst), "ic_ankomst");
            structureEksaktPosisjon.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_EksaktPosisjon_Avreise), "ic_avreise");

            // Nivå
            StructureSectionDto structureNiva = structure.AddStructureSection(Resources.GetString(Resource.String.Structure_Niva_Header), "");
            structureNiva.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Niva_1), "ic_1");
            structureNiva.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Niva_2), "ic_2");
            structureNiva.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Niva_3), "ic_3");
            structureNiva.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Niva_QuattroVarsling), "ic_quattro");

            // Sikkerhet
            StructureSectionDto structureSikkerhet = structure.AddStructureSection(Resources.GetString(Resource.String.Structure_Sikkerhet_Header), "");
            structureSikkerhet.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Sikkerhet_Farer), "ic_farer");
            structureSikkerhet.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Sikkerhet_Brann), "ic_brann");
            structureSikkerhet.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Sikkerhet_Sikkerhet), "ic_sikkerhet");

            // Pasienter
            StructureSectionDto structurePasienter = structure.AddStructureSection(Resources.GetString(Resource.String.Structure_Pasienter_Header), "");
            structurePasienter.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Pasienter_Antall), "ic_pasienter");
            structurePasienter.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Pasienter_Type), "ic_skader");
            structurePasienter.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Pasienter_Omfang), "ic_skademekanikk");

            // Evakuering
            StructureSectionDto structureEvakuering = structure.AddStructureSection(Resources.GetString(Resource.String.Structure_Evakuering_Header), "");
            structureEvakuering.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Evakuering_Flaskehalser), "ic_flaskehalser");
            structureEvakuering.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Evakuering_Kjeder), "ic_evakuering");
            structureEvakuering.AddStructureElement(StructureElementDto.ElementType.Normal, Resources.GetString(Resource.String.Structure_Evakuering_Rett), "ic_rett");
        }
    }
}