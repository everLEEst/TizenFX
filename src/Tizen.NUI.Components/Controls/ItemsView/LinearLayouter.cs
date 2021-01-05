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
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class LinearLayouter : ItemsLayouter
    {
        private List<float> ItemPosition = new List<float>();
        private List<float> ItemSize = new List<float>();
        private int ItemSizeChanged = -1;
        private CollectionView colView; 

        /// <summary>
        /// Clean up ItemsLayouter.
        /// </summary>
        /// <param name="view"> ItemsView of layouter. </param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Initialize(ItemsView view)
        {
            colView = view as CollectionView;
            if (colView == null)
            {
                throw new ArgumentException("GridLayouter only can be applied CollectionView.", nameof(view));
            }
            // 1. Clean Up
            if (VisibleItems != null)
            {
                foreach (ViewItem item in VisibleItems)
                {
                    colView.UnrealizeItem(item, false);
                }
                VisibleItems.Clear();
            }

            FirstVisible = 0;
            LastVisible = 0;
            ItemPosition.Clear();
            ItemSize.Clear();

            isHorizontal = (colView.ScrollingDirection == ScrollableBase.Direction.Horizontal);

            ViewItem Header = colView?.Header;
            ViewItem Footer = colView?.Footer;
            float HeaderSize = 0f, FooterSize = 0f;
            float Width, Height, FingerSize = 30.0f;
            int count = colView.InternalItemSource.Count;

            if (Header)
            {
                Header.Layout?.Measure(new MeasureSpecification(new LayoutLength(isHorizontal ? FingerSize : colView.Size.Width), 
                                        ( isHorizontal ? MeasureSpecification.ModeType.Unspecified :
                                                        MeasureSpecification.ModeType.Exactly)),
                                        new MeasureSpecification(new LayoutLength(isHorizontal ? colView.Size.Height : FingerSize),
                                        ( isHorizontal ? MeasureSpecification.ModeType.Exactly :
                                                        MeasureSpecification.ModeType.Unspecified)));
                                                              
                Width = Header.Layout != null ? Header.Layout.MeasuredWidth.Size.AsRoundedValue() : 0;
                Height = Header.Layout != null ? Header.Layout.MeasuredHeight.Size.AsRoundedValue() : 0;              

                Width = Header.Size.Width == 0 ? Width : Header.Size.Width;
                Height = Header.Size.Height == 0 ? Height : Header.Size.Height;

                HeaderSize = isHorizontal ? Width : Height;
            }

            if (Footer)
            {
                Footer.Layout?.Measure(new MeasureSpecification(new LayoutLength(isHorizontal ? FingerSize : colView.Size.Width), 
                                        ( isHorizontal ? MeasureSpecification.ModeType.Unspecified :
                                                        MeasureSpecification.ModeType.Exactly)),
                                        new MeasureSpecification(new LayoutLength(isHorizontal ? colView.Size.Height : FingerSize),
                                        ( isHorizontal ? MeasureSpecification.ModeType.Exactly :
                                                        MeasureSpecification.ModeType.Unspecified)));
                                                              
                Width = Footer.Layout != null ? Footer.Layout.MeasuredWidth.Size.AsRoundedValue() : 0;
                Height = Footer.Layout != null ? Footer.Layout.MeasuredHeight.Size.AsRoundedValue() : 0;              

                Width = Footer.Size.Width == 0 ? Width : Footer.Size.Width;
                Height = Footer.Size.Height == 0 ? Height : Footer.Size.Height;

                FooterSize = isHorizontal ? Width : Height;
                Footer.Index = count - 1;
            }

            int FirstIndex = Header? 1 : 0;
            // Get Size Deligate. FIXME if group exist index must be changed.
            ViewItem SizeDeligate = colView.RealizeItem(FirstIndex);   
            if (SizeDeligate == null)
            {
                // error !
                throw new ArgumentException("Cannot create content from DatTemplate.", nameof(colView));
            }
            //FIXME: if header is exist, index must be changed.

            SizeDeligate.BindingContext = colView.InternalItemSource.GetItem(FirstIndex);

            // Need to Set proper hieght or width on scroll direciton.

            if (SizeDeligate.Layout == null)
            {
                Width = SizeDeligate.Size.Width;
                Height = SizeDeligate.Size.Height;
            }
            else
            {                 
                SizeDeligate.Layout.Measure(new MeasureSpecification(new LayoutLength(isHorizontal ? FingerSize : colView.Size.Width), 
                                             ( isHorizontal ? MeasureSpecification.ModeType.Unspecified :
                                                              MeasureSpecification.ModeType.Exactly)),
                                            new MeasureSpecification(new LayoutLength(isHorizontal ? colView.Size.Height : FingerSize),
                                             ( isHorizontal ? MeasureSpecification.ModeType.Exactly :
                                                              MeasureSpecification.ModeType.Unspecified)));

                Width = SizeDeligate.Layout.MeasuredWidth.Size.AsRoundedValue();
                Height = SizeDeligate.Layout.MeasuredHeight.Size.AsRoundedValue();

                Width = SizeDeligate.Size.Width == 0 ? Width : SizeDeligate.Size.Width;
                Height = SizeDeligate.Size.Height == 0 ? Height : SizeDeligate.Size.Height;
            }

            colView.UnrealizeItem(SizeDeligate);
            if (colView.Header != null) colView.UnrealizeItem(colView.Header);
            if (colView.Footer != null) colView.UnrealizeItem(colView.Footer);
            
            //Console.WriteLine("LSH : Layout Size {0} :{0}", Width, Height);

            // pick the StepCandidate.
            StepCandidate = isHorizontal ? Width : Height;
            if (StepCandidate == 0) StepCandidate = 1;

            float Current = 0f;
            for (int i = 0; i < count; i++)
            {
                if (colView.SizingStrategy == ItemSizingStrategy.MeasureAll)
                {
                    if (i == 0 && Header != null)
                        ItemSize.Add(HeaderSize);
                    else if (i == count -1 && Footer != null)
                        ItemSize.Add(FooterSize);
                    else ItemSize.Add(StepCandidate);
                }
                ItemPosition.Add(Current);
                if (i == 0 && Header != null)
                    Current = HeaderSize;
                else if (i == count -1 && Footer != null)
                    Current += FooterSize;
                else Current += StepCandidate;                
            }

            // colView.UnrealizeItem(SizeDeligate); keep the SizeDeligate for tracking SizeChange.
            ScrollContentSize = Current;
            if (isHorizontal) colView.ContentContainer.SizeWidth = ScrollContentSize;
            else colView.ContentContainer.SizeHeight = ScrollContentSize;

            base.Initialize(view);
            //Console.WriteLine("Init Done, StepCnadidate{0}, Scroll{1}", StepCandidate, ScrollContentSize);
        }

        /// <summary>
        /// This is called to find out where items are lain out according to current scroll position.
        /// </summary>
        /// <param name="scrollPosition">Scroll position which is calculated by ScrollableBase</param>
        /// <param name="force">boolean force flag to layouting forcely.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void RequestLayout(float scrollPosition, bool force = false)
        {
            // Layouting is only possible after once it intialized.
            if (!IsInitialized) return;
            int LastIndex = colView.InternalItemSource.Count;

            if (!force && PrevScrollPosition == Math.Abs(scrollPosition)) return;
            PrevScrollPosition = Math.Abs(scrollPosition);

            if (ItemSizeChanged >= 0)
            {
                for (int i = ItemSizeChanged; i < LastIndex; i++)
                    UpdatePosition(i);
                ScrollContentSize = ItemPosition[LastIndex - 1] + GetItemSize(LastIndex);
            }

            int prevFirstVisible = FirstVisible;
            int prevLastVisible = LastVisible;

            (float X, float Y) visibleArea = (PrevScrollPosition,
                PrevScrollPosition + ( isHorizontal ? colView.Size.Width : colView.Size.Height)
            );

            //Console.WriteLine("LSH :: itemsView [{0},{1}] [{2},{3}]", colView.Size.Width, colView.Size.Height, colView.ContentContainer.Size.Width, colView.ContentContainer.Size.Height);

            // 1. Set First/Last Visible Item Index. 
            FindVisibleItems(visibleArea);
            //Console.WriteLine("LSH :: visibleArea before [{0},{1}] after [{2},{3}]", prevFirstVisible, prevLastVisible, FirstVisible, LastVisible);

            // 2. Unrealize invisible items.
            List<ViewItem> unrealizedItems = new List<ViewItem>();
            foreach (ViewItem item in VisibleItems)
            {
                if (item.Index < FirstVisible || item.Index > LastVisible)
                {
                   //Console.WriteLine("LSH :: Unrealize{0}!", item.Index);

                   unrealizedItems.Add(item);
                   colView.UnrealizeItem(item);                  
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
                if (item == null) item = colView.RealizeItem(i);                

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
                //Console.WriteLine("LSH :: ["+item+"]["+item.Index+"] ["+item.Position.X+", "+item.Position.Y+" ==== \n");
            }
            //Console.WriteLine("Realize Done");
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void NotifyItemSizeChanged(ViewItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (!IsInitialized ||
                (colView.SizingStrategy == ItemSizingStrategy.MeasureFirst &&
                item.Index != 0) ||
                (item.Index < 0))
                return;

            float PrevSize, CurrentSize;
            if (item.Index == (colView.InternalItemSource.Count-1))
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
                if (colView.SizingStrategy == ItemSizingStrategy.MeasureAll)
                  ItemSize[item.Index] = CurrentSize;
                else
                  StepCandidate = CurrentSize;
            }
            if (ItemSizeChanged == -1) ItemSizeChanged = item.Index;
            else ItemSizeChanged = Math.Min(ItemSizeChanged, item.Index);

            //ScrollContentSize += Diff; UpdateOnce?
        }

        /// <summary>
        /// This is called to find out how much container size can be.
        /// </summary>
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
        [EditorBrowsable(EditorBrowsableState.Never)]
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
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override View RequestNextFocusableView(View currentFocusedView, View.FocusDirection direction, bool loopEnabled)
        {
            if (currentFocusedView == null)
                throw new ArgumentNullException(nameof(currentFocusedView));

            View nextFocusedView = null;
            int targetSibling = -1;

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
                if(candidate.Index >= 0 && candidate.Index < colView.InternalItemSource.Count)
                {
                    nextFocusedView = candidate;
                }
            }

            return nextFocusedView;
        }

        private float GetItemSize(int index)
        {
            if (colView.SizingStrategy == ItemSizingStrategy.MeasureAll)
            {
                return ItemSize[index];
            }
            else
            {
                return StepCandidate;
            }
            
        }
        private void UpdatePosition(int Index)
        {
            bool IsGroup = (colView.InternalItemSource is IGroupableItemSource);
            bool IsGroupHeader = false;
            bool IsGroupFooter = false;

            if (Index <= 0) return;
            if (Index >= colView.InternalItemSource.Count)

            if (IsGroup)
            {
                IsGroupHeader = (colView.InternalItemSource as IGroupableItemSource).IsGroupHeader(Index);
                IsGroupFooter = (colView.InternalItemSource as IGroupableItemSource).IsGroupFooter(Index);
                //Do Something
            }

            ItemPosition[Index] = ItemPosition[Index-1] + GetItemSize(Index);           
        }

        private void FindVisibleItems((float X, float Y) visibleArea)
        {
            int MaxIndex = colView.InternalItemSource.Count - 1;
            if (colView.SizingStrategy == ItemSizingStrategy.MeasureAll)
            {
                //If Position is exist, need to find proper visible item!
                FirstVisible = Convert.ToInt32(Math.Abs(visibleArea.X / StepCandidate)) - 3;
                LastVisible = Convert.ToInt32(Math.Abs(visibleArea.Y / StepCandidate)) + 3;
            }
            else
            {
                //Need to Consider GroupHeight!!!!
                FirstVisible = Convert.ToInt32(Math.Abs(visibleArea.X / StepCandidate)) - 3;
                LastVisible = Convert.ToInt32(Math.Abs(visibleArea.Y / StepCandidate)) + 3;
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

    }
}
