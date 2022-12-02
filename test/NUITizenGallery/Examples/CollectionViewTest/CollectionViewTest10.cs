
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Binding;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class CollectionViewGridContentPage : ContentPage
    {
        CollectionView colView;
        int itemCount = 500;
        ItemSelectionMode selMode;
        ObservableCollection<CollectionViewTest.Gallery> gallerySource;
        CollectionViewTest.Gallery insertMenu = new CollectionViewTest.Gallery(999, "Insert item to 3rd");
        CollectionViewTest.Gallery deleteMenu = new CollectionViewTest.Gallery(999, "Delete item at 3rd");
        CollectionViewTest.Gallery moveMenu = new CollectionViewTest.Gallery(999, "Move last item to 3rd");

        public CollectionViewGridContentPage()
        {
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            AppBar = new AppBar()
            {
                Title = "CollectionView Grid Sample",
            };

            var myViewModelSource = gallerySource = new CollectionViewTest.GalleryViewModel(itemCount);
            // Add test menu options.
            gallerySource.Insert(0, moveMenu);
            gallerySource.Insert(0, deleteMenu);
            gallerySource.Insert(0, insertMenu);

            selMode = ItemSelectionMode.Multiple;
            DefaultTitleItem myTitle = new DefaultTitleItem();
            myTitle.Text = "Grid Sample Count["+itemCount+"]";
            //Set Width Specification as MatchParent to fit the Item width with parent View.
            myTitle.WidthSpecification = LayoutParamPolicies.MatchParent;

            colView = new CollectionView()
            {
                ItemsSource = myViewModelSource,
                ItemsLayouter = new GridLayouter(),
                ItemTemplate = new DataTemplate(() =>
                {
                    DefaultGridItem item = new DefaultGridItem();
                    item.WidthSpecification = 180;
                    item.HeightSpecification = 240;
                    //Decorate Label
                    item.Label.SetBinding(TextLabel.TextProperty, "ViewLabel");
                    item.Label.HorizontalAlignment = HorizontalAlignment.Center;
                    //Decorate Image
                    item.Image.SetBinding(ImageView.ResourceUrlProperty, "ImageUrl");
                    item.Image.WidthSpecification = 170;
                    item.Image.HeightSpecification = 170;
                    //Decorate Badge checkbox.
                    //[NOTE] This is sample of CheckBox usage in CollectionView.
                    // Checkbox change their selection by IsSelectedProperty bindings with
                    // SelectionChanged event with Mulitple ItemSelectionMode of CollectionView.
                    item.Badge = new CheckBox();
                    //FIXME : SetBinding in RadioButton crashed as Sensitive Property is disposed.
                    //item.Badge.SetBinding(CheckBox.IsSelectedProperty, "Selected");
                    item.Badge.WidthSpecification = 30;
                    item.Badge.HeightSpecification = 30;

                    return item;
                }),
                Header = myTitle,
                ScrollingDirection = ScrollableBase.Direction.Vertical,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                SelectionMode = selMode
            };
            colView.SelectionChanged += SelectionEvt;

            List<object> selItems = new List<object>();
            selItems.Add(insertMenu);
            colView.UpdateSelectedItems(selItems);

            Content = colView;
        }

        public void SelectionEvt(object sender, SelectionChangedEventArgs ev)
        {
            List<object> oldSel = new List<object>(ev.PreviousSelection);
            List<object> newSel = new List<object>(ev.CurrentSelection);

            foreach (object item in oldSel)
            {
                if (item != null && item is CollectionViewTest.Gallery)
                {
                    CollectionViewTest.Gallery galItem = (CollectionViewTest.Gallery)item;
                    if (!(newSel.Contains(item)))
                    {
                        galItem.Selected = false;
                        Tizen.Log.Debug("Unselected: {0}", galItem.ViewLabel);
                    }
                }
                else
                {
                    continue;
                }
            }
            foreach (object item in newSel)
            {
                if (item != null && item is CollectionViewTest.Gallery)
                {
                    CollectionViewTest.Gallery galItem = (CollectionViewTest.Gallery)item;
                    if (!(oldSel.Contains(item)))
                    {
                        galItem.Selected = true;
                        Tizen.Log.Debug("Selected: {0}", galItem.ViewLabel);

                        // Check test menu options.
                        if (galItem == insertMenu)
                        {
                            // Insert new item to index 3.
                            //Random rand = new Random();
                            //int idx = rand.Next(1000);
                            int idx = 10;
                            gallerySource.Insert(3, new CollectionViewTest.Gallery(idx, "Inserted Item"));
                        }
                        else if (galItem == deleteMenu)
                        {
                            // Remove item in index 3.
                            gallerySource.RemoveAt(3);
                        }
                        else if (galItem == moveMenu)
                        {
                            // Move last indexed item to index 3.
                            gallerySource.Move(gallerySource.Count - 1, 3);
                        }
                    }

                }
                else
                {
                    continue;
                }
            }
            if (colView.Header != null && colView.Header is DefaultTitleItem)
            {
                DefaultTitleItem title = (DefaultTitleItem)colView.Header;
                title.Text = "Grid Sample Count["+gallerySource.Count+"] Selected["+newSel.Count+"]";
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

    class CollectionViewGridTest10 : IExample
    {
        private Window window;

        public void Activate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new CollectionViewGridContentPage());
        }

        public void Deactivate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
            window.GetDefaultNavigator().Pop();
        }
    }
}
