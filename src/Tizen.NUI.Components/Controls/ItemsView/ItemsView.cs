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
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Binding;

namespace Tizen.NUI.Components
{
    /// <summary>
    /// [Draft] This class provides a View that can layouting items in list and grid with high performance.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class ItemsView : ScrollableBase, ICollectionChangedNotifier
    {
        /// <summary>
        /// Base Constructor
        /// </summary>
       [EditorBrowsable(EditorBrowsableState.Never)]
        public ItemsView() : base()
        {
            Scrolling += OnScrolling;
        }

        /// <summary>
        /// Base Constructor with itemsSource
        /// </summary>
        /// <param name="itemsSource">item's data source</param>
       [EditorBrowsable(EditorBrowsableState.Never)]
        public ItemsView(IEnumerable itemsSource) : this()
        {
            ItemsSource = itemsSource;
        }

        /// <summary>
        /// Base Constructor with itemsSource
        /// </summary>
        /// <param name="itemsSource">item's data source</param>
        /// <param name="layouter">item's layout manager</param>
        /// <param name="template">item's view template with data bindings</param>
       [EditorBrowsable(EditorBrowsableState.Never)]
        public ItemsView(IEnumerable itemsSource, ItemsLayouter layouter, DataTemplate template) : this()
        {
            ItemsSource = itemsSource;
            ItemTemplate = template;
            ItemsLayouter = layouter;

            Scrolling += OnScrolling;
        }

        /// <summary>
        /// Align item in the viewport when ScrollTo() calls.
        /// </summary>
       [EditorBrowsable(EditorBrowsableState.Never)]
        public enum ItemScrollTo
        {
            /// <summary>
            /// Scroll to show item in nearest viewport on scroll direction.
            /// item is above the scroll viewport, item will be came into front,
            /// item is under the scroll viewport, item will be came into end,
            /// item is in the scroll viewport, no scroll.
            /// </summary>
            Nearest,
            /// <summary>
            /// Scroll to show item in start of the viewport.
            /// </summary>
            Start,
            /// <summary>
            /// Scroll to show item in center of the viewport.
            /// </summary>
            Center,
            /// <summary>
            /// Scroll to show item in end of the viewport.
            /// </summary>
            End,
        }

        /// <summary>
        /// Item's source data.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual IEnumerable ItemsSource { get; set; }
        
        /// <summary>
        /// DataTemplate for items.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual DataTemplate ItemTemplate { get; set; }

        /// <summary>
        /// Items Layouter.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual ItemsLayouter ItemsLayouter { get; set; }

        /// <summary>
        /// Internal encapsulated items data source.
        /// </summary>
        internal IItemSource InternalItemSource { get; set;}

        /// <summary>
        /// RecycleCache of ViewItem.
        /// </summary>
        protected List<ViewItem> RecycleCache { get; } = new List<ViewItem>();
            
        /// <summary>
        /// Max size of RecycleCache. Default is 50.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected int CacheMax { get; set; } = 50;

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void OnRelayout(Vector2 size, RelayoutContainer container)
        {
            //Console.WriteLine("LSH :: On ReLayout [{0} {0}]", size.X, size.Y);
            base.OnRelayout(size, container);
            if (ItemsLayouter != null && ItemsSource != null && ItemTemplate != null) 
            {
                ItemsLayouter.Initialize(this);
                ItemsLayouter.RequestLayout(ScrollingDirection == Direction.Horizontal ? ContentContainer.CurrentPosition.X : ContentContainer.CurrentPosition.Y, true);
            }
        }

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyDataSetChanged()
        {
            //Need to update view.
            if (ItemsLayouter != null)
            {
                ItemsLayouter.NotifyDataSetChanged();
                if (ScrollingDirection == Direction.Horizontal)
                {
                    ContentContainer.SizeWidth =
                        ItemsLayouter.CalculateLayoutOrientationSize();
                }
                else
                {
                    ContentContainer.SizeHeight =
                        ItemsLayouter.CalculateLayoutOrientationSize();
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyItemChanged(IItemSource source, int startIndex)
        {
            if (ItemsLayouter != null)
            {
                ItemsLayouter.NotifyItemChanged(source, startIndex);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyItemInserted(IItemSource source, int startIndex)
        {
            if (ItemsLayouter != null)
            {
                ItemsLayouter.NotifyItemInserted(source, startIndex);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyItemMoved(IItemSource source, int fromPosition, int toPosition)
        {
            if (ItemsLayouter != null)
            {
                ItemsLayouter.NotifyItemMoved(source, fromPosition, toPosition);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyItemRangeChanged(IItemSource source, int start, int end)
        {
            if (ItemsLayouter != null)
            {
                ItemsLayouter.NotifyItemRangeChanged(source, start, end);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyItemRangeInserted(IItemSource source, int startIndex, int count)
        {
            if (ItemsLayouter != null)
            {
                ItemsLayouter.NotifyItemRangeInserted(source, startIndex, count);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyItemRangeRemoved(IItemSource source, int startIndex, int count)
        {
            if (ItemsLayouter != null)
            {
                ItemsLayouter.NotifyItemRangeRemoved(source, startIndex, count);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyItemRemoved(IItemSource source, int startIndex)
        {
            if (ItemsLayouter != null)
            {
                ItemsLayouter.NotifyItemRemoved(source, startIndex);
            }
        }

        /// <summary>
        /// Scroll to position.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="animate"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new void ScrollTo(float position, bool animate) => base.ScrollTo(position, animate);
        

        /// <summary>
        /// Scroll to item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="animate"></param>
        /// <param name="align"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void ScrollTo(object item, bool animate = false, ItemScrollTo align = ItemScrollTo.Nearest)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (ItemsLayouter == null) throw new Exception("Item Layouter must exist.");

            if (InternalItemSource.GetPosition(item) == -1)
            {
                throw new Exception("ScrollTo parameter item is not a member of ItemsSource");
            }

            float scrollPos, curPos, curSize, curItemSize;
            (float X, float Y) itemPos = ItemsLayouter.GetItemPosition(item);
            (float X, float Y) itemSize = ItemsLayouter.GetItemSize(item);
            if (ScrollingDirection == Direction.Horizontal)
            {
                scrollPos = itemPos.X;
                curPos = ScrollPosition.X;
                curSize = Size.Width;
                curItemSize = itemSize.X;
            }
            else
            {
                scrollPos = itemPos.Y;
                curPos = ScrollPosition.Y;
                curSize = Size.Height;
                curItemSize = itemSize.Y;
            }

            //Console.WriteLine("LSH :: ScrollTo [{0}:{1}], curPos{2}, itemPos{3}, curSize{4}, itemSize{5}", InternalItemSource.GetPosition(item), align, curPos, scrollPos, curSize, curItemSize);
            switch (align)
            {
                case ItemScrollTo.Start:
                    //nothing necessary.
                break;
                case ItemScrollTo.Center:
                    scrollPos = scrollPos - (curSize / 2) + (curItemSize / 2);
                break;
                case ItemScrollTo.End:
                    scrollPos = scrollPos - curSize + curItemSize;
                break;
                case ItemScrollTo.Nearest:
                    if (scrollPos < curPos - curItemSize)
                    {
                        // item is placed before the current screen. scrollTo.Top
                    }
                    else if (scrollPos >= curPos + curSize + curItemSize)
                    {
                        // item is placed after the current screen. scrollTo.End
                        scrollPos = scrollPos - curSize + curItemSize;
                    }
                    else
                    {
                        // item is in the scroller. ScrollTo() is ignored.
                        return;
                    }
                break;
            }

            //Console.WriteLine("LSH :: ScrollTo [{0}]-------------------", scrollPos);
            base.ScrollTo(scrollPos, animate);
        }        

        /// <summary>
        /// Realize indexed item.
        /// </summary>
        /// <param name="index"> Index position of realizing item </param>
        internal virtual ViewItem RealizeItem(int index)
        {
            // Check DataTemplate is Same!
            if (ItemTemplate is DataTemplateSelector)
            {
                // Need to implements
            }
            else
            {
               // pop item
               ViewItem item = PopRecycleCache(ItemTemplate);
               if (item != null)
               {
                    DecorateItem(item, index);
                    return item;
               }
            }

            object content = ItemTemplate.CreateContent() ?? throw new Exception("Template return null object.");
            if (content is ViewItem)
            {
                ViewItem item = (ViewItem)content;
                ContentContainer.Add(item);
                DecorateItem(item, index);
                return item;
            }
            else
            {
                throw new Exception("Template content must be type of ViewItem");
            }

        }

        /// <summary>
        /// Unrealize indexed item.
        /// </summary>
        /// <param name="item"> Target item for unrealizing </param>
        /// <param name="recycle"> Allow recycle. default is true </param>
        internal virtual void UnrealizeItem(ViewItem item, bool recycle = true)
        {
            item.Index = -1;
            item.ParentItemsView = null;
            // Remove BindingContext null set for performance improving.
            //item.BindingContext = null; 
            item.isPressed = false;
            item.IsSelected = false;
            item.IsEnabled = true;
            // Remove Update Style on default for performance improving.
            //item.UpdateState();
            item.Relayout -= OnItemRelayout;

            if (!recycle || !PushRecycleCache(item))
            {
                ContentContainer.Remove(item);
                item.Dispose();
            }
        }


        /// <summary>
        /// Adjust scrolling position by own scrolling rules.
        /// Override this function when developer wants to change destination of flicking.(e.g. always snap to center of item)
        /// </summary>
        /// <param name="position">Scroll position which is calculated by ScrollableBase</param>
        /// <returns>Adjusted scroll destination</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override float AdjustTargetPositionOfScrollAnimation(float position)
        {
            // Destination is depending on implementation of layout manager.
            // Get destination from layout manager.
            return ItemsLayouter.CalculateCandidateScrollPosition(position);
        }

        /// <summary>
        /// Push the item into the recycle cache. this item will be reused in view update.
        /// </summary>
        /// <param name="item"> Target item to push into recycle cache. </param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual bool PushRecycleCache(ViewItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (RecycleCache.Count >= CacheMax) return false;
            if (item.Template == null) return false;
            item.Hide();
            item.Index = -1;
            RecycleCache.Add(item);
            return true;
        }

        /// <summary>
        /// Pop the item from the recycle cache.
        /// </summary>
        /// <param name="Template"> Template of wanted item. </param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual ViewItem PopRecycleCache(DataTemplate Template)
        {
           for (int i = 0; i < RecycleCache.Count; i++)
           {
               ViewItem item = RecycleCache[i];
               if (item.Template == Template)
               {
                   RecycleCache.Remove(item);
                   item.Show();
                   return item;
               }
           }
           return null;
        }

        /// <summary>
        /// On scroll event callback.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual void OnScrolling(object source, ScrollEventArgs args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            if (!disposed && ItemsLayouter != null && ItemsSource != null && ItemTemplate != null)
            {
                //Console.WriteLine("LSH :: On Scrolling! {0} => {1}", ScrollPosition.Y, args.Position.Y);
                ItemsLayouter.RequestLayout(ScrollingDirection == Direction.Horizontal ? args.Position.X : args.Position.Y);
            }
        }

        /// <summary>
        /// Dispose ItemsView and all children on it.
        /// </summary>
        /// <param name="type">Dispose type.</param>
        protected override void Dispose(DisposeTypes type)
        {
            if (disposed)
            {
                return;
            }

            if (type == DisposeTypes.Explicit)
            {
                disposed = true;
                // call the clear!
                if (RecycleCache != null)
                {
                    foreach (ViewItem item in RecycleCache)
                    {
                        ContentContainer.Remove(item);
                        item.Dispose();
                    }
                    RecycleCache.Clear();
                }
                ItemsLayouter.Clear();
                ItemsLayouter = null;
                ItemsSource = null;
                ItemTemplate = null;
                if (InternalItemSource != null) InternalItemSource.Dispose();
                //
            }

            base.Dispose(type);
        }   

        private void OnItemRelayout(object sender, EventArgs e)
        {
            //ItemsLayouter.NotifyItemSizeChanged((sender as ViewItem));
            //ItemsLayouter.RequestLayout(ScrollingDirection == Direction.Horizontal ? ContentContainer.CurrentPosition.X : ContentContainer.CurrentPosition.Y);
        }

        private void DecorateItem(ViewItem item, int index)
        {
            item.Index = index;
            item.ParentItemsView = this;
            item.Template = (ItemTemplate as DataTemplateSelector)?.SelectDataTemplate(InternalItemSource.GetItem(index), this) ?? ItemTemplate;
            item.BindingContext = InternalItemSource.GetItem(index);
            item.Relayout += OnItemRelayout;
        }
    }
}
