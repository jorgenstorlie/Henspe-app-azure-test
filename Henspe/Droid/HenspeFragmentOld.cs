using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Content;
using NavUtils = Android.Support.V4.App.NavUtils;
using System.Collections.Generic;
using System;
using Henspe.Core.Model.Dto;
using Henspe.Droid.Adapters;
using Android.Locations;
//using Android.Gms.Location;
//using Android.Gms.Common.Apis;
using Android;
using Android.Content.PM;
using static Android.Support.V4.App.ActivityCompat;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using ClipboardManager = Android.Content.ClipboardManager;
using Geocoder = Android.Locations.Geocoder;
using Android.Support.V7.Widget;
using static Android.Media.Audiofx.BassBoost;
using Android.Arch.Lifecycle;
using static Android.Views.View;
using Android.Support.Design.Widget;
using Android.Gms.Tasks;
using Android.Support.V13.App;
using Android.Gms.Location;
using Android.Gms.Common.Apis;
using Henspe.Core.Util;
using Android.Gms.Common;
using Henspe.Core.Communication.Dto;
using Android.Runtime;

namespace Henspe.Droid
{
	class HenspeFragmentOld : ListFragment, Android.Gms.Location.ILocationListener, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, ActivityCompat.IOnRequestPermissionsResultCallback
	{
		public const string parameterSelectedBase = "com.computas.latsamband.onduty.selectedBase";

		private HenspeRowAdapter _itemsAdapter;
		private Spinner _spiFilter;
		private bool createdFinished = false;

		private RecyclerView _recyclerView;
		private Button _btnInfo;

		private bool viewCreated = false;

		private ViewGroup root;

		// GPS
		private GoogleApiClient apiClient;
        private LocationManager locationManager;
        private LocationRequest locationRequest;
		private const int priorityAccuracy = LocationRequest.PriorityBalancedPowerAccuracy;
        private const long updateIntervalInMilliseconds = 2000;
        private const long fastestUpdateIntervalInMilliseconds = 5000;
		private bool isGrantedPermissionGps;
		private string degrees = "";
		private string minutes = "";
		private string seconds = "";
		private string north = "";
		private string east = "";
		private string south = "";
		private string west = "";

		public HenspeFragmentOld()
		{
			createdFinished = false;

			//UserUtil.Credentials.OnDutyCountry = inputTittelType;
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			viewCreated = false;

			SetupStrings();

			if (Build.VERSION.SdkInt >= Build.VERSION_CODES.M)
			{
  				isGrantedPermissionGps = ContextCompat.CheckSelfPermission(this.Activity, Manifest.Permission.AccessFineLocation) == (int)Permission.Granted;
				if (isGrantedPermissionGps)
				{
					InitializeLocationManager();
					RequestLocation();
				}
				else
				{
					const int permissionRequestCode = 200; // 123

					List<string> permissionsPosition = new List<string>();
					permissionsPosition.Add(Manifest.Permission.AccessFineLocation);
					permissionsPosition.Add(Manifest.Permission.AccessCoarseLocation);

					ActivityCompat.RequestPermissions(this.Activity, permissionsPosition.ToArray(), permissionRequestCode);
				}
			}
			else
			{
				// Old android. Dont need to ask for access
				InitializeLocationManager();
				RequestLocation();
			}
		}

		public override global::Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
		{
			root = (ViewGroup)inflater.Inflate(Resource.Layout.FragmentHenspeList, container, false);

			// Recycler View
			_recyclerView = root.FindViewById<RecyclerView>(Resource.Id.myList);

			Dictionary<HenspeSectionModel, List<HenspeRowModel>> henspeRowDictionary = PopulateList();
			_itemsAdapter = new HenspeRowAdapter(henspeRowDictionary, this.Activity);
			_recyclerView.SetAdapter(_itemsAdapter);
			_recyclerView.SetLayoutManager(new LinearLayoutManager(this.Activity) { Orientation = LinearLayoutManager.Vertical });

			// Button info
			_btnInfo = root.FindViewById<Button>(Resource.Id.btnInfo);
			_btnInfo.Click += OnButtonInfoClicked;

			/*var fragmentTransaction = FragmentManager.BeginTransaction ();
			fragmentTransaction.Add (Resource.Id.frameLayout1, _henspeListFragment);
			fragmentTransaction.Commit ();
            */

			viewCreated = true;

			return root;
		}

		private void SetupStrings()
        {
            degrees = Resources.GetString(Resource.String.Location_Element_Degrees_Text);
            minutes = Resources.GetString(Resource.String.Location_Element_Minutes_Text);
            seconds = Resources.GetString(Resource.String.Location_Element_Seconds_Text);
            north = Resources.GetString(Resource.String.Location_Element_North_Text);
            east = Resources.GetString(Resource.String.Location_Element_East_Large_Text);
            south = Resources.GetString(Resource.String.Location_Element_South_Text);
            west = Resources.GetString(Resource.String.Location_Element_West_Text);
        }

		void OnButtonInfoClicked(object sender, EventArgs eventArgs)
        {
			GoToInfoScreen();
        }

		private void GoToInfoScreen()
        {
			var intent = new Intent(this.Activity, typeof(InitialActivity));
            StartActivity(intent);
			this.Activity.Finish();
        }

		public override void OnViewCreated(global::Android.Views.View view, Bundle savedInstanceState)
		{
			//base.OnViewCreated(view, savedInstanceState);
		}

		/*
		public override void OnListItemClick(ListView l, global::Android.Views.View v, int position, long id)
		{
			int pos = _adapter.SectionedPositionToPosition(position);
			HenspeRowModel henspeRowModel = _itemsAdapter[pos];
		}
		*/

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			inflater.Inflate(Resource.Menu.MenuHome, menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.homeMenuItem:
					NavUtils.NavigateUpFromSameTask(this.Activity);
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}

		private void UpdateActionBarNavigation()
		{
			//var drawerLayout = Activity.FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
			//bool navOpen = drawerLayout != null && drawerLayout.IsDrawerOpen(GravityCompat.Start);
			//Activity.ActionBar.SetDisplayUseLogoEnabled(false);
			//Activity.ActionBar.SetDisplayShowTitleEnabled(false);
			//Activity.ActionBar.NavigationMode = navOpen ? ActionBarNavigationMode.Standard : ActionBarNavigationMode.List;
		}

		public override void OnResume()
		{
			base.OnResume();
		}

		private Dictionary<HenspeSectionModel, List<HenspeRowModel>> PopulateList()
		{
			Dictionary<HenspeSectionModel, List<HenspeRowModel>> result = new Dictionary<HenspeSectionModel, List<HenspeRowModel>>();

			HenspeSectionModel key = null;

			foreach (StructureSectionDto structureSectionDto in Henspe.Current.structure.structureSectionList)
			{
				key = new HenspeSectionModel(structureSectionDto.image, structureSectionDto.description);

				foreach (StructureElementDto structureElementDto in structureSectionDto.structureElementList)
				{
					HenspeRowModel henspeRowModel = new HenspeRowModel(structureElementDto.elementType, structureElementDto.description, structureElementDto.image, structureElementDto.percent);

					if (result.ContainsKey(key))
					{
						result[key].Add(henspeRowModel);
					}
					else
					{
						List<HenspeRowModel> HenspeRowList = new List<HenspeRowModel>();
						HenspeRowList.Add(henspeRowModel);
						result.Add(key, HenspeRowList);
					}
				}
			}

			return result;
		}

		public override void OnDetach()
		{
			//LocalBroadcastManager.GetInstance(Activity.ApplicationContext).UnregisterReceiver(_receiverCall);
			//LocalBroadcastManager.GetInstance(Activity.ApplicationContext).UnregisterReceiver(_receiverSms);
			base.OnDetach();
		}

		public bool OnNavigationItemSelected(int itemPosition, long itemId)
		{
			throw new NotImplementedException();
		}

		public void RefreshRow(int index)
        {
			if(viewCreated && _itemsAdapter != null)
    		 	_itemsAdapter.NotifyItemChanged(index);
        }

		#region GPS
		public void InitializeLocationManager()
		{
			SetupApiClient();
			SetupLocationRequest();
		}

		private void SetupApiClient()
		{
			apiClient = new GoogleApiClient.Builder(Activity, this, this).AddApi(LocationServices.API).Build();
			apiClient.Connect();
		}

		private void SetupLocationRequest()
		{
			if(CheckLocationServiceAvailability() == true)
			{
				locationRequest = new LocationRequest();
				locationRequest.SetPriority(priorityAccuracy);
				locationRequest.SetInterval(updateIntervalInMilliseconds);
				locationRequest.SetFastestInterval(fastestUpdateIntervalInMilliseconds);
            }
		}

		public async void RequestLocation()
        {
			if ((apiClient.IsConnected) && locationRequest != null)
            {
				await LocationServices.FusedLocationApi.RequestLocationUpdates(apiClient, locationRequest, this);
            }

            locationManager = (LocationManager)Context.GetSystemService(Context.LocationService);
			Henspe.Current.myLocation = locationManager.GetLastKnownLocation(LocationManager.GpsProvider);

			UpdateLocation(Henspe.Current.myLocation);
        }

		private bool CheckLocationServiceAvailability()
        {
            locationManager = (LocationManager)Context.GetSystemService(Context.LocationService);
            return isGrantedPermissionGps && locationManager != null &&
                (locationManager.IsProviderEnabled(LocationManager.GpsProvider) ||
                 locationManager.IsProviderEnabled(LocationManager.NetworkProvider));
        }

		public void OnConnected(Bundle connectionHint)
        {
			if(CheckLocationServiceAvailability())
			{
				SetupLocationRequest();
				RequestLocation();
            }
		}

		public void OnConnectionSuspended(int cause)
        {
			if (ContextCompat.CheckSelfPermission(this.Activity, Manifest.Permission.AccessFineLocation) != (int)Permission.Granted
			    && ContextCompat.CheckSelfPermission(this.Activity, Manifest.Permission.CallPhone) != (int)Permission.Granted)
            {
                //Intent intent = new Intent(this, typeof(SetupActivity));
                //StartActivity(intent);
            }
        }

        public void OnLocationChanged(Location location)
        {
		    UpdateLocation(location);
        }

		public void OnProviderDisabled(string locationProvider)
        {
            // called when the user disables the provider
        }

		public void OnProviderEnabled(string locationProvider)
        {
            // called when the user enables the provider
        }

		public void OnStatusChanged(string locationProvider, Availability status, Bundle extras)
        {
            // called when the status of the provider changes (there are a variety of reasons for this)
        }

        private void UpdateLocation(Location location)
		{
			if(location != null)
			{
				CreatePositionTextAndRefreshPositionRow(location);
				CreateAddressTextAndRefreshAddressRow(location);
			}
			else
			{
				Henspe.Current.coordinatesText = Henspe.Current.unknownCoordinates;
				Henspe.Current.addressText = Henspe.Current.unknownAddress;
			}
		}

		private void CreatePositionTextAndRefreshPositionRow(Location location)
		{
			FormattedCoordinatesDto formattedCoordinatesDto = CoordinateUtil.GetFormattedCoordinateDescription(2, location.Latitude, location.Longitude, degrees, minutes, seconds, north, east, south, west);
            Henspe.Current.coordinatesText = formattedCoordinatesDto.latitudeDescription + "\n" + formattedCoordinatesDto.longitudeDescription;

            RefreshRow(4);
		}

		private void CreateAddressTextAndRefreshAddressRow(Location location)
        {
			Geocoder geocoder = new Geocoder(this.Activity);

			IList<Address> addresses = geocoder.GetFromLocation(location.Latitude, location.Longitude, 1);

            string wholeAddress = "";

            try
            {
                wholeAddress = addresses[0].GetAddressLine(0);
            }
            catch (System.ArgumentOutOfRangeException aoore)
            {
            }

			if(wholeAddress.Length > 0)
	    		Henspe.Current.addressText = wholeAddress;
			else
				Henspe.Current.addressText = Henspe.Current.unknownAddress;

            RefreshRow(5);

            /*
            string[] areaType = wholeAddress.Split(',');
            string city = "";

            try
            {
                if (areaType[1].Any(char.IsDigit) == true)
                {
                    city = areaType[1];
                    city = Regex.Replace(areaType[1], @"[0-9\-]", string.Empty).Trim();
                }
                else
                {
                    city = areaType[0];
                    city = Regex.Replace(areaType[0], @"[0-9\-]", string.Empty).Trim();
                }
            }
            catch (System.IndexOutOfRangeException exception)
            {
            }

            addressText.Text = city;
            */
        }

		public void OnConnectionFailed(ConnectionResult result)
        {
        }
		#endregion
	}
}