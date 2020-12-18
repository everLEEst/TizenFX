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
        private List<float> ItemSize = new List<float>();
        private int ItemSizeChanged = -1;

        private float GetItemSize(int index)
        {
            CollectionView colView = ItemsView as CollectionView;
            if (colView.SizingStrategy == ItemSizingStrategy.MeasureAll)
            {
                return ItemSize[index];
            }
            else
            {
                return SizeCandidate;
            }
            
        }
        private void UpdatePosition(int Index)
        {
            CollectionView colView = ItemsView as CollectionView;
            bool IsGroup = (colView.InternalItemSource is IGroupableItemSource);
            bool IsGroupHeader = false;
            bool IsGroupFooter = false;

            if (Index <= 0) return;
            if (Index >= colView.InternalItemSource.Count)

            if (IsGroup)
            {
                IsGroupHeader = (colView.InternalItemSource as IGroupableItemSource).IsGroupHeader(Index);
                IsGroupFooter = (colView.InternalItemSource as IGroupableItemSource).IsGroupFooter(Index);
            }

            ItemPosition[Index] = ItemPosition[Index-1] + GetItemSize(Index);           
        }

        private void FindVisibleItems(Vector2 visibleArea)
        {
            int MaxIndex = ItemsView.InternalItemSource.Count - 1;
            if ((ItemsView as CollectionView).SizingStrategy == ItemSizingStrategy.MeasureAll)
            {
                FirstVisible = Convert.ToInt32(Math.Abs(visibleArea.X / SizeCandidate)) - 3;
                LastVisible = Convert.ToInt32(Math.Abs(visibleArea.Y / SizeCandidate)) + 3;
            }
            else
            {
                //Need to Consider GroupHeight!!!!
                FirstVisible = Convert.ToInt32(Math.Abs(visibleArea.X / SizeCandidate)) - 3;
                LastVisible = Convert.ToInt32(Math.Abs(visibleArea.Y / SizeCandidate)) + 3;
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
                for (int i = 0; i < VisibleItems.Count; i++)
                {
                    ViewItem item = VisibleItems[i];
                    VisibleItems.Remove(item);
                    ItemsView.UnrealizeItem(item);
                }                
            }

            FirstVisible = 0;
            LastVisible = 0;
            ItemPosition.Clear();
            ItemSize.Clear();

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

            float Width, Height, FingerSize = 50.0f;
            // Need to Set proper hieght or width on scroll direciton.



            if (SizeDeligate.Layout == null)
            {
                Console.WriteLine("LSH: Layout is NULL!!!!!");
                Width = SizeDeligate.Size.Width;
                Height = SizeDeligate.Size.Height;
            }
            else
            {                 
                SizeDeligate.Layout.Measure(new MeasureSpecification(new LayoutLength(isHorizontal ? FingerSize : view.Size.Width), 
                                             ( isHorizontal ? MeasureSpecification.ModeType.Unspecified :
                                                              MeasureSpecification.ModeType.Exactly)),
                                            new MeasureSpecification(new LayoutLength(isHorizontal ? view.Size.Height : FingerSize),
                                             ( isHorizontal ? MeasureSpecification.ModeType.Exactly :
                                                              MeasureSpecification.ModeType.Unspecified)));

                Width = SizeDeligate.Layout.MeasuredWidth.Size.AsRoundedValue();
                Height = SizeDeligate.Layout.MeasuredHeight.Size.AsRoundedValue();

                Width = Math.Max(Width, SizeDeligate.Size.Width);
                Height = Math.Max(Height, SizeDeligate.Size.Height);
            }
            
            Console.WriteLine("LSH : Layout Size {0} :{0}", Width, Height);

            // pick the SizeCandidate.
            SizeCandidate = isHorizontal ? Width : Height;
            if (SizeCandidate == 0) SizeCandidate = 1;

            for (int i = 0; i < view.InternalItemSource.Count; i++)
            {
                if ((view as CollectionView).SizingStrategy == ItemSizingStrategy.MeasureAll)
                  ItemSize.Add(SizeCandidate);
                ItemPosition.Add(SizeCandidate * i);
            }

            // ItemsView.UnrealizeItem(SizeDeligate); keep the SizeDeligate for tracking SizeChange.
            ScrollContentSize = SizeCandidate * view.InternalItemSource.Count;

            base.Initialize(view);
            Console.WriteLine("Init Done");
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
            int LastIndex = ItemsView.InternalItemSource.Count;

            if (ItemSizeChanged >= 0)
            {
                for (int i = ItemSizeChanged; i < LastIndex; i++)
                    UpdatePosition(i);
            }
            ScrollContentSize = ItemPosition[LastIndex - 1] + GetItemSize(LastIndex);

            int prevFirstVisible = FirstVisible;
            int prevLastVisible = LastVisible;
            bool isHorizontal = (ItemsView.ScrollingDirection == ScrollableBase.Direction.Horizontal);
          
            Vector2 visibleArea = new Vector2(Math.Abs(scrollPosition),
                Math.Abs(scrollPosition) + ( isHorizontal ? ItemsView.Size.Width : ItemsView.Size.Height)
            );

            //Console.WriteLine("LSH :: itemsView [{0},{1}] [{2},{3}]", ItemsView.Size.Width, ItemsView.Size.Height, ItemsView.ContentContainer.Size.Width, ItemsView.ContentContainer.Size.Height);

            // 1. Set First/Last Visible Item Index. 
            FindVisibleItems(visibleArea);
            Console.WriteLine("LSH :: visibleArea before [{0},{1}] after [{2},{3}]", prevFirstVisible, prevLastVisible, FirstVisible, LastVisible);

            // 2. Unrealize invisible items.
            for (int i = 0; i < VisibleItems.Count; i++)
            {
                ViewItem item = VisibleItems[i];

                if (item.Index < FirstVisible || item.Index > LastVisible)
                {
                   //Console.WriteLine("LSH :: Unrealize{0}!", item.Index);
                   VisibleItems.Remove(item);
                   ItemsView.UnrealizeItem(item);                  
                }
            }
            
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
                    if (item)
                    {
                        /*Console.WriteLine("item[{0}] is exist!", i);
                        item.Position = ( isHorizontal ?
                        new Position(
                            ItemPosition[i],
                            item.PositionY
                        ):
                        new Position(
                            item.PositionX,
                            ItemPosition[i]
                        ));*/
                        continue;
                    }

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
                item.Position = ( isHorizontal ?
                        new Position(
                            ItemPosition[i],
                            item.PositionY
                        ):
                        new Position(
                            item.PositionX,
                            ItemPosition[i]
                        ));
                // Console.WriteLine("LSH :: ["+item.Index+"] ["+item.Position.X+", "+item.Position.Y+" ==== \n");
            }
            //Console.WriteLine("Realize Done");
        }

        /// <inheritdoc/>
        public override void NotifyItemSizeChanged(ViewItem item)
        {
            if (!IsInitialized) return;
            if ((ItemsView as CollectionView).SizingStrategy == ItemSizingStrategy.MeasureFirst &&
                item.Index != 0)
              return;

            Console.WriteLine("item index is {0}", item.Index); 

            if (item.Index < 0) return;
            bool isHorizontal = ItemsView.ScrollingDirection == ScrollableBase.Direction.Horizontal;
            float PrevSize, CurrentSize, Diff;
            if (item.Index == (ItemsView.InternalItemSource.Count-1))
            {
                PrevSize = ScrollContentSize - ItemPosition[item.Index];
            }
            else
            {
                PrevSize = ItemPosition[item.Index + 1] - ItemPosition[item.Index];
            }

            CurrentSize = (isHorizontal ? item.Size.Width : item.Size.Height);

            if (CurrentSize != PrevSize)
            {
                if ((ItemsView as CollectionView).SizingStrategy == ItemSizingStrategy.MeasureAll)
                  ItemSize[item.Index] = CurrentSize;
                else
                  SizeCandidate = CurrentSize;
            }
            if (ItemSizeChanged == -1) ItemSizeChanged = item.Index;
            else ItemSizeChanged = Math.Min(ItemSizeChanged, item.Index);

            //ScrollContentSize += Diff; UpdateOnce?
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

            Console.WriteLine("LSH :: Calculate Layout ScrollContentSize {0}", ScrollContentSize);
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
            Console.WriteLine("LSH :: Calculate Candidate ScrollContentSize {0}", ScrollContentSize);
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
