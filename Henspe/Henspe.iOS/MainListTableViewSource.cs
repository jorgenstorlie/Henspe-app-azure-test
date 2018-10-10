﻿// This file has been autogenerated from a class added in the UI designer.

using System;
using Foundation;
using Henspe.Core.Model.Dto;
using Henspe.iOS.Const;
using Henspe.iOS.Util;
using UIKit;
using System.Linq;

namespace Henspe.iOS
{
    // Table view source
    public partial class MainListTableViewSource : UITableViewSource
    {
        public const int LocationRow = 1;
        public const int AddressRow = 2;

        private int headerHeight = 80;
        private WeakReference<MainViewController> _parent;

        private string lastPositionText = "";
        private string lastAddressText = "";

        public StructureDto sectionsWithRows;
        private int _selectedSegment;

        public MainListTableViewSource(MainViewController controller)
        {
            _parent = new WeakReference<MainViewController>(controller);
        }

        // UITablViewSource methods
        public override nint NumberOfSections(UITableView tableView)
        {
            if (sectionsWithRows != null)
            {
                return sectionsWithRows.structureSectionList.Count;
            }
            else
            {
                return 0;
            }
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (sectionsWithRows.structureSectionList != null && sectionsWithRows.structureSectionList.Count > 0)
            {
                StructureSectionDto structureSection = sectionsWithRows.structureSectionList[(int)section];

                if (structureSection.structureElementList != null && structureSection.structureElementList.Count > 0)
                {
                    return structureSection.structureElementList.Count;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            if (sectionsWithRows.structureSectionList != null && sectionsWithRows.structureSectionList.Count > 0)
            {
                StructureSectionDto structureSection = sectionsWithRows.structureSectionList[(int)section];

                if (structureSection.structureElementList != null && structureSection.structureElementList.Count > 0)
                {
                    return headerHeight;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            StructureSectionDto structureSection = null;
            if (sectionsWithRows.structureSectionList != null && sectionsWithRows.structureSectionList.Count > 0)
            {
                structureSection = sectionsWithRows.structureSectionList[(int)section];
            }
            else
            {
                return null;
            }

            var cell = (HeaderTableCell)tableView.DequeueReusableCell("HeaderTableCell");
            cell.SetContent(structureSection.description);
            cell.ContentView.BackgroundColor = ColorConst.headerBackgroundColor;
            return cell.ContentView;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            const string normalCellIdentifier = "SingleLine";
            const string positionIdentifier = "PositionCell";
            const string addressIdentifier = "AddressCell";

            int section = indexPath.Section;
            int row = indexPath.Row;

            StructureElementDto structureElement = null;

            if (sectionsWithRows.structureSectionList != null && sectionsWithRows.structureSectionList.Count > 0)
            {
                StructureSectionDto structureSection = sectionsWithRows.structureSectionList[(int)section];

                if (structureSection.structureElementList != null && structureSection.structureElementList.Count > 0)
                {
                    structureElement = structureSection.structureElementList[row];
                }
            }

            if (structureElement == null)
                return null;

            if (structureElement.elementType == StructureElementDto.ElementType.Normal)
            {
                // Normal row
                MainNormalRowViewCell mainNormalRowViewCell = tableView.DequeueReusableCell(normalCellIdentifier, indexPath) as MainNormalRowViewCell;

                mainNormalRowViewCell.SetContent(structureElement);

                return mainNormalRowViewCell;
            }
            else if (structureElement.elementType == StructureElementDto.ElementType.Position)
            {
                // Location row
                MainLocationRowViewCell locationCell = tableView.DequeueReusableCell(positionIdentifier, indexPath) as MainLocationRowViewCell;
                locationCell.SetContent(structureElement, _selectedSegment == 0);
                return locationCell;
            }
            else if(structureElement.elementType == StructureElementDto.ElementType.Address)
            {
                var addressCell = tableView.DequeueReusableCell(addressIdentifier, indexPath) as AddressCell;
                addressCell.SetContent(structureElement, _selectedSegment == 0);
                return addressCell;
            }
            else if(structureElement.elementType == StructureElementDto.ElementType.Selector)
            {
                var segmentedCell = tableView.DequeueReusableCell("SegmentedCell", indexPath) as SegmentedCell;
                segmentedCell.SegmentSelected -= SegmentedCell_SegmentSelected;
                segmentedCell.SetContent(new [] { "Din posisjon", "Angitt posisjon" }.ToList(), _selectedSegment);
                segmentedCell.SegmentSelected += SegmentedCell_SegmentSelected;
                return segmentedCell;
            }
            else if(structureElement.elementType == StructureElementDto.ElementType.Buttons)
            {
                var buttonsCell = tableView.DequeueReusableCell("ButtonsCell", indexPath) as ButtonsCell;
                buttonsCell.SetContent();
                return buttonsCell;
            }
            else
            {
                return null;
            }
        }

        private void UpdatePosition(UILabel labLabelBottom)
        {
            labLabelBottom.Text = LangUtil.Get("GPS.UnknownPosition");

            if (AppDelegate.current.gpsCurrentPositionObject != null)
            {
                string latitudeText = AppDelegate.current.gpsCurrentPositionObject.latitudeDescription;
                string longitudeText = AppDelegate.current.gpsCurrentPositionObject.longitudeDescription;
                //string accuracySmall = LangUtil.Get("") + ": " + AppDelegate.current.gpsCurrentPositionObject.accuracy + " " + LangUtil.Get("Location.Element.Meters.Text");

                string newPositionText = latitudeText + "\n" + longitudeText;

                lastPositionText = FlashTextUtil.FlashChangedText(lastPositionText, newPositionText, labLabelBottom, FlashTextUtil.Type.Position);
            }
            else
            {
                lastPositionText = LangUtil.Get("GPS.UnknownPosition");
                labLabelBottom.Text = lastPositionText;
            }
        }

        void SegmentedCell_SegmentSelected(object sender, int e)
        {
            _selectedSegment = e;
            if (!_parent.TryGetTarget(out MainViewController parent))
                return;
            parent.RefreshPositionAndAddressRows();
        }
    }
}