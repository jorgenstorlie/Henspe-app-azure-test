﻿using System;
using CoreGraphics;
using UIKit;
using Henspe.iOS.Const;
using Henspe.iOS.Util;

namespace Henspe.iOS
{
    public partial class InitialCardViewController : UIViewController
    {
        private int pageNumber;
        private double scrollTime = 1.0;
        private bool animated = false;

        // Intro 1
        UIImageView img11ImageView;
        CGPoint img11ViewOriginCenter;
        UIImageView img12ImageView;
        CGPoint img12ViewOriginCenter;
		UIImageView img13ImageView;
        CGPoint img13ViewOriginCenter;

        // Intro 2
        UIImageView img21ImageView;
		CGPoint img21ViewOriginCenter;
		UIImageView img22ImageView;
        CGPoint img22ViewOriginCenter;
		UIImageView img23ImageView;
        CGPoint img23ViewOriginCenter;
		UIImageView img24ImageView;
        CGPoint img24ViewOriginCenter;
		UIImageView img25ImageView;
        CGPoint img25ViewOriginCenter;
		UIImageView img26ImageView;
        CGPoint img26ViewOriginCenter;

        // Intro 3
        UIImageView img31ImageView;
		CGPoint img31ViewOriginCenter;

        CGPoint imageOriginCenter;

        public InitialCardViewController(int pageNumber) : base("InitialCardViewController", null)
        {
			this.pageNumber = pageNumber;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            SetupView();
        }

        public override void ViewWillAppear(bool animated)
        {
            this.View.BackgroundColor = UIColor.Clear;

            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            imageOriginCenter = imgImage.Center;

            Setup();
        }

        void SetupView()
        {
            this.View.BackgroundColor = UIColor.Clear;

            labHeader.TextColor = ColorConst.textColor;
            labText.TextColor = ColorConst.textColor;

            if (pageNumber == 1)
            {
                labHeader.Text = LangUtil.Get("Initial.PageOne.Header");
                imgImage.Image = UIImage.FromFile("intro1.png");
                labText.Text = LangUtil.Get("Initial.PageOne.Text");
            }
            else if (pageNumber == 2)
            {
                labHeader.Text = LangUtil.Get("Initial.PageTwo.Header");
                imgImage.Image = UIImage.FromFile("intro2.png");
                labText.Text = LangUtil.Get("Initial.PageTwo.Text");
            }
            else if (pageNumber == 3)
            {
                labHeader.Text = LangUtil.Get("Initial.PageThree.Header");
                imgImage.Image = UIImage.FromFile("intro3.png");
                labText.Text = LangUtil.Get("Initial.PageThree.Text");
            }
        }

        public async void Setup()
        {
            //if (animated == true)
            //    return;

            //animated = true;

            if (pageNumber == 1)
            {
                img11ImageView = CreateImage("intro1-1.png", 0.08f, 0.5f, 0.15f, 0.15f, 1.0f);
				img11ViewOriginCenter = img11ImageView.Center;

                img12ImageView = CreateImage("intro1-2.png", 0.9f, 0.2f, 0.15f, 0.15f, 1.0f);
				img12ViewOriginCenter = img12ImageView.Center;

				img13ImageView = CreateImage("intro1-3.png", 0.9f, 1.0f, 0.15f, 0.15f, 1.0f);
				img13ViewOriginCenter = img13ImageView.Center;
            }
            else if (pageNumber == 2)
            {
				img21ImageView = CreateImage("intro2-1.png", 0.0f, 0.1f, 0.15f, 0.15f, 1.0f);
                img21ViewOriginCenter = img21ImageView.Center;

				img22ImageView = CreateImage("intro2-2.png", 0.0f, 0.5f, 0.15f, 0.15f, 1.0f);
                img22ViewOriginCenter = img22ImageView.Center;

				img23ImageView = CreateImage("intro2-3.png", 0.0f, 0.9f, 0.15f, 0.15f, 1.0f);
                img23ViewOriginCenter = img23ImageView.Center;

				img24ImageView = CreateImage("intro2-4.png", 1.0f, 0.1f, 0.15f, 0.15f, 1.0f);
                img24ViewOriginCenter = img24ImageView.Center;

				img25ImageView = CreateImage("intro2-5.png", 1.0f, 0.5f, 0.15f, 0.15f, 1.0f);
                img25ViewOriginCenter = img25ImageView.Center;

				img26ImageView = CreateImage("intro2-6.png", 1.0f, 0.9f, 0.15f, 0.15f, 1.0f);
                img26ViewOriginCenter = img26ImageView.Center;
            }
            else if (pageNumber == 3)
            {
				img31ImageView = CreateImage("intro3-1.png", 0.9f, 0.1f, 0.15f, 0.15f, 1.0f);
				img31ViewOriginCenter = img31ImageView.Center;
            }
        }

        public void SetScrollValue(nfloat width, nfloat scrollValue)
        {
            nfloat pageWidth = pageNumber * width;
            nfloat pageValue = pageWidth - width;
            nfloat value = scrollValue - pageValue;

            MoveInX(imgImage, imageOriginCenter, value / 2);

            if (pageNumber == 1)
            {
                MoveInX(img11ImageView, img11ViewOriginCenter, value * 1.3f);
                MoveInX(img12ImageView, img12ViewOriginCenter, value * 0.7f);
				MoveInX(img13ImageView, img13ViewOriginCenter, value * 0.5f);
            }
            else if (pageNumber == 2)
            {
				MoveInX(img21ImageView, img21ViewOriginCenter, value * 1.7f);
				MoveInX(img22ImageView, img22ViewOriginCenter, value * 1.5f);
				MoveInX(img23ImageView, img23ViewOriginCenter, value * 1.3f);
				MoveInX(img24ImageView, img24ViewOriginCenter, value * 0.9f);
				MoveInX(img25ImageView, img25ViewOriginCenter, value * 0.7f);
				MoveInX(img26ImageView, img26ViewOriginCenter, value * 0.5f);
            }
            else if (pageNumber == 3)
            {
				MoveInX(img31ImageView, img31ViewOriginCenter, value * 1.3f);
            }
        }

        void MoveInX(UIImageView imageView, CGPoint originCenter, nfloat value)
        {
            imageView.Center = new CGPoint(originCenter.X - value, originCenter.Y);
        }

        private UIImageView CreateImage(string imageName, nfloat xpercent, nfloat ypercent, nfloat widthPercent, nfloat heightPercent, nfloat alpha)
        {
            imgImage.LayoutIfNeeded();

            UIImage image = UIImage.FromFile(imageName);

            nfloat width;
            nfloat height;

            width = (widthPercent * (float)imgImage.Frame.Width) / 1.0f;
            height = (heightPercent * (float)imgImage.Frame.Height) / 1.0f;

            nfloat x = imgImage.Frame.X + (imgImage.Frame.Width * xpercent) - (width / 2);
            nfloat y = imgImage.Frame.Y + (imgImage.Frame.Height * ypercent) - (height / 2);

            UIImageView imageView = new UIImageView(image);
            imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            imageView.Frame = new CGRect(x, y, width, height);

            imageView.Alpha = alpha;

            //imgImage.BackgroundColor = UIColor.Green;
            this.View.AddSubview(imageView);

            return imageView;
        }

        private void FadeIn(UIImageView imageView)
        {
            double fadeInTime = 0.2f;

            imageView.Alpha = 0.0f;

            UIView.Animate(fadeInTime, () =>
            {
                UIView.SetAnimationCurve(UIViewAnimationCurve.EaseOut);

                imageView.Alpha = 1.0f;
                imageView.LayoutIfNeeded();
            });

            return;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}