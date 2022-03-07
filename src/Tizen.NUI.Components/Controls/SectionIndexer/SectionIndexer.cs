/* Copyright (c) 2022 Samsung Electronics Co., Ltd.
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
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Input;
using System.ComponentModel;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Binding;

namespace Tizen.NUI.Components
{
    public class SectionIndexer : Control
    {
        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly BindableProperty SectionsProperty =
            BindableProperty.Create(nameof(Sections), typeof(string []), typeof(SectionIndexer), null,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var indexer = (SectionIndexer)bindable;
                    indexer.sections = newValue as string[];
                    indexer.UpdateSections();
                },
                defaultValueCreator: (bindable) =>
                {
                    var indexer = (SectionIndexer)bindable;
                    return indexer.sections;
                });

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly BindableProperty BarWidthProperty =
            BindableProperty.Create(nameof(BarWidth), typeof(int), typeof(SectionIndexer), 0,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var indexer = (SectionIndexer)bindable;
                    indexer.barWidth = (int)newValue;
                    if (indexer.sectionIndexBar != null)
                    {
                        indexer.sectionIndexBar.WidthSpecification = (int)newValue;
                    }
                },
                defaultValueCreator: (bindable) =>
                {
                    var indexer = (SectionIndexer)bindable;
                    return indexer.barWidth;
                });

        private string[] sections;
        private List<SectionItem> sectionItems;
        private View sectionIndexBar;
        private SectionIndicator sectionIndicator;
        private int barWidth;
        private string currentSection;
        private Animation animShowIndicator;
        private Animation animHideIndicator;
        private int indicatorAnimDuration = 200;
        private int indicatorHideDelay = 800;

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string[] Sections
        {
            get => GetValue(SectionsProperty) as string[];
            set
            {
                SetValue(SectionsProperty, value);
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int BarWidth
        {
            get => (int)GetValue(BarWidthProperty);
            set
            {
                SetValue(BarWidthProperty, value);
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string CurrentSection
        {
            get => currentSection;
        }

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SectionIndexer(): base()
        {
            //barWidth = 30;
            sectionIndexBar = new View()
            {
                BackgroundColor = Color.Blue,
                WidthSpecification = barWidth,
                HeightSpecification = LayoutParamPolicies.MatchParent,

                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    CellPadding = new Size2D(0, 0),
                }
            };
            Add(sectionIndexBar);
            sectionIndicator = new SectionIndicator()
            {
                Opacity = 0f,
                WidthSpecification = 100,
                HeightSpecification = 100,
                PixelSize = 80,
            };
            Add(sectionIndicator);
            sectionItems = new List<SectionItem>();

            Layout = new RelativeLayout();
            UpdateSections();
            UpdateLayout();
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void OnRelayout(Vector2 size, RelayoutContainer container)
        {
            base.OnRelayout(size, container);
            Tizen.Log.Debug("[SectionIndexer] ##", $"OnRelayout [{size.X}],[{size.Y}]" + "\n");
            UpdateSections();
            UpdateLayout();
        }

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected View SectionIndexBar
        {
            get => sectionIndexBar;
            set {
                if (sectionIndexBar != null)
                {
                    ClearSections();
                    Remove(sectionIndexBar);
                }
                sectionIndexBar = value;
                Add(sectionIndexBar);
                UpdateSections();
                UpdateLayout();
            }
        }

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected SectionIndicator SectionIndicator
        {
            get => sectionIndicator;
            set {
                if (SectionIndicator != null)
                {
                    Remove(sectionIndicator);
                }
                sectionIndicator = value;
                //sectionIndicator.Opacity = 0f;
                Add(sectionIndicator);
                UpdateLayout();
            }
        }


        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual bool OnSection(object item, TouchEventArgs e)
        {
            var section = item as SectionItem;
            if (section == null) return false;

            var context = section.Text;
            var state = e.Touch.GetState(0);
            Tizen.Log.Debug("[SectionIndexer] ##", $"[{section}({section.Text})] is on touched [{state}], [{context.GetHashCode()}], [{currentSection?.GetHashCode()}]" + "\n");


            switch(state)
            {
                case PointStateType.Motion :
                case PointStateType.Down :
                {
                    if (currentSection != context)
                    {
                        sectionIndicator.Text = section.Text;
                        currentSection = context;
                        Tizen.Log.Debug("[SectionIndexer] ##", $" In and set session to crrent"+"\n");
                        AnimShowIndicator();

                    }
                    break;
                }
                case PointStateType.Leave :
                case PointStateType.Finished :
                {
                    if (currentSection == context)
                    {
                        currentSection = null;
                        Tizen.Log.Debug("[SectionIndexer] ##", $" Leave and set session null"+"\n");
                        AnimHideIndicator();
                    }
                    break;
                }
            }

            return true;
        }


        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual void ToSection(string section)
        {
        }


        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual void ClearSections()
        {
            if (sectionIndexBar != null)
            {
                foreach(SectionItem item in sectionItems)
                {
                    sectionIndexBar.Remove(item);
                }
            }
            sectionItems?.Clear();

        }


        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual void UpdateSections()
        {
            if (sectionIndexBar != null)
            {
                if (sections == null || sections.Count() == 0) return;

                float itemHeight = Size.Height / sections.Count();
                if (itemHeight == 0) return;

                foreach(string section in sections)
                {
                    SectionItem item = new SectionItem();
                    item.PixelSize = 30;
                    item.Text = section;
                    item.WidthSpecification = LayoutParamPolicies.MatchParent;
                    item.HeightSpecification = (int)itemHeight;
                    item.TouchEvent += OnSection;
                    SectionIndexBar.Add(item);
                    sectionItems.Add(item);

                    Tizen.Log.Debug("[SectionIndexer] ##", $"[{item}] height[{itemHeight}], [{section}]" + "\n");
                }
            }
        }


        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual void UpdateLayout()
        {
            if (sectionIndexBar != null)
            {
                RelativeLayout.SetLeftRelativeOffset(sectionIndexBar, 1.0f);
                RelativeLayout.SetRightRelativeOffset(sectionIndexBar, 1.0f);
                RelativeLayout.SetHorizontalAlignment(sectionIndexBar, RelativeLayout.Alignment.End);
                RelativeLayout.SetVerticalAlignment(sectionIndexBar, RelativeLayout.Alignment.Center);
            }

            if (sectionIndicator != null)
            {
                RelativeLayout.SetTopRelativeOffset(sectionIndicator, 0.5F);
                RelativeLayout.SetLeftRelativeOffset(sectionIndicator, 0.5f);
                RelativeLayout.SetRightRelativeOffset(sectionIndicator, 0.5f);
                RelativeLayout.SetBottomRelativeOffset(sectionIndicator, 0.5f);
                RelativeLayout.SetHorizontalAlignment(sectionIndicator, RelativeLayout.Alignment.Center);
                RelativeLayout.SetVerticalAlignment(sectionIndicator, RelativeLayout.Alignment.Center);
            }
        }

        private void AnimShowIndicator()
        {
            if (animShowIndicator == null)
            {
                Tizen.Log.Debug("[SectionIndexer] ##", $" create visible anim"+"\n");
                //sectionIndicator.SetVisible(true);
                animShowIndicator = new Animation(1000);
                animShowIndicator.AnimateTo(sectionIndicator, "Opacity", 1.0f, 0, indicatorAnimDuration);
                animShowIndicator.EndAction = Animation.EndActions.StopFinal;
                animShowIndicator.Finished += (object sender, EventArgs args) =>
                {
                    animShowIndicator = null;
                };
                animShowIndicator.Play();
                animHideIndicator = null;
            }
        }


        private void AnimHideIndicator()
        {
            if (animHideIndicator == null)
            {
                Tizen.Log.Debug("[SectionIndexer] ##", $" create invisible anim"+"\n");
                animHideIndicator = new Animation(1000);
                animHideIndicator.AnimateTo(sectionIndicator, "Opacity", 0.0f, indicatorHideDelay, indicatorAnimDuration+indicatorHideDelay);
                animHideIndicator.EndAction = Animation.EndActions.StopFinal;
                animHideIndicator.Finished += (object sender, EventArgs args) =>
                {
                    animHideIndicator = null;
                };
                animHideIndicator.Play();
                animShowIndicator = null;
            }
        }
    }

    /// <summary>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class SectionIndicator : View
    {
        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(SectionIndicator), null,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var indicator = (SectionIndicator)bindable;
                    indicator.sectionTitle = (string)newValue;
                    if (null != indicator.label)
                    {
                        Tizen.Log.Error("[SectionIndicator] ::", $"Text is {newValue}"+"\n");
                        indicator.label.Text = (string)newValue;
                    }
                },
                defaultValueCreator: (bindable) =>
                {
                    var indicator = (SectionIndicator)bindable;
                    return indicator.sectionTitle;
                });

        private TextLabel label;
        private string sectionTitle ="";
        private float textSize;

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set
            {
                SetValue(TextProperty, value);
                NotifyPropertyChanged();
            }
        }


        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public float PixelSize
        {
            get => textSize;
            set
            {
                textSize = value;
                if (null != label)
                {
                    label.PixelSize = textSize;
                }
            }
        }

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SectionIndicator() : base()
        {
            string bg = "#f5f2e7ff";
            string main = "#2c3333ff";
            Tizen.Log.Error("[SectionIndicator] ::", $" is created"+"\n");
            BackgroundColor = new Color(bg);
            BorderlineOffset = 0.5f;
            BorderlineWidth = 2f;
            BorderlineColor = new Color(main);
            Layout = new AbsoluteLayout();

            label = new TextLabel()
            {
                Text = sectionTitle,
                PixelSize = textSize,
                TextColor = new Color(main),
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Add(label);

            //Opacity = 0f;
        }
    }


    /// <summary>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class SectionItem : Control
    {
        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(SectionItem), null,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var item = (SectionItem)bindable;
                    item.sectionTitle = (string)newValue;
                    if (null != item.label)
                    {
                        Tizen.Log.Error("[SectionItem] ::", $"Text is {newValue}"+"\n");
                        item.label.Text = (string)newValue;
                    }
                },
                defaultValueCreator: (bindable) =>
                {
                    var item = (SectionItem)bindable;
                    return item.sectionTitle;
                });
        private TextLabel label;
        private string sectionTitle ="";
        private float textSize;

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set
            {
                SetValue(TextProperty, value);
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public float PixelSize
        {
            get => textSize;
            set
            {
                textSize = value;
                if (null != label)
                {
                    label.PixelSize = textSize;
                }
            }
        }

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SectionItem() : base()
        {
            string bg = "#f5f2e7ff";
            string main = "#2c3333ff";
            Tizen.Log.Error("[SectionItem] ::", $" is created"+"\n");
            BackgroundColor = new Color(bg);
            BorderlineOffset = 0.5f;
            BorderlineWidth = 2f;
            BorderlineColor = new Color(main);
            Layout = new AbsoluteLayout();

            label = new TextLabel()
            {
                Text = sectionTitle,
                PixelSize = textSize,
                TextColor = new Color(main),
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Add(label);
        }


        /// <summary>
        /// Initializes AT-SPI object.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void OnInitialize()
        {
            Tizen.Log.Error("[SectionItem] ::", $" on Initialized "+"\n");
            base.OnInitialize();
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool HandleControlStateOnTouch(Touch touch)
        {
            if (null == touch)
            {
                return false;
            }

            PointStateType state = touch.GetState(0);

            Tizen.Log.Error("[SectionItem] ::", $"[{sectionTitle}] Touch!!! {state}"+"\n");

            return base.HandleControlStateOnTouch(touch);
        }




    }
}
