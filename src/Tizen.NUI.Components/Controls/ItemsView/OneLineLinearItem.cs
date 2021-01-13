/* Copyright (c) 2020 Samsung Electronics Co., Ltd.
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
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Binding;
using Tizen.NUI.Components.Extension;
using Tizen.NUI.Accessibility;

namespace Tizen.NUI.Components
{
    /// <summary>
    /// OneLineLinearItem is one kind of common component, a OneLineLinearItem clearly describes what action will occur when the user selects it.
    /// OneLineLinearItem may contain text or an icon.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class OneLineLinearItem : ViewItem
    {     
        /// <summary>
        /// Extents padding around Icon
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly BindableProperty IconPaddingProperty = BindableProperty.Create(nameof(IconPadding), typeof(Extents), typeof(OneLineLinearItem), null, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var instance = (OneLineLinearItem)bindable;
            instance.iconPadding = (Extents)((Extents)newValue).Clone();
            instance.UpdateContent();
        },
        defaultValueCreator: (bindable) => ((OneLineLinearItem)bindable).iconPadding);
        
        /// <summary>
        /// Extents padding around Label
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly BindableProperty LabelPaddingProperty = BindableProperty.Create(nameof(LabelPadding), typeof(Extents), typeof(OneLineLinearItem), null, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var instance = (OneLineLinearItem)bindable;
            instance.labelPadding = (Extents)((Extents)newValue).Clone();
            instance.UpdateContent();
        },
        defaultValueCreator: (bindable) => ((OneLineLinearItem)bindable).labelPadding);

        private TextLabel itemLabel;
        private View itemIcon;
        private Extents iconPadding;
        private Extents labelPadding;

        static OneLineLinearItem() {}

        /// <summary>
        /// Creates a new instance of OneLineLinearItem.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public OneLineLinearItem() : base()
        {
            Initialize();
        }
         
        /// <summary>
        /// Creates a new instance of a OneLineLinearItem with style.
        /// </summary>
        /// <param name="style">Create ViewItem by style defined in UX.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public OneLineLinearItem(string style) : base(style)
        {
            Initialize();
        }

        /// <summary>
        /// Creates a new instance of a OneLineLinearItem with style.
        /// </summary>
        /// <param name="itemStyle">Create ViewItem by style customized by user.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public OneLineLinearItem(ViewItemStyle itemStyle) : base(itemStyle)
        {
            Initialize();
        }

        /// <summary>
        /// Icon part of OneLineLinearItem.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public View Icon
        {
            get
            {
                if (itemIcon == null)
                {
                    itemIcon = CreateIcon();
                    if (itemIcon != null)
                    {
                        Add(itemIcon);
                        itemIcon.Relayout += OnIconRelayout;
                    }
                }
                return itemIcon;
            }
            internal set
            {
                itemIcon = value;
            }
        }

        /* open when imageView using Uri not string.
        /// <summary>
        /// Icon image's resource url. Only activatable for icon as ImageView.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string IconUrl
        {
            get
            {
                return (Icon as ImageView)?.ResourceUrl;
            }
            set
            {
                if (itemIcon != null && !(itemIcon is ImageView))
                {
                    // Tizen.Log.Error("IconUrl only can set Icon is ImageView");
                    return;
                }
                (Icon as ImageView).ResourceUrl = value; 
            }
        }
        */

        /// <summary>
        /// OneLineLinearItem's text part of OneLineLinearItem
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TextLabel Label
        {
            get
            {
                if (itemLabel == null)
                {
                    itemLabel = CreateLabel();
                    if (itemLabel != null)
                    {
                        Add(itemLabel);
                    }
                }
                return itemLabel;
            }
            internal set
            {
                itemLabel = value;
                AccessibilityManager.Instance.SetAccessibilityAttribute(this, AccessibilityManager.AccessibilityAttribute.Label, itemLabel.Text);
            }
        }

        /// <summary>
        /// The text of OneLineLinearItem.
        /// </summary>
       [EditorBrowsable(EditorBrowsableState.Never)]
        public string Text
        {
            get
            {
                return Label.Text;
            }
            set
            {
                Label.Text = value;
            }
        }

        /// <summary>
        /// Icon padding in OneLineLinearItem, work only when show icon and text.
        /// </summary>
       [EditorBrowsable(EditorBrowsableState.Never)]
        public Extents IconPadding
        {
            get => (Extents)GetValue(IconPaddingProperty) ?? new Extents();
            set => SetValue(IconPaddingProperty, value);
        }

        /// <summary>
        /// Text padding in OneLineLinearItem, work only when show icon and text.
        /// </summary>
       [EditorBrowsable(EditorBrowsableState.Never)]
        public Extents LabelPadding
        {
            get => (Extents)GetValue(LabelPaddingProperty) ?? new Extents();
            set => SetValue(LabelPaddingProperty, value);
        }

        /// <summary>
        /// Creates Item's text part.
        /// </summary>
        /// <return>The created Item's text part.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual TextLabel CreateLabel()
        {
            return new TextLabel
            {
                PositionUsesPivotPoint = true,
                ParentOrigin = NUI.ParentOrigin.Center,
                PivotPoint = NUI.PivotPoint.Center,
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent,
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        /// <summary>
        /// Creates Item's icon part.
        /// </summary>
        /// <return>The created Item's icon part.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual ImageView CreateIcon()
        {
            return new ImageView
            {
                PositionUsesPivotPoint = true,
                ParentOrigin = NUI.ParentOrigin.Center,
                PivotPoint = NUI.PivotPoint.Center
            };
        }
 
        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void MeasureChild()
        {
            if (itemIcon == null || itemLabel == null)
            {
                return;
            }
            itemLabel.WidthResizePolicy = ResizePolicyType.Fixed;
            itemLabel.HeightResizePolicy = ResizePolicyType.Fixed;

            var labelPadding = LabelPadding;
            int labelPaddingStart = labelPadding.Start;
            int labelPaddingEnd = labelPadding.End;
            int labelPaddingTop = labelPadding.Top;
            int labelPaddingBottom = labelPadding.Bottom;

            var iconPadding = IconPadding;
            int iconPaddingStart = iconPadding.Start;
            int iconPaddingEnd = iconPadding.End;
            int iconPaddingTop = iconPadding.Top;
            int iconPaddingBottom = iconPadding.Bottom;

            itemLabel.SizeWidth = SizeWidth - labelPaddingStart - labelPaddingEnd - iconPaddingStart - iconPaddingEnd - itemIcon.SizeWidth;
            itemLabel.SizeHeight = SizeHeight - labelPaddingTop - labelPaddingBottom;
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void LayoutChild()
        {
            if (itemIcon == null || itemLabel == null)
            {
                return;
            }

            var labelPadding = LabelPadding;
            int labelPaddingStart = labelPadding.Start;
            int labelPaddingEnd = labelPadding.End;
            int labelPaddingTop = labelPadding.Top;
            int labelPaddingBottom = labelPadding.Bottom;

            var iconPadding = IconPadding;
            int iconPaddingStart = iconPadding.Start;
            int iconPaddingEnd = iconPadding.End;
            int iconPaddingTop = iconPadding.Top;
            int iconPaddingBottom = iconPadding.Bottom;

            if (LayoutDirection == ViewLayoutDirectionType.LTR)
            {
                itemIcon.PositionUsesPivotPoint = true;
                itemIcon.ParentOrigin = NUI.ParentOrigin.CenterLeft;
                itemIcon.PivotPoint = NUI.PivotPoint.CenterLeft;
                itemIcon.Position2D = new Position2D(iconPaddingStart, 0);

                itemLabel.PositionUsesPivotPoint = true;
                itemLabel.ParentOrigin = NUI.ParentOrigin.CenterRight;
                itemLabel.PivotPoint = NUI.PivotPoint.CenterRight;
                itemLabel.Position2D = new Position2D(-labelPaddingEnd, 0);
            }
            else
            {
                itemIcon.PositionUsesPivotPoint = true;
                itemIcon.ParentOrigin = NUI.ParentOrigin.CenterRight;
                itemIcon.PivotPoint = NUI.PivotPoint.CenterRight;
                itemIcon.Position2D = new Position2D(-iconPaddingStart, 0);

                itemLabel.PositionUsesPivotPoint = true;
                itemLabel.ParentOrigin = NUI.ParentOrigin.CenterLeft;
                itemLabel.PivotPoint = NUI.PivotPoint.CenterLeft;
                itemLabel.Position2D = new Position2D(labelPaddingEnd, 0);
            }

            if (string.IsNullOrEmpty(itemLabel.Text))
            {
                itemIcon.ParentOrigin = NUI.ParentOrigin.Center;
                itemIcon.PivotPoint = NUI.PivotPoint.Center;
            }
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        private void Initialize()
        {
            base.OnInitialize();
            LayoutDirectionChanged += OnLayoutDirectionChanged;
        }

        /// <summary>
        /// Dispose Item and all children on it.
        /// </summary>
        /// <param name="type">Dispose type.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void Dispose(DisposeTypes type)
        {
            if (disposed)
            {
                return;
            }

            if (type == DisposeTypes.Explicit)
            {
                //Extension : Extension?.OnDispose(this);

                if (itemIcon != null)
                {
                    Utility.Dispose(itemIcon);
                }
                if (itemLabel != null)
                {
                    Utility.Dispose(itemLabel);
                }
            }

            base.Dispose(type);
        }

        private void OnLayoutDirectionChanged(object sender, LayoutDirectionChangedEventArgs e)
        {
            MeasureChild();
            LayoutChild();
        }

        private void OnIconRelayout(object sender, EventArgs e)
        {
            MeasureChild();
            LayoutChild();
        }
    }
}
