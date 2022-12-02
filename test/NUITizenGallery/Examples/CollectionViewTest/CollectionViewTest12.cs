
using System;
using System.Collections.ObjectModel;
using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Binding;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    public class CollectionViewLinearGroupContentPage : ContentPage
    {
        CollectionView colView;
        string selectedItem;
        ItemSelectionMode selMode;
        ObservableCollection<CollectionViewTest.Album> albumSource;
        CollectionViewTest.Album insertDeleteGroup = new CollectionViewTest.Album(999, "Insert / Delete Groups", new DateTime(1999, 12, 31));
        CollectionViewTest.Gallery insertMenu = new CollectionViewTest.Gallery(999, "Insert item to first of 3rd Group");
        CollectionViewTest.Gallery deleteMenu = new CollectionViewTest.Gallery(999, "Delete first item in 3rd Group");
        CollectionViewTest.Album moveGroup = new CollectionViewTest.Album(999, "move Groups", new DateTime(1999, 12, 31));
        CollectionViewTest.Gallery moveMenu = new CollectionViewTest.Gallery(999, "Move last item to first in 3rd group");

        public CollectionViewLinearGroupContentPage()
        {
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            AppBar = new AppBar()
            {
                Title = "CollectionView Linear Group Sample",
            };

            albumSource = new CollectionViewTest.AlbumViewModel();
            // Add test menu options.
            moveGroup.Add(moveMenu);
            //albumSource.Insert(0, moveGroup);
            //insertDeleteGroup.Add(insertMenu);
            //insertDeleteGroup.Add(deleteMenu);
            //albumSource.Insert(0, insertDeleteGroup);

            selMode = ItemSelectionMode.Single;
            DefaultTitleItem myTitle = new DefaultTitleItem();
            //To Bind the Count property changes, need to create custom property for count.
            myTitle.Text = "Linear Sample Group["+ albumSource.Count+"]";
            //Set Width Specification as MatchParent to fit the Item width with parent View.
            myTitle.WidthSpecification = LayoutParamPolicies.MatchParent;

            colView = new CollectionView()
            {
                ItemsSource = albumSource,
                ItemsLayouter = new LinearLayouter(),
                ItemTemplate = new DataTemplate(() =>
                {
                    DefaultLinearItem item = new DefaultLinearItem();
                    //Set Width Specification as MatchParent to fit the Item width with parent View.
                    item.WidthSpecification = LayoutParamPolicies.MatchParent;
                    //Decorate Label
                    item.Label.SetBinding(TextLabel.TextProperty, "ViewLabel");
                    item.Label.HorizontalAlignment = HorizontalAlignment.Begin;
                    //Decorate Icon
                    item.Icon.SetBinding(ImageView.ResourceUrlProperty, "ImageUrl");
                    item.Icon.WidthSpecification = 50;
                    item.Icon.HeightSpecification = 40;
                    //Decorate Extra RadioButton.
                    //[NOTE] This is sample of RadioButton usage in CollectionView.
                    // RadioButton change their selection by IsSelectedProperty bindings with
                    // SelectionChanged event with Single ItemSelectionMode of CollectionView.
                    // be aware of there are no RadioButtonGroup.
                    item.Extra = new RadioButton();
                    //FIXME : SetBinding in RadioButton crashed as Sensitive Property is disposed.
                    //item.Extra.SetBinding(RadioButton.IsSelectedProperty, "Selected");
                    item.Extra.WidthSpecification = 40;
                    item.Extra.HeightSpecification = 40;

                    return item;
                }),
                GroupHeaderTemplate = new DataTemplate(() =>
                {
                    DefaultTitleItem group = new DefaultTitleItem();
                    //Set Width Specification as MatchParent to fit the Item width with parent View.
                    group.WidthSpecification = LayoutParamPolicies.MatchParent;

                    group.Label.SetBinding(TextLabel.TextProperty, "Date");
                    group.Label.HorizontalAlignment = HorizontalAlignment.Begin;

                    return group;
                }),
                //Header = myTitle,
                IsGrouped = true,
                ScrollingDirection = ScrollableBase.Direction.Vertical,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                SelectionMode = selMode
            };
            colView.SelectionChanged += SelectionEvt;

                albumSource[0].Add(new CollectionViewTest.Gallery(0, "Galaxy Note 10"));
                albumSource[0].Add(new CollectionViewTest.Gallery(1, "Galaxy Buds2 Pro"));
                albumSource[0].Add(new CollectionViewTest.Gallery(2, "Sony WH-1000XM5"));
                albumSource[0].Add(new CollectionViewTest.Gallery(3, "Logitech Anywhere M2"));

            Content = colView;

            var timer = new Timer(1000);
            timer.Interval = 1000;


            timer.Tick += ((object target, Timer.TickEventArgs args) =>
            {
                Console.WriteLine("Timer Called!!!!!!!!!!!!!!!!!!!!!!!!!");
                var newalbum = new CollectionViewTest.Album(0, "New Album", new DateTime(2022, 11, 30));
                newalbum.Add(new CollectionViewTest.Gallery(0, "Logitech Anywhere M2"));
                newalbum.Add(new CollectionViewTest.Gallery(0, "Sony WH-1000XM5"));
                albumSource.Insert(0, newalbum);
                return false;
            });

            timer.Start();

            //colView.ScrollTo(20, true);
        }

        public void SelectionEvt(object sender, SelectionChangedEventArgs ev)
        {
            //Tizen.Log.Debug("NUI", "LSH :: SelectionEvt called");
            foreach (object item in ev.CurrentSelection)
            {
                if (item == null) break;
                if (item is CollectionViewTest.Gallery selItem)
                {
                    selItem.Selected = true;
                    selectedItem = selItem.Name;

                    Console.WriteLine("LSH :: Selected: {0}", selItem.ViewLabel);

                    if (albumSource[0].Contains(selItem))
                    {
                        albumSource[0].Remove(selItem);
                        //selItem.Dispose();


                        if (albumSource[0].Count == 0)
                        {
                            Console.WriteLine("album will be removed!");
                            albumSource.RemoveAt(0);
                        }
                    }

                }
            }
        }

        protected override void Dispose(DisposeTypes type)
        {
            if (Disposed)
            {
                return;
            }

            if (type == DisposeTypes.Explicit)
            {
                Deactivate();
            }

            base.Dispose(type);
        }

        private void Deactivate()
        {
            if (colView != null)
            {
                colView.Dispose();
            }
        }
    }

    class CollectionViewLinearGroupTest12 : IExample
    {
        private Window window;

        public void Activate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new CollectionViewLinearGroupContentPage());
        }

        public void Deactivate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
            window.GetDefaultNavigator().Pop();
        }
    }
}
