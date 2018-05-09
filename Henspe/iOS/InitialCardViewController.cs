using System;
using CoreGraphics;
using UIKit;
using Henspe.iOS.Const;

namespace Henspe.iOS
{
    public partial class InitialCardViewController : UIViewController
    {
        private int pageNumber;
        private double scrollTime = 1.0;
        private bool animated = false;

        // Intro 1
        UIImageView img11ImageView;
        CGPoint innerCircleImageViewOriginCenter;
        UIImageView img12ImageView;
        CGPoint heartImageViewOriginCenter;

        // Intro 2
        UIImageView img21ImageView;
        CGPoint waterImageViewOriginCenter;

        // Intro 3
        UIImageView img31ImageView;
        CGPoint arrowRightImageViewOriginCenter;
        UIImageView img32ImageView;
        CGPoint arrowLeftImageViewOriginCenter;

        CGPoint imageOriginCenter;

        public InitialCardViewController(int inputPageNumber) : base("InitialCardViewController", null)
        {
            pageNumber = inputPageNumber;
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
                labHeader.Text = Foundation.NSBundle.MainBundle.LocalizedString("Initial.PageOne.Header", null);
                imgImage.Image = UIImage.FromFile("intro1.png");
                labText.Text = Foundation.NSBundle.MainBundle.LocalizedString("Initial.PageOne.Text", null);
            }
            else if (pageNumber == 2)
            {
                labHeader.Text = Foundation.NSBundle.MainBundle.LocalizedString("Initial.PageTwo.Header", null);
                imgImage.Image = UIImage.FromFile("intro2.png");
                labText.Text = Foundation.NSBundle.MainBundle.LocalizedString("Initial.PageTwo.Text", null);
            }
            else if (pageNumber == 3)
            {
                labHeader.Text = Foundation.NSBundle.MainBundle.LocalizedString("Initial.PageThree.Header", null);
                imgImage.Image = UIImage.FromFile("intro3.png");
                labText.Text = Foundation.NSBundle.MainBundle.LocalizedString("Initial.PageThree.Text", null);
            }
        }

        public async void Setup()
        {
            //if (animated == true)
            //    return;

            //animated = true;

            if (pageNumber == 1)
            {
                img11ImageView = CreateImage("intro1innerCircle.png", 0.5f, 0.5f, 0.7f, 0.7f, 1.0f);
                innerCircleImageViewOriginCenter = img11ImageView.Center;

                img12ImageView = CreateImage("intro1heart.png", 0.5f, 0.5f, 0.3f, 0.3f, 1.0f);
                heartImageViewOriginCenter = img12ImageView.Center;
            }
            else if (pageNumber == 2)
            {
                img21ImageView = CreateImage("intro2pie.png", 0.50f, 0.50f, 1.0f, 1.0f, 1.0f);
                waterImageViewOriginCenter = img21ImageView.Center;
            }
            else if (pageNumber == 3)
            {
                img31ImageView = CreateImage("intro3arrowRight.png", 0.05f, 0.5f, 0.25f, 0.25f, 1.0f);
                arrowRightImageViewOriginCenter = img31ImageView.Center;

                img32ImageView = CreateImage("intro3arrowLeft.png", 0.95f, 0.5f, 0.25f, 0.25f, 1.0f);
                arrowLeftImageViewOriginCenter = img32ImageView.Center;
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
                MoveInX(img11ImageView, innerCircleImageViewOriginCenter, value * 0.9f);
                MoveInX(img12ImageView, heartImageViewOriginCenter, value * 1.3f);
            }
            else if (pageNumber == 2)
            {
                MoveInX(img21ImageView, waterImageViewOriginCenter, value * 1.3f);
            }
            else if (pageNumber == 3)
            {
                MoveInX(img31ImageView, arrowRightImageViewOriginCenter, value * 1.4f);
                MoveInX(img32ImageView, arrowLeftImageViewOriginCenter, value * 1.5f);
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

            /*
            if (Math.Abs(image.Size.Width - image.Size.Height) < 2)
            {
                width = image.Size.Width / 2.7f;
                height = image.Size.Height / 2.7f;
            }
            else
            {
                width = image.Size.Width / 2.0f;
                height = image.Size.Height / 2.0f; ;
            }
            */

            nfloat x = imgImage.Frame.X + (imgImage.Frame.Width * xpercent) - (width / 2);
            nfloat y = imgImage.Frame.Y + (imgImage.Frame.Height * ypercent) - (height / 2);

            UIImageView imageView = new UIImageView(image);
            //imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
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