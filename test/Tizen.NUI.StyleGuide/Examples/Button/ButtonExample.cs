﻿/*
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
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace Tizen.NUI.StyleGuide
{
    // IExample inehrited class will be automatically added in the main examples list.
    internal class ButtonExample : IExample
    {
        private Window window;
        private ContentPage examplePage;
        public void Activate()
        {
            Console.WriteLine($"@@@ this.GetType().Name={this.GetType().Name}, Activate()");

            window = NUIApplication.GetDefaultWindow();

            examplePage = new ContentPage()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };
            DecorateExamplePage();

            window.GetDefaultNavigator().Push(examplePage);
        }
        public void Deactivate()
        {
            Console.WriteLine($"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
            window.GetDefaultNavigator().Pop();
            examplePage = null;
            window = null;
        }

        /// Modify this method for adding other examples.
        private void DecorateExamplePage()
        {
            // Navigator bar title is added here.
            examplePage.AppBar = new AppBar()
            {
                Title = "Button Default Style",
            };

            // Example root content view.
            // you can decorate, add children on this view.
            var rootContent = new View()
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

            // Button style examples.

            var enabledButton = new Button()
            {
                Text = "Enabled"
            };
            enabledButton.Clicked += (object obj, ClickedEventArgs ev) =>
            {
                Tizen.Log.Info("ButtonExample", "Enabled Button Clicked\n");
            };
            rootContent.Add(enabledButton);

            var disabledButton = new Button()
            {
                Text = "Disabled",
                IsEnabled = false,
            };
            disabledButton.Clicked += (object obj, ClickedEventArgs ev) =>
            {
                // This event should not be recieved. button is disabled.
                Tizen.Log.Fatal("ButtonExample", "Disabled Button Clicked\n");

            };
            rootContent.Add(disabledButton);

            var selectableButton = new Button()
            {
                Text = "Unselected",
                IsSelectable = true,
            };
            selectableButton.Clicked += (object obj, ClickedEventArgs ev) =>
            {
                Tizen.Log.Info("ButtonExample", "Selected Button Clicked\n");
                if (obj is Button button)
                {
                   if (button.IsSelected)
                    {
                        button.Text = "Selected";
                    }
                    else
                    {
                        button.Text = "Unselected";
                    }
                }
            };
            rootContent.Add(selectableButton);

            examplePage.Content = rootContent;
        }
    }
}
