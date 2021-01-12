using System.Collections.Generic;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using Tizen.NUI.Binding;

namespace Tizen.NUI.Samples
{
    public class CollectionViewLinearSample : IExample
    {
        CollectionView colView;
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
        class SampleLinearTitleItem : OneLineLinearItem
        {
            public SampleLinearTitleItem(ViewItemStyle titleStyle) : base(titleStyle)
            {
                WidthSpecification = LayoutParamPolicies.MatchParent;
                HeightSpecification = 100;
            }
        }

        class SampleLinearItem : OneLineLinearItem
        {
            public SampleLinearItem() : base()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent;
                HeightSpecification = 130;
            }
        }

        public void Activate()
        {
            Window window = NUIApplication.GetDefaultWindow();

            List<Gallery> myViewModelSource = new GalleryViewModel().CreateData(500);

            colView = new CollectionView()
            {
                ItemsSource = myViewModelSource,
                ItemsLayouter = new LinearLayouter(),
                ItemTemplate = new DataTemplate(() => {
                    SampleLinearItem item = new SampleLinearItem();
                        item.Label.SetBinding(TextLabel.TextProperty, "ViewLabel");
						item.Label.HorizontalAlignment=HorizontalAlignment.Begin;
						item.LabelPadding = new Extents(5, 5, 5, 5);
						item.Icon.SetBinding(ImageView.ResourceUrlProperty, "ImageUrl");
						item.Icon.WidthSpecification = 90;
						item.Icon.HeightSpecification = 90;
						item.IconPadding = new Extents(4, 4, 4, 4);
                    return item;
                }),
                Header = new SampleLinearTitleItem(titleStyle)
                {
                    Text = "Linear Layout Sample : [" + myViewModelSource.Count +"]"
                },
                ScrollingDirection = ScrollableBase.Direction.Vertical,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
				SelectionMode = ItemSelectionMode.SingleSelection
            };
            colView.SelectionChanged += SelectionEvt;

            window.Add(colView);

        }

        public void SelectionEvt(object sender, SelectionChangedEventArgs ev)
        {
            foreach (object item in ev.PreviousSelection)
            {
                Tizen.Log.Debug("Unselected: {0}", (item as Gallery)?.ViewLabel);
            }
            foreach (object item in ev.CurrentSelection)
            {
                Tizen.Log.Debug("Selected: {0}", (item as Gallery)?.ViewLabel);
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
