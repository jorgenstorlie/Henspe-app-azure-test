using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Henspe.Core.Model.Dto;
using Henspe.Droid.Adapters;
using Henspe.Droid.Util;
using System.Collections.Generic;
using System.Linq;

namespace Henspe.Droid.Adapters
{
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
        public override RecyclerView.ViewHolder OnCreateItemViewHolder(ViewGroup parent)
        {
			global::Android.Views.View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.HenspeRow, parent, false);

            var viewHolder = new ItemViewHolder(row);

            //viewHolder.txvOtherInfo.SetTextSize(Android.Util.ComplexUnitType.Sp, 10);
            //viewHolder.txvOtherInfo.SetTextColor(Android.Graphics.Color.LightGray);

            return viewHolder;
        }

        /// <summary>
        /// Let's populate Item views
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="indexPath"></param>
        public override void OnBindItemViewHolder(RecyclerView.ViewHolder holder, IndexPath indexPath)
        {
            var viewHolder = (holder as ItemViewHolder);
			HenspeRowModel henspeRowModel = this.GetItem(indexPath);

            // description
			viewHolder.description.Text = henspeRowModel.description;

            // info
			if (henspeRowModel.elementType == StructureElementDto.ElementType.Normal)
            {
				viewHolder.info.Text = "";
				viewHolder.info.Visibility = ViewStates.Gone;

				viewHolder.infoHelper.Text = "";
				viewHolder.infoHelper.Visibility = ViewStates.Gone;
            }
            else if (henspeRowModel.elementType == StructureElementDto.ElementType.Position)
            {
				viewHolder.info.Text = Henspe.Current.coordinatesText;
				viewHolder.info.Visibility = ViewStates.Visible;

				viewHolder.infoHelper.Text = "";
				viewHolder.infoHelper.Visibility = ViewStates.Visible;

				lastPositionText = FlashTextUtil.FlashChangedText(activity, activity.ApplicationContext, lastPositionText, Henspe.Current.coordinatesText, viewHolder.infoHelper, FlashTextUtil.Type.LatText);
            }
            else if (henspeRowModel.elementType == StructureElementDto.ElementType.Address)
            {
				viewHolder.info.Text = Henspe.Current.addressText;
				viewHolder.info.Visibility = ViewStates.Visible;

				viewHolder.infoHelper.Text = "";
				viewHolder.infoHelper.Visibility = ViewStates.Visible;

				lastAddressText = FlashTextUtil.FlashChangedText(activity, activity.ApplicationContext, lastAddressText, Henspe.Current.addressText, viewHolder.infoHelper, FlashTextUtil.Type.AddressText);
            }

            // image
			int imageResourceId = ResourceUtil.GetResourceIdForImagename(henspeRowModel.image);

			viewHolder.image.SetImageResource(imageResourceId);
			viewHolder.image.ScaleX = henspeRowModel.percent;
			viewHolder.image.ScaleY = henspeRowModel.percent;
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

			// Image
			int imageResourceId = ResourceUtil.GetResourceIdForImagename(henspeSectionModel.image);

			viewHolder.image.SetImageResource(imageResourceId);
			viewHolder.image.ScaleX = scale;
			viewHolder.image.ScaleY = scale;

            // Description
			if (viewHolder.description != null)
				viewHolder.description.SetText(henspeSectionModel.description, TextView.BufferType.Normal);
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
			public ImageView image { get; set; }
            public TextView description { get; set; }
            public TextView info { get; set; }
			public TextView infoHelper { get; set; }

			public ItemViewHolder(global::Android.Views.View itemView) : base(itemView)
            {
				this.image = itemView.FindViewById<ImageView>(Resource.Id.image);
                this.description = itemView.FindViewById<TextView>(Resource.Id.description);
				this.info = itemView.FindViewById<TextView>(Resource.Id.info);
				this.infoHelper = itemView.FindViewById<TextView>(Resource.Id.info_helper);
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
			public ImageView image { get; set; }
			public TextView description { get; set; }

			public SectionViewHolder(global::Android.Views.View itemView) : base(itemView)
            {
				this.image = itemView.FindViewById<ImageView>(Resource.Id.image);
				this.description = itemView.FindViewById<TextView>(Resource.Id.description);
            }
        }
    }
}