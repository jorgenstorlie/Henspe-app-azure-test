using Android.Views;
using Android.OS;
using NavUtils = Android.Support.V4.App.NavUtils;
using System.Collections.Generic;
using System;
using Henspe.Core.Model.Dto;
using Android.Support.V7.Widget;
using Android.Support.V7.App;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Locations;
using Henspe.Core.Service;

namespace Henspe.Droid
{
    class HenspeFragment : Fragment
    {
        public const string parameterSelectedBase = "com.computas.latsamband.onduty.selectedBase";
        private HenspeRowAdapter _itemsAdapter;
        private RecyclerView _recyclerView;
        private ViewGroup root;
        private bool isGrantedPermissionGps;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
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
            return root;
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

        /*
    public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
    {
        inflater.Inflate(Resource.Menu.MenuHome, menu);
    }
    */

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
            base.OnDetach();
        }

        public bool OnNavigationItemSelected(int itemPosition, long itemId)
        {
            throw new NotImplementedException();
        }

        public void RefreshRow(int index)
        {
            //jls    if (viewCreated && _itemsAdapter != null)
            _itemsAdapter.NotifyItemChanged(index);
        }

        internal void UpdateLocation(Location location)
        {
            CreatePositionTextAndRefreshPositionRow(location);
            CreateAddressTextAndRefreshAddressRow(location);
            RefreshRow(4);
        }

        private void CreatePositionTextAndRefreshPositionRow(Location location)
        {
            FormattedCoordinatesDto formattedCoordinatesDto = Henspe.Current.CoordinateService.GetFormattedCoordinateDescription(CoordinateFormat.DDM, location.Latitude, location.Longitude);
            Henspe.Current.coordinatesText = formattedCoordinatesDto.latitudeDescription + "\n" + formattedCoordinatesDto.longitudeDescription;
            //    RefreshRow(4);
        }

        private void CreateAddressTextAndRefreshAddressRow(Location location)
        {
            Geocoder geocoder = new Geocoder(this.Activity);
            IList<Address> addresses = geocoder.GetFromLocation(location.Latitude, location.Longitude, 1);
            string wholeAddress = "";

            try
            {
                var dd = addresses[0];
                wholeAddress = dd.Thoroughfare + " " + dd.SubThoroughfare;
                wholeAddress = wholeAddress + System.Environment.NewLine  + (dd.SubLocality + ", " ?? string.Empty) + dd.Locality;
            }
            catch (System.ArgumentOutOfRangeException aoore)
            {
            }

            /*
            try
              {
                  wholeAddress = addresses[0].GetAddressLine(0);
              }
              catch (System.ArgumentOutOfRangeException aoore)
              {
              }
              */

            if (wholeAddress.Length > 0)
                Henspe.Current.addressText = wholeAddress;
            else
                Henspe.Current.addressText = Henspe.Current.unknownAddress;

            //    RefreshRow(5);

        }


    }
}