/*
 * Copyright(c) 2022 Samsung Electronics Co., Ltd.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */
using System;
using System.ComponentModel;
using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using Tizen.NUI.Binding;

namespace Tizen.NUI.StyleGuide
{
    // IExample inehrited class will be automatically added in the main examples list.
    internal class CollectionViewExample : ContentPage, IExample
    {
        private Window window;
        private List<ColViewOption> coViewMenu;
        public void Activate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()\n");
        }
        public void Deactivate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()\n");
            window = null;
            coViewMenu = null;
        }

        /// Modify this method for adding other examples.
        public CollectionViewExample() : base()
        {
            Log.Info(this.GetType().Name, $"{this.GetType().Name} is contructed\n");

            // Navigator bar title is added here.
            AppBar = new AppBar()
            {
                Title = "CollectionView Default Style",
            };

            coViewMenu = new List<ColViewOption>();
            coViewMenu.Add(new ColViewOption("Vertical", "Linear"));
            coViewMenu.Add(new ColViewOption("Horizontal", "Linear"));
            coViewMenu.Add(new ColViewOption("Vertical", "Grid"));
            coViewMenu.Add(new ColViewOption("Horizontal", "Grid"));

            // Example root content view.
            // you can decorate, add children on this view.
            // ScrollableBase need two different style guide.
            // so here we adding new CollectionView for 2-depth option.

            var rootView = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size2D(10, 10)
                }
            };

            var playBtn = new Button()
            {
                IsSelectable = true,
                Text = "Paused"
            };

            playBtn.Clicked += (object obj, ClickedEventArgs ev) =>
            {
                var btn = obj as Button;
                Log.Info(this.GetType().Name, $"btn clicked {btn.IsSelected}\n");
                if (btn.IsSelected)
                {
                    btn.Text = "Playing";
                    coViewMenu[0].IsPlaying = true;
                }
                else
                {
                    btn.Text = "Paused";
                    coViewMenu[0].IsPlaying = false;
                }
            };

            rootView.Add(playBtn);


            var colViewOptionView = new CollectionView()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                ItemsSource = coViewMenu,
                ItemsLayouter = new LinearLayouter(),
                ItemTemplate = new DataTemplate(() =>
                {
                    ListItemLayout item = new ListItemLayout()
                    {
                        WidthSpecification = LayoutParamPolicies.MatchParent,
                    };
                    item.TitleLabel.SetBinding(TextLabel.TextProperty, "Direction");
                    item.TitleLabel.HorizontalAlignment = HorizontalAlignment.Begin;
                    item.SubtitleLabel.SetBinding(TextLabel.TextProperty, "Layouter");
                    item.SubtitleLabel.HorizontalAlignment = HorizontalAlignment.Begin;

                    item.Icon.SetBinding(ImageView.ResourceUrlProperty, "ImgUrl");
                    item.SetBinding(ListItemLayout.IsPlayingProperty, "IsPlaying");
                    return item;
                }),
                ScrollingDirection = ScrollableBase.Direction.Vertical,
                SelectionMode = ItemSelectionMode.SingleAlways,
            };
            Log.Info(this.GetType().Name, $"{colViewOptionView} created!\n");
            colViewOptionView.SelectionChanged += (object colView, SelectionChangedEventArgs ev) =>
            {
                if (ev.CurrentSelection == null || ev.CurrentSelection.Count == 0) return;
                if (ev.CurrentSelection[0] is ColViewOption colViewItem)
                {
                    Log.Info(this.GetType().Name, $"{colViewItem.Direction} will be activated!\n");
                }
                colViewOptionView.SelectedItem = null;
            };
            rootView.Add(colViewOptionView);

            Content = rootView;
            Log.Info(this.GetType().Name, $"{colViewOptionView} done!\n");
        }
    }

    internal class ColViewOption : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
        private string direction;
        private string layouter;
        private bool isPlaying;
        public string Direction { get => direction; }
        public string Layouter { get => layouter; }
        public bool IsPlaying
        {
            get => isPlaying;
            set
            {
                isPlaying = value;
                OnPropertyChanged("IsPlaying");
            }
        }
        public string ImgUrl { get => Tizen.Applications.Application.Current.DirectoryInfo.Resource + "images/picture.png"; }

        public ColViewOption(string dir, string lay)
        {
            direction = dir;
            layouter = lay;
            isPlaying = false;
        }
    }
}
