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
    /// Default layout manager for CollectionView.
    /// Lay out ViewItem and recycle ViewItem.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class ItemsLayouter : ICollectionChangedNotifier
    {

        /// <summary>
        /// Container which contains ViewItems.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected View Container{ get ; set; }

        /// <summary>
        /// Parent ItemsView.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected ItemsView ItemsView{ get; set; }

        /// <summary>
        /// The last scrolled position which is calculated by ScrollableBase. The value should be updated in the Recycle() method.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected float PrevScrollPosition { get; set; }

        /// <summary>
        /// First index of visible items.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected int FirstVisible { get; set; } = -1;

        /// <summary>
        /// Last index of visible items.
        /// </summary>
        protected int LastVisible { get; set; } = -1;

        /// <summary>
        /// Visible ViewItem.
        /// </summary>
        protected List<ViewItem> VisibleItems { get; set; } = new List<ViewItem>();

        /// <summary>
        /// Flag of layouter initialization.
        /// </summary>
        protected bool IsInitialized { get; set; } = false;

        /// <summary>
        /// Candidate item step size for scroll size measure.
        /// </summary>
        protected float StepCandidate { get; set; }

        /// <summary>
        /// Content size of scrollable.
        /// </summary>
        protected float ScrollContentSize { get; set; }
    
        /// <summary>
        /// boolean flag of scrollable horizontal direction.
        /// </summary>
        protected bool isHorizontal { get; set; }

        /// <summary>
        /// Clean up ItemsLayouter.
        /// </summary>
        /// <param name="view"> ItemsView of layouter. </param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void Initialize(ItemsView view)
        {
            ItemsView = view ?? throw new ArgumentNullException(nameof(view));
            Container = view.ContentContainer;            
            PrevScrollPosition = 0.0f;

            isHorizontal = (view.ScrollingDirection == ScrollableBase.Direction.Horizontal);

            IsInitialized = true;
        }


        /// <summary>
        /// This is called to find out where items are lain out according to current scroll position.
        /// </summary>
        /// <param name="scrollPosition">Scroll position which is calculated by ScrollableBase</param>
        /// <param name="force">boolean force flag to layouting forcely.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void RequestLayout(float scrollPosition, bool force = false)
        {
           // Layouting Items in scrollPosition.
        }

        /// <summary>
        /// This is called to find out how much container size can be.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual float CalculateLayoutOrientationSize()
        {
            return 0.0f;
        }

        /// <summary>
        /// Adjust scrolling position by own scrolling rules.
        /// </summary>
        /// <param name="scrollPosition">Scroll position which is calculated by ScrollableBase</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual float CalculateCandidateScrollPosition(float scrollPosition)
        {
            return scrollPosition;
        }

        /// <summary>
        /// </summary>
        public virtual void NotifyItemSizeChanged(ViewItem item)
        {
        }

        /// <summary>
        /// </summary>
        public void NotifyDataSetChanged()
        {
            Initialize(ItemsView);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        public void NotifyItemChanged(IItemSource source, int startIndex)
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        public void NotifyItemInserted(IItemSource source, int startIndex)
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        public void NotifyItemMoved(IItemSource source, int fromPosition, int toPosition)
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void NotifyItemRangeChanged(IItemSource source, int start, int end)
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        public void NotifyItemRangeInserted(IItemSource source, int startIndex, int count)
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        public void NotifyItemRangeRemoved(IItemSource source, int startIndex, int count)
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        public void NotifyItemRemoved(IItemSource source, int startIndex)
        {

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
        public virtual View RequestNextFocusableView(View currentFocusedView, View.FocusDirection direction, bool loopEnabled)
        {
            return null;
        }

    }
}
