/*
 * Copyright(c) 2023 Samsung Electronics Co., Ltd.
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using Tizen.NUI.Binding;

namespace Tizen.Guide.Samples
{
    public class SpannableGridLayouter : ItemsLayouter
    {
        private SpannableGridView gridView;
        private (float Width, float Height) sizeCandidate;
        private int spanSize = 1;
        private float align = 0.5f;
        /* Skipped Header, Footer exist for now.
        private bool hasHeader;
        private Extents headerMargin;
        private float headerSize;
        private Extents footerMargin;
        private bool hasFooter;
        private float footerSize;
        */
        private Timer requestLayoutTimer = null;
        private bool isSourceEmpty;

        public SpannableGridLayouter(int spanSize = 1, int lineHeight = 0)
        {
            SpanSize = spanSize;
            StepCandidate = lineHeight;
        }

        /// <summary>
        /// Span Size
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected int SpanSize
        {
            get => spanSize;
            set => spanSize = value;
        }

        /// <summary>
        /// Size Candidate
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected (float Width, float Height) SizeCandidate
        {
            get => sizeCandidate;
            set => sizeCandidate = value;
        }

        protected override void OnInitialize()
        {
            foreach (RecyclerViewItem item in VisibleItems)
            {
                gridView.InternalUnrealizeItem(item, false);
            }
            VisibleItems?.Clear();
            GroupItems?.Clear();
            FirstVisible = 0;
            LastVisible = 0;
        }

        protected override void OnMeasure()
        {
            if (Container.SizeWidth != 0)
            {
                sizeCandidate = ((Container.SizeWidth / SpanSize), StepCandidate);
            }

            RecyclerViewItem header = gridView?.Header;
            RecyclerViewItem footer = gridView?.Footer;
            int count = Source.Count;
            int pureCount = count - (header? 1 : 0) - (footer? 1 : 0);

            if (pureCount == 0)
            {
                isSourceEmpty = true;
                base.Initialize(gridView);
                return;
            }
            isSourceEmpty = false;
            int firstIndex = header? 1 : 0;

            bool failed = false;
            //Final Check of FirstIndex
            if ((Source.Count - 1 < firstIndex) ||
                (Source.IsFooter(firstIndex) && (Source.Count - 1) == firstIndex))
            {
                //StepCandidate = 0F;
                failed = true;
            }

            while (!failed &&
                    Source.IsHeader(firstIndex))
            {
                if (Source.IsFooter(firstIndex)
                    || ((Source.Count - 1) <= firstIndex))
                {
                    StepCandidate = 0F;
                    failed = true;
                    break;
                }
                firstIndex++;
            }

            object firstItem = Source.GetItem(firstIndex);

            //Check item is spannable
            if (!(firstItem is IItemSpannable))
            {
                throw new Exception("Source item must be implement IItemSpannable");
            }

        }

        public override void Initialize(RecyclerView view)
        {
            gridView = view as SpannableGridView;
            if (gridView == null)
            {
                throw new Exception("ItemsView must be SpannableGridView");
            }
            if (SpanSize == 0 || StepCandidate == 0)
            {
                throw new Exception("Span Size and Line Height must have a value");
            }

            base.Initialize(view);


        }

        public override void RequestLayout(float scrollPosition, bool force = false)
        {
            // Layouting is only possible after once it intialized.
            if (!IsInitialized) return;
            int LastIndex = Source.Count;

            if (!force && PrevScrollPosition == Math.Abs(scrollPosition)) return;
            PrevScrollPosition = Math.Abs(scrollPosition);

            int prevFirstVisible = FirstVisible;
            int prevLastVisible = LastVisible;
            bool IsHorizontal = (gridView.ScrollingDirection == ScrollableBase.Direction.Horizontal);

            (float X, float Y) visibleArea = (PrevScrollPosition,
                PrevScrollPosition + (IsHorizontal? gridView.Size.Width : gridView.Size.Height)
            );

            // 1. Set First/Last Visible Item Index.
            (int start, int end) = FindVisibleItems(visibleArea);
            FirstVisible = start;
            LastVisible = end;

            // 2. Unrealize invisible items.
            List<RecyclerViewItem> unrealizedItems = new List<RecyclerViewItem>();
            foreach (RecyclerViewItem item in VisibleItems)
            {
                if (item.Index < FirstVisible || item.Index > LastVisible)
                {
                    unrealizedItems.Add(item);
                    gridView.InternalUnrealizeItem(item);
                }
            }
            VisibleItems.RemoveAll(unrealizedItems.Contains);

            // 3. Realize and placing visible items.
            for (int i = FirstVisible; i <= LastVisible; i++)
            {
                RecyclerViewItem item = null;
                int index = i;

                if (i >= prevFirstVisible && i <= prevLastVisible)
                {
                    item = GetVisibleItem(i);
                    if (item != null && !force) continue;
                }
                if (item == null)
                {
                    item = gridView.InternalRealizeItem(i);
                    if (item != null) VisibleItems.Add(item);
                    else throw new Exception("Failed to create RecycerViewItem index of ["+ i + "]");
                }

                //item Position without Padding and Margin.
                (float x, float y) = GetItemPosition(i);
                // 5. Placing item with Padding and Margin.
                item.Position = new Position(x, y);

                if (item.IsHeader || item.IsFooter || item.IsGroupHeader || item.IsGroupFooter)
                {
                    var size = (IsHorizontal? item.SizeWidth: item.SizeHeight);

                    if (IsHorizontal && item.HeightSpecification == LayoutParamPolicies.MatchParent)
                    {
                        item.Size = new Size(size, Container.Size.Height - Padding.Top - Padding.Bottom - item.Margin.Top - item.Margin.Bottom);
                    }
                    else if (!IsHorizontal && item.WidthSpecification == LayoutParamPolicies.MatchParent)
                    {
                        item.Size = new Size(Container.Size.Width - Padding.Start - Padding.End - item.Margin.Start - item.Margin.End, size);
                    }
                }
            }
        }

        public override void Clear()
        {
            base.Clear();
        }
    }