using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Svg.Platform;
using UIKit;

namespace Henspe.iOS.Util
{
    public class SVGUtil
    {
        public SVGUtil()
        {
        }

        public static async Task<UIImage> LoadSVG(string filename)
        {
            UIImage image = await ImageService.Instance
                .LoadFile(filename)
                .WithCustomDataResolver(new SvgDataResolver(200, 0, true))
                .AsUIImageAsync();

            return image;
        }

        public static void LoadSVGToImageView(UIImageView imageView, string imageFilename, Dictionary<string, string> replaceStringMap)
        {
            ImageService.Instance
                        .LoadFile(imageFilename)
                        .WithCustomDataResolver(new SvgDataResolver(0, 0, true, replaceStringMap))
                        .Into(imageView);
        }

        public static async Task LoadSVGToButton(UIButton button, string imageNormalFilename, string imageDisabledFilename)
        {
            UIImage imageNormal = await ImageService.Instance
                .LoadFile(imageNormalFilename)
                .WithCustomDataResolver(new SvgDataResolver(200, 0, true))
                .AsUIImageAsync();

            button.SetBackgroundImage(imageNormal, UIControlState.Normal);

            if(imageDisabledFilename != null)
            {
                UIImage imageDisabled = await ImageService.Instance
                    .LoadFile(imageDisabledFilename)
                    .WithCustomDataResolver(new SvgDataResolver(200, 0, true))
                    .AsUIImageAsync();

                button.SetBackgroundImage(imageDisabled, UIControlState.Disabled);
            }
        }
    }
}
