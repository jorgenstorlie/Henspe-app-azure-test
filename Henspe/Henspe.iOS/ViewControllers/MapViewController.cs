using System;
using Foundation;
using UIKit;

namespace Henspe.iOS
{
    public partial class MapViewController : UIViewController
    {
        public MapViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupView();
        }

        private void SetupView()
        {
            SetupNavigationBar();
            SetupMap();
        }

        private void SetupNavigationBar()
        {
        }

        private void SetupMap()
        {
            map.ShowsUserLocation = true;
            map.ZoomEnabled = true;
            map.ScrollEnabled = true;
            //map.Delegate = new MapDelegate(this, map); JØRGEN, DENNE MÅ INN. Koden ligger nederst. Kommenter ut den også. Må gjøres om til veikameraer.
            map.RotateEnabled = false;
            map.ShowsCompass = false;
            map.IsAccessibilityElement = false;
            map.AccessibilityElementsHidden = true;

            //jls       SVGUtil.LoadSVGToButton(btnMapType, "ic_btn_map_toggle_up.svg", "ic_btn_map_toggle_down.svg", "ic_btn_map_toggle_down.svg");
            //jls     SVGUtil.LoadSVGToButton(btnZoomHome, "ic_btn_map_center_up.svg", "ic_btn_map_center_down.svg");

            btnMapType.Hidden = false;
            btnZoomHome.Hidden = false;
        }

        partial void OnBtnMapTypeClicked(NSObject sender)
        {
        }

        partial void OnBtnZoomHomeClicked(NSObject sender)
        {
        }
    }
}