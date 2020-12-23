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
    /// Button is one kind of common component, a button clearly describes what action will occur when the user selects it.
    /// Button may contain text or an icon.
    /// </summary>
    /// <since_tizen> 6 </since_tizen>
    public class OutLineGridItem : ViewItem
    {
        static OutLineGridItem() {}

        /// <summary>
        /// Creates a new instance of OutLineGridItem.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public OutLineGridItem() : base()
        {
            Layout = new RelativeLayout();
        }

        /// This will be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly BindableProperty IconPaddingProperty = BindableProperty.Create(nameof(IconPadding), typeof(Extents), typeof(OutLineGridItem), null, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var instance = (OutLineGridItem)bindable;
            instance.iconPadding = (Extents)((Extents)newValue).Clone();
            instance.UpdateContent();
        },
        defaultValueCreator: (bindable) => ((OutLineGridItem)bindable).iconPadding);

        /// This will be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly BindableProperty TextPaddingProperty = BindableProperty.Create(nameof(TextPadding), typeof(Extents), typeof(OutLineGridItem), null, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var instance = (OutLineGridItem)bindable;
            instance.textPadding = (Extents)((Extents)newValue).Clone();
            instance.UpdateContent();
        },
        defaultValueCreator: (bindable) => ((OutLineGridItem)bindable).textPadding);

        private TextLabel itemText;
        private ImageView itemIcon;
        private Extents iconPadding;
        private Extents textPadding;

        /// <summary>
        /// Button's icon part.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        public ImageView Icon
        {
            get
            {
                if (null == itemIcon)
                {
                    itemIcon = CreateIcon();
                    /*
                    if (null != Extension)
                    {
                        itemIcon = Extension.OnCreateIcon(this, itemIcon);
                    }
                    */
                    if (null != itemIcon)
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

        /// <summary>
        /// Icon image's resource url in Button.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public string IconURL
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

        /// <summary>
        /// Button's text part.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        public TextLabel Label
        {
            get
            {
                if (null == itemText)
                {
                    itemText = CreateText();
                    /*
                    if (null != Extension)
                    {
                        itemText = Extension.OnCreateText(this, itemText);
                    }
                    */
                    if (null != itemText)
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
        /// The text of Button.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
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
        /// <since_tizen> 6 </since_tizen>
        public Extents IconPadding
        {
            get => (Extents)GetValue(IconPaddingProperty) ?? new Extents();
            set => SetValue(IconPaddingProperty, value);
        }

        /// <summary>
        /// Text padding in ViewItem, work only when show icon and text.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public Extents TextPadding
        {
            get => (Extents)GetValue(TextPaddingProperty) ?? new Extents();
            set => SetValue(TextPaddingProperty, value);
        }

/* Style :
        /// <summary>
        /// Apply style to button.
        /// </summary>
        /// <param name="viewStyle">The style to apply.</param>
        /// <since_tizen> 8 </since_tizen>
        public override void ApplyStyle(ViewStyle viewStyle)
        {
            styleApplied = false;

            base.ApplyStyle(viewStyle);

            if (null != buttonStyle)
            {
                Extension = buttonStyle.CreateExtension();
                if (buttonStyle.Text != null)
                {
                    Label?.ApplyStyle(buttonStyle.Text);
                }

                if (buttonStyle.Icon != null)
                {
                    Icon?.ApplyStyle(buttonStyle.Icon);
                }
            }

            styleApplied = true;
        }
*/


        /// <summary>
        /// Creates Item's text part.
        /// </summary>
        /// <return>The created Item's text part.</return>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual TextLabel CreateText()
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

            var textPadding = TextPadding;
            int textPaddingStart = textPadding.Start;
            int textPaddingEnd = textPadding.End;
            int textPaddingTop = textPadding.Top;
            int textPaddingBottom = textPadding.Bottom;

            var iconPadding = IconPadding;
            int iconPaddingStart = iconPadding.Start;
            int iconPaddingEnd = iconPadding.End;
            int iconPaddingTop = iconPadding.Top;
            int iconPaddingBottom = iconPadding.Bottom;

            itemText.SizeWidth = SizeWidth - textPaddingStart - textPaddingEnd;
            itemText.SizeHeight = SizeHeight - textPaddingTop - textPaddingBottom - iconPaddingTop - iconPaddingBottom - itemIcon.SizeHeight;
        }


        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void LayoutChild()
        {
            if (itemIcon == null || itemText == null)
            {
                return;
            }

            var textPadding = TextPadding;
            int textPaddingStart = textPadding.Start;
            int textPaddingEnd = textPadding.End;
            int textPaddingTop = textPadding.Top;
            int textPaddingBottom = textPadding.Bottom;

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
            itemText.Position2D = new Position2D(0, -textPaddingBottom);

           if (string.IsNullOrEmpty(itemText.Text))
            {
                itemIcon.ParentOrigin = NUI.ParentOrigin.Center;
                itemIcon.PivotPoint = NUI.PivotPoint.Center;
            }
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void Initialize()
        {
            base.OnInitialize();
            LayoutDirectionChanged += OnLayoutDirectionChanged;
        }

        private void OnLayoutDirectionChanged(object sender, LayoutDirectionChangedEventArgs e)
        {
            MeasureChild();
            LayoutChild();
        }

        /// <summary>
        /// Dispose Item and all children on it.
        /// </summary>
        /// <param name="type">Dispose type.</param>
        /// <since_tizen> 6 </since_tizen>
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

        private void OnIconRelayout(object sender, EventArgs e)
        {
            MeasureChild();
            LayoutChild();
        }
    }
}
