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

        public static async Task LoadSVGToImageView(UIImageView imageView, string imageFilename, Dictionary<string, string> replaceStringMap)
        {
            SvgDataResolver svgDataResolver = new SvgDataResolver(200, 0, true, replaceStringMap);

            UIImage image = await ImageService.Instance
                .LoadFile(imageFilename)
                .WithCustomDataResolver(svgDataResolver)
                .AsUIImageAsync();

            imageView.Image = image;
        }

        public static async Task LoadSVGToImageView(UIImageView imageView, string imageFilename)
        {
            UIImage image = await ImageService.Instance
                .LoadFile(imageFilename)
                .WithCustomDataResolver(new SvgDataResolver((int)imageView.Frame.Width, (int)imageView.Frame.Height, true))
                .AsUIImageAsync();

            imageView.Image = image;
        }

        public static async Task LoadSVGToButton(UIButton button, string imageNormalFilename, string imageHighlightedFilename)
        {
            UIImage imageNormal = await ImageService.Instance
                .LoadFile(imageNormalFilename)
                .WithCustomDataResolver(new SvgDataResolver(200, 0, true))
                .AsUIImageAsync();

            button.SetImage(imageNormal, UIControlState.Normal);

            if (imageHighlightedFilename != null)
            {
                UIImage imageHighlighted = await ImageService.Instance
                    .LoadFile(imageHighlightedFilename)
                    .WithCustomDataResolver(new SvgDataResolver(200, 0, true))
                    .AsUIImageAsync();

                button.SetImage(imageHighlighted, UIControlState.Highlighted);
            }
        }

        public static async Task LoadSVGToButton(UIButton button, string imageNormalFilename, string imageHighlightedFilename, string imageSelectedFilename)
        {
            UIImage imageNormal = await ImageService.Instance
                .LoadFile(imageNormalFilename)
                .WithCustomDataResolver(new SvgDataResolver(200, 0, true))
                .AsUIImageAsync();

            button.SetImage(imageNormal, UIControlState.Normal);

            if (imageHighlightedFilename != null)
            {
                UIImage imageHighlighted = await ImageService.Instance
                    .LoadFile(imageHighlightedFilename)
                    .WithCustomDataResolver(new SvgDataResolver(200, 0, true))
                    .AsUIImageAsync();

                button.SetImage(imageHighlighted, UIControlState.Highlighted);
            }

            if (imageSelectedFilename != null)
            {
                UIImage imageSelected = await ImageService.Instance
                    .LoadFile(imageHighlightedFilename)
                    .WithCustomDataResolver(new SvgDataResolver(200, 0, true))
                    .AsUIImageAsync();

                button.SetImage(imageSelected, UIControlState.Selected);
            }
        }
    }
}