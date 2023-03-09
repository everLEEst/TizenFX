/*
 * Copyright(c) 2023 Samsung Electronics Co., Ltd.
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
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace Tizen.Guide.Samples
{
    // ISample inehrited class will be automatically added in the main examples list.
    internal class TooltipSample : ContentPage, ISample
    {
        private MenuItem[] tooltipItem = new MenuItem[2];
        private Tooltip tooltip;

        public void Activate()
        {
        }
        public void Deactivate()
        {
        }

        /// Modify this method for adding other examples.
        public TooltipSample() : base()
        {
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            // Navigator bar title is added here.
            AppBar = new AppBar()
            {
                Title = "Tooltip Sample",
            };

            // Example root content view.
            // you can decorate, add children on this view.
            Content = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,

                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    CellPadding = new Size2D(10, 20),
                },
            };

            var tooltipButton = new Button()
            {
                Text = "Click",
                WidthSpecification = 300,
                HeightSpecification = 150,
            };

            Content.Add(tooltipButton);


            var dalitooltipButton = new Button()
            {
                Text = "Click2",
                WidthSpecification = 300,
                HeightSpecification = 150,
            };

            Content.Add(dalitooltipButton);

            dalitooltipButton.TooltipText = "test!!!";

            var menuItemStyle = new ButtonStyle()
            {
                Icon = new ImageViewStyle
                {
                    Color = new Selector<Color>
                    {
                        Normal = Color.White,
                        Pressed = Color.OrangeRed,
                        Selected = Color.Orange,
                    }
                }
            };

            for (int i = 0; i < 2; i++)
            {
                var item = new MenuItem(menuItemStyle)
                {
                    BackgroundColor = Color.Transparent,
                    WidthSpecification = 80,
                    HeightSpecification = 40,
                    Padding = new Extents(0,0,0,0),
                };
                tooltipItem[i] = item;

                item.Icon.ResourceUrl = Tizen.Applications.Application.Current.DirectoryInfo.Resource + "home.png";
                item.Icon.PositionUsesPivotPoint = true;
                item.Icon.PivotPoint = NUI.PivotPoint.Center;
                item.Icon.ParentOrigin = NUI.ParentOrigin.Center;

                item.SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
                {
                    var item = sender as MenuItem;
                    global::System.Console.WriteLine(item.Text + $"'s IsSelected is changed to {args.IsSelected}.");
                };
            }

            tooltipButton.Clicked += (object sender, ClickedEventArgs args) =>
            {
                tooltip = new Tooltip()
                {
                    Anchor = tooltipButton,
                    HorizontalPositionToAnchor = Menu.RelativePosition.Center,
                    VerticalPositionToAnchor = Menu.RelativePosition.Start,
                    Items = new MenuItem[] { tooltipItem[0], tooltipItem[1] },
                };
                tooltip.Post();
            };
        }


        public class Tooltip : Menu
        {
            private ImageView tail;

            public Tooltip() : base(new MenuStyle())
            {
                BackgroundColor = Color.Transparent;
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                };

                WidthSpecification = 160;
                HeightSpecification = 110;
                HeightSpecification = LayoutParamPolicies.WrapContent;

                // Internally set the content and add to menu.
                Content = new View()
                {
                    BackgroundColor = Color.DarkGray,
                    CornerRadius = 10f,
                    WidthSpecification = LayoutParamPolicies.WrapContent,
                    HeightSpecification = 80,
                    Layout = new LinearLayout()
                    {
                        LinearOrientation = LinearLayout.Orientation.Horizontal,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    },
                };

                tail = new ImageView()
                {
                    ResourceUrl = Tizen.Applications.Application.Current.DirectoryInfo.Resource + "tail.png",
                };
                tail.Color = Color.DarkGray;
                Add(tail);
            }
        }
    }
}
