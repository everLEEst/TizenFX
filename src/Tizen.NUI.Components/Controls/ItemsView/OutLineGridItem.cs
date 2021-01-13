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
    /// OutLineGridItem is one kind of common component, a OutLineGridItem clearly describes what action will occur when the user selects it.
    /// OutLineGridItem may contain text or an icon.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class OutLineGridItem : ViewItem
    {
        /// <summary>
        /// Extents padding around Icon
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly BindableProperty IconPaddingProperty = BindableProperty.Create(nameof(IconPadding), typeof(Extents), typeof(OutLineGridItem), null, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var instance = (OutLineGridItem)bindable;
            instance.iconPadding = (Extents)((Extents)newValue).Clone();
            instance.UpdateContent();
        },
        defaultValueCreator: (bindable) => ((OutLineGridItem)bindable).iconPadding);

        /// <summary>
        /// Extents padding around Label
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly BindableProperty LabelPaddingProperty = BindableProperty.Create(nameof(LabelPadding), typeof(Extents), typeof(OutLineGridItem), null, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var instance = (OutLineGridItem)bindable;
            instance.labelPadding = (Extents)((Extents)newValue).Clone();
            instance.UpdateContent();
        },
        defaultValueCreator: (bindable) => ((OutLineGridItem)bindable).labelPadding);

        private TextLabel itemText;
        private ImageView itemIcon;
        private Extents iconPadding;
        private Extents labelPadding;
        static OutLineGridItem() {}

        /// <summary>
        /// Creates a new instance of OutLineGridItem.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public OutLineGridItem() : base()
        {
            Initialize();
        }

        /// <summary>
        /// Creates a new instance of OutLineGridItem with style
        /// </summary>
        /// <param name="style=">Create OutLineGridItem by special style defined in UX.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public OutLineGridItem(string style) : base(style)
        {
            Initialize();
        }

        /// <summary>
        /// Creates a new instance of OutLineGridItem with style
        /// </summary>
        /// <param name="itemStyle=">Create OutLineGridItem by style customized by user.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public OutLineGridItem(ViewItemStyle itemStyle) : base(itemStyle)
        {
            Initialize();
        }

        /// <summary>
        /// OutLineGridItem's icon part.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ImageView Icon
        {
            get
            {
                if ( itemIcon == null)
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

/* open when ImageView using Uri not string
        /// <summary>
        /// Icon image's resource url in OutLineGridItem.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string IconUrl
        {
            get
            {
                return Icon.ResourceUrl;
            }
            set
            {
                Icon.ResourceUrl = value;
            }
        }
*/

        /// <summary>
        /// OutLineGridItem's text part.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TextLabel Label
        {
            get
            {
                if (itemText == null)
                {
                    itemText = CreateLabel();
                    if (itemText != null)
                    {
                        Add(itemText);
                    }
                }
                return itemText;
            }
            internal set
            {
                itemText = value;
                AccessibilityManager.Instance.SetAccessibilityAttribute(this, AccessibilityManager.AccessibilityAttribute.Label, itemText.Text);
            }
        }

        /// <summary>
        /// The text of OutLineGridItem.
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
        /// Icon padding in ViewItem, work only when show icon and text.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Extents IconPadding
        {
            get => (Extents)GetValue(IconPaddingProperty) ?? new Extents();
            set => SetValue(IconPaddingProperty, value);
        }

        /// <summary>
        /// Text padding in ViewItem, work only when show icon and text.
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
                ParentOrigin = NUI.ParentOrigin.BottomCenter,
                PivotPoint = NUI.PivotPoint.BottomCenter,
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent,
                HorizontalAlignment = HorizontalAlignment.Center,
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
                ParentOrigin = NUI.ParentOrigin.TopCenter,
                PivotPoint = NUI.PivotPoint.TopCenter
            };
        }

         /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void MeasureChild()
        {
            if (itemIcon == null || itemText == null)
            {
                return;
            }
            itemText.WidthResizePolicy = ResizePolicyType.Fixed;
            itemText.HeightResizePolicy = ResizePolicyType.Fixed;

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

            itemText.SizeWidth = SizeWidth - labelPaddingStart - labelPaddingEnd;
            itemText.SizeHeight = SizeHeight - labelPaddingTop - labelPaddingBottom - iconPaddingTop - iconPaddingBottom - itemIcon.SizeHeight;
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void LayoutChild()
        {
            if (itemIcon == null || itemText == null)
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

            itemIcon.PositionUsesPivotPoint = true;
            itemIcon.ParentOrigin = NUI.ParentOrigin.TopCenter;
            itemIcon.PivotPoint = NUI.PivotPoint.TopCenter;
            itemIcon.Position2D = new Position2D(0, iconPaddingTop);

            itemText.PositionUsesPivotPoint = true;
            itemText.ParentOrigin = NUI.ParentOrigin.BottomCenter;
            itemText.PivotPoint = NUI.PivotPoint.BottomCenter;
            itemText.Position2D = new Position2D(0, -labelPaddingBottom);

           if (string.IsNullOrEmpty(itemText.Text))
            {
                itemIcon.ParentOrigin = NUI.ParentOrigin.Center;
                itemIcon.PivotPoint = NUI.PivotPoint.Center;
            }
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        private void Initialize()
        {
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
                if (itemText != null)
                {
                    Utility.Dispose(itemText);
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
