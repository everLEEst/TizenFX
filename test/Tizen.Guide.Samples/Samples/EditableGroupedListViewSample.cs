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
    internal class EditableGroupedListViewSample : ContentPage, ISample
    {
        private Window window;
        public void Activate()
        {
        }
        public void Deactivate()
        {
            window = null;
        }

        DefaultTitleItem CreateTitle()
        {
            var item = new DefaultTitleItem()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent
            };

            item.SetBinding(DefaultTitleItem.TextProperty, "GroupName");

            return item;
        }

        internal class DeviceItem : RecyclerViewItem
        {
            private CheckBox mCheck = new CheckBox();
            private ImageView mIcon = new ImageView()
            {
                WidthSpecification = 50,
                HeightSpecification = 50,
            };
            private TextLabel mLabel = new TextLabel()
            {
                PixelSize = 40,
            };

            public DeviceItem() : base()
            {
                // Set layout
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    LinearAlignment = LinearLayout.Alignment.CenterVertical,
                    CellPadding = new Size2D(20, 0),
                };

                // Set size specifications
                WidthSpecification = LayoutParamPolicies.MatchParent;
                HeightSpecification = 100;

                // Set style
                ApplyStyle( new ViewStyle()
                {
                    BackgroundColor = new Selector<Color>
                    {
                        Normal = Color.White,
                        Pressed = Color.Gray,
                        Selected = Color.LightGray,
                    },
                    Padding = new Extents(20, 20, 0, 0),
                });

                // Add children
                Add(mCheck);
                Add(mIcon);
                Add(mLabel);
            }

            public CheckBox Check
            {
                get => mCheck;
            }
            public ImageView Icon
            {
                get => mIcon;
            }
            public TextLabel Label
            {
                get => mLabel;
            }
        }

        DeviceItem CreateItem()
        {
            var item = new DeviceItem();

            // Binding properties
            item.Icon.SetBinding(ImageView.ResourceUrlProperty, "ResourceUrl");
            item.Label.SetBinding(TextLabel.TextProperty, "DeviceName");

            // Do not binding Twoway as CollectionView item is cached and reused.
            // update object IsSelected on CollectionView SelectionChanged callback
            // or Clicked.
            item.SetBinding(RecyclerViewItem.IsSelectedProperty, "IsSelected");

            item.Clicked += (s, e) =>
            {
                DeviceItem item = s as DeviceItem;
                if (item == null) return;

                Device dev = item.BindingContext as Device;
                if (dev == null) return;

                Console.WriteLine($"{dev.DeviceName} is clicked! {dev.IsSelected} -> {item.IsSelected}");
                dev.IsSelected = !dev.IsSelected;
            };

            // CheckBox does not process touch event by setting Sensitive false.
            // So, when user touches CheckBox, the touch event is passed to item and item becomes selected/unselected.
            // When item becomes selected/unselected, CheckBox becomes selected/unselected.
            // Because item's ControlState is propagated to its children by default by setting item.EnableControlStatePropagation true.
            item.EnableControlStatePropagation = true;
            item.Check.Sensitive = false;

            return item;
        }

        DeviceGroups CreateDevices()
        {
            //Collection for groups
            var devices = new DeviceGroups();

            DeviceGroup sence = new DeviceGroup("Sences");
            sence.Add(new Device("Sence 01"));
            sence.Add(new Device("Sence 02"));
            sence.Add(new Device("Sence 03"));
            sence.Add(new Device("Sence 04"));
            sence.Add(new Device("Sence 05"));
            devices.Add(sence);

            DeviceGroup device = new DeviceGroup("Devices");
            device.Add(new Device("Device 01"));
            device.Add(new Device("Device 02"));
            device.Add(new Device("Device 03"));
            device.Add(new Device("Device 04"));
            device.Add(new Device("Device 05"));
            device.Add(new Device("Device 06"));
            devices.Add(device);

            DeviceGroup service = new DeviceGroup("Services");
            service.Add(new Device("Service 01"));
            service.Add(new Device("Service 02"));
            service.Add(new Device("Service 03"));
            service.Add(new Device("Service 04"));
            devices.Add(service);

            return devices;
        }


        /// Modify this method for adding other examples.
        public EditableGroupedListViewSample() : base()
        {
            Log.Info(this.GetType().Name, $"{this.GetType().Name} is contructed\n");

            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            //Create Data Source
            var devices = CreateDevices();

            // Navigator bar title is added here.
            AppBar = new AppBar()
            {
                Title = "Editable Grouped List View Sample",
            };

            var mainView = new View
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    LinearAlignment = LinearLayout.Alignment.CenterVertical,
                },
            };
            // Example root content view.
            // you can decorate, add children on this view.
            var colView = new CollectionView()
            {
                IsGrouped = true,
                ItemsSource = devices,
                ItemsLayouter = new LinearLayouter(),
                SelectionMode = ItemSelectionMode.Multiple,
                ScrollingDirection = ScrollableBase.Direction.Vertical,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                ItemTemplate = new DataTemplate(CreateItem),
                GroupHeaderTemplate = new DataTemplate(CreateTitle),
            };

            /*
            colView.SelectionChanged += (o, args) =>
            {
                Can track the selections on this event callback.
            }
            */

            mainView.Add(colView);

            var editView = new View
            {
                BackgroundColor = Color.White,
                WidthSpecification = 150,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    LinearAlignment = LinearLayout.Alignment.Center,
                    CellPadding = new Size2D(0, 30),
                },
                BoxShadow = new Shadow(10.0f, new Color(0.0f, 0.0f, 0.0f, 0.16f), new Vector2(-2.0f, 0.0f)),

            };


            var childCount = new TextLabel()
            {
                PixelSize = 40,
                // Set binding Context!!
                BindingContext = devices,
            };
            childCount.SetBinding(TextLabel.TextProperty, "ChildCount");
            editView.Add(childCount);

            var selectCount = new TextLabel()
            {
                PixelSize = 40,
                // Set binding Context!!
            };
            selectCount.SetBinding(TextLabel.TextProperty, "SelectCount");
            selectCount.BindingContext = devices;
            editView.Add(selectCount);

            var selectAll = new CheckBox()
            {
                Text = "SelectAll",
                IconRelativeOrientation = Button.IconOrientation.Bottom,
            };
            selectAll.SetBinding(Button.IsSelectedProperty, "SelectAll");
            selectAll.BindingContext = devices;
            selectAll.Clicked += (o, e) =>
            {
                CheckBox check = o as CheckBox;
                if (check == null) return;

                DeviceGroups groups = check.BindingContext as DeviceGroups;
                if (groups == null) return;

                Console.WriteLine($"SelectAll clicked! {check.IsSelected}");

                if (check.IsSelected)
                {
                    groups.SelectAll = true;
                }
                else
                {
                    groups.DeselectAll();
                }
            };

            editView.Add(selectAll);

            mainView.Add(editView);

            Content = mainView;
        }
    }


    /********************************* Data Source  ************************************/
    public class Device : INotifyPropertyChanged
    {
        private string mDeviceName;
        private bool isSelected;
        private bool isConnected;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Device(string deviceName)
        {
            mDeviceName = deviceName;
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                Console.WriteLine($"{mDeviceName}{GetHashCode()} is {isSelected} -> {value}");
                // This check is mendetory as property can be set same value.
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        public bool IsConnected
        {
            get
            {
                return isConnected;
            }
            set
            {
                // This check is mendetory as property can be set same value.
                if (isConnected != value)
                {
                    isConnected = value;
                    OnPropertyChanged("IsConnected");
                }
            }
        }

        public string DeviceName
        {
            get
            {
                return mDeviceName;
            }
            set
            {
                mDeviceName = value;
                OnPropertyChanged("DeviceName");
            }
        }

        public string ResourceUrl
        {
            get
            {
                return Tizen.Applications.Application.Current.DirectoryInfo.Resource + "device.png";
            }
        }
    }

    public class DeviceGroup : ObservableCollection<Device>
    {
        private string mGroupName;
        private int mSelectCount;
        private bool mSelectAll;

        // PropertyChanged in observableCollection is protected. this event is for public usage.
        public event PropertyChangedEventHandler GroupPropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            var args = new PropertyChangedEventArgs(propertyName);
            OnPropertyChanged(args);
            GroupPropertyChanged?.Invoke(this, args);
        }

        public DeviceGroup(string groupName)
        {
            mGroupName = groupName;
        }

        public string GroupName
        {
            get
            {
                return mGroupName;
            }

            set
            {
                // This check is mendetory as property can be set same value.
                if (mGroupName != value)
                {
                    mGroupName = value;
                    OnPropertyChanged("GroupName");
                }
            }
        }

        public int SelectCount
        {
            get
            {
                return mSelectCount;
            }
            set
            {
                // This check is mendetory as property can be set same value.
                if (mSelectCount != value)
                {
                    mSelectCount = value;
                    OnPropertyChanged("SelectCount");
                }
            }
        }

        public bool SelectAll
        {
            get => mSelectAll;
            set
            {
                if (mSelectAll != value)
                {
                    mSelectAll = value;
                    if (value && mSelectCount == Count)
                    {
                        return;
                    }
                    else if (!value && mSelectCount == 0)
                    {
                        return;
                    }
                    Console.WriteLine($"Group Select All {value}");

                    if (value)
                    {
                        foreach (Device dev in this)
                        {
                            dev.IsSelected = value;
                        };
                    }
                    mSelectAll = value;
                    OnPropertyChanged("SelectAll");
                }
            }
        }

        public void DeselectAll()
        {
            foreach (Device dev in this)
            {
                dev.IsSelected = false;
            };
            if (mSelectAll) SelectAll = false;
        }

        protected override void InsertItem(int index, Device item)
        {
            base.InsertItem(index, item);
            item.PropertyChanged += SelectionChanged;

        }

        protected override void RemoveItem(int index)
        {
            Device item = this[index];
            item.PropertyChanged -= SelectionChanged;
            base.RemoveItem(index);
        }

        private void SelectionChanged(object o, PropertyChangedEventArgs args)
        {
            Device dev = o as Device;
            if (dev == null) return;
            if (dev.IsSelected)
            {
                SelectCount++;
            }
            else
            {
                SelectCount--;
            }
        }
    }

    public class DeviceGroups : ObservableCollection<DeviceGroup>
    {
        private int mSelectCount;
        private int mChildCount;
        private bool mSelectAll;
        private bool onSelection;

        public int SelectCount
        {
            get
            {
                return mSelectCount;
            }
            set
            {
                // This check is mendetory as property can be set same value.
                if (mSelectCount != value)
                {
                    mSelectCount = value;
                    OnPropertyChanged( new PropertyChangedEventArgs("SelectCount"));
                }
            }
        }

        public bool SelectAll
        {
            get => mSelectAll;
            set
            {
                if (mSelectAll != value)
                {
                    mSelectAll = value;
                    if (value && mSelectCount == mChildCount)
                    {
                        return;
                    }
                    if (!value && mSelectCount == 0)
                    {
                        return;
                    }
                    Console.WriteLine($"Groups Select All {value}");
                    onSelection = true;
                    if (value)
                    {
                        foreach (DeviceGroup group in this)
                        {
                            group.SelectAll = value;
                        };
                    }
                    onSelection = false;
                    mSelectAll = value;
                    OnPropertyChanged( new PropertyChangedEventArgs("SelectAll"));
                }
            }
        }

        public void DeselectAll()
        {
            onSelection = true;
            foreach (DeviceGroup group in this)
            {
                group.DeselectAll();
            };
            onSelection = false;

            if (mSelectAll) SelectAll = false;
        }

        protected override void InsertItem(int index, DeviceGroup item)
        {
            base.InsertItem(index, item);
            item.GroupPropertyChanged += SelectionChanged;
            ChildCount += item.Count;
        }

        protected override void RemoveItem(int index)
        {
            DeviceGroup item = this[index];
            item.GroupPropertyChanged -= SelectionChanged;
            ChildCount -= item.Count;
            base.RemoveItem(index);
        }

        public int ChildCount
        {
            get
            {
                return mChildCount;
            }
            set
            {
                // This check is mendetory as property can be set same value.
                if (mChildCount != value)
                {
                    mChildCount = value;
                    OnPropertyChanged( new PropertyChangedEventArgs("ChildCount"));
                }
            }
        }

        private void SelectionChanged(object o, PropertyChangedEventArgs args)
        {
            int selectCount = 0;
            foreach(DeviceGroup group in this)
            {
                selectCount += group.SelectCount;
            }
            SelectCount = selectCount;

            if (onSelection) return;

            if (selectCount == mChildCount)
            {
                Console.WriteLine("SelectAll is activated!!");
                SelectAll = true;
            }
            else if (SelectAll)
            {
                Console.WriteLine("SelectAll is cancelled!!");
                SelectAll = false;
            }
        }
    }
}
