using System;
using System.Collections.Generic;
using Henspe.Core.Service;
using Henspe.Core.Util;
using System.Linq;

namespace Henspe.Core.ViewModel
{
    public class SettingsViewModel
    {
        private readonly SettingsService _settingsService;

        public string CoordinateTypeText => CoordinateRows[SelectedCoordinatesRow].Title;

        public string ResponseText
        {
            get
            {
                var response = _settingsService.GetSettings().ResponseType;
                if (response == ResponseType.NotDecided)
                    return "Settings.NotDecided".Translate();
                if (response == ResponseType.SMS)
                    return "Settings.SMS".Translate();
                if (response == ResponseType.Email)
                    return "Settings.Email".Translate();
                if (response == ResponseType.None)
                    return "Settings.None";
                throw new Exception("Unknown ResponseType");
            }
        }

        public List<CoordianteRow> CoordinateRows { get; private set; }

        public int SelectedCoordinatesRow => CoordinateRows.IndexOf(CoordinateRows.First(x => x.CoordinateFormat == _settingsService.GetSettings().CoordinateFormat));

        public SettingsViewModel()
        {
            _settingsService = new SettingsService();
            CoordinateRows = new List<CoordianteRow>();
        }

        public void Init()
        {
            double lat = 53.2314d;
            double lon = 10.9283d;

            var dd = CoordinateUtil.FormatDD(lat, lon);
            var ddm = CoordinateUtil.FormatDDM(lat, lon);
            var dms = CoordinateUtil.FormatDMS(lat, lon);
            var utm = CoordinateUtil.FormatUTM(lat, lon);

            CoordinateRows.Add(new CoordianteRow
            {
                CoordinateFormat = CoordinateFormat.DD,
                Title = "Settings.DDTitle".Translate(),
                Sub1 = dd.latitudeDescription,
                Sub2 = dd.longitudeDescription
            });
            CoordinateRows.Add(new CoordianteRow
            {
                CoordinateFormat = CoordinateFormat.DDM,
                Title = "Settings.DDMTitle".Translate(),
                Sub1 = ddm.latitudeDescription,
                Sub2 = ddm.longitudeDescription
            });
            CoordinateRows.Add(new CoordianteRow
            {
                CoordinateFormat = CoordinateFormat.DMS,
                Title = "Settings.DMSTitle".Translate(),
                Sub1 = dms.latitudeDescription,
                Sub2 = dms.longitudeDescription
            });
            CoordinateRows.Add(new CoordianteRow
            {
                CoordinateFormat = CoordinateFormat.UTM,
                Title = "Settings.UTMTitle".Translate(),
                Sub1 = utm.latitudeDescription,
                Sub2 = utm.longitudeDescription
            });
        }

        public void SaveCoordinatesFormat(CoordinateFormat type)
        {
            var settings = _settingsService.GetSettings();
            settings.CoordinateFormat = type;
            _settingsService.SaveSettings(settings);
        }

        public class CoordianteRow
        {
            public CoordinateFormat CoordinateFormat { get; set; }
            public string Title { get; set; }
            public string Sub1 { get; set; }
            public string Sub2 { get; set; }
        }
    }
}
