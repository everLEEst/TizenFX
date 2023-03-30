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
    // ISample inehrited class will be automatically added in the main examples list.
    internal class SpannableGridViewSample : ContentPage, ISample
    {
        private Window window;

        private string[] countryName =
        {
            "Vietnam", "Aland", "Andorra", "Argentina", "Armenia", "Australia", "Austria", "Azerbaijan", "Barbados", "Belgium",
            "Bosnia and Herzegovina", "Bulgaria", "Canada", "Chile", "China", "Côte d'Ivoire", "Costa Rica", "Cuba", "Czech", "Denmark",
            "Finland", "France", "Georgia", "Germany", "Greece", "Guinea", "Guinea-Bissau", "Italia", "Jamaica", "Japan",
            "Niger", "Nigeria", "Puerto Rico", "Romania", "Seychelles", "Sierra Leone", "Spain", "Sudan", "Suriname", "Sweden",
            "Swiss", "Tajikistan", "Thailand", "Togolese", "Türkiye", "Ukraine", "UAE", "United Kingdom", "USA", "Venezuela",
        };

        public void Activate()
        {
        }
        public void Deactivate()
        {
            window = null;
        }

        DefaultGridItem CreateItem()
        {
            var item = new DefaultGridItem()
            {
                WidthSpecification = 404,
                HeightSpecification = 272 + 20,
            };

            // Binding properties
            item.Image.SetBinding(ImageView.ResourceUrlProperty, "ResourceUrl");
            item.Label.SetBinding(TextLabel.TextProperty, "Name");

            return item;
        }

        ObservableCollection<SpannableCountry> CreateCountries()
        {
            //Collection for groups
            var country = new ObservableCollection<SpannableCountry>();

            for(int i = 0; i < 100; i++)
            {
                country.Add(new SpannableCountry(i, countryName[i%50]));
            }
            return country;
        }


        /// Modify this method for adding other examples.
        public SpannableGridViewSample() : base()
        {
            Log.Info(this.GetType().Name, $"{this.GetType().Name} is contructed\n");

            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            //Create Data Source
            var countries = CreateCountries();

            // Navigator bar title is added here.
            AppBar = new AppBar()
            {
                Title = "Spannable GridView Sample",
            };

            var layouter = new SpannableGridLayouter();

            // Example root content view.
            // you can decorate, add children on this view.
            var gridView = new SpannableGridView()
            {
                ItemsSource = countries,
                ItemsLayouter = layouter,
                ScrollingDirection = ScrollableBase.Direction.Vertical,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                ItemTemplate = new DataTemplate(CreateItem),
                ScrollingEventThreshold = 100,
            };

            /*
            gridView.SelectionChanged += (o, args) =>
            {
                Can track the selections on this event callback.
            }
            */

            Content = gridView;
        }
    }


    /******************************* Custom GridView ************************************/

    public class SpannableGridView : CollectionView
    {
        public SpannableGridView() : base()
        {
        }

        protected override void Dispose(DisposeTypes type)
        {

            base.Dispose();
        }

        protected override RecyclerViewItem RealizeItem(int index)
        {
            var item = base.RealizeItem(index);
            return item;
        }

        internal RecyclerViewItem InternalRealizeItem(int index)
        {
            return RealizeItem(index);
        }

        protected override void UnrealizeItem(RecyclerViewItem item, bool recycle = true)
        {
            base.UnrealizeItem(item, recycle);
        }

        internal void InternalUnrealizeItem(RecyclerViewItem item, bool recycle = true)
        {
            UnrealizeItem(item, recycle);
        }

    }

    public interface IItemSpannable
    {
        int RowSpan { get; set; }
        int ColumnSpan {get; set; }
    }
    /********************************* Data Source  ************************************/
    public class SpannableCountry : INotifyPropertyChanged, IItemSpannable
    {
        private string mName;
        private int mIndex;
        private int mRowSpan;
        private int mColumnSpan;
        private string resourcePath = Tizen.Applications.Application.Current.DirectoryInfo.Resource;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SpannableCountry(int index, string name, int rowSpan = 1, int columnSpan = 1)
        {
            mIndex = index;
            mName = name;
            mRowSpan = rowSpan;
            mColumnSpan = columnSpan;
        }

        public string Name
        {
            get => mName;
            set
            {
                if (mName != value)
                {
                    mName = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string ResourceUrl
        {
            get => resourcePath + "flags/flag_" + mIndex % 50 + ".png";
        }

        public int RowSpan
        {
            get => mRowSpan;
            set => mRowSpan = value;
        }

        public int ColumnSpan
        {
            get => mColumnSpan;
            set => mColumnSpan = value;
        }
    }
}