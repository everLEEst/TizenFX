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
                return ItemsSource;
            }
            set
            {
                ItemsSource = value;

                if (InternalItemSource != null) 
                    InternalItemSource.Dispose();
                InternalItemSource = ItemsSourceFactory.Create(value, this);

                if (NeedInitalizeLayouter)
                {
                    if (ItemsTemplate != null)
                    {
                        ItemsLayouter.Initialze(this);
                        ItemsLayouter.RequestLayout(0.0f);
                    }
                }
            }
        }

        /// <summary>
        /// Internal encapsulated items data source.
        /// </summary>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        internal IItemSource InternalItemSource;

        
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
                return ItemsTemplate;
            }
            set
            {
                ItemsTemplate = value;
                if (NeedInitalizeLayouter)
                {
                    if (ItemsSource != null)
                    {
                        ItemsLayouter.Initialze(this);
                        ItemsLayouter.RequestLayout(0.0f);
                    }
                }
            }
        }

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
                return ItemsLayouter;
            }
            set
            {
                ItemsLayouter = value;
                if (value == null) NeedInitalizeLayouter = false;
                if ((ItemsSource != null) && (ItemsTemplate != null))
                {
                    value.Initialze(this);
                    value.RequestLayout(0.0f);
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
           foreach (ViewItem item in RecycleCache)
           {
               if (item.Template == Template)
               {
                   RecycleCache.Remove(item);
                   return item;
               }
           }
           return null;
        }

        private void OnItemSizeChanged(object source, PropertyNotification.NotifyEventArgs args)
        {
            ItemsLayouter.RequestLayout(ScrollingDirection == Direction.Horizontal ? ContentContainer.CurrentPosition.X : ContentContainer.CurrentPosition.Y);
        }

        /// <summary>
        /// Realize indexed item.
        /// </summary>
        /// <param name="Index"> Index position of realizing item </param>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal ViewItem RealizeItem(int Index)
        {
            // Check DataTemplate is Same!
            if (ItemsTemplate is DataTemplateSelector)
            {

            }
            else
            {
               // pop item
               ViewItem item = PopRecycleCache(ItemsTemplate);
               if (item != null)
               {
                   item.Index = Index;
                   item.BindingContext = InternalItemSource.GetItem(Index);
                   ContentContainer.Children.Add(item);
                   PropertyNotification noti = item.AddPropertyNotification("size", PropertyCondition.Step(0.1f));
                   noti.Notified += OnItemSizeChanged;
                   notifications.Add(noti);
                   return item;
               }
            }

            object content = ItemsTemplate.CreateContent();
            if (content is ViewItem)
            {
                ViewItem item = content as ViewItem;
                item.Index = Index;
                item.Template = (ItemsTemplate is DataTemplateSelector ?
                                     (ItemsTemplate as DataTemplateSelector).SelectDataTemplate(InternalItemSource.GetItem(Index), this) :
                                     ItemsTemplate);
               item.BindingContext = InternalItemSource.GetItem(Index);
               ContentContainer.Children.Add(item);
               PropertyNotification noti = item.AddPropertyNotification("size", PropertyCondition.Step(0.1f));
               noti.Notified += OnItemSizeChanged;
               notifications.Add(noti);
                
                //viewItem.Selected =   
                //viewItem.Disbled =
                //viewItem.ClickedEventArgs +=
                return item;
            }
            else if (content is View)
            {
                ViewItem item = new ViewItem((content as View));
                item.Index = Index;
                item.Template = (ItemsTemplate is DataTemplateSelector ?
                                     (ItemsTemplate as DataTemplateSelector).SelectDataTemplate(InternalItemSource.GetItem(Index), this) :
                                     ItemsTemplate);
               item.BindingContext = InternalItemSource.GetItem(Index);
               ContentContainer.Children.Add(item);

               PropertyNotification noti = item.AddPropertyNotification("size", PropertyCondition.Step(0.1f));
               noti.Notified += OnItemSizeChanged;
               notifications.Add(noti);
                
                //viewItem.Selected = 
                //viewItem.Disbled =
                //viewItem.ClickedEventArgs +=
                return item;
                
            }
            else
            {
                //wrong input
                return null;
            }

        }

        /// <summary>
        /// Unrealize indexed item.
        /// </summary>
        /// <param name="Item"> Target item for unrealizing </param>
        /// <since_tizen> 8 </since_tizen>
        /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal void UnrealizeItem(ViewItem Item)
        {
                Item.Index = -1;
                Item.IsSelected = false;
                Item.IsEnabled = true;
                ContentContainer.Children.Remove(Item);

/* PLEASE LET ME KNOW HOW TO GET RID OF NOTI!
                 PropertyNotification noti = Item.AddPropertyNotification("size", PropertyCondition.Step(0.1f));
                   noti.Notified += OnItemSizeChanged;
                   */
                                //Item.ClickedEventArgs -=
                if (!PushRecycleCache(Item))
                {
                    Item.Dispose();
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
            if (ItemsLayouter != null) ItemsLayouter.Initialze(this);
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
