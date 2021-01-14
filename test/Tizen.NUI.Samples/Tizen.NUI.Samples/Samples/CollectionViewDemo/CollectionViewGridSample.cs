﻿using System.Collections.Generic;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using Tizen.NUI.Binding;

namespace Tizen.NUI.Samples
{
    public class CollectionViewGridSample : IExample
    {
        CollectionView colView;
        int itemCount = 500;
        int selectedCount;
        ItemSelectionMode selMode;
        ViewItemStyle titleStyle = new ViewItemStyle()
        {
            Name = "titleStyle",
            BackgroundColor = new Selector<Color>()
            {
                Normal = new Color(0.972F, 0.952F, 0.749F, 1),
                Pressed = new Color(0.1F, 0.85F, 0.85F, 1),
                Disabled = new Color(0.70F, 0.70F, 0.70F, 1),
                Selected = new Color(0.701F, 0.898F, 0.937F, 1)
            }
        };
        class SampleGridTitleItem : OneLineLinearItem
        {
            public SampleGridTitleItem(ViewItemStyle titleStyle) : base(titleStyle)
            {
                WidthSpecification = LayoutParamPolicies.MatchParent;
                HeightSpecification = 120;
            }
        }

        class SampleGridItem : OutLineGridItem
        {
            public SampleGridItem() : base()
            {
                WidthSpecification = 180;
                HeightSpecification = 240;
            }
        }

        public void Activate()
        {
            Window window = NUIApplication.GetDefaultWindow();

            List<Gallery> myViewModelSource = new GalleryViewModel().CreateData(itemCount);
            selMode = ItemSelectionMode.MultipleSelections;
            SampleGridTitleItem myTitle = new SampleGridTitleItem(titleStyle);
            myTitle.Text = "Grid Sample Count["+itemCount+"] Selected["+selectedCount+"]";
            myTitle.Label.PointSize = 20;

            colView = new CollectionView()
            {
                ItemsSource = myViewModelSource,
                ItemsLayouter = new GridLayouter(),
                ItemTemplate = new DataTemplate(() =>
                {
                    SampleGridItem item = new SampleGridItem();
                    //Decorate Label
                    item.Label.SetBinding(TextLabel.TextProperty, "ViewLabel");
                    item.Label.HorizontalAlignment = HorizontalAlignment.Center;
                    item.LabelPadding = new Extents(5, 5, 5, 5);
                    //Decorate Image
                    item.Image.SetBinding(ImageView.ResourceUrlProperty, "ImageUrl");
                    item.Image.WidthSpecification = 170;
                    item.Image.HeightSpecification = 170;
                    item.ImagePadding = new Extents(5, 5, 5, 5);
                    //Decorate Badge checkbox.
                    //[NOTE] This is sample of CheckBox usage in CollectionView.
                    // Checkbox change their selection by IsSelectedProperty bindings with
                    // SelectionChanged event with MulitpleSelections ItemSelectionMode of CollectionView.
                    item.Badge = new CheckBox();
                    item.Badge.SetBinding(CheckBox.IsSelectedProperty, "Selected");
                    item.Badge.WidthSpecification = 30;
                    item.Badge.HeightSpecification = 30;
                    item.BadgePadding = new Extents(2, 2, 2, 2);

                    return item;
                }),
                Header = myTitle,
                ScrollingDirection = ScrollableBase.Direction.Vertical,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
				SelectionMode = selMode
            };
            colView.SelectionChanged += SelectionEvt;

            window.Add(colView);
        }

        public void SelectionEvt(object sender, SelectionChangedEventArgs ev)
        {
            List<object> oldSel = new List<object>(ev.PreviousSelection);
            List<object> newSel = new List<object>(ev.CurrentSelection);

            foreach (object item in oldSel)
            {
                if (item is Gallery galItem)
                {
                    if (!(newSel.Contains(item)))
                    {
                        galItem.Selected = false;
                        Tizen.Log.Debug("Unselected: {0}", galItem.ViewLabel);
                        selectedCount--;
                    }
                }
                else continue;
            }
            foreach (object item in newSel)
            {
                if (item is Gallery galItem)
                {
                    if (!(oldSel.Contains(item)))
                    {
                        galItem.Selected = true;
                        Tizen.Log.Debug("Selected: {0}", galItem.ViewLabel);
                        selectedCount++;
                    }
                }
                else continue;
            }
            if (colView.Header != null && colView.Header is SampleGridTitleItem title)
            {
                title.Text = "Grid Sample Count["+itemCount+"] Selected["+selectedCount+"]";
            }
        }

        public void Deactivate()
        {
            if (colView != null)
            {
                colView.Dispose();
            }
        }
    }
}
