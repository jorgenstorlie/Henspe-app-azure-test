using Android.OS;
using Android.Views;
using Android.Locations;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace Henspe.Droid
{
    public class PositionFragment : Fragment
    {
		public PositionFragment(int id)
		{

		}

		public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return base.OnCreateView(inflater, container, savedInstanceState);
        }

		public void UpdateLocation(Location location)
		{
		//	throw new NotImplementedException();
		}

	}
}
