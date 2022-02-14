using Tizen.NUI.Components;
using Tizen.NUI.BaseComponents;
using Tizen.NUI;
using Tizen.NUI.Binding;

namespace Tizen.NUI.StyleGuide
{
    class ListItemLayout : RecyclerViewItem
    {
        readonly string ResourcePath = Tizen.Applications.Application.Current.DirectoryInfo.Resource + "images/";
        private static int Width = 1792;
        private static int Height = 108;
        private static bool AnimationRequired;

        private const int IconSize = 64;
        private const int LayoutMargin = 16;
        private const int LayoutPadding = 32;
        private const int SeperatorHeight = 1;
        private const int LeftPadding = 64;
        private const int x = 0;

        private View itemSeperator;
        private TextLabel titleLabel;
        private TextLabel subtitleLabel;
        private ImageView icon;
        private Button playPauseIcon;
        private bool isAnimating = false;
        private bool isPlayingStatus = false;

        public static readonly BindableProperty IsPlayingProperty = BindableProperty.Create(nameof(IsPlaying), typeof(bool), typeof(ListItemLayout), false, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var instance = (ListItemLayout)bindable;
            if (newValue != null)
            {
                Tizen.Log.Debug("Item", $"IsPlaying property changed\n");
                bool newPlaying = (bool)newValue;
                bool oldPlaying = (bool)oldValue;
                if (oldPlaying != newPlaying)
                {
                    Tizen.Log.Debug("Item", $"IsPlaying Changed {oldPlaying} to {newPlaying}\n");
                    instance.UpdateItem(newPlaying);
                }
            }
        },
        defaultValueCreator: (bindable) => ((ListItemLayout)bindable).isPlaying);

        public static readonly BindableProperty PlayingStatusProperty = BindableProperty.Create(nameof(PlayingStatus), typeof(bool), typeof(ListItemLayout), false, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var instance = (ListItemLayout)bindable;
            if (newValue != null)
            {
                bool newPlayingStatus = (bool)newValue;
                bool oldPlayingStatus = (bool)oldValue;
                if (oldPlayingStatus != newPlayingStatus)
                {
                    instance.UpdatePlayingStatus(newPlayingStatus);
                }
            }
        },
defaultValueCreator: (bindable) => ((ListItemLayout)bindable).playingStatus);

        public ListItemLayout(bool animationRequired = false, int width = 1792, int height = 108) : base()
        {
            base.OnInitialize();
            Width = width;
            Height = height;
            AnimationRequired = animationRequired;
            WidthSpecification = Width;
            HeightSpecification = Height;
            Size2D = new Size2D(Width, Height).DpToPx();
            // to show the rounded rect of the bg
            BackgroundColor = Color.Transparent;

            icon = new ImageView()
            {
                Size2D = new Size2D(IconSize, IconSize).DpToPx(),
                Position2D = new Position2D(x, ((Height / 2) - (IconSize / 2))).DpToPx(),
            };
            base.Add(icon);

            itemSeperator = new View()
            {
                Size2D = new Size2D((Width - (2 * LeftPadding)), SeperatorHeight).DpToPx(),
                ExcludeLayouting = true,
                Position2D = new Position2D(x, Height - SeperatorHeight).DpToPx(),
                BackgroundColor = Color.Black,
            };
            base.Add(itemSeperator);

            titleLabel = new TextLabel()
            {
                Size2D = new Size2D((Width - (2 * LeftPadding) - IconSize - LayoutPadding), 40).DpToPx(),
                PixelSize = 32.DpToPx(),
                FontFamily = "BreezeSans",
                VerticalAlignment = VerticalAlignment.Center,
                Position2D = new Position2D((x + IconSize + LayoutPadding), LayoutMargin).DpToPx(),
            };
            base.Add(titleLabel);

            subtitleLabel = new TextLabel()
            {
                Size2D = new Size2D((Width - (2 * LeftPadding) - IconSize - LayoutPadding), 36).DpToPx(),
                PixelSize = 28.DpToPx(),
                FontFamily = "BreezeSans",
                VerticalAlignment = VerticalAlignment.Center,
                Position2D = new Position2D((x + IconSize + LayoutPadding), LayoutMargin + 40).DpToPx(),
            };
            base.Add(subtitleLabel);
            UpdateLabelColors();
            ThemeManager.ThemeChanged += OnThemeUpdated;
        }
        public ImageView Icon
        {
            get => icon;
        }
        public TextLabel TitleLabel
        {
            get => titleLabel;
        }
        public TextLabel SubtitleLabel
        {
            get => subtitleLabel;
        }

        private bool isPlaying = false;

        public bool IsPlaying
        {
            get => (bool)GetValue(IsPlayingProperty);
            set => SetValue(IsPlayingProperty, value);
        }

        private bool playingStatus = false;

        public bool PlayingStatus
        {
            get => (bool)GetValue(PlayingStatusProperty);
            set => SetValue(PlayingStatusProperty, value);
        }

        protected override void Dispose(DisposeTypes type)
        {
            if(Disposed)
            {
                return;
            }
            if (type == DisposeTypes.Explicit)
            {
                base.Remove(itemSeperator);
                itemSeperator?.Dispose();
                itemSeperator = null;

                RemoveAnimation();
                base.Remove(icon);
                icon?.Dispose();
                icon = null;

                base.Remove(titleLabel);
                titleLabel?.Dispose();
                titleLabel = null;

                base.Remove(subtitleLabel);
                subtitleLabel?.Dispose();
                subtitleLabel = null;
            }

            base.Dispose(type);
        }

        private void UpdateItem(bool currentValue)
        {
            if(currentValue)
            {
                Tizen.Log.Debug("Item", "Adding animation/play-pause icon and setting highlight color\n");
                if(titleLabel != null)
                {
                    titleLabel.TextColor = Color.Blue;
                }
                if(subtitleLabel != null)
                {
                    subtitleLabel.TextColor = Color.Red;
                }
                if(AnimationRequired == true && isAnimating == false)
                {
                    isAnimating = AddAnimation();
                }
                else if(AnimationRequired == false && isPlayingStatus == false )
                {
                    isPlayingStatus = AddPlayingStatus();
                }
            }
            else
            {
                Tizen.Log.Debug("Item", "Remove animation/play-pause icon and setting normal color\n");
                UpdateLabelColors();
                if(AnimationRequired == true && isAnimating)
                {
                    RemoveAnimation();
                    isAnimating = false;
                }
                else if(AnimationRequired == false && isPlayingStatus == true)
                {
                    RemovePlayingStatus();
                    isPlayingStatus = false;
                }
            }
            isPlaying = currentValue;
        }

        private void UpdatePlayingStatus(bool currentValue)
        {
            if (isPlayingStatus == true)
            {
                playPauseIcon.IconURL = currentValue ? ResourcePath + "pause_icon.png" : ResourcePath + "play_icon.png";
            }
            playingStatus = currentValue;
        }

        private void OnThemeUpdated(object sender, ThemeChangedEventArgs e)
        {
            if(e.IsPlatformThemeChanged && IsPlaying == false)
            {
                UpdateLabelColors();
            }
        }

        private void UpdateLabelColors()
        {

        }

        private bool AddAnimation()
        {
            if(icon == null)
            {
                return false;
            }
            LottieAnimationView lottieAnimationView = new LottieAnimationView();
            if(lottieAnimationView == null)
            {
                return false;
            }
            lottieAnimationView.URL = ResourcePath + "now_playing_opacity.json";
            lottieAnimationView.LoopCount = -1;
            icon.Add(lottieAnimationView);
            lottieAnimationView.Play();
            return true;
        }
        private void RemoveAnimation()
        {
            if(icon != null)
            {
                View child = icon.GetChildAt(0);
                if (child != null && child is LottieAnimationView)
                {
                    icon.Remove(child);
                    LottieAnimationView lottieAnimationView = child as LottieAnimationView;
                    lottieAnimationView.Stop();
                    lottieAnimationView.Dispose();
                }
            }
        }

        private bool AddPlayingStatus()
        {
            if (icon == null)
            {
                return false;
            }
            ButtonStyle buttonStyle = new ButtonStyle()
            {
                Icon = new ImageViewStyle()
                {
                    ResourceUrl = new Selector<string>()
                    {
                        Normal = ResourcePath + "play_icon.png",
                        Selected = ResourcePath + "pause_icon.png"
                    }
                },
                IsEnabled = true,
                IsSelectable = true,
            };
            playPauseIcon = new Button(buttonStyle);
            //playPauseIcon = new Button();
            playPauseIcon.WidthSpecification = IconSize;
            playPauseIcon.HeightSpecification = IconSize;
            playPauseIcon.Clicked += (object sender, ClickedEventArgs e) =>
            {
                Tizen.Log.Debug("Item", "Button Clicked!\n");
            };
            icon.Add(playPauseIcon);
            return true;
        }

        private void RemovePlayingStatus()
        {
            if (icon != null)
            {
                View child = icon.GetChildAt(0);
                if (child != null && child is Button)
                {
                    icon.Remove(child);
                    Button playPauseIcon = child as Button;
                    playPauseIcon.Dispose();
                }
            }
        }
    }
}
