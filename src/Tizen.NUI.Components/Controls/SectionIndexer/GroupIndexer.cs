/* Copyright (c) 2022 Samsung Electronics Co., Ltd.
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

/*
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Input;
using System.ComponentModel;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Binding;

namespace Tizen.NUI.Components
{
    public class GroupIndexer : SectionIndexer
    {
        private CollectionView colView;
        private
        internal IGroupableItemSource GroupableItemSource
        {
            get =>
            set
            {

            }
        }

        internal CollectionView CollectionView
        {
            get => colView;
            set
            {
                colView = value;
                List<object> groups;
                if (colView != null && colView.IsGrouped)
                {
                    if (colView.ItemsSource == null) return;
                    if (colView.ItemsSource is IEnumerable<object> source)
                    {
                      groups = new List<object>(source);
                    }
                    else
                    {
                        groups = new List<object>();
                        foreach (object item in colView.ItemsSource)
                        {
                            groups.Add(item);
                        }
                    }
            }
        }

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public GroupIndexer(): base()
        {
        }

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool OnSection(object item, TouchEventArgs e)
        {
            bool ret = base.OnSection(item, e);
            return ret;
        }

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void ClearSections()
        {
            base.ClearSections();
        }
    }
}
*/