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
using Fragment = Henspe.Droid.position_fragment;

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
        public override Android.Support.V4.App.Fragment GetItem(int postion)
        {
            return mFragmentList[postion];
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


    /// <summary>
    /// Adapter for Movies by Year
    /// </summary>
    public class HenspeRowAdapter : SectionedRecyclerViewAdapter<HenspeRowModel>
    {
        /// <summary>
        /// Data to be shown
        /// </summary>
        private Dictionary<HenspeSectionModel, List<HenspeRowModel>> itemList { get; set; }

        private Android.App.Activity activity;

        private string lastPositionText = "";
        private string lastAddressText = "";

        /// <summary>
        /// Our simple constructor
        /// </summary>
        /// <param name="movies"></param>
        public HenspeRowAdapter(Dictionary<HenspeSectionModel, List<HenspeRowModel>> itemList, Android.App.Activity activity)
        {
            this.itemList = itemList;
            this.activity = activity;
        }

        /// <summary>
        /// Show section header?
        /// </summary>
        public override bool ShowHeader
        {
            get { return true; }
        }

        /// <summary>
        /// Our movies already grouped in Dictionary.
        /// So we know how many sections are required.
        /// </summary>
        /// <returns></returns>
        public override int NumbersOfSections()
        {
            return this.itemList.Keys.Count;
        }

        /// <summary>
        /// Let's set each sections item count
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public override int RowsInSection(int section)
        {
            var key = this.itemList.Keys.ElementAt(section);
            return this.itemList[key].Count();
        }

        /// <summary>
        /// Get item with (section, position) pair
        /// </summary>
        /// <param name="indexPath"></param>
        /// <returns></returns>
        public override HenspeRowModel GetItem(IndexPath indexPath)
        {
            var section = this.itemList.ElementAt(indexPath.SectionIndex);
            return section.Value[indexPath.ItemIndex.GetValueOrDefault()];
        }




        /// <summary>
        /// Let's create view for Item template
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        /// <remarks>
        /// We use Android's internal Android.Resource.Layout.SimpleListItem2 resource.
        /// You can change this with your custom XML Layout
        /// </remarks>
        public override RecyclerView.ViewHolder OnCreateItemViewHolder(ViewGroup parent, int viewType)
        {
            if (viewType == 2)
            {

                var layout2 = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.TabLayout, parent, false);
                var adressItemViewHolder = new AdressItemViewHolder(layout2);
                setupViewPager(adressItemViewHolder.viewPager);
                adressItemViewHolder.tabLayout.SetupWithViewPager(adressItemViewHolder.viewPager);
                return adressItemViewHolder;

                /*
                var layout2 = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.adress_row, parent, false);
                var adressItemViewHolder = new AdressItemViewHolder(layout2);


                Adresses adresses = new Adresses();

           AdressAdapter adapter = new AdressAdapter((activity as AppCompatActivity).SupportFragmentManager, adresses);
       adressItemViewHolder.viewPager.Adapter = adapter;

              

                return adressItemViewHolder;
                */
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
            else
                return 1;
        }

        public void setupViewPager(ViewPager viewPager)
        {
            //  InitFragment();

            position_fragment f = new position_fragment(1);
            position_fragment f2 = new position_fragment(2);

            ViewPagerAdapter adapter = new ViewPagerAdapter((activity as AppCompatActivity).SupportFragmentManager);
            adapter.addFragment(f, "Angitt posisjon ");
            adapter.addFragment(f2, "Valgt posisjon");

            viewPager.Adapter = adapter;

            Henspe.Current.PositionFragment = f;
        }


        /// <summary>
        /// Let's populate Item views
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="indexPath"></param>
        public override void OnBindItemViewHolder(RecyclerView.ViewHolder holder, IndexPath indexPath)
        {
            HenspeRowModel henspeRowModel = this.GetItem(indexPath);

            if (indexPath.SubType == 2)
            {
                var viewHolder2 = (holder as AdressItemViewHolder);

                /*
                if (viewHolder2.adress == null)
                {
                    var inflater2 = LayoutInflater.From(activity);
                    //   LayoutInflater inflater = (LayoutInflater)activity.GetSystemService("LayoutInflaterService");
                    global::Android.Views.View view2 = inflater2.Inflate(Resource.Layout.TabLayout, null);

                    viewHolder2.adress = view2;



                    // Get our button from the layout resource,
                    // and attach an event to it
                    ViewPager viewPager = (ViewPager)view2.FindViewById(Resource.Id.viewpager);


                    setupViewPager(viewPager);



                    TabLayout tabLayout = view2.FindViewById<TabLayout>(Resource.Id.sliding_tabs);
                    tabLayout.SetupWithViewPager(viewPager);
                    viewHolder2.layout.AddView(view2);

                }

                viewHolder2.info.Visibility = ViewStates.Gone;
                viewHolder2.infoHelper.Visibility = ViewStates.Gone;
                viewHolder2.image.Visibility = ViewStates.Gone;
                viewHolder2.description.Visibility = ViewStates.Gone;

                viewHolder2.adress.Visibility = ViewStates.Visible;
                */
            }
            else
            {
                var viewHolder = (holder as ItemViewHolder);


                // description
                viewHolder.description.Text = henspeRowModel.description;

                // info
                if (henspeRowModel.elementType == StructureElementDto.ElementType.Normal)
                {
                    viewHolder.image.Visibility = ViewStates.Visible;
                    viewHolder.description.Visibility = ViewStates.Visible;

                    viewHolder.info.Text = "";
                    viewHolder.info.Visibility = ViewStates.Gone;

                    viewHolder.infoHelper.Text = "";
                    viewHolder.infoHelper.Visibility = ViewStates.Gone;
                }
                else if (henspeRowModel.elementType == StructureElementDto.ElementType.Position)
                {


                    viewHolder.description.Visibility = ViewStates.Visible;

                    viewHolder.info.Text = Henspe.Current.coordinatesText;
                    viewHolder.info.Visibility = ViewStates.Visible;

                    viewHolder.infoHelper.Text = "";
                    viewHolder.infoHelper.Visibility = ViewStates.Visible;

                    lastPositionText = FlashTextUtil.FlashChangedText(activity, activity.ApplicationContext, lastPositionText, Henspe.Current.coordinatesText, viewHolder.infoHelper, FlashTextUtil.Type.LatText, Resource.Animator.abc_fade_out);
                }
                else if (henspeRowModel.elementType == StructureElementDto.ElementType.Address)
                {


                }


                // image
                //    int imageResourceId = ResourceUtil.GetResourceIdForImagename(henspeRowModel.image);
                //  int imageResourceId = ResourceUtil.GetResourceIdForImagename("ic_evakuering");
                //  viewHolder.image.SetImageResource(Resource.Drawable.ic_evakuering);


                var resourceId = (int)typeof(Resource.Drawable).GetField(henspeRowModel.image).GetValue(null);
                viewHolder.image.SetImageResource(resourceId);
                //   viewHolder.image.ImageAlpha = 0;


                Color t = new Android.Graphics.Color(ContextCompat.GetColor(activity, Resource.Color.text_normal));

                viewHolder.image.SetColorFilter(t);

                //    viewHolder.image.ScaleX = henspeRowModel.percent;
                //    viewHolder.image.ScaleY = henspeRowModel.percent;
            }
        }
        /// <summary>
        /// Let's create view for Section template
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        /// <remarks>
        /// We use Android's internal Android.Resource.Layout.SimpleListItem2 resource.
        /// You can change this with your custom XML Layout
        /// </remarks>
        public override RecyclerView.ViewHolder OnCreateSectionViewHolder(ViewGroup parent)
        {
            global::Android.Views.View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemSection, parent, false);

            var viewHolder = new SectionViewHolder(row);

            /*
            viewHolder.txvGroupName.SetTextSize(Android.Util.ComplexUnitType.Sp, 30);
            viewHolder.txvItemCount.SetTextSize(Android.Util.ComplexUnitType.Sp, 10);

            viewHolder.txvGroupName.SetTextColor(Android.Graphics.Color.Red);
            viewHolder.txvItemCount.SetTextColor(Android.Graphics.Color.Pink);
            */

            return viewHolder;
        }

        /// <summary>
        /// Let's populate Section views
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="sectionIndex"></param>
        public override void OnBindSectionViewHolder(RecyclerView.ViewHolder holder, int sectionIndex)
        {
            float scale = 0.8f;

            var viewHolder = (holder as SectionViewHolder);

            HenspeSectionModel henspeSectionModel = this.itemList.ElementAt(sectionIndex).Key;

            /*
            // Image
            int imageResourceId = ResourceUtil.GetResourceIdForImagename(henspeSectionModel.image);

            viewHolder.image.SetImageResource(imageResourceId);
            viewHolder.image.ScaleX = scale;
            viewHolder.image.ScaleY = scale;
            */


            // Description
            if (viewHolder.description != null)
                viewHolder.description.SetText(henspeSectionModel.description, TextView.BufferType.Normal);

            // Header
            if (viewHolder.header != null)
                viewHolder.header.Text = viewHolder.description.Text[0].ToString();


        }

        /// <summary>
        /// This internal class that holds ItemView's data
        /// </summary>
        /// <remarks>
        /// Text1 and Text2 Id's came from, Android.Resource.Layout.SimpleListItem2
        /// If you use custom XML layout in OnCreateItemViewHolder, you must change this Id's accordingly
        /// </remarks>
        internal class ItemViewHolder : RecyclerView.ViewHolder
        {

            public RelativeLayout layout { get; set; }
            public ImageView image { get; set; }
            public TextView description { get; set; }
            public TextView info { get; set; }
            public TextView infoHelper { get; set; }

            public ItemViewHolder(global::Android.Views.View itemView) : base(itemView)
            {
                this.layout = itemView.FindViewById<RelativeLayout>(Resource.Id.row_layout);
                this.image = itemView.FindViewById<ImageView>(Resource.Id.image);
                this.description = itemView.FindViewById<TextView>(Resource.Id.description);
                this.info = itemView.FindViewById<TextView>(Resource.Id.info);
                this.infoHelper = itemView.FindViewById<TextView>(Resource.Id.info_helper);
            }
        }


        internal class AdressItemViewHolder : RecyclerView.ViewHolder
        {
            public LinearLayout layout { get; set; }
            public ViewPager viewPager { get; set; }
            public TabLayout tabLayout { get; set; }
            public PagerTabStrip pagertab { get; set; }

            public AdressItemViewHolder(global::Android.Views.View itemView) : base(itemView)
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

        /// <summary>
        /// This internal class holds SectionView's data
        /// </summary>
        /// <remarks>
        /// Text1 and Text2 Id's came from, Android.Resource.Layout.SimpleListItem2
        /// If you use custom XML layout in OnCreateItemViewHolder, you must change this Id's accordingly
        /// </remarks>
        internal class SectionViewHolder : RecyclerView.ViewHolder
        {
            //    public ImageView image { get; set; }
            public TextView description { get; set; }
            public TextView header { get; set; }

            public SectionViewHolder(global::Android.Views.View itemView) : base(itemView)
            {
                //    this.image = itemView.FindViewById<ImageView>(Resource.Id.image);
                this.description = itemView.FindViewById<TextView>(Resource.Id.description);
                this.header = itemView.FindViewById<TextView>(Resource.Id.header);
            }
        }
    }
}