using Android.App;
using Android.OS;
using Android.Content;
using Android.Views;
using Android.Support.V7.App;
using Android.Util;
using Java.Lang;
using Henspe.Droid.Utils;

namespace Henspe.Droid
{
	[Activity(Label = "Henspe", MainLauncher = true, Icon = "@mipmap/icon", Theme ="@style/AppTheme")]
	public class MainActivity : AppCompatActivity
	{
		/** 
        * Fragment managing the behaviors, interactions and presentation of the navigation drawer.  
        */
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			RequestWindowFeature(WindowFeatures.NoTitle);

			DisplayMetrics displayMetrics = new DisplayMetrics();
			WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
			Henspe.Current.SetScreenSize(displayMetrics.HeightPixels, displayMetrics.WidthPixels);

			if (UserUtil.settings.instructionsFinished)
			{
				Intent intent = new Intent(this, typeof(HenspeActivity));
				StartActivity(intent);
				Finish();
			}
			else
			{
				GoToInfoScreen();
			}
		}

		protected override void OnPostCreate(Bundle savedInstanceState)
		{
			base.OnPostCreate(savedInstanceState);
		}

		protected override void OnDestroy()
		{
			try
			{
				base.OnDestroy();
			}
			catch (Exception ex)
			{
				ex.ToString();
			}
		}

		private void GoToInfoScreen()
		{
			var intent = new Intent(this, typeof(InitialActivity));
			StartActivity(intent);
			Finish();
		}

		public override void OnBackPressed()
		{
			base.OnBackPressed();
		}
	}
}