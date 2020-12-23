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
    /// [Draft] This class implements a grid box layout.
    /// </summary>
    /// <since_tizen> 8 </since_tizen>
    /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class GridLayouter : ItemsLayouter
    {
        private int spanSize = 1;
        private int userSpanSize;
        private int ItemSizeChanged = -1;
        private float align = 0.5f;
        private Size2D SizeCandidate;
        private Position GetPosition(ViewItem item)
        {
            if (SizeCandidate == null) return null;
            bool isHorizontal = (ItemsView.ScrollingDirection == ScrollableBase.Direction.Horizontal);
            // int convert must be truncate value.
            int division = item.Index / spanSize;
            int remainder = item.Index % spanSize;
            int emptyArea = isHorizontal ? (int)(ItemsView.Size.Height - (SizeCandidate.Height * spanSize)) :
                                            (int)(ItemsView.Size.Width - (SizeCandidate.Width * spanSize));
            if (division < 0) division = 0;
            if (remainder < 0) remainder = 0;

            float xPos = isHorizontal ? division * SizeCandidate.Width : emptyArea * align + remainder * SizeCandidate.Width;
            float yPos = isHorizontal ? emptyArea * align + remainder * SizeCandidate.Height : division * SizeCandidate.Height;

            return new Position(xPos, yPos);
        }

        private void FindVisibleItems(Vector2 visibleArea)
        {
            int MaxIndex = ItemsView.InternalItemSource.Count - 1;
            if ((ItemsView as CollectionView).SizingStrategy == ItemSizingStrategy.MeasureAll)
            {
                FirstVisible = (Convert.ToInt32(Math.Abs(visibleArea.X / StepCandidate)) - 1) * spanSize;
                LastVisible = (Convert.ToInt32(Math.Abs(visibleArea.Y / StepCandidate)) + 1) * spanSize;
            }
            else
            {
                //Need to Consider GroupHeight!!!!
                FirstVisible = (Convert.ToInt32(Math.Abs(visibleArea.X / StepCandidate)) - 1) * spanSize;
                LastVisible = (Convert.ToInt32(Math.Abs(visibleArea.Y / StepCandidate)) + 1) * spanSize;
            }
            if (FirstVisible < 0) FirstVisible = 0;
            if (LastVisible > (MaxIndex)) LastVisible = MaxIndex;
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
        public override void Initialize(ItemsView view)
        {
            // 1. Clean Up
            if (VisibleItems != null)
            {
                foreach (ViewItem item in VisibleItems)
                {
                    view.UnrealizeItem(item);
                }
                VisibleItems.Clear();          
            }

            FirstVisible = 0;
            LastVisible = 0;

            bool isHorizontal = (view.ScrollingDirection == ScrollableBase.Direction.Horizontal);

            // Get Size Deligate. FIXME if group exist index must be changed.
            ViewItem SizeDeligate = view.RealizeItem(0);   
            if (SizeDeligate == null)
            {
                // error !
                return;
            }
            //FIXME: if header is exist, index must be changed.
            int FirstIndex = 0;
            SizeDeligate.BindingContext = view.InternalItemSource.GetItem(FirstIndex);

            float Width, Height, FingerSize = 30.0f;
            // Need to Set proper hieght or width on scroll direciton.

            if (SizeDeligate.Size.Width != 0 &&
                SizeDeligate.Size.Height != 0)
            {
                Width = SizeDeligate.Size.Width;
                Height = SizeDeligate.Size.Height;
            }
            else
            {                 
                SizeDeligate.Layout.Measure(new MeasureSpecification(new LayoutLength(FingerSize), MeasureSpecification.ModeType.Unspecified),
                                            new MeasureSpecification(new LayoutLength(FingerSize), MeasureSpecification.ModeType.Unspecified));

                Width = SizeDeligate.Layout.MeasuredWidth.Size.AsRoundedValue();
                Height = SizeDeligate.Layout.MeasuredHeight.Size.AsRoundedValue();

                Width = Math.Max(Width, SizeDeligate.Size.Width);
                Height = Math.Max(Height, SizeDeligate.Size.Height);
            }
            
            //Console.WriteLine("LSH : item Size {0} :{1}", Width, Height);

            // pick the StepCandidate.
            StepCandidate = isHorizontal ? Width : Height;
            spanSize = isHorizontal ? Convert.ToInt32(Math.Truncate((double)(view.Size.Height / Height))) : 
                                      Convert.ToInt32(Math.Truncate((double)(view.Size.Width / Width)));
            if (StepCandidate < 1) StepCandidate = 1;
            if (spanSize < 1) spanSize = 1;

            SizeCandidate = new Size2D(Convert.ToInt32(Width), Convert.ToInt32(Height));

            int count = view.InternalItemSource.Count;
            // ItemsView.UnrealizeItem(SizeDeligate); keep the SizeDeligate for tracking SizeChange?
            ScrollContentSize = StepCandidate * Convert.ToInt32(Math.Ceiling((double)count / (double)spanSize));
            if (isHorizontal) view.ContentContainer.SizeWidth = ScrollContentSize;
            else view.ContentContainer.SizeHeight = ScrollContentSize; 

            view.UnrealizeItem(SizeDeligate);

            base.Initialize(view);
            //Console.WriteLine("Init Done, StepCnadidate{0}, spanSize{1}, Scroll{2}", StepCandidate, spanSize, ScrollContentSize);
        }

        /// <summary>
        /// This is called to find out where items are lain out according to current scroll position.
        /// </summary>
        /// <param name="scrollPosition">Scroll position which is calculated by ScrollableBase</param>
        /// <param name="force">boolean force flag to layouting forcely.</param>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        public override void RequestLayout(float scrollPosition, bool force = false)
        {
            // Layouting is only possible after once it intialized.
            if (!IsInitialized) return;
            int LastIndex = ItemsView.InternalItemSource.Count;

            if (!force && PrevScrollPosition == Math.Abs(scrollPosition)) return;
            PrevScrollPosition = Math.Abs(scrollPosition);

            int prevFirstVisible = FirstVisible;
            int prevLastVisible = LastVisible;
            bool isHorizontal = (ItemsView.ScrollingDirection == ScrollableBase.Direction.Horizontal);

          
            Vector2 visibleArea = new Vector2(PrevScrollPosition,
                PrevScrollPosition + ( isHorizontal ? ItemsView.Size.Width : ItemsView.Size.Height)
            );

            //Console.WriteLine("LSH :: itemsView [{0},{1}] [{2},{3}]", ItemsView.Size.Width, ItemsView.Size.Height, ItemsView.ContentContainer.Size.Width, ItemsView.ContentContainer.Size.Height);

            // 1. Set First/Last Visible Item Index. 
            FindVisibleItems(visibleArea);
            //Console.WriteLine("LSH :: {0} :visibleArea before [{1},{2}] after [{3},{4}]", scrollPosition, prevFirstVisible, prevLastVisible, FirstVisible, LastVisible);

            // 2. Unrealize invisible items.
            List<ViewItem> unrealizedItems = new List<ViewItem>();
            foreach (ViewItem item in VisibleItems)
            {
                if (item.Index < FirstVisible || item.Index > LastVisible)
                {
                   //Console.WriteLine("LSH :: Unrealize{0}!", item.Index);
                   unrealizedItems.Add(item);
                   ItemsView.UnrealizeItem(item);                  
                }
            }
            VisibleItems.RemoveAll(unrealizedItems.Contains);
          
            //Console.WriteLine("Realize Begin [{0} to {1}]", FirstVisible, LastVisible);
            // 3. Realize and placing visible items.
            for (int i = FirstVisible; i <= LastVisible; i++)
            {
                //Console.WriteLine("LSH :: Realize!");
                ViewItem item = null;
                // 4. Get item if visible or realize new.
                if (i >= prevFirstVisible && i <= prevLastVisible)
                {
                    item = GetVisibleItem(i);
                    if (item) continue;
                }
                if (item == null) item = ItemsView.RealizeItem(i);                

                //if (item is not calced) do measure!   
                if (item == null)
                {
                    Console.WriteLine("LSH :: Failed to realize item");
                    return;
                }
                VisibleItems.Add(item);
                // 5. Placing item.
                item.Position = GetPosition(item);
                // Console.WriteLine("LSH :: ["+item.Index+"] ["+item.Position.X+", "+item.Position.Y+" ==== \n");
            }
            //Console.WriteLine("Realize Done");
        }

        /// <inheritdoc/>
        public override void NotifyItemSizeChanged(ViewItem item)
        {
            // All Item size need to be same in grid!
            // if you want to change item size, change dataTemplate to re-initing.
            return;
        }

        /// <summary>
        /// This is called to find out how much container size can be.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override float CalculateLayoutOrientationSize()
        {
            //Console.WriteLine("LSH :: Calculate Layout ScrollContentSize {0}", ScrollContentSize);
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
            //Console.WriteLine("LSH :: Calculate Candidate ScrollContentSize {0}", ScrollContentSize);
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
