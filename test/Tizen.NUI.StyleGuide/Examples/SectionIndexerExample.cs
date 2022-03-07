/*
 * Copyright(c) 2022 Samsung Electronics Co., Ltd.
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
using System.Linq;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using Tizen.NUI.Binding;

namespace Tizen.NUI.StyleGuide
{
    // IExample inehrited class will be automatically added in the main examples list.
    internal class SectionIndexerExample : ContentPage, IExample
    {
        private Window window;
        public void Activate()
        {
           Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()\n");
        }
        public void Deactivate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()\n");
            window = null;
        }

        public class Section
        {
            private string title;
            public string Title
            {
                get => title;
            }

            public Section(string sectionTitle)
            {
                title = sectionTitle;
            }
        }

        public class mySectionIndexer :SectionIndexer
        {
            private  Dictionary<string, object> sectionDictionary;
            public Dictionary<string, object> SectionDictionary
            {
                get => sectionDictionary;
                set
                {
                    sectionDictionary = value;
                    AddSection();
                }
            }

            protected override bool OnSection(object item, TouchEventArgs e)
            {
                var section = item as SectionItem;
                if (section == null) return false;

                var context = section.Text;
                var state = e.Touch.GetState(0);
                Tizen.Log.Debug("[mySectionIndexer] ##", $"[{section}({section.Text})] is on touched [{state}], [{context}], [{CurrentSection}]" + "\n");

                object colitem = null;
                if (sectionDictionary.TryGetValue(context, out colitem))
                {
                      Tizen.Log.Debug("[mySectionIndexer] ##", $"[{context}] find[{colitem.GetHashCode()}]" + "\n");
                      int index = Array.IndexOf(Sections, context);
                      if (GetParent() is CollectionView collectionView)
                      collectionView.ScrollTo(colitem, false, CollectionView.ItemScrollTo.Start);
                }

                return base.OnSection(item, e);
            }

            private void AddSection()
            {
                List<string> sections = new List<string>();
                foreach (string section in sectionDictionary.Keys)
                {
                    sections.Add(section);
                    Tizen.Log.Debug("[mySectionIndexer] ##", $"[{section} is added in {sections}" + "\n");
                };

                string[] secitonArr = sections.ToArray();

                foreach(string str in secitonArr)
                {

                    Tizen.Log.Debug("[mySectionIndexer] ##", $"[{str} is exist in array {secitonArr}" + "\n");

                }

                Sections = secitonArr;

            }

        }

        public class myColView: CollectionView
        {
            private  Dictionary<string, object>  sectionDictionary;
            public Dictionary<string, object>  SectionDictionary
            {
                get => sectionDictionary;
                set
                {
                    sectionDictionary = value;
                    Init();
                }
            }

            private SectionIndexer sectionIndexer;
            protected override void Init()
            {
                base.Init();
                if (ItemsSource == null) return;
                if (ItemsLayouter == null) return;
                if (ItemTemplate == null) return;

                if (sectionDictionary == null) return;

                Tizen.Log.Debug("[myColView] ##", $"Init begin" + "\n");
                if (sectionIndexer != null)
                {
                    Remove(sectionIndexer);
                }


                sectionIndexer = new mySectionIndexer()
                {
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.MatchParent,
                    SectionDictionary = sectionDictionary,
                    BarWidth = 40,
                };
                Add(sectionIndexer);

                sectionIndexer.RaiseToTop();
            }
        }

        /// Modify this method for adding other examples.
        public SectionIndexerExample() : base()
        {
            Log.Info(this.GetType().Name, $"{this.GetType().Name} is contructed\n");

            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;
            // Navigator bar title is added here.
            AppBar = new AppBar()
            {
                Title = "SectionIndexer Style",
            };
/*
            // Example root content view.
            // you can decorate, add children on this view.
            var rootContent = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,

                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    CellPadding = new Size2D(0, 0),
                },
            };
*/
            var dict = new Dictionary<string, object>();

            var groupSource = new GroupTestSourceModel(23, 5);

            foreach (GroupItem item in groupSource.TestSource)
            {
                dict.Add(item.GroupName, item);
            }

            var colView = new myColView()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                ItemsSource = groupSource.TestSource,
                SectionDictionary = dict,
                ItemsLayouter = new LinearLayouter(),
                ItemTemplate = new DataTemplate(() =>
                {
                    var item = new DefaultLinearItem()
                    {
                        WidthSpecification = LayoutParamPolicies.MatchParent,
                    };
                    item.Label.SetBinding(TextLabel.TextProperty, "Name");

                    return item;
                }),
                GroupHeaderTemplate = new DataTemplate(() =>
                {
                    var header = new DefaultTitleItem()
                    {
                        WidthSpecification = LayoutParamPolicies.MatchParent,
                        BackgroundColor = Color.LightGray,
                    };
                    header.Label.SetBinding(TextLabel.TextProperty, "GroupName");

                    return header;
                }),
                ScrollingDirection = ScrollableBase.Direction.Vertical,
                IsGrouped = true,
            };

            Content = colView;

            // SectionIndexer style examples.


            /*
            List<object> sections = new List<object>();

            string[] titles = new string[] {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R"};
            foreach(string title in titles)
            {
                var section = new Section(title);
                sections.Add(section);
            }

            List<object> list = titles.Select(f => (object)f).ToList();



            var sectionIndexer = new SectionIndexer()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                BarWidth = 40,
                SectionsSource = list,
                //SectionKey = "Title",
            };
            rootContent.Add(sectionIndexer);

            Content = rootContent;
            */
        }
    }
}
