/*
 * Copyright(c) 2021 Samsung Electronics Co., Ltd.
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

using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using System;
using System.Collections.Generic;

namespace NUITizenGallery
{

    static public class CardData
    {


        static public ushort CardWidth
        {
            get => Card_WIDTH_SIZE;
        }

        static public ushort CardHeight
        {
            get => Card_HEIGHT_SIZE;
        }
        static public ushort PaddingSize
        {
            get => Card_PAD;
        }


    }

    public class FlexTestCardPage : ContentPage
    {
        public FlexTestCardPage()
        {
            var scrollLayout = new ScrollableBase()
            {
                WidthSpecification = (CardData.GetSpanWidth(4) + CardData.PaddingSize * 4),
                HeightSpecification = LayoutParamPolicies.MatchParent,
                BackgroundColor = Color.Gray,
                Padding = new Extents(CardData.PaddingSize),
                Layout = new FlexLayout()
                {
                    Direction = FlexLayout.FlexDirection.Row,
                    Justification = FlexLayout.FlexJustification.FlexStart,
                    Alignment = FlexLayout.AlignmentType.FlexStart,
                    WrapType = FlexLayout.FlexWrapType.Wrap,
                    //LayoutWithTransition=true,
                },
                ScrollingDirection = ScrollableBase.Direction.Vertical,
            };
            var first = new Card_2_by_1("1st - 2*1");
            scrollLayout.Add(first);
            scrollLayout.Add(new Card_2_by_1("2nd - 2*1"));
            scrollLayout.Add(new Card_2_by_2("3rd - 2*2"));
            scrollLayout.Add(new Card_2_by_2("4th - 2*2"));
            scrollLayout.Add(new Card_4_by_3("5th - 4*3"));
            scrollLayout.Add(new Card_2_by_2("6th - 2*2"));
            scrollLayout.Add(new Card_2_by_2("7th - 2*2"));
            scrollLayout.Add(new Card_2_by_2("8th - 2*2"));
            scrollLayout.Add(new Card_2_by_2("9th - 2*2"));
            scrollLayout.Add(new Card_4_by_3("10th - 4*3"));
            View last = new Card_4_by_3("11");
            scrollLayout.Add(last);
            //last.SiblingOrder = 3;
            //scrollLayout.Insert(last, 3);

            Add(scrollLayout);

            var timer = new Timer(3000);
            timer.Tick += (o, e) =>
            {
                scrollLayout.Remove(first);//.WidthSpecification = CardData.GetSpanWidth(4);
                //Console.WriteLine($"last geometry [{last.Position.X},{last.Position.Y},{last.Size.Width},{last.Size.Height}] [{last.WidthSpecification}, {last.HeightSpecification}]");

                return false;
            };
            timer.Start();
       }

    }

    public interface Spannable
    {
        public uint RowSpan { get; set; }
        public uint ColumnSpan { get; set; }
    }

    public class SpanItem : Spannable
    {
        private uint mRowSpan = 1;
        private uint mColumnSpan = 1;
        private string mTitle = 1;

        public SpanItem(string title)
        {
            mTitle = title;
        }

        public SpanItem(string title, unit rowSpan, uint colSpan)
        {d
            mTitle = title;
            mRowSpan = rowSpan;
            mColumnSpan = colSpan;
        }

        public uint RowSpan
        {
            get => mRowSpan;
            set => mRowSpan = value;
        }

        public uint ColumnSpan
        {
            get => mColumnSpan;
            set => mColumnSpan = value;
        }

        public string Title
        {
            get => mTitle;
            set => mTitle = value;
        }
    }

    public class SpannableGridLayouter : ItemsLayouter
    {
        private ushort spanCount = 1;
        private float lineSize;
        private Extent itemPadding;

        private List<SpanLine> lines = new List<SpanLine>();

        private ObservableCollection<Spannable> spanSource;

        public ushort SpanCount
        {
            get => spanCount;
            set => spanCount = value;
        }

        public float LineSize
        {
            get => lineSize;
            set => lineSize = value;
        }

        public Extent ItemPadding
        {
            get => itemPadding;
            set => itemPadding = value;
        }



                /// <summary>
        /// Clean up ItemsLayouter.
        /// </summary>
        /// <param name="view"> CollectionView of layouter.</param>
        /// <remarks>please note that, view must be type of CollectionView</remarks>
        /// <since_tizen> 9 </since_tizen>
        public override void Initialize(RecyclerView view)
        {
            colView = view as CollectionView;
            if (colView == null)
            {
                throw new ArgumentException("LinearLayouter only can be applied CollectionView.", nameof(view));
            }

            base.Initialize(view);


            // 1. Clean Up
            Clear();

            ItemSource source = colView.InternalItemSource;
            SpannableItemSource spanSource = source as SpannableItemSource;
            if ()
            {
            }



             int count = source.Count;
        }







        class SpanItem
        {
            public Spannable Span { get; set; }
            public SpanLine StartLine;
            public SpanLine EndLine;
            public unit StartLineIndex;
            public unit EndLineIndex;
        }

        class SpanLine: List<SpanItem>
        {
            public float Position { get; set; }
            public unit Index {} get; set; }
        }
    }




    // Container
    public class CardsView : Control
    {
        const ushort Card_WIDTH_SIZE = 150;
        const ushort Card_HEIGHT_SIZE = 100;
        const ushort Card_PAD = 10;

        struct CardMatrix
        {
            public int Row { get; set; }
            public int Column { get; set; }
        }

        private List<CardMatrix> MatrixSpan = new List<CardMatrix>();
        public CardsView() : base()
        {
            Layout = new AbsoluteLayout();
        }

        public int RowCount { get; set; }
        public int ColumnCount { get; set; }

        public override Add(View child)
        {
            base.Add(child);
            //PlaceCard
        }

        public virtual Insert(View child, int index)
        {
            base.Add(child);
            child.SiblingOrder = index;
            //PlaceCard
        }

        public bool PlaceCard(View child, int index)
        {
            // check available current row

            // if possible update position and place it.

            // if not possible place another row
        }

        public (int, int) FindAvaliablePosition(View child, int index)
        {
            int i = 0, maxRow = 0, maxColumn = 0;
            foreach (CardMatrix card in MatrixSpan)
            {
                if (i == index)
                {
                    // Claculate Current Row and Column.
                    return (maxRow, maxColumn)
                }

                if (card.Row > maxRow) maxRow = cardRow;
                if (card.Column > maxColumn) maxColumn = cardColumn;
                i++;
            }
        }


                static public int GetSpanWidth(ushort i)
        {
            return ((Card_WIDTH_SIZE * i) + Card_PAD * 2 * (i-1));
        }
        static public int GetSpanHeight(ushort i)
        {
            return ((Card_HEIGHT_SIZE * i) + Card_PAD * 2 * (i-1));
        }
    }


    // Items
    public class CardItem : Button
    {
        protected virtual void Initialize()
        {
            BackgroundColor = new Color(255,255,255,100);
            CornerRadius = 20f;
            Margin = new Extents(CardData.PaddingSize);
            TextColor = Color.Black;
        }

        public CardItem() : base()
        {
            Initialize();
        }

        public CardItem(string text) : base()
        {
            Initialize();
            Text = text;
        }

        public int Row { get; set; }
        public int RowSpan { get; set; }
        public int Column { get; set; }
        public int ColumnSpan { get; set; }
    }

    public class Card_2_by_1 : CardItem
    {
        protected override void Initialize()
        {
            base.Initialize();
            WidthSpecification = CardData.GetSpanWidth(2);
            HeightSpecification = CardData.GetSpanHeight(1);
        }

        public Card_2_by_1() : base()
        {
            //Initialize();
            Text = "2X1";
        }

        public Card_2_by_1(string text) : base(text)
        {
            //Initialize();
        }
    }

    public class Card_2_by_2 : CardItem
    {
        protected override void Initialize()
        {
            base.Initialize();
            WidthSpecification = CardData.GetSpanWidth(2);
            HeightSpecification = CardData.GetSpanHeight(2);
        }
        public Card_2_by_2() : base()
        {
            //Initialize();
            Text = "2X2";
        }

        public Card_2_by_2(string text) : base(text)
        {
            //Initialize();
        }
    }

    public class Card_4_by_3 : CardItem
    {
        protected override void Initialize()
        {
            base.Initialize();
            WidthSpecification = CardData.GetSpanWidth(4);
            HeightSpecification = CardData.GetSpanHeight(3);
        }
        public Card_4_by_3() : base()
        {
            //Initialize();
            Text = "4X3";
        }
        public Card_4_by_3(string text) : base(text)
        {
            //Initialize();
        }
    }
}