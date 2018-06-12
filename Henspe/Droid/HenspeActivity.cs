using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Runtime;

namespace Henspe.Droid
{
	[Activity(Label = "", ScreenOrientation = ScreenOrientation.Portrait)]
	class HenspeActivity : SinglePaneActivity
    {
		HenspeFragment henspeFragment;

		protected override Fragment OnCreatePane()
		{
			henspeFragment = new HenspeFragment();
			return henspeFragment;
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			henspeFragment.InitializeLocationManager();
			henspeFragment.RequestLocation();

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		public override void OnBackPressed()
		{
			base.OnBackPressed();

			Intent main = new Intent(Intent.ActionMain);
			main.AddCategory(Intent.CategoryHome);
			StartActivity(main);
		}
	}
}