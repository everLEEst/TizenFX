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
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace Tizen.Guide.Samples
{
    // ISample inehrited class will be automatically added in the main examples list.
    internal class ProgressSample : ContentPage, ISample
    {
        private View rootContent;

        public void Activate()
        {
        }
        public void Deactivate()
        {
        }

        /// Modify this method for adding other examples.
        public ProgressSample() : base()
        {
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            // Navigator bar title is added here.
            AppBar = new AppBar()
            {
                Title = "Progress Samples",
            };

            // Example root content view.
            // you can decorate, add children on this view.
            rootContent = new View()
            {
                BackgroundColor = Color.LightGray,
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

            // Progress examples.
            var bufferingProgress = new Progress()
            {
                MinValue = 0,
                MaxValue = 100,
                BufferValue = 10,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                ProgressState = Progress.ProgressStatusType.Buffering,
                TrackColor = Color.DarkGray,
                ProgressColor = Color.DodgerBlue,
                BufferColor = Color.LightSkyBlue,
            };
            rootContent.Add(bufferingProgress);

            var determinatedProgress = new Progress()
            {
                MinValue = 0,
                MaxValue = 100,
                CurrentValue = 80,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                ProgressState = Progress.ProgressStatusType.Determinate,
                TrackColor = Color.DarkGray,
                ProgressColor = Color.DodgerBlue,
                BufferColor = Color.LightSkyBlue,
            };
            rootContent.Add(determinatedProgress);

            var indeterminatedProgress = new Progress()
            {
                MinValue = 0,
                MaxValue = 100,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                ProgressState = Progress.ProgressStatusType.Indeterminate,
                TrackColor = Color.DarkGray,
                ProgressColor = Color.DodgerBlue,
                BufferColor = Color.LightSkyBlue,
            };
            rootContent.Add(indeterminatedProgress);


            var disabledProgress = new Progress()
            {
                MinValue = 0,
                MaxValue = 100,
                IsEnabled = false,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                ProgressState = Progress.ProgressStatusType.Determinate,
                TrackColor = Color.DarkGray,
                ProgressColor = Color.DodgerBlue,
                BufferColor = Color.LightSkyBlue,
            };
            rootContent.Add(disabledProgress);

            // Progress Animation Sample

            var animaitonProgressView = new View
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                CornerRadius = 15f,
                BackgroundColor = Color.White,
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    LinearAlignment = LinearLayout.Alignment.Center,
                    CellPadding = new Size2D(0, 20),
                },
                Padding = new Extents(30, 30, 30, 30),
                Margin = new Extents(20, 20, 20, 20),
            };
            rootContent.Add(animaitonProgressView);

            var progressInfo = new View
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    LinearAlignment = LinearLayout.Alignment.Center,
                }
            };

            var progressStatus = new TextLabel
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HorizontalAlignment = HorizontalAlignment.Begin,
                Text = "Bufferring...",
            };
            progressInfo.Add(progressStatus);

            var progressValue = new TextLabel
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HorizontalAlignment = HorizontalAlignment.End,
                Text = "0 %",
            };
            progressInfo.Add(progressValue);
            animaitonProgressView.Add(progressInfo);


            var animationProgress = new Progress()
            {
                MinValue = 0,
                MaxValue = 100,
                CurrentValue = 0,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                ProgressState = Progress.ProgressStatusType.Determinate,
                TrackColor = Color.DarkGray,
                ProgressColor = Color.DodgerBlue,
                BufferColor = Color.LightSkyBlue,
            };
            animaitonProgressView.Add(animationProgress);

            var downloadButton = new Button()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                Text = "Start Download",
            };
            animaitonProgressView.Add(downloadButton);
            downloadButton.Clicked += (o, e) =>
            {
                // Note: BufferValue and CurrentValue is not anmiatable property so please use Timer istead of Animation for updating.

                var progressTimer = new Timer(300);
                var rand = new Random();

                progressTimer.Tick += (o, args) =>
                {
                    if (animationProgress.CurrentValue < 100)
                    {
                        if (animationProgress.CurrentValue == 0)
                        {
                            progressStatus.Text = "Downloading...";
                        }
                        animationProgress.CurrentValue += rand.Next(3);
                        progressValue.Text = $"{animationProgress.CurrentValue} %";
                        return true;
                    }
                    else
                    {
                        progressStatus.Text = "Done";
                        return false;
                    }
                };
                progressTimer.Start();
            };
            Content = rootContent;
        }
    }
}
