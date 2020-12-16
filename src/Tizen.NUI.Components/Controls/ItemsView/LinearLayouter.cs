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
using Tizen.NUI.BaseComponents;
using System.Collections.Generic;
using System.ComponentModel;
using Tizen.NUI.Binding;

namespace Tizen.NUI.Components
{
    /// <summary>
    /// [Draft] This class implements a linear box layout.
    /// </summary>
    /// <since_tizen> 8 </since_tizen>
    /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class LinearLayouter : ItemsLayouter
    {
        private List<float> ItemPosition = new List<float>();

        private float CalculatePosition(int Index, ItemSizingStrategy Strategy)
        {
            CollectionView colView = ItemsView as CollectionView;
            float StepSize = SizeCandidate;
            bool IsGroup = (colView.InternalItemSource is IGroupableItemSource);
            bool IsGroupHeader = false;
            bool IsGroupFooter = false;

            if (IsGroup)
            {
                IsGroupHeader = (colView.InternalItemSource as IGroupableItemSource).IsGroupHeader(Index);
                IsGroupFooter = (colView.InternalItemSource as IGroupableItemSource).IsGroupFooter(Index);
            }

            if (Strategy.Equals(ItemSizingStrategy.MeasureAll))
            {
                //Do Measure ?

                //Prev Item Size;;;

                // Add SizeChangedEventArgs = UpdatePositionEvent..

            }
            
            if (Index == 0) ItemPosition[Index] = 0.0f;
            else
            {
                float PrevPosition = ItemPosition[Index - 1];
                ItemPosition[Index] = PrevPosition + StepSize;
            }
            
            return ItemPosition[Index];      
        }

        private void FindVisibleItems(Vector2 visibleArea)
        {
            if ((ItemsView as CollectionView).SizingStrategy == ItemSizingStrategy.MeasureAll)
            {
                // NEED TO FIX!!!!!!
                FirstVisible = Convert.ToInt32(Math.Abs(visibleArea.X / SizeCandidate)) - 1;
                LastVisible = Convert.ToInt32(Math.Abs(visibleArea.Y / SizeCandidate)) + 1;
            }
            else
            {
                //Need to Consider GroupHeight!!!!
                FirstVisible = Convert.ToInt32(Math.Abs(visibleArea.X / SizeCandidate)) - 1;
                LastVisible = Convert.ToInt32(Math.Abs(visibleArea.Y / SizeCandidate)) + 1;
            }
        }

        private ViewItem GetVisibleItem(int index)
        {
            foreach (ViewItem item in VisibleItems)
            {
                if (item.Index == index) return item;
            }

            return null;
        }

        /// <summary>
        /// Clean up ItemsLayouter.
        /// </summary>
        /// <param name="view"> ItemsView of layouter. </param>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Initialze(ItemsView view)
        {
            base.Initialze(view);

            View SizeDeligate = null;
            object Proxy = ItemsView.ItemsTemplate.CreateContent();
            if (Proxy is View) SizeDeligate = (View)Proxy;
            else
            {
                // error !
                return;
            }
            //FIXME: if header is exist, index must be changed.
            int FirstIndex = 0;
            float Pos = 0.0f;
            SizeDeligate.BindingContext = ItemsView.InternalItemSource.GetItem(FirstIndex);

            float Width, Height;
            // Need to Set proper hieght or width on scroll direciton.
            MeasureSpecification WidthSpec = new MeasureSpecification(new LayoutLength(0.0f), MeasureSpecification.ModeType.AtMost);
            MeasureSpecification HeightSpec = new MeasureSpecification(new LayoutLength(0.0f), MeasureSpecification.ModeType.AtMost);

            SizeDeligate.Layout.Measure(WidthSpec, HeightSpec);

            Width = SizeDeligate.Layout.MeasuredWidth.Size.AsDecimal();
            Height = SizeDeligate.Layout.MeasuredHeight.Size.AsDecimal();

            // pick the SizeCandidate.
            SizeCandidate = (ItemsView.ScrollingDirection.Equals(ScrollableBase.Direction.Horizontal) ? Width : Height);

            for (int i = 0; i < ItemsView.InternalItemSource.Count; i++)
            {
                Pos = CalculatePosition(i, ItemSizingStrategy.MeasureFirst);
                if (i == ItemsView.InternalItemSource.Count - 1)
                {
                    Pos = Pos + SizeCandidate;
                }
            }

            ScrollContentSize = Pos;
        }

        /// <summary>
        /// This is called to find out where items are lain out according to current scroll position.
        /// </summary>
        /// <param name="scrollPosition">Scroll position which is calculated by ScrollableBase</param>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        public override void RequestLayout(float scrollPosition)
        {
            // Layouting is only possible after once it intialized.
            if (!IsInitialized) return;

            int prevFirstVisible = FirstVisible;
            int prevLastVisible = LastVisible;
            bool isHorizontal = (ItemsView.ScrollingDirection == ScrollableBase.Direction.Horizontal);

            List<ViewItem> currentVisibleItems = new List<ViewItem>();
            
            Vector2 visibleArea = new Vector2(Math.Abs(scrollPosition),
                Math.Abs(scrollPosition) + ( isHorizontal ? ItemsView.Size.Width : ItemsView.Size.Height)
            );

            // 1. Set First/Last Visible Item Index. 
            FindVisibleItems(visibleArea);

            // 2. Unrealize invisible items.
            foreach (ViewItem item in VisibleItems)
            {
                if (item.Index < FirstVisible || item.Index > LastVisible)
                {
                   VisibleItems.Remove(item);
                   ItemsView.UnrealizeItem(item);
                }
            }
            
            // 3. Realize and placing visible items.
            for (int i = FirstVisible; i <= LastVisible; i++)
            {
                ViewItem item = null;
                // 4. Get item if visible or realize new.
                if (i >= prevFirstVisible && i <= prevLastVisible)
                {
                    item = GetVisibleItem(i);
                    VisibleItems.Remove(item);

                }
                else item = ItemsView.RealizeItem(i);

                //if (item is not calced) do measure!   

                // 5. Placing item.
                item.Position = ( isHorizontal ?
                        new Position(
                            ItemPosition[i],
                            item.PositionY
                        ):
                        new Position(
                            item.PositionX,
                            ItemPosition[i]
                        ));
                Tizen.Log.Error("NUI","["+item.Index+"] ["+item.Position.X+", "+item.Position.Y+" ==== \n");
            }
        }

        /// <summary>
        /// This is called to find out how much container size can be.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override float CalculateLayoutOrientationSize()
        {
            // if (Container.SizingStrategy == ItemSizingStrategy.MeasureFirst)
            return ScrollContentSize;
        }

        /// <summary>
        /// Adjust scrolling position by own scrolling rules.
        /// </summary>
        /// <param name="scrollPosition">Scroll position which is calculated by ScrollableBase</param>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        public override float CalculateCandidateScrollPosition(float scrollPosition)
        {
            return scrollPosition;
        }

        /// <summary>
        /// Gets the next keyboard focusable view in this control towards the given direction.<br />
        /// A control needs to override this function in order to support two dimensional keyboard navigation.<br />
        /// </summary>
        /// <param name="currentFocusedView">The current focused view.</param>
        /// <param name="direction">The direction to move the focus towards.</param>
        /// <param name="loopEnabled">Whether the focus movement should be looped within the control.</param>
        /// <returns>The next keyboard focusable view in this control or an empty handle if no view can be focused.</returns>
        public override View RequestNextFocusableView(View currentFocusedView, View.FocusDirection direction, bool loopEnabled)
        {
            View nextFocusedView = null;
            int targetSibling = -1;
            bool isHorizontal = ItemsView.ScrollingDirection == ScrollableBase.Direction.Horizontal;

            switch(direction)
            {
                case View.FocusDirection.Left :
                {
                    targetSibling = isHorizontal ? currentFocusedView.SiblingOrder - 1 : targetSibling;
                    break;
                }
                case View.FocusDirection.Right :
                {
                    targetSibling = isHorizontal ? currentFocusedView.SiblingOrder + 1 : targetSibling;
                    break;
                }
                case View.FocusDirection.Up :
                {
                    targetSibling = isHorizontal ? targetSibling : currentFocusedView.SiblingOrder - 1;
                    break;
                }
                case View.FocusDirection.Down :
                {
                    targetSibling = isHorizontal ? targetSibling : currentFocusedView.SiblingOrder + 1;
                    break;
                }
            }

            if(targetSibling > -1 && targetSibling < Container.Children.Count)
            {
                ViewItem candidate = Container.Children[targetSibling] as ViewItem;
                if(candidate.Index >= 0 && candidate.Index < ItemsView.InternalItemSource.Count)
                {
                    nextFocusedView = candidate;
                }
            }

            return nextFocusedView;
        }
    }
}
