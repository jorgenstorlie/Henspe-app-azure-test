﻿using System;
using System.Collections.Generic;
using Henspe.Core.Model.Dto;
using Henspe.iOS.Const;
using Henspe.iOS.Util;
using UIKit;

namespace Henspe.iOS
{
    public partial class MainNormalRowViewCell : UITableViewCell
	{
        private NSLayoutConstraint _constraint;

        public MainNormalRowViewCell (IntPtr handle) : base (handle)
		{
		}

        internal void SetContent(StructureElementDto structureElement)
        {
            BackgroundColor = UIColor.Clear;
            
            labLabel.TextColor = ColorConst.snlaText;
            labLabel.Text = structureElement.description;
            /*jls
         viewImageContainer.BackgroundColor = UIColor.Clear;
         if (_constraint != null)
         {
             viewImageContainer.RemoveConstraint(_constraint);
             _constraint.Dispose();
             _constraint = null;
         }


         if(structureElement.percent < 1)
         {

             //ImageView.TranslatesAutoresizingMaskIntoConstraints = false;
             _constraint = NSLayoutConstraint.Create(imgImage, NSLayoutAttribute.Width, NSLayoutRelation.Equal, viewImageContainer, NSLayoutAttribute.Width, structureElement.percent, 1);
             viewImageContainer.AddConstraint(_constraint);
         }
         else
        
            {
                constraintImageWidth.Active = true;
            }
             */
            SVGUtil.LoadSVGToImageView(imgImage, structureElement.image, new Dictionary<string, string>());
        }
    }
}
