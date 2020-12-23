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
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Binding;

namespace Tizen.NUI.Components
{
    /// <summary>
    /// [Draft] This class provides a View that can layouting items in list and grid with high performance.
    /// </summary>
    /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API.
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class CollectionView : ItemsView
    {

		public DataTemplate GroupHeaderTemplate;
        public DataTemplate GroupFooterTemplate;
        public View Header;
        public View Footer;
        public bool IsGrouped = false;
        public ItemSelectionMode SelectionMode;

		public static readonly BindableProperty SelectedItemProperty =
			BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(CollectionView), null,
				propertyChanged: (bindable, oldValue, newValue)=>
                {
			        var colView = (CollectionView)bindable;
                    var args = new SelectionChangedEventArgs(oldValue, newValue);
                    oldValue = colView.selectedItem;
                    
                    foreach(View item in colView.ContentContainer.Children)
                    {
                        if (item as ViewItem)
                        {
                            var vItem = item as ViewItem;
                            if (vItem.BindingContext == null) continue;
                            if (vItem.BindingContext == oldValue) vItem.IsSelected = false;
                            else if (item.BindingContext == newValue) vItem.IsSelected = true;
                        }
                    }

                    SelectionPropertyChanged(colView, args);
		        },
				defaultValueCreator: (bindable)=>
                {
                    var colView = (CollectionView)bindable;
                    return colView.selectedItem;
                });

		public static readonly BindableProperty SelectedItemsProperty =
			BindableProperty.Create(nameof(SelectedItems), typeof(IList<object>), typeof(CollectionView), null,
				propertyChanged: (bindable, oldValue, newValue)=>
                {
                    var colView = (CollectionView)bindable;
                    var oldSelection = (IList<object>)colView.selectedItems ?? s_empty;
                    //FIXME : CoerceSelectedItems calls only isCreatedByXaml
                    var newSelection = (SelectionList)CoerceSelectedItems(colView, newValue);
                    colView.selectedItems = newSelection;
			        colView.SelectedItemsPropertyChanged(oldSelection, newSelection);
                },
				defaultValueCreator: (bindable) =>
                {
                    var colView = (CollectionView)bindable;
                    colView.selectedItems = colView.selectedItems ?? new SelectionList(colView);
                    return colView.selectedItems;
                });

		static readonly IList<object> s_empty = new List<object>(0);
        private object selectedItem;

		public object SelectedItem
		{
			get => GetValue(SelectedItemProperty);
			set => SetValue(SelectedItemProperty, value);
		}

        private SelectionList selectedItems;

		public IList<object> SelectedItems
		{
			get => (IList<object>)GetValue(SelectedItemsProperty);
			set => SetValue(SelectedItemsProperty, new SelectionList(this, value));
		}

        public ICommand SelectionChangedCommand;

        public object SelectionChangedCommandParameter;

		public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

		bool _suppressSelectionChangeNotification;

        public ItemSizingStrategy SizingStrategy;
        private List<PropertyNotification> notifications = new List<PropertyNotification>();


        
        public CollectionView() : base()
        {
            FocusGroup = true;
            Init();
            SetKeyboardNavigationSupport(true);
            Scrolling += OnScrolling;
        }



        private void Init()
        {
            if (ItemsSource == null) return;
            if (ItemsLayouter == null) return;
            if (ItemsTemplate == null) return;

            ItemsLayouter.RequestLayout(0.0f, true);

            if (ScrollingDirection == Direction.Horizontal)
            {
                ContentContainer.SizeWidth = ItemsLayouter.CalculateLayoutOrientationSize();
            }
            else
            {
                ContentContainer.SizeHeight = ItemsLayouter.CalculateLayoutOrientationSize();
            }
        }

        internal override ViewItem RealizeItem(int Index)
        {
            ViewItem item = base.RealizeItem(Index);

            switch (SelectionMode)
            {
                case ItemSelectionMode.Single:
                    if (item.BindingContext == SelectedItem) item.IsSelected = true;
                    break;

                case ItemSelectionMode.Multiple:
                    if (SelectedItems!=null && SelectedItems.Contains(item.BindingContext)) item.IsSelected = true;
                    break;
                case ItemSelectionMode.None:
                    item.IsSelectable = false;
                    break;
            }
            
            
            return item;
        }

        internal override void UnrealizeItem(ViewItem item)
        {
            item.IsSelected = false;
            base.UnrealizeItem(item);
        }


        


		public void UpdateSelectedItems(IList<object> newSelection)
		{
			var oldSelection = new List<object>(SelectedItems);

			_suppressSelectionChangeNotification = true;

			SelectedItems.Clear();

			if (newSelection?.Count > 0)
			{
				for (int n = 0; n < newSelection.Count; n++)
				{
					SelectedItems.Add(newSelection[n]);
				}
			}

			_suppressSelectionChangeNotification = false;

			SelectedItemsPropertyChanged(oldSelection, newSelection);
		}

		protected virtual void OnSelectionChanged(SelectionChangedEventArgs args)
		{
		}

		static object CoerceSelectedItems(BindableObject bindable, object value)
		{
			if (value == null)
			{
				return new SelectionList((CollectionView)bindable);
			}

			if (value is SelectionList)
			{
				return value;
			}

			return new SelectionList((CollectionView)bindable, value as IList<object>);
		}


		internal void SelectedItemsPropertyChanged(IList<object> oldSelection, IList<object> newSelection)
		{
			if (_suppressSelectionChangeNotification)
			{
				return;
			}

            foreach(View content in ContentContainer.Children)
            {
                if (content as ViewItem)
                {
                    var item = (ViewItem)content;
                    var binding = item.BindingContext;
                    if (binding == null) continue;
                    if (newSelection.Contains(binding))
                    {
                        if (!item.IsSelected) item.IsSelected = true;
                    }
                    else
                    {
                        if (item.IsSelected) item.IsSelected = false;
                    }
                }
            }
			SelectionPropertyChanged(this, new SelectionChangedEventArgs(oldSelection, newSelection));

			OnPropertyChanged(SelectedItemsProperty.PropertyName);
		}

		static void SelectionPropertyChanged(CollectionView colView, SelectionChangedEventArgs args)
		{
			var command = colView.SelectionChangedCommand;

			if (command != null)
			{
				var commandParameter = colView.SelectionChangedCommandParameter;

				if (command.CanExecute(commandParameter))
				{
					command.Execute(commandParameter);
				}
			}
			colView.SelectionChanged?.Invoke(colView, args);
			colView.OnSelectionChanged(args);
		}

		static void SelectionModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var colView = (CollectionView)bindable;

			var oldMode = (ItemSelectionMode)oldValue;
			var newMode = (ItemSelectionMode)newValue;

			IList<object> previousSelection = new List<object>();
			IList<object> newSelection = new List<object>();

			switch (oldMode)
			{
				case ItemSelectionMode.None:
					break;
				case ItemSelectionMode.Single:
					if (colView.SelectedItem != null)
					{
						previousSelection.Add(colView.SelectedItem);
					}
					break;
				case ItemSelectionMode.Multiple:
					previousSelection = colView.SelectedItems;
					break;
			}

			switch (newMode)
			{
				case ItemSelectionMode.None:
					break;
				case ItemSelectionMode.Single:
					if (colView.SelectedItem != null)
					{
						newSelection.Add(colView.SelectedItem);
					}
					break;
				case ItemSelectionMode.Multiple:
					newSelection = colView.SelectedItems;
					break;
			}

			if (previousSelection.Count == newSelection.Count)
			{
				if (previousSelection.Count == 0 || (previousSelection[0] == newSelection[0]))
				{
					// Both selections are empty or have the same single item; no reason to signal a change
					return;
				}
			}

			var args = new SelectionChangedEventArgs(previousSelection, newSelection);
			SelectionPropertyChanged(colView, args);
		}

        public new Direction ScrollingDirection
        {
            get
            {
                return base.ScrollingDirection;
            }
            set
            {
                base.ScrollingDirection = value;

                if (ScrollingDirection == Direction.Horizontal)
                {
                    ContentContainer.SizeWidth = ItemsLayouter.CalculateLayoutOrientationSize();
                }
                else
                {
                    ContentContainer.SizeHeight = ItemsLayouter.CalculateLayoutOrientationSize();
                }
            }
        }

        private void OnScrolling(object source, ScrollEventArgs args)
        {
            //ItemsLayouter.RequestLayout(ScrollingDirection == Direction.Horizontal ? args.Position.X : args.Position.Y);
        }

        /// <summary>
        /// Adjust scrolling position by own scrolling rules.
        /// Override this function when developer wants to change destination of flicking.(e.g. always snap to center of item)
        /// </summary>
        /// <param name="position">Scroll position which is calculated by ScrollableBase</param>
        /// <returns>Adjusted scroll destination</returns>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override float AdjustTargetPositionOfScrollAnimation(float position)
        {
            // Destination is depending on implementation of layout manager.
            // Get destination from layout manager.
            return ItemsLayouter.CalculateCandidateScrollPosition(position);
        }

        private View focusedView;
        private int prevFocusedDataIndex = 0;

        public override View GetNextFocusableView(View currentFocusedView, View.FocusDirection direction, bool loopEnabled)
        {
            View nextFocusedView = null;

            if (!focusedView)
            {
                // If focusedView is null, find child which has previous data index
                if (Children.Count > 0 && InternalItemSource.Count > 0)
                {
                    for (int i = 0; i < Children.Count; i++)
                    {
                        RecycleItem item = Children[i] as RecycleItem;
                        if (item.DataIndex == prevFocusedDataIndex)
                        {
                            nextFocusedView = item;
                            break;
                        }
                    }
                }
            }
            else
            {
                // If this is not first focus, request next focus to LayoutManager
                nextFocusedView = ItemsLayouter.RequestNextFocusableView(currentFocusedView, direction, loopEnabled);
            }

            if (nextFocusedView)
            {
                // Check next focused view is inside of visible area.
                // If it is not, move scroll position to make it visible.
                Position scrollPosition = ContentContainer.CurrentPosition;
                float targetPosition = -(ScrollingDirection == Direction.Horizontal ? scrollPosition.X : scrollPosition.Y);

                float left = nextFocusedView.Position.X;
                float right = nextFocusedView.Position.X + nextFocusedView.Size.Width;
                float top = nextFocusedView.Position.Y;
                float bottom = nextFocusedView.Position.Y + nextFocusedView.Size.Height;

                float visibleRectangleLeft = -scrollPosition.X;
                float visibleRectangleRight = -scrollPosition.X + Size.Width;
                float visibleRectangleTop = -scrollPosition.Y;
                float visibleRectangleBottom = -scrollPosition.Y + Size.Height;

                if (ScrollingDirection == Direction.Horizontal)
                {
                    if ((direction == View.FocusDirection.Left || direction == View.FocusDirection.Up) && left < visibleRectangleLeft)
                    {
                        targetPosition = left;
                    }
                    else if ((direction == View.FocusDirection.Right || direction == View.FocusDirection.Down) && right > visibleRectangleRight)
                    {
                        targetPosition = right - Size.Width;
                    }
                }
                else
                {
                    if ((direction == View.FocusDirection.Up || direction == View.FocusDirection.Left) && top < visibleRectangleTop)
                    {
                        targetPosition = top;
                    }
                    else if ((direction == View.FocusDirection.Down || direction == View.FocusDirection.Right) && bottom > visibleRectangleBottom)
                    {
                        targetPosition = bottom - Size.Height;
                    }
                }

                focusedView = nextFocusedView;
                prevFocusedDataIndex = (nextFocusedView as RecycleItem).DataIndex;

                ScrollTo(targetPosition, true);
            }
            else
            {
                // If nextView is null, it means that we should move focus to outside of Control.
                // Return FocusableView depending on direction.
                switch (direction)
                {
                    case View.FocusDirection.Left:
                    {
                        nextFocusedView = LeftFocusableView;
                        break;
                    }
                    case View.FocusDirection.Right:
                    {
                        nextFocusedView = RightFocusableView;
                        break;
                    }
                    case View.FocusDirection.Up:
                    {
                        nextFocusedView = UpFocusableView;
                        break;
                    }
                    case View.FocusDirection.Down:
                    {
                        nextFocusedView = DownFocusableView;
                        break;
                    }
                }

                if(nextFocusedView)
                {
                    focusedView = null;
                }
                else
                {
                    //If FocusableView doesn't exist, not move focus.
                    nextFocusedView = focusedView;
                }
            }

            return nextFocusedView;
        }
    }
}
