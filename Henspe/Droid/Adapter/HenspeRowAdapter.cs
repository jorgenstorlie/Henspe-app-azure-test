using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Henspe.Core.Model.Dto;
using Henspe.Droid.Adapters;
using Java.Lang;
using System.Collections.Generic;
using System.Linq;
using SNLA.Droid.Util;
using System;
using Xamarin.Essentials;
using Android.App;

namespace Henspe.Droid
{

    public class ViewPagerAdapter : FragmentPagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> mFragmentList = new List<Android.Support.V4.App.Fragment>();
        private List<string> mFragmentTitleList = new List<string>();

        public ViewPagerAdapter(Android.Support.V4.App.FragmentManager manager) : base(manager)
        {
            //base.OnCreate(manager);
        }

        public override int Count
        {
            get
            {
                return mFragmentList.Count;
            }
        }
        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return mFragmentList[position];
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(mFragmentTitleList[position].ToLower());// display the title
            //return null;// display only the icon
        }

        public void addFragment(Android.Support.V4.App.Fragment fragment, string title)
        {
            mFragmentList.Add(fragment);
            mFragmentTitleList.Add(title);
        }
    }

    public class HenspeRowAdapter : SectionedRecyclerViewAdapter<HenspeRowModel>
    {
        private Dictionary<HenspeSectionModel, List<HenspeRowModel>> itemList { get; set; }
        private Android.App.Activity activity;
        private string lastPositionText = "";
        private string lastAddressText = "";

        public HenspeRowAdapter(Dictionary<HenspeSectionModel, List<HenspeRowModel>> itemList, Android.App.Activity activity)
        {
            this.itemList = itemList;
            this.activity = activity;
        }

        public override bool ShowHeader
        {
            get { return true; }
        }

        public override int NumbersOfSections()
        {
            return this.itemList.Keys.Count;
        }

        public override int RowsInSection(int section)
        {
            var key = this.itemList.Keys.ElementAt(section);
            return this.itemList[key].Count();
        }

        public override HenspeRowModel GetItem(IndexPath indexPath)
        {
            var section = this.itemList.ElementAt(indexPath.SectionIndex);
            return section.Value[indexPath.ItemIndex.GetValueOrDefault()];
        }

        public override RecyclerView.ViewHolder OnCreateItemViewHolder(ViewGroup parent, int viewType)
        {
            if (viewType == 2)
            {
                var layout2 = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.TabLayout, parent, false);
                var adressItemViewHolder = new AdressItemViewHolder(layout2);
                setupViewPager(adressItemViewHolder.viewPager);
                adressItemViewHolder.tabLayout.SetupWithViewPager(adressItemViewHolder.viewPager);
                return adressItemViewHolder;
            }
            else if (viewType == 3)
            {
                var layout = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.position_row, parent, false);
                return new PositionViewHolder(layout);
            }
            else
            {
                var layout = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.HenspeRow, parent, false);
                return new ItemViewHolder(layout);
            }
        }

        public override int GetSubType(IndexPath indexPath)
        {
            if (GetItem(indexPath).elementType == StructureElementDto.ElementType.Address)
                return 2;
            else if (GetItem(indexPath).elementType == StructureElementDto.ElementType.Position)
                return 3;
            else return 1;
        }

        public void setupViewPager(ViewPager viewPager)
        {
            position_fragment f = new position_fragment(1);
            position_fragment f2 = new position_fragment(2);

            ViewPagerAdapter adapter = new ViewPagerAdapter((activity as AppCompatActivity).SupportFragmentManager);
            adapter.addFragment(f, "Angitt posisjon ");
            adapter.addFragment(f2, "Valgt posisjon");
            viewPager.Adapter = adapter;
            Henspe.Current.PositionFragment = f;
        }

        public override void OnBindItemViewHolder(RecyclerView.ViewHolder holder, IndexPath indexPath)
        {
            HenspeRowModel henspeRowModel = this.GetItem(indexPath);

            Color t;
            if (IsOdd(indexPath.SectionIndex))
                t = new Color(ContextCompat.GetColor(activity, Resource.Color.evenrow));
            else
                t = new Color(ContextCompat.GetColor(activity, Resource.Color.oddrow));

            if (indexPath.SubType == 2)
            {
                var viewHolder2 = (holder as AdressItemViewHolder);
            }
            else if (indexPath.SubType == 3)
            {
                var positionViewHolder = (holder as PositionViewHolder);
                positionViewHolder.description.Visibility = ViewStates.Gone;

                positionViewHolder.layout.SetBackgroundColor(t);

                positionViewHolder.info.Text = Henspe.Current.coordinatesText;
                positionViewHolder.info.Visibility = ViewStates.Visible;

                positionViewHolder.infoHelper.Text = "";
                positionViewHolder.infoHelper.Visibility = ViewStates.Visible;

                lastPositionText = FlashTextUtil.FlashChangedText(activity, activity.ApplicationContext, lastPositionText, Henspe.Current.coordinatesText, positionViewHolder.infoHelper, FlashTextUtil.Type.LatText, Resource.Animator.abc_fade_out);

                positionViewHolder.description2.Visibility = ViewStates.Gone;

                positionViewHolder.info2.Text = Henspe.Current.addressText;
                positionViewHolder.info2.Visibility = ViewStates.Visible;

                positionViewHolder.infoHelper2.Text = "";
                positionViewHolder.infoHelper2.Visibility = ViewStates.Visible;
                lastAddressText = FlashTextUtil.FlashChangedText(activity, activity.ApplicationContext, lastAddressText, Henspe.Current.addressText, positionViewHolder.infoHelper2, FlashTextUtil.Type.LatText, Resource.Animator.abc_fade_out);



                positionViewHolder.image.SetImageResource(Resource.Drawable.ic_posisjon);
                positionViewHolder.image2.SetImageResource(Resource.Drawable.ic_adresse);
                Color tt = new Android.Graphics.Color(ContextCompat.GetColor(activity, Resource.Color.icon_color));
                positionViewHolder.image.SetColorFilter(tt);
                positionViewHolder.image2.SetColorFilter(tt);
            }
            else
            {
                var viewHolder = (holder as ItemViewHolder);


                viewHolder.layout.SetBackgroundColor(t);

                // description
                viewHolder.description.Text = henspeRowModel.description;

                // info
                if (henspeRowModel.elementType == StructureElementDto.ElementType.Normal)
                {
                    viewHolder.image.Visibility = ViewStates.Visible;
                    viewHolder.description.Visibility = ViewStates.Visible;
                }

                else if (henspeRowModel.elementType == StructureElementDto.ElementType.Address)
                {

                }

                var resourceId = (int)typeof(Resource.Drawable).GetField(henspeRowModel.image).GetValue(null);
                viewHolder.image.SetImageResource(resourceId);
                Color tt = new Android.Graphics.Color(ContextCompat.GetColor(activity, Resource.Color.icon_color));
                viewHolder.image.SetColorFilter(tt);
            }
        }

        public override RecyclerView.ViewHolder OnCreateSectionViewHolder(ViewGroup parent)
        {
            global::Android.Views.View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemSection, parent, false);
            var viewHolder = new SectionViewHolder(row);
            return viewHolder;
        }

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        public override void OnBindSectionViewHolder(RecyclerView.ViewHolder holder, int sectionIndex)
        {
            var viewHolder = (holder as SectionViewHolder);

            HenspeSectionModel henspeSectionModel = this.itemList.ElementAt(sectionIndex).Key;

            // Description
            if (viewHolder.description != null)
                viewHolder.description.SetText(henspeSectionModel.description, TextView.BufferType.Normal);

            // Header
            if (viewHolder.header != null)
                viewHolder.header.Text = viewHolder.description.Text[0].ToString();

            Color t;
            if (IsOdd(sectionIndex))
                t = new Color(ContextCompat.GetColor(activity, Resource.Color.evenrow));
            else
                t = new Color(ContextCompat.GetColor(activity, Resource.Color.oddrow));

            viewHolder.layout.SetBackgroundColor(t);
        }

        internal class ItemViewHolder : RecyclerView.ViewHolder
        {
            public RelativeLayout layout { get; set; }
            public ImageView image { get; set; }
            public TextView description { get; set; }


            public ItemViewHolder(View itemView) : base(itemView)
            {
                this.layout = itemView.FindViewById<RelativeLayout>(Resource.Id.row_layout);
                this.image = itemView.FindViewById<ImageView>(Resource.Id.image);
                this.description = itemView.FindViewById<TextView>(Resource.Id.description);
            }
        }

        internal class PositionViewHolder : RecyclerView.ViewHolder
        {
            public RelativeLayout layout { get; set; }
            public ImageView image { get; set; }
            public TextView description { get; set; }
            public TextView info { get; set; }
            public TextView infoHelper { get; set; }

            public ImageView image2 { get; set; }
            public TextView description2 { get; set; }
            public TextView info2 { get; set; }
            public TextView infoHelper2 { get; set; }

            public Button share { get; set; }
            public Button map { get; set; }

            public PositionViewHolder(View itemView) : base(itemView)
            {
                this.layout = itemView.FindViewById<RelativeLayout>(Resource.Id.row_layout);
                this.image = itemView.FindViewById<ImageView>(Resource.Id.image);
                this.description = itemView.FindViewById<TextView>(Resource.Id.description);
                this.info = itemView.FindViewById<TextView>(Resource.Id.info);
                this.infoHelper = itemView.FindViewById<TextView>(Resource.Id.info_helper);

                this.image2 = itemView.FindViewById<ImageView>(Resource.Id.image2);
                this.description2 = itemView.FindViewById<TextView>(Resource.Id.description2);
                this.info2 = itemView.FindViewById<TextView>(Resource.Id.info2);
                this.infoHelper2 = itemView.FindViewById<TextView>(Resource.Id.info_helper2);

                this.share = itemView.FindViewById<Button>(Resource.Id.shareposition);
                this.map = itemView.FindViewById<Button>(Resource.Id.showmap);

                share.Click += ShareClicked;
                map.Click += MapClicked;
            }

            private void MapClicked(object sender, EventArgs e)
            {
                if (Henspe.Current.myLocation != null)
                {
                    var location = new Location(Henspe.Current.myLocation.Latitude, Henspe.Current.myLocation.Longitude);

                    NavigationMode mode = NavigationMode.None;
                    string name = "HENSPE";
                    var options = new MapLaunchOptions
                    {
                        Name = name,
                        NavigationMode = mode
                    };

                    Map.OpenAsync(location);
                }
            }

            private void ShareClicked(object sender, EventArgs e)
            {
                if (Henspe.Current.myLocation != null)
                {
                    string latString = Henspe.Current.coordinatesText;
                    string lonString = Henspe.Current.addressText;
                    string posString = latString + "\n" + lonString;
                    string lat = Henspe.Current.myLocation.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    string lon = Henspe.Current.myLocation.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    string linkString = "https://www.google.com/maps/search/?api=1&query=" + lat + "," + lon;
                    string common_share_my_position = Application.Context.Resources.GetString(Resource.String.common_share_my_position);

                    posString = posString + "\n" + linkString;
                    var shareTextRequest = new ShareTextRequest(posString, common_share_my_position);
                    Share.RequestAsync(shareTextRequest);
                }
            }
        }

        internal class AdressItemViewHolder : RecyclerView.ViewHolder
        {
            public LinearLayout layout { get; set; }
            public ViewPager viewPager { get; set; }
            public TabLayout tabLayout { get; set; }
            public PagerTabStrip pagertab { get; set; }

            public AdressItemViewHolder(View itemView) : base(itemView)
            {
                //https://guides.codepath.com/android/google-play-style-tabs-using-tablayout
                //https://guides.codepath.com/android/ViewPager-with-FragmentPagerAdapter

                this.viewPager = itemView.FindViewById<ViewPager>(Resource.Id.viewpager);
                this.tabLayout = itemView.FindViewById<TabLayout>(Resource.Id.sliding_tabs);

                this.viewPager.NestedScrollingEnabled = true;
                this.tabLayout.NestedScrollingEnabled = true;

                // this.viewPager = itemView.FindViewById<ViewPager>(Resource.Id.pager);
                //     this.pagertab = itemView.FindViewById<PagerTabStrip>(Resource.Id.pagertab);
                //
            }
        }

        internal class SectionViewHolder : RecyclerView.ViewHolder
        {
            //    public ImageView image { get; set; }
            public LinearLayout layout { get; set; }
            public TextView description { get; set; }
            public TextView header { get; set; }

            public SectionViewHolder(global::Android.Views.View itemView) : base(itemView)
            {
                //    this.image = itemView.FindViewById<ImageView>(Resource.Id.image);
                this.layout = itemView.FindViewById<LinearLayout>(Resource.Id.headerbg);
                this.description = itemView.FindViewById<TextView>(Resource.Id.description);
                this.header = itemView.FindViewById<TextView>(Resource.Id.header);
            }
        }
    }
}