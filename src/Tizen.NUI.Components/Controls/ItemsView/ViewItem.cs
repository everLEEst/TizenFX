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
    /// [Draft] This class provides a basic item for CollectionView.
    /// </summary>
    /// <since_tizen> 8 </since_tizen>
    /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
    [EditorBrowsable(EditorBrowsableState.Never)]
    public partial class ViewItem : Control
    {

        /// This will be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly BindableProperty IsEnabledProperty = BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(ViewItem), true, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var instance = (ViewItem)bindable;
            if (newValue != null)
            {
                bool newEnabled = (bool)newValue;
                if (instance.isEnabled != newEnabled)
                {
                    instance.isEnabled = newEnabled;
/* Style
                    if (instance.viewItemStyle != null)
                    {
                        instance.viewItemStyle.IsEnabled = newEnabled;
                    }
*/
                    instance.UpdateState();
                }
            }
        },
        defaultValueCreator: (bindable) => ((ViewItem)bindable).isEnabled);
        /// This will be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(ViewItem), true, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var instance = (ViewItem)bindable;
            if (newValue != null)
            {
                bool newSelected = (bool)newValue;
                if (instance.isSelected != newSelected)
                {
                    instance.isSelected = newSelected;
/* Style
                    if (instance.viewItemStyle != null)
                    {
                        instance.viewItemStyle.IsSelected = newSelected;
                    }
*/
                    if (instance.isSelectable)
                    {
                        instance.UpdateState();
                    }
                }
            }
        },
        defaultValueCreator: (bindable) =>
        {
            var instance = (ViewItem)bindable;
            return instance.isSelectable && instance.isSelected;
        });
        /// This will be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly BindableProperty IsSelectableProperty = BindableProperty.Create(nameof(IsSelectable), typeof(bool), typeof(ViewItem), true, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var instance = (ViewItem)bindable;
            if (newValue != null)
            {
                bool newSelectable = (bool)newValue;
                if (instance.isSelectable != newSelectable)
                {
                    instance.isSelectable = newSelectable;
/*Style
                    if (instance.viewItemStyle != null)
                    {
                        instance.viewItemStyle.IsSelectable = newSelectable;
                    }
*/
                    instance.UpdateState();
                }
            }
        },
        defaultValueCreator: (bindable) => ((ViewItem)bindable).isSelectable);

        /// <summary>
        /// Internal boolean flag for select state.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        protected bool isSelected = false;
        
        /// <summary>
        /// Internal boolean flag for seletable.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        protected bool isSelectable = true;
        
        /// <summary>
        /// Internal boolean flag for enabled state.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        protected bool isEnabled = true;

        /// <summary>
        /// Internal boolean flag for pressed state.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        protected bool isPressed = false;

        static ViewItem()
        {
        }

        /// <summary>
        /// Creates a new instance of ViewItem.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public ViewItem() : base()
        {
            IsCreateByXaml = true;
            Initialize();
        }

        /// <summary>
        /// Creates a new instance of ViewItem with style.
        /// </summary>
        /// <param name="style">Create ViewItem by special style defined in UX.</param>
        /// <since_tizen> 8 </since_tizen>
        public ViewItem(string style) : base(style)
        {
            Initialize();
        }

        /// <summary>
        /// Creates a new instance of a ViewItem with View content.
        /// </summary>
        /// <param name="content">Create ViewItem with given content</param>
        /// <since_tizen> 8 </since_tizen>
        public ViewItem(View content)
        {
            Initialize();
            Children.Add(content);
        }

        /// <summary>
        /// An event for the ViewItem clicked signal which can be used to subscribe or unsubscribe the event handler provided by the user.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        public event EventHandler<ClickedEventArgs> Clicked;

        /// <summary>
        /// Flag to decide ViewItem can be selected or not.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public bool IsSelectable
        {
            get
            {
                return (bool)GetValue(IsSelectableProperty);
            }
            set
            {
                SetValue(IsSelectableProperty, value);
            }
        }

        /// <summary>
        /// Flag to decide selected state in ViewItem.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public bool IsSelected
        {
            get
            {
                return (bool)GetValue(IsSelectedProperty);
            }
            set
            {
                SetValue(IsSelectedProperty, value);
            }
        }

        /// <summary>
        /// Flag to decide enable or disable in ViewItem.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        public bool IsEnabled
        {
            get
            {
                return (bool)GetValue(IsEnabledProperty);
            }
            set
            {
                SetValue(IsEnabledProperty, value);
            }
        }

/* Style
        private ViewItemStyle viewItemStyle => ViewStyle as ViewItemStyle;
*/

        /// <summary>
        /// Called after a key event is received by the view that has had its focus set.
        /// </summary>
        /// <param name="key">The key event.</param>
        /// <returns>True if the key event should be consumed.</returns>
        /// <since_tizen> 6 </since_tizen>
        public override bool OnKey(Key key)
        {
            if (!IsEnabled || null == key)
            {
                return false;
            }

            if (key.State == Key.StateType.Down)
            {
                if (key.KeyPressedName == "Return")
                {
                    isPressed = true;
                    UpdateState();
                }
            }
            else if (key.State == Key.StateType.Up)
            {
                if (key.KeyPressedName == "Return")
                {
                    bool clicked = isPressed && IsEnabled;

                    isPressed = false;

                    if (IsSelectable)
                    {
                        IsSelected = !IsSelected;
                    }
                    else
                    {
                        UpdateState();
                    }

                    if (clicked)
                    {
                        ClickedEventArgs eventArgs = new ClickedEventArgs();
                        OnClickedInternal(eventArgs);
                    }
                }
            }
            return base.OnKey(key);
        }

        /// <summary>
        /// Called when the control gain key input focus. Should be overridden by derived classes if they need to customize what happens when the focus is gained.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        public override void OnFocusGained()
        {
            base.OnFocusGained();
            UpdateState();
        }

        /// <summary>
        /// Called when the control loses key input focus. Should be overridden by derived classes if they need to customize what happens when the focus is lost.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        public override void OnFocusLost()
        {
            base.OnFocusLost();
            UpdateState();
        }

/* Style
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
                if (buttonStyle.Overlay != null)
                {
                    OverlayImage?.ApplyStyle(buttonStyle.Overlay);
                }

                if (buttonStyle.Text != null)
                {
                    TextLabel?.ApplyStyle(buttonStyle.Text);
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
        /// Data index which is binded to item.
        /// Can access to data using this index.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int Index { get; set; } = 0;

        /// <summary>
        /// DataTemplate of this view object
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DataTemplate Template { get; set; }

        /// <summary>
        /// State of Realization
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsRealized {get; protected set;}

        internal bool IsMeasured;
    }
}
