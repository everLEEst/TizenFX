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
    internal class ReorderableGridViewSample : ContentPage, ISample
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

        ObservableCollection<Country> CreateCountries()
        {
            //Collection for groups
            var country = new ObservableCollection<Country>();

            for(int i = 0; i < 100; i++)
            {
                country.Add(new Country(i, countryName[i%50]));
            }
            return country;
        }


        /// Modify this method for adding other examples.
        public ReorderableGridViewSample() : base()
        {
            Log.Info(this.GetType().Name, $"{this.GetType().Name} is contructed\n");

            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            //Create Data Source
            var countries = CreateCountries();

            // Navigator bar title is added here.
            AppBar = new AppBar()
            {
                Title = "Reorderable GridView Sample",
            };

            var layouter = new ReorderableGridLayouter();

            // Example root content view.
            // you can decorate, add children on this view.
            var gridView = new ReorderableGridView()
            {
                ItemsSource = countries,
                ItemsLayouter = layouter,
                ScrollingDirection = ScrollableBase.Direction.Vertical,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                ItemTemplate = new DataTemplate(CreateItem),
                ScrollingEventThreshold = 100,
            };

            layouter.ItemReordered += (o, e) =>
            {
                Console.WriteLine($"Item Reordered!!! move data from[{e.From}]-to[{e.To}]");
                countries.Move(e.From, e.To);
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


    /******************************* Custom Layouter & GridView ************************************/

    public class ReorderableGridView : CollectionView
    {
        LongPressGestureDetector mLongPressGestureDetector;
        public ReorderableGridView() : base()
        {
            mLongPressGestureDetector = new LongPressGestureDetector();
            mLongPressGestureDetector.Detected += OnLongPressGestureDetected;
        }

        protected override void Dispose(DisposeTypes type)
        {
            //mLongPressGestureDetector?.DetachAll();
            //mLongPressGestureDetector?.Dispose();
            mLongPressGestureDetector = null;

            base.Dispose();
        }

        protected override RecyclerViewItem RealizeItem(int index)
        {
            var item = base.RealizeItem(index);
            mLongPressGestureDetector.Attach(item);

            Console.WriteLine($"Gesture Attached on {item}:{item.GetHashCode()}");

            return item;
        }

        internal RecyclerViewItem InternalRealizeItem(int index)
        {
            return RealizeItem(index);
        }

        protected override void UnrealizeItem(RecyclerViewItem item, bool recycle = true)
        {
            mLongPressGestureDetector.Detach(item);
            Console.WriteLine($"Gesture Detached on {item}:{item.GetHashCode()}");
            base.UnrealizeItem(item, recycle);
        }

        internal void InternalUnrealizeItem(RecyclerViewItem item, bool recycle = true)
        {
            UnrealizeItem(item, recycle);
        }

        private void OnLongPressGestureDetected(object o, LongPressGestureDetector.DetectedEventArgs e)
        {
            RecyclerViewItem item = e.View as RecyclerViewItem;
            ReorderableGridLayouter layouter = ItemsLayouter as ReorderableGridLayouter;

            Console.WriteLine($"OnLongPressGestureDetected called on {item}:{item.GetHashCode()}");
            if (item == null || layouter == null) return;

            layouter.ItemOnLongPressed(item, e);
        }

    }

    public class ReorderableGridLayouter : GridLayouter
    {
        PanGestureDetector mPanGestureDetector;
        private RecyclerViewItem onReordering;
        private ReorderableGridView gridView;
        private Animation itemShiftAnimation;
        private int itemShiftDuration = 1000;
        private int targetIndex = -1;
        private View targetPreview;

        public class ItemReorderEventArgs : EventArgs
        {
            public int From { get; internal set; }
            public int To  {get; internal set; }
        }

        public EventHandler<ItemReorderEventArgs> ItemReordered;

        public override void Initialize(RecyclerView view)
        {
            base.Initialize(view);
            gridView = ItemsView as ReorderableGridView;
            mPanGestureDetector = new PanGestureDetector();
            mPanGestureDetector.Detected += OnPanGestureDetected;

            itemShiftAnimation = new Animation(itemShiftDuration);
            itemShiftAnimation.EndAction = Animation.EndActions.StopFinal;

            targetPreview = new View()
            {
                CornerRadius = 12f,
                BorderlineWidth = 8f,
                BorderlineOffset = -1,
                BorderlineColor = new Color("#ffcd4e"),
                BackgroundColor = new Color("#fbe392"),
                Opacity = 0f,
            };

            Container?.Add(targetPreview);

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
                    if (item != onReordering)
                    {
                        unrealizedItems.Add(item);
                        gridView.InternalUnrealizeItem(item);
                    }
                }
            }
            VisibleItems.RemoveAll(unrealizedItems.Contains);

            // 3. Realize and placing visible items.
            for (int i = FirstVisible; i <= LastVisible; i++)
            {
                RecyclerViewItem item = null;
                int index = i;

                // Index shift after the reodering item.
                if ((onReordering != null) && (onReordering.Index <= index))
                {
                    index--;
                }

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

                //if (item.IsHeader || item.IsFooter || item.isGroupHeader || item.isGroupFooter)
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
            if (onReordering != null)
            {
                gridView.InternalUnrealizeItem(onReordering, false);
                onReordering = null;
            }

            mPanGestureDetector?.DetachAll();
            mPanGestureDetector?.Dispose();
            mPanGestureDetector = null;

            itemShiftAnimation?.Dispose();
            itemShiftAnimation = null;

            targetIndex = -1;
            targetPreview?.Dispose();
            targetPreview = null;

            base.Clear();
        }


        internal void ItemOnLongPressed(RecyclerViewItem item, LongPressGestureDetector.DetectedEventArgs args)
        {
            DefaultGridItem gridItem = item as DefaultGridItem;
            View detectedView = args.View;
            LongPressGesture gesture = args.LongPressGesture;
            Console.WriteLine($"Item : [{gridItem?.Text}] is LongPressed! View: [{detectedView}], State:[{gesture.State}] Touch: [{gesture.LocalPoint.X}, {gesture.LocalPoint.Y}] itemPos : [{gridItem.PositionX}, {gridItem.PositionY}]");

            switch (gesture.State) {
                case Gesture.StateType.Started :
                onReordering = gridItem;
                Position originPos = onReordering.Position;
                Size originSize = onReordering.Size;

                float xPos = onReordering.PositionX + (gesture.LocalPoint.X - onReordering.SizeWidth / 2);
                float yPos = onReordering.PositionY + (gesture.LocalPoint.Y - onReordering.SizeHeight / 2);
                onReordering.Position = new Position(xPos, yPos);

                //AnimateItemShift(onReordering.Index, Int32.MaxValue);

                targetPreview.Position = originPos;
                targetPreview.Size = originSize;
                targetPreview.LowerBelow(onReordering);
                targetPreview.Opacity = 0f;
                itemShiftAnimation.AnimateTo(targetPreview, "Opacity", 1f, 0, itemShiftDuration, new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseOut));
                itemShiftAnimation.Play();

                onReordering.RaiseToTop();
                mPanGestureDetector.Attach(Container);
                break;

                case Gesture.StateType.Finished :
                (float X, float Y) target;
                if (targetIndex != -1)
                {
                    target = GetItemPosition(targetIndex);
                }
                else
                {
                    target = GetItemPosition(onReordering.Index);
                }

                Console.WriteLine($"prev index: {onReordering.Index}, target index: {targetIndex}, targetPos : [{target.X}, {target.Y}]");

                var reorderDuration = 500;
                var reorderAnimation = new Animation(reorderDuration);
                reorderAnimation.EndAction = Animation.EndActions.StopFinal;
                reorderAnimation.AnimateTo(onReordering, "Position", new Position(target.X, target.Y), 0, reorderDuration, new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseOut));
                reorderAnimation.AnimateTo(targetPreview, "Opacity", 0f, 0, reorderDuration, new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseOut));
                reorderAnimation.Finished += (object o, EventArgs e) =>
                {
                    if (targetIndex != -1 && onReordering.Index != targetIndex)
                    {
                    // reordering data.
                        var args = new ItemReorderEventArgs()
                        {
                            From = onReordering.Index,
                            To = targetIndex,
                        };
                        ItemReordered?.Invoke(this, args);
                    }

                    onReordering = null;
                };

                reorderAnimation.Play();

                mPanGestureDetector.Detach(Container);

                break;

                case Gesture.StateType.Cancelled :
                mPanGestureDetector.Detach(Container);
                onReordering = null;
                break;
            }

        }

        internal void OnPanGestureDetected(object view, PanGestureDetector.DetectedEventArgs args)
        {
            DefaultGridItem gridItem = onReordering as DefaultGridItem;
            View detectedView = args.View;
            PanGesture gesture = args.PanGesture;

            // no longpress exist here.
            if (gridItem == null) return;

            Vector2 itemCenter = new Vector2(gridItem.PositionX + gridItem.SizeWidth /2, gridItem.PositionY + gridItem.SizeHeight/2);

            // 1. Move item position. gesture position is based on ContentContainer geometry.
            float xPos = (gesture.Position.X - gridItem.SizeWidth / 2);
            float yPos = (gesture.Position.Y - gridItem.SizeHeight / 2);
            gridItem.Position = new Position(xPos, yPos);
            gridItem.RaiseToTop();

            //Console.WriteLine($"Item : [{gridItem?.Text}] is On Pan! State:[{gesture.State}] Touch: [{gesture.Position.X}, {gesture.Position.Y}] itemPos : [{gridItem.PositionX}, {gridItem.PositionY}]");

            // 2. Check item is available for Reorder
            int prevTargetIndex = targetIndex;
            targetIndex = CheckAvailableReorderPlace(xPos, yPos, 50);
            if (targetIndex != -1)
            {
                (float X, float Y) targetPos = GetItemPosition(targetIndex);
                (float W, float H) targetSize = GetItemSize(targetIndex);

                targetPreview.Position = new Position(targetPos.X, targetPos.Y);
                targetPreview.Size = new Size(targetSize.W, targetSize.H);
                targetPreview.LowerBelow(GetVisibleItem(targetIndex + 1));
                targetPreview.Opacity = 0f;
                itemShiftAnimation.AnimateTo(targetPreview, "Opacity", 1f, 0, itemShiftDuration, new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseOut));
                Console.WriteLine($"Item : [{gridItem?.Text}:{onReordering.Index}] is moved-->[{gesture.Position.X}, {gesture.Position.Y}] [{targetIndex}]! Position [{targetPos.X}, {targetPos.Y}]");

                AnimateItemShift(onReordering.Index, targetIndex);


            }
            else
            {
                targetIndex = prevTargetIndex;
            }

            // 3. Move scroll if necessary.
            // It only cares vertical case, please add more if you need horizontal scroll.
            // Movement is not smooth enough currently, you need to find better value for that.
            if (yPos + gridItem.SizeHeight > ItemsView.ScrollCurrentPosition.Y + ItemsView.SizeHeight)
            {
                ItemsView.ScrollTo(yPos + gridItem.SizeHeight - ItemsView.SizeHeight, true);
            }
            else if (ItemsView.ScrollCurrentPosition.Y > yPos)
            {
                ItemsView.ScrollTo(yPos, true);
            }
        }

        private void AnimateItemShift(int from, int to)
        {
            int index;

            //itemShiftAnimation.Stop();
            for (int i = FirstVisible; i <= LastVisible; i++)
            {
                RecyclerViewItem cur = null;
                cur = GetVisibleItem(i);
                if (cur == onReordering) continue;

                // Get position of index - 1.
                index = i;

                if (i > from)
                {
                    index--;
                }
                if (i > to)
                {
                    index++;
                }
                if (i == to && from > to)
                {
                    index++;
                }

                (float x, float y) pos = GetItemPosition(index);
                itemShiftAnimation.AnimateTo(cur, "Position",
                                        new Position(pos.x, pos.y), 0, itemShiftDuration,
                                        new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseOut));
            }

            itemShiftAnimation.Play();
        }

        private int CheckAvailableReorderPlace(float x, float y, float offset)
        {
            int ret = -1;

            int row = (int)(x / SizeCandidate.Width);
            int col = (int)((y + (SizeCandidate.Height/2)) / SizeCandidate.Height);

            int index = col * SpanSize + row;

            (float x, float y) pos = (row * SizeCandidate.Width, col * SizeCandidate.Height);

            if (pos.x <= x && pos.x + offset >= x)
            {
                ret = index;
                Console.Write($"pos:[{x}, {y}] row:[{row}], col:[{col}], index:[{index}]-----");
                Console.WriteLine($"Calced Pos[{pos.x}, {pos.y}] Left of {index}! return [{ret}]");
            }
            // check on right (current index + 1)
            else if (pos.x + SizeCandidate.Width >= x && pos.x + SizeCandidate.Width - offset <= x)
            {
                ret = index + 1;
                Console.Write($"pos:[{x}, {y}] row:[{row}], col:[{col}], index:[{index}]-----");
                Console.WriteLine($"Calced Pos[{pos.x}, {pos.y}] Right of{index}! return [{ret}]");
            }

            return ret;
        }


    }

    /********************************* Data Source  ************************************/
    public class Country : INotifyPropertyChanged
    {
        private string mName;
        private int mIndex;
        private string resourcePath = Tizen.Applications.Application.Current.DirectoryInfo.Resource;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Country(int index, string name)
        {
            mIndex = index;
            mName = name;
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

    }
}