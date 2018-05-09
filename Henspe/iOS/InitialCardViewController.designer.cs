// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Henspe.iOS
{
    [Register("InitialCardViewController")]
    partial class InitialCardViewController
    {
        [Outlet]
        UIKit.UIImageView imgImage { get; set; }

        [Outlet]
        UIKit.UILabel labHeader { get; set; }

        [Outlet]
        UIKit.UILabel labText { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (imgImage != null)
            {
                imgImage.Dispose();
                imgImage = null;
            }

            if (labHeader != null)
            {
                labHeader.Dispose();
                labHeader = null;
            }

            if (labText != null)
            {
                labText.Dispose();
                labText = null;
            }
        }
    }
}
