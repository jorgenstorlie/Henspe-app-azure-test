using System;
using UIKit;

namespace Henspe.iOS.Extension
{
	public static class ViewExtensions
	{
		public static void MakeCircleView(this UIView view, float borderWith, UIColor lineColor) 
		{
			float width = (float)view.Frame.Size.Width;
			view.ClipsToBounds = true;
			view.Layer.CornerRadius = width / 2.0f;
			view.Layer.BorderWidth = borderWith;
			view.Layer.BorderColor = lineColor.CGColor;
		}

		public static void MakeColorFilter(this UIView view, int colorHexValue, float alpha)
		{
			float r = (((float)((colorHexValue & 0xFF0000) >> 16)) / 255.0f) * 0.65f;
			float g = (((float)((colorHexValue & 0xFF00) >> 8)) / 255.0f) * 0.65f;
			float b = (((float)(colorHexValue & 0xFF)) / 255.0f) * 0.65f;

			view.BackgroundColor = UIColor.FromRGBA (r, g, b, alpha);
		}

		/// <summary>
		/// Find the first responder in the <paramref name="view"/>'s subview hierarchy
		/// </summary>
		/// <param name="view">
		/// A <see cref="UIView"/>
		/// </param>
		/// <returns>
		/// A <see cref="UIView"/> that is the first responder or null if there is no first responder
		/// </returns>
		public static UIView FindFirstResponder(this UIView view)
		{
			if (view.IsFirstResponder)
			{
				return view;
			}
			foreach (UIView subView in view.Subviews)
			{
				var firstResponder = subView.FindFirstResponder();
				if (firstResponder != null)
					return firstResponder;
			}
			return null;
		}

		/// <summary>
		/// Find the first Superview of the specified type (or descendant of)
		/// </summary>
		/// <param name="view">
		/// A <see cref="UIView"/>
		/// </param>
		/// <param name="stopAt">
		/// A <see cref="UIView"/> that indicates where to stop looking up the superview hierarchy
		/// </param>
		/// <param name="type">
		/// A <see cref="Type"/> to look for, this should be a UIView or descendant type
		/// </param>
		/// <returns>
		/// A <see cref="UIView"/> if it is found, otherwise null
		/// </returns>
		public static UIView FindSuperviewOfType(this UIView view, UIView stopAt, Type type)
		{
			if (view.Superview != null)
			{
				if (type.IsAssignableFrom(view.Superview.GetType()))
				{
					return view.Superview;
				}

				if (view.Superview != stopAt)
					return view.Superview.FindSuperviewOfType(stopAt, type);
			}

			return null;
		}
	}
}