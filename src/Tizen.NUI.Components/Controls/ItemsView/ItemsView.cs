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
    /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API.
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class ItemsView : ScrollableBase, ICollectionChangedNotifier
    {
        private List<PropertyNotification> notifications = new List<PropertyNotification>();
        /// <summary>
        /// Base Constructor
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        public ItemsView() : base()
        {
            Scrolling += OnScrolling;
        }

        /// <summary>
        /// Base Constructor with itemsSource
        /// </summary>
        /// <param name="itemsSource">item's data source</param>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
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
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        public ItemsView(IEnumerable itemsSource, ItemsLayouter layouter, DataTemplate template) : base()
        {
            ItemsSource = itemsSource;
            ItemsTemplate = template;
            ItemsLayouter = layouter;
        }

        /// <summary>
        /// Scrolling Direction.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        public new Direction ScrollingDirection
        {
            get
            {
                return base.ScrollingDirection;
            }
            set
            {
                base.ScrollingDirection = value;
            }
        }

        private IEnumerable _itemsSource = null;
        /// <summary>
        /// Item's source data.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerable ItemsSource
        {
            get
            {
                return _itemsSource;
            }
            set
            {
                _itemsSource = value;

                if (InternalItemSource != null) 
                    InternalItemSource.Dispose();
                InternalItemSource = ItemsSourceFactory.Create(_itemsSource, this);

                if (NeedInitalizeLayouter)
                {
                    if (_itemsTemplate != null)
                    {
                        _itemsLayouter.Initialize(this);
                        _itemsLayouter.RequestLayout(0.0f, true);
                        if (ScrollingDirection == Direction.Horizontal)
                        {
                            ContentContainer.SizeWidth =
                                _itemsLayouter.CalculateLayoutOrientationSize();
                        }
                        else
                        {
                            ContentContainer.SizeHeight =
                                _itemsLayouter.CalculateLayoutOrientationSize();
                        }
                        NeedInitalizeLayouter = false;
                    }
                }
            }
        }

        /// <summary>
        /// Internal encapsulated items data source.
        /// </summary>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        internal IItemSource InternalItemSource;

        
        private DataTemplate _itemsTemplate = null;
        /// <summary>
        /// DataTemplate for items.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DataTemplate ItemsTemplate
        {
            get
            {
                return _itemsTemplate;
            }
            set
            {
                _itemsTemplate = value;
                if (NeedInitalizeLayouter)
                {
                    if (_itemsLayouter != null)
                    {
                        _itemsLayouter.Initialize(this);
                        _itemsLayouter.RequestLayout(0.0f, true);
                        if (ScrollingDirection == Direction.Horizontal)
                        {
                            ContentContainer.SizeWidth =
                                _itemsLayouter.CalculateLayoutOrientationSize();
                        }
                        else
                        {
                            ContentContainer.SizeHeight =
                                _itemsLayouter.CalculateLayoutOrientationSize();
                        }
                        NeedInitalizeLayouter = false;
                    }
                }
            }
        }

        private ItemsLayouter _itemsLayouter = null;
        /// <summary>
        /// Items Layouter.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ItemsLayouter ItemsLayouter
        {
            get
            {
                return _itemsLayouter;
            }
            set
            {
                _itemsLayouter = value;
                if (value == null) NeedInitalizeLayouter = false;
                if ((ItemsSource != null) && (ItemsTemplate != null))
                {
                    _itemsLayouter.Initialize(this);
                    _itemsLayouter.RequestLayout(0.0f, true);
                    if (ScrollingDirection == Direction.Horizontal)
                        {
                            ContentContainer.SizeWidth =
                                _itemsLayouter.CalculateLayoutOrientationSize();
                        }
                        else
                        {
                            ContentContainer.SizeHeight =
                                _itemsLayouter.CalculateLayoutOrientationSize();
                        }
                }
                else NeedInitalizeLayouter = true;
            }
        }

        /// <summary>
        /// RecycleCache of ViewItem.
        /// </summary>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        protected List<ViewItem> RecycleCache = new List<ViewItem>();
            
        /// <summary>
        /// Max size of RecycleCache. Default is 50.
        /// </summary>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        protected int CacheMax = 50;

        /// <inheritdoc/>
         public override void OnRelayout(Vector2 size, RelayoutContainer container)
        {
            //Console.WriteLine("LSH :: On ReLayout [{0} {0}]", size.X, size.Y);
            base.OnRelayout(size, container);
            if (_itemsLayouter != null) 
            {
                _itemsLayouter.Initialize(this);
                _itemsLayouter.RequestLayout(ScrollingDirection == Direction.Horizontal ? ContentContainer.CurrentPosition.X : ContentContainer.CurrentPosition.Y, true);
            }
        }

        private void OnScrolling(object source, ScrollEventArgs args)
        {
            //Console.WriteLine("LSH :: On Scrolling! {0} => {1}", ScrollPosition.Y, args.Position.Y);
            ItemsLayouter.RequestLayout(ScrollingDirection == Direction.Horizontal ? args.Position.X : args.Position.Y);
        }        

        /// <summary>
        /// Push the item into the recycle cache. this item will be reused in view update.
        /// </summary>
        /// <param name="item"> Target item to push into recycle cache. </param>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual bool PushRecycleCache(ViewItem item)
        {
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
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
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

        private void OnItemRelayout(object sender, EventArgs e)
        {
            _itemsLayouter.NotifyItemSizeChanged((sender as ViewItem));
            //_itemsLayouter.RequestLayout(ScrollingDirection == Direction.Horizontal ? ContentContainer.CurrentPosition.X : ContentContainer.CurrentPosition.Y);
        }

        private void DecorateItem(ViewItem item, int Index)
        {
            item.Index = Index;
            item.ParentItemsView = this;
            item.Template = (ItemsTemplate is DataTemplateSelector ?
                             (ItemsTemplate as DataTemplateSelector).SelectDataTemplate(InternalItemSource.GetItem(Index), this) :
                              ItemsTemplate);
               item.BindingContext = InternalItemSource.GetItem(Index);
            item.BindingContext = InternalItemSource.GetItem(Index);
            //item.BackgroundColor = Color.Yellow;
            item.Relayout += OnItemRelayout;
            ContentContainer.Add(item);
/*
            PropertyNotification noti = item.AddPropertyNotification("size", PropertyCondition.Step(0.1f));
            noti.Notified += OnItemSizeChanged;
            notifications.Add(noti);
*/
        }

        /// <summary>
        /// Realize indexed item.
        /// </summary>
        /// <param name="Index"> Index position of realizing item </param>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal virtual ViewItem RealizeItem(int Index)
        {
            // Check DataTemplate is Same!
            if (ItemsTemplate is DataTemplateSelector)
            {
               Console.WriteLine("LSH :: DataTemplate is Selector??");
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

            object content = ItemsTemplate.CreateContent();
            if (content == null)
            {
                Console.WriteLine("LSH :: Template CreateContent Failed!!!");
            }
            if (content as ViewItem)
            {
                ViewItem item = (ViewItem)content;
                DecorateItem(item, Index);
                return item;
            }
            else if (content as View)
            {
                ViewItem item = new ViewItem((content as View));
                DecorateItem(item, Index);
                return item;
                
            }
            else
            {
                //wrong input
                Console.WriteLine("LSH :: content is wrong class");
                return null;
            }

        }

        /// <summary>
        /// Unrealize indexed item.
        /// </summary>
        /// <param name="item"> Target item for unrealizing </param>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal virtual void UnrealizeItem(ViewItem item)
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

/* PLEASE LET ME KNOW HOW TO GET RID OF NOTI!
                 PropertyNotification noti = Item.AddPropertyNotification("size", PropertyCondition.Step(0.1f));
                   noti.Notified += OnItemSizeChanged;
                   */
                                //Item.ClickedEventArgs -=
                if (!PushRecycleCache(item))
                {
                    Console.WriteLine("Cache is Full!");
                    item.Dispose();
                }
        }

        private bool NeedInitalizeLayouter = false;

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

        /// <summary>
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
		public void NotifyDataSetChanged()
        {
            //Need to update view.
            if (ItemsLayouter != null)
            {
                ItemsLayouter.Initialize(this);
                if (ScrollingDirection == Direction.Horizontal)
                {
                    ContentContainer.SizeWidth =
                        _itemsLayouter.CalculateLayoutOrientationSize();
                }
                else
                {
                    ContentContainer.SizeHeight =
                        _itemsLayouter.CalculateLayoutOrientationSize();
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
		public void NotifyItemChanged(IItemSource source, int startIndex)
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
		public void NotifyItemInserted(IItemSource source, int startIndex)
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
		public void NotifyItemMoved(IItemSource source, int fromPosition, int toPosition)
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
		public void NotifyItemRangeChanged(IItemSource source, int start, int end)
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
		public void NotifyItemRangeInserted(IItemSource source, int startIndex, int count)
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
		public void NotifyItemRangeRemoved(IItemSource source, int startIndex, int count)
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
		public void NotifyItemRemoved(IItemSource source, int startIndex)
        {

        }
    }
}
