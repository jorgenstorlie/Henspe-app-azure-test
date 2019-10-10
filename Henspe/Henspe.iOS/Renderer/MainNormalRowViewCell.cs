﻿using System;
using Henspe.Core.Model.Dto;
using Henspe.iOS.Const;
using SNLA.iOS.Util;
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
            labLabel.TextColor = ColorHelper.FromType(ColorType.Font);
            labLabel.Text = structureElement.description;

            imgImage.Image = UIImage.FromBundle(structureElement.image).ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
  imgImage.TintColor = ColorHelper.FromType(ColorType.Image);



            //       imgImage.TintColor = UIColor.Red;
        }
    }
}
