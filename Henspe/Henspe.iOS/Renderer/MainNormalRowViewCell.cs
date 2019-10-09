using System;
using Henspe.Core.Model.Dto;
using Henspe.iOS.Const;
using UIKit;

namespace Henspe.iOS
{
    public partial class MainNormalRowViewCell : UITableViewCell
    {
        public MainNormalRowViewCell(IntPtr handle) : base(handle)
        {
        }

        internal void SetContent(StructureElementDto structureElement)
        {
            BackgroundColor = UIColor.Clear;
            labLabel.TextColor = UIColor.FromName("FontColor");
            labLabel.Text = structureElement.description;

            imgImage.Image = UIImage.FromBundle(structureElement.image).ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
  imgImage.TintColor =  UIColor.FromName("ImageColor");

            

            //       imgImage.TintColor = UIColor.Red;
        }
    }
}
