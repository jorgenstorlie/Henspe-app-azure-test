using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Content;
using NavUtils = Android.Support.V4.App.NavUtils;
using System.Collections.Generic;
using System;
using Henspe.Core.Service;
using Henspe.Core.Model.Dto;
using Android.Locations;
using Android;
using Android.Content.PM;
using Geocoder = Android.Locations.Geocoder;
using Android.Support.V7.Widget;
using Android.Support.V13.App;
using Android.Gms.Location;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Support.V7.App;
using Fragment = Android.Support.V4.App.Fragment;

namespace Henspe.Droid
{
    class NewHenspeFragment : Fragment, Android.Gms.Location.ILocationListener, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        public const string parameterSelectedBase = "com.computas.latsamband.onduty.selectedBase";

        private HenspeRowAdapter _itemsAdapter;
        private Spinner _spiFilter;
        private bool createdFinished = false;
        private RecyclerView _recyclerView;
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

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState); 
            viewCreated = false;
            SetupStrings();
            CheckGrantedGps();
        }

        private void CheckGrantedGps()
        {
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

        public override global::Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
 {
            root = (ViewGroup)inflater.Inflate(Resource.Layout.henspe_fragment, container, false);

            // Recycler View
            _recyclerView = root.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            _recyclerView.NestedScrollingEnabled = true;

          Dictionary<HenspeSectionModel, List<HenspeRowModel>> henspeRowDictionary = PopulateList();
          _itemsAdapter = new HenspeRowAdapter(henspeRowDictionary, this.Activity as AppCompatActivity);
          _recyclerView.SetAdapter(_itemsAdapter);
          _recyclerView.SetLayoutManager(new LinearLayoutManager(this.Activity) { Orientation = LinearLayoutManager.Vertical });
       
          viewCreated = true;

            int paddingTop = 30;
            _recyclerView.SetPadding(0,40,0,0);

            return root;
        }

        private void SetupStrings()
        {
            degrees = Resources.GetString(Resource.String.Location_Element_Degrees_Text);
            minutes = Resources.GetString(Resource.String.Location_Element_Minutes_Text);
            seconds = Resources.GetString(Resource.String.Location_Element_Seconds_Text);
            north = Resources.GetString(Resource.String.Location_Element_North_Text);
            east = Resources.GetString(Resource.String.Location_Element_East_Text);
            south = Resources.GetString(Resource.String.Location_Element_South_Text);
            west = Resources.GetString(Resource.String.Location_Element_West_Text);
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
                    HenspeRowModel henspeRowModel = new HenspeRowModel(structureElementDto.elementType, structureElementDto.description, structureElementDto.image);

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
        //jls    if (viewCreated && _itemsAdapter != null)
        //jls        _itemsAdapter.NotifyItemChanged(index);
        }

        #region GPS
        public void InitializeLocationManager()
        {
            locationManager = (LocationManager)Context.GetSystemService(Context.LocationService);
            SetupApiClient();
            SetupLocationRequest();
        }

        private void SetupApiClient()
        {
            Console.WriteLine("SetupApiClient 1");
            if(apiClient == null)
            {
                Console.WriteLine("SetupApiClient 2");
                apiClient = new GoogleApiClient.Builder(Activity, this, this).AddApi(LocationServices.API).Build();
                apiClient.Connect();
            }
        }

        #region GoogleAPI callbacks
        public void OnConnected(Bundle connectionHint)
        {
            CheckGrantedGps();
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

        public void OnConnectionFailed(ConnectionResult result)
        {
        }
        #endregion

        private void SetupLocationRequest()
        {
            Console.WriteLine("SetupLocationRequest 1");
            if(locationRequest == null)
            {
                Console.WriteLine("SetupLocationRequest 2");
                locationRequest = new LocationRequest();
                locationRequest.SetPriority(priorityAccuracy);
                locationRequest.SetInterval(updateIntervalInMilliseconds);
                locationRequest.SetFastestInterval(fastestUpdateIntervalInMilliseconds);
            }
        }

        public async void RequestLocation()
        {
            Console.WriteLine("RequestLocation 1");
            if (apiClient != null && (apiClient.IsConnected) && locationRequest != null)
            {
                Console.WriteLine("RequestLocation 2");
                await LocationServices.FusedLocationApi.RequestLocationUpdates(apiClient, locationRequest, this);
            }

            Console.WriteLine("RequestLocation 3");
            locationManager = (LocationManager)Context.GetSystemService(Context.LocationService);
            if(locationManager != null)
            {
                Console.WriteLine("RequestLocation 3");
                Henspe.Current.myLocation = locationManager.GetLastKnownLocation(LocationManager.GpsProvider);
            }

            Console.WriteLine("RequestLocation 5");
           UpdateLocation(Henspe.Current.myLocation);
        }

        private bool CheckLocationServiceAvailability()
        {
            locationManager = (LocationManager)Context.GetSystemService(Context.LocationService);

            if(locationManager != null)
            {
                bool gpsProviderEnabled = locationManager.IsProviderEnabled(LocationManager.GpsProvider);
                bool networkProviderEnabled = locationManager.IsProviderEnabled(LocationManager.NetworkProvider);
            
                return isGrantedPermissionGps && (gpsProviderEnabled || networkProviderEnabled);
            }
            else
            {
                return false;   
            }
        }

        public void OnLocationChanged(Location location)
        {
            Console.WriteLine("OnLocationChanged 1");
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
            Console.WriteLine("UpdateLocation 1");
            if (location != null)
            {
                Console.WriteLine("UpdateLocation 2");
				Henspe.Current.PositionFragment?.UpdateLocation(location);

            //    Cre   atePositionTex
                //  sitionRow(location);
             //   CreateAddre
           //    AddressRow(location);
            }
            else
            {
                Console.WriteLine("UpdateLocation 3");
                Henspe.Current.coordinatesText = Henspe.Current.unknownCoordinates;
                Henspe.Current.addressText = Henspe.Current.unknownAddress;
            }
        }

        private void CreatePositionTextAndRefreshPositionRow(Location location)
        {
            FormattedCoordinatesDto formattedCoordinatesDto = Henspe.Current.CoordinateService.GetFormattedCoordinateDescription(CoordinateFormat.DD, location.Latitude, location.Longitude);
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

            if (wholeAddress.Length > 0)
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

        #endregion
    }
}