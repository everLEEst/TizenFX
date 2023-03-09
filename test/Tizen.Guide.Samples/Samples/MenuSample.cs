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
    internal class MenuSample : ContentPage, ISample
    {
        private View rootContent;
        private int itemCount = 7;
        private MenuItem[] menuItem = new MenuItem[7];

        public void Activate()
        {
        }
        public void Deactivate()
        {
            for (int i = 0; i < itemCount; i++)
            {
                if (menuItem[i] != null)
                {
                    menuItem[i].Dispose();
                    menuItem[i] = null;
                }
            }

            if (rootContent != null)
            {
                rootContent.Dispose();
                rootContent = null;
            }
        }

        /// Modify this method for adding other examples.
        public MenuSample() : base()
        {
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            // Navigator bar title is added here.
            AppBar = new AppBar()
            {
                Title = "Menu Default Style",
            };

            // Example root content view.
            // you can decorate, add children on this view.
            rootContent = new View()
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

            Content = rootContent;


            var pageContent = new Button()
            {
                Text = "Page Content",
                CornerRadius = 0,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };

            var moreButton = new Button()
            {
                Text = "More",
            };

            var appBar = new AppBar()
            {
                AutoNavigationContent = false,
                Title = "Title",
                Actions = new View[] { moreButton, },
            };

            var page = new ContentPage()
            {
                AppBar = appBar,
                Content = pageContent,
            };

            rootContent.Add(page);

            for (int i = 0; i < itemCount; i++)
            {
                menuItem[i] = new MenuItem();
                menuItem[i].Text = "Menu" + (i + 1);
                menuItem[i].SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
                {
                    var item = sender as MenuItem;
                    global::System.Console.WriteLine(item.Text + $"'s IsSelected is changed to {args.IsSelected}.");
                };
            }

            moreButton.Clicked += (object sender, ClickedEventArgs args) =>
            {
                var menu = new Menu()
                {
                    Anchor = moreButton,
                    HorizontalPositionToAnchor = Menu.RelativePosition.Center,
                    VerticalPositionToAnchor = Menu.RelativePosition.End,
                    Items = new MenuItem[] { menuItem[0], menuItem[1], menuItem[2], menuItem[3], menuItem[4], menuItem[5], menuItem[6]  },
                };
                menu.Post();
            };
        }
    }
}
