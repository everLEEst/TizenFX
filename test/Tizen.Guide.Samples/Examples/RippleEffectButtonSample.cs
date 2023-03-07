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
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using Tizen.NUI.Components.Extension;
using Tizen.NUI;

namespace Tizen.Guide.Samples
{
    // IExample inehrited class will be automatically added in the main examples list.
    internal class RippleEffectButtonSample : ContentPage, IExample
    {
        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        /// Modify this method for adding other examples.
        public RippleEffectButtonSample() : base()
        {
            Log.Info(this.GetType().Name, $"{this.GetType().Name} is contructed\n");

            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;
            // Navigator bar title is added here.
            AppBar = new AppBar()
            {
                Title = "Ripple Effect Button Sample",
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

            // Outlined style Button examples.

            var enabledRippleButton = new Button(new RippleEffectButtonStyle())
            {
                Text = "Enabled"
            };
            enabledRippleButton.Clicked += (object obj, ClickedEventArgs ev) =>
            {
                Log.Info(this.GetType().Name, "Enabled Button Clicked\n");
            };
            rootContent.Add(enabledRippleButton);

            var disabledRippleButton = new Button(new RippleEffectButtonStyle())
            {
                Text = "Disabled",
                IsEnabled = false,
            };
            disabledRippleButton.Clicked += (object obj, ClickedEventArgs ev) =>
            {
                // This event should not be recieved. Button is disabled.
                Log.Info(this.GetType().Name, "Disabled Button Clicked\n");

            };
            rootContent.Add(disabledRippleButton);

            var selectableRippleButton = new Button(new RippleEffectButtonStyle())
            {
                Text = "Unselected",
                IsSelectable = true,
            };
            selectableRippleButton.Clicked += (object obj, ClickedEventArgs ev) =>
            {
                Log.Info(this.GetType().Name, "Selected Button Clicked\n");
                if (obj is Button button)
                {
                    disabledRippleButton.IsEnabled = button.IsSelected;
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
            rootContent.Add(selectableRippleButton);
            Content = rootContent;
        }
    public class RippleEffectButtonStyle : ButtonStyle
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RippleEffectButtonStyle() : base()
        {
            Padding = new Extents(32, 32, 8, 8);
            ItemSpacing = new Size2D(8, 8);
            CornerRadius = 20f;
            Size = new Size(300f, 150f);
            ItemHorizontalAlignment = HorizontalAlignment.Center;
            ItemVerticalAlignment = VerticalAlignment.Center;
            ClippingMode = ClippingModeType.ClipChildren;
            BackgroundColor = new Selector<Color>()
            {
                Normal = new Color(1.0f, 0.384f, 0.0f, 1),
                Pressed = new Color(0.85f, 0.325f, 0.0f, 1),
                Focused = new Color(1.0f, 0.827f, 0.624f, 1),
                Selected = new Color(0.624f, 0.239f, 0.0f, 1),
                Disabled = new Color(0.792f, 0.792f, 0.792f, 1),
            };
            Text = new TextLabelStyle()
            {
                TextColor = new Color("#FDFDFD"),
                PixelSize = 24,
                FontSizeScale = FontSizeScale.UseSystemSetting,
            };
            Opacity = new Selector<float?>
            {
                Disabled = 0.3f,
                Other = 1.0f
            };
        }

        public override ButtonExtension CreateExtension()
        {
            return new OverlayAnimationButtonExtension();
        }
    }

    /// <summary>
    /// OverlayAnimationButtonExtension class is a extended ButtonExtension class that make the overlay image blinking on a Button pressed.
    /// </summary>
    internal class OverlayAnimationButtonExtension : ButtonExtension
    {
        private Animation PressAnimation { get; set; }

        /// <summary>
        /// Creates a new instance of a OverlayAnimationButtonExtension.
        /// </summary>
        public OverlayAnimationButtonExtension() : base()
        {
        }

        public override ImageView OnCreateOverlayImage(Button button, ImageView overlayImage)
        {
            overlayImage.Hide();

            return overlayImage;
        }

        public override void OnControlStateChanged(Button button, View.ControlStateChangedEventArgs args)
        {
            if (button.ControlState != ControlState.Pressed)
            {
                return;
            }

            var overlayImage = button.OverlayImage;

            if (overlayImage == null)
            {
                return;
            }

            if (null == PressAnimation)
            {
                var keyFrames = new KeyFrames();
                keyFrames.Add(0.0f, 0.0f);
                AlphaFunction linear = new AlphaFunction(AlphaFunction.BuiltinFunctions.Linear);
                keyFrames.Add(0.25f, 1.0f, linear);
                linear.Dispose();
                AlphaFunction ease = new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseOut);
                keyFrames.Add(1.0f, 0.0f, ease);
                ease.Dispose();

                PressAnimation = new Animation(1000);
                PressAnimation.EndAction = Animation.EndActions.StopFinal;
                PressAnimation.AnimateBetween(overlayImage, "Opacity", keyFrames);
                keyFrames.Dispose();
                Vector3 vec = new Vector3(1, 1, 1);

                // Please modify here to change animation alpha function!
                AlphaFunction easeout = new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseOut);

                // Please modify here to change animation duration!
                // Scale 0.01 to 1
                PressAnimation.AnimateTo(overlayImage, "Scale", vec, 0, 1000, easeout);
                vec.Dispose();
                easeout.Dispose();
            }

            if (PressAnimation.State == Animation.States.Playing)
            {
                PressAnimation.Stop();
                overlayImage.Hide();
            }
            // Raise Text above the overlay circle image.
            button.TextLabel.RaiseAbove(overlayImage);

            overlayImage.Opacity = 0.0f;
            overlayImage.BackgroundColor = button.BackgroundColor;
            float maxSize = 500;//(button.Size.Width > button.Size.Height? button.Size.Width : button.Size.Height);
            overlayImage.Size = new Size(maxSize, maxSize);
            // Make the overlay image to circle.
            overlayImage.CornerRadius = maxSize / 2;

            // Note: overlayImage use pivot as button center to it's own center.
            Vector2 touchPos = TouchInfo.GetLocalPosition(0);
            Vector2 buttonSize = new Vector2(button.SizeWidth, button.SizeHeight);
            Vector2 imagePos = touchPos - buttonSize / 2;

            overlayImage.Position = new Position(imagePos.X, imagePos.Y);

            // Scale start 0.01
            overlayImage.Scale = new Vector3(0.01f, 0.01f, 1);
            overlayImage.Show();

            PressAnimation.Play();
        }

        public override void OnDispose(Button button)
        {
            if (PressAnimation == null)
            {
                return;
            }

            if (PressAnimation.State == Animation.States.Playing)
            {
                PressAnimation.Stop();
            }
            PressAnimation.Dispose();
            PressAnimation = null;
        }
    }
    }
}
