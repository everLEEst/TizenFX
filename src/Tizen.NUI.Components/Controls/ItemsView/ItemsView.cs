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
        public ItemsView(IEnumerable itemsSource) : base()
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
        public ItemsView(IEnumerable itemsSource, ItemsLayouter layouter, DataTemplate template) : base()
        {
            ItemsSource = itemsSource;
            ItemsTemplate = template;
            ItemsLayouter = layouter;
        }

        /// <summary>
        /// Item's source data.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerable ItemsSource { get; set; }
        
        /// <summary>
        /// DataTemplate for items.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DataTemplate ItemsTemplate { get; set; }

        /// <summary>
        /// Items Layouter.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ItemsLayouter ItemsLayouter { get; set; }

        /// <summary>
        /// Internal encapsulated items data source.
        /// </summary>
       [EditorBrowsable(EditorBrowsableState.Never)]
        internal IItemSource InternalItemSource { get; set;}

        /// <summary>
        /// RecycleCache of ViewItem.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected List<ViewItem> RecycleCache {get; set; } = new List<ViewItem>();
            
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
            if (ItemsLayouter != null && ItemsSource != null && ItemsTemplate != null) 
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
                ItemsLayouter.Initialize(this);
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

        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyItemInserted(IItemSource source, int startIndex)
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyItemMoved(IItemSource source, int fromPosition, int toPosition)
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyItemRangeChanged(IItemSource source, int start, int end)
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyItemRangeInserted(IItemSource source, int startIndex, int count)
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyItemRangeRemoved(IItemSource source, int startIndex, int count)
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyItemRemoved(IItemSource source, int startIndex)
        {

        }

        /// <summary>
        /// Realize indexed item.
        /// </summary>
        /// <param name="Index"> Index position of realizing item </param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal virtual ViewItem RealizeItem(int Index)
        {
            // Check DataTemplate is Same!
            if (ItemsTemplate is DataTemplateSelector)
            {
                // Need to implements
            }
            else
            {
               // pop item
               ViewItem item = PopRecycleCache(ItemsTemplate);
               if (item != null)
               {
                    DecorateItem(item, Index);
                    return item;
               }
            }

            object content = ItemsTemplate.CreateContent() ?? throw new ArgumentNullException(nameof(content));
            if (content is ViewItem)
            {
                ViewItem item = (ViewItem)content;
                DecorateItem(item, Index);
                return item;
            }
            else
            {
                throw new ArgumentException("Template content must be type of ViewItem", nameof(content));
            }

        }

        /// <summary>
        /// Unrealize indexed item.
        /// </summary>
        /// <param name="item"> Target item for unrealizing </param>
        /// <param name="recycle"> Allow recycle. default is true </param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal virtual void UnrealizeItem(ViewItem item, bool recycle = true)
        {
            item.Index = -1;
            item.ParentItemsView = null;
            item.BindingContext = null;
            item.isPressed = false;
            item.IsSelected = false;
            item.IsEnabled = true;
            //Update Style UI
            item.UpdateState();
            item.Relayout -= OnItemRelayout;
            ContentContainer.Remove(item);

            if (!recycle || !PushRecycleCache(item))
            {
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

        private void OnScrolling(object source, ScrollEventArgs args)
        {
            if (ItemsLayouter != null && ItemsSource != null && ItemsTemplate != null)
            {
            //Console.WriteLine("LSH :: On Scrolling! {0} => {1}", ScrollPosition.Y, args.Position.Y);
                ItemsLayouter.RequestLayout(ScrollingDirection == Direction.Horizontal ? args.Position.X : args.Position.Y);
            }
        }        

        private void OnItemRelayout(object sender, EventArgs e)
        {
            ItemsLayouter.NotifyItemSizeChanged((sender as ViewItem));
            //ItemsLayouter.RequestLayout(ScrollingDirection == Direction.Horizontal ? ContentContainer.CurrentPosition.X : ContentContainer.CurrentPosition.Y);
        }

        private void DecorateItem(ViewItem item, int Index)
        {
            item.Index = Index;
            item.ParentItemsView = this;
            item.Template = (ItemsTemplate as DataTemplateSelector)?.SelectDataTemplate(InternalItemSource.GetItem(Index), this) ?? ItemsTemplate;
            item.BindingContext = InternalItemSource.GetItem(Index);
            item.Relayout += OnItemRelayout;
            ContentContainer.Add(item);
        }
    }
}
