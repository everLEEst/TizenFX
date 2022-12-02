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
    public class CollectionViewGridGroupContentPage : ContentPage
    {
        CollectionView colView;
        ItemSelectionMode selMode;
        ObservableCollection<CollectionViewTest.Album> albumSource;
        CollectionViewTest.Album insertDeleteGroup = new CollectionViewTest.Album(999, "Insert / Delete Groups", new DateTime(1999, 12, 31));
        CollectionViewTest.Gallery insertMenu = new CollectionViewTest.Gallery(999, "Insert item to first of 3rd Group");
        CollectionViewTest.Gallery deleteMenu = new CollectionViewTest.Gallery(999, "Delete first item in 3rd Group");
        CollectionViewTest.Album moveGroup = new CollectionViewTest.Album(999, "move Groups", new DateTime(1999, 12, 31));
        CollectionViewTest.Gallery moveMenu = new CollectionViewTest.Gallery(999, "Move last item to first in 3rd group");

        public CollectionViewGridGroupContentPage()
        {
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            AppBar = new AppBar()
            {
                Title = "CollectionView Grid Group Sample",
            };

            albumSource = new CollectionViewTest.AlbumViewModel();
            // Add test menu options.
            moveGroup.Add(moveMenu);
            albumSource.Insert(0, moveGroup);
            insertDeleteGroup.Add(insertMenu);
            insertDeleteGroup.Add(deleteMenu);
            albumSource.Insert(0, insertDeleteGroup);

            selMode = ItemSelectionMode.Multiple;
            DefaultTitleItem myTitle = new DefaultTitleItem();
            myTitle.Text = "Grid Sample Count["+ albumSource.Count+"]";
            //Set Width Specification as MatchParent to fit the Item width with parent View.
            myTitle.WidthSpecification = LayoutParamPolicies.MatchParent;

            colView = new CollectionView()
            {
                ItemsSource = albumSource,
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
                GroupFooterTemplate = new DataTemplate(() =>
                {
                    DefaultTitleItem group = new DefaultTitleItem();
                    //Set Width Specification as MatchParent to fit the Item width with parent View.
                    group.WidthSpecification = LayoutParamPolicies.MatchParent;

                    group.Label.SetBinding(TextLabel.TextProperty, "Date");
                    group.Label.HorizontalAlignment = HorizontalAlignment.Begin;

                    return group;
                }),
                Footer = myTitle,
                IsGrouped = true,
                ScrollingDirection = ScrollableBase.Direction.Vertical,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                SelectionMode = selMode
            };
            colView.SelectionChanged += SelectionEvt;

            Content = colView;
        }

        public void SelectionEvt(object sender, SelectionChangedEventArgs ev)
        {
            List<object> oldSel = new List<object>(ev.PreviousSelection);
            List<object> newSel = new List<object>(ev.CurrentSelection);

            foreach (object item in oldSel)
            {
                if (item != null && item is Gallery)
                {
                    Gallery galItem = (Gallery)item;
                    if (!newSel.Contains(item))
                    {
                        galItem.Selected = false;
                        // Tizen.Log.Debug("Unselected: {0}", galItem.ViewLabel);
                    }
                }
                else if (item is CollectionViewTest.Album selAlbum)
                {
                    if (!newSel.Contains(selAlbum))
                    {
                        selAlbum.Selected = false;
                        // Tizen.Log.Debug("Unselected Group: {0}", selAlbum.Title);
                        if (selAlbum == insertDeleteGroup)
                        {
                            albumSource.RemoveAt(2);
                        }
                    }
                }
            }
            foreach (object item in newSel)
            {
                if (item != null && item is CollectionViewTest.Gallery)
                {
                    CollectionViewTest.Gallery galItem = (CollectionViewTest.Gallery)item;
                    if (!oldSel.Contains(item))
                    {
                        galItem.Selected = true;
                        // Tizen.Log.Debug("Selected: {0}", galItem.ViewLabel);
                        
                        if (galItem == insertMenu)
                        {
                            // Insert new item to index 3.
                            Random rand = new Random();
                            int idx = rand.Next(1000);
                            albumSource[2].Insert(3, new CollectionViewTest.Gallery(idx, "Inserted Item"));
                        }
                        else if (galItem == deleteMenu)
                        {
                            // Remove item in index 3.
                            albumSource[2].RemoveAt(0);
                        }
                        else if (galItem == moveMenu)
                        {
                            // Move last indexed item to index 3.
                            albumSource[2].Move(albumSource[2].Count - 1, 0);                   
                        }
                    }
                }
                else if (item is CollectionViewTest.Album selAlbum)
                {
                    if (!oldSel.Contains(selAlbum))
                    {
                        selAlbum.Selected = true;
                        // Tizen.Log.Debug("Selected Group: {0}", selAlbum.Title);
                        if (selAlbum == insertDeleteGroup)
                        {
                            Random rand = new Random();
                            int groupIdx = rand.Next(1000);
                            CollectionViewTest.Album insertAlbum = new CollectionViewTest.Album(groupIdx, "Inserted group", new DateTime(1999, 12, 31));
                            int idx = rand.Next(1000);
                            insertAlbum.Add(new CollectionViewTest.Gallery(idx, "Inserted Item 1"));
                            idx = rand.Next(1000);
                            insertAlbum.Add(new CollectionViewTest.Gallery(idx, "Inserted Item 2"));
                            idx = rand.Next(1000);
                            insertAlbum.Add(new CollectionViewTest.Gallery(idx, "Inserted Item 3"));
                            albumSource.Insert(2, insertAlbum);
                        }
                        else if (selAlbum == moveGroup)
                        {
                            albumSource.Move(albumSource.Count - 1, 2);
                        }
                    }
                }
            }
            if (colView.Footer != null && colView.Footer is DefaultTitleItem)
            {
                DefaultTitleItem title = (DefaultTitleItem)colView.Footer;
                title.Text = "Grid Sample Count["+ albumSource.Count + "] Selected["+newSel.Count+"]";
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

    class CollectionViewGridGroupTest13 : IExample
    {
        private Window window;

        public void Activate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new CollectionViewGridGroupContentPage());
        }

        public void Deactivate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
            window.GetDefaultNavigator().Pop();
        }
    }
}
