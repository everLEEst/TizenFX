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
            var item = new DeviceItem()
            {
            };

            // Binding properties
            item.Icon.SetBinding(ImageView.ResourceUrlProperty, "ResourceUrl");
            item.Label.SetBinding(TextLabel.TextProperty, "DeviceName");
            item.Check.SetBinding(CheckBox.IsSelectedProperty, "IsSelected");

            item.Check.SelectedChanged += (s, e) =>
            {
                if (e.IsSelected)
                {
                    Log.Info(this.GetType().Name, "CheckBox is Selected\n");
                }
                else
                {
                    Log.Info(this.GetType().Name, "CheckBox is Unselected\n");
                }
            };
            // CheckBox does not process touch event by setting Sensitive false.
            // So, when user touches CheckBox, the touch event is passed to item and item becomes selected/unselected.
            // When item becomes selected/unselected, CheckBox becomes selected/unselected.
            // Because item's ControlState is propagated to its children by default by setting item.EnableControlStatePropagation true.
            item.EnableControlStatePropagation = true;
            item.Check.Sensitive = false;

            return item;
        }

        ObservableCollection<DeviceGroup> CreateDevices()
        {
            //Collection for groups
            var devices = new ObservableCollection<DeviceGroup>();

            DeviceGroup sence = new DeviceGroup("Sences");
            sence.Add(new Device("Sence Name"));
            sence.Add(new Device("Sence Name"));
            sence.Add(new Device("Sence Name"));
            sence.Add(new Device("Sence Name"));
            sence.Add(new Device("Sence Name"));
            devices.Add(sence);

            DeviceGroup device = new DeviceGroup("Devices");
            device.Add(new Device("Device Name 01"));
            device.Add(new Device("Device Name 02"));
            device.Add(new Device("Device Name 03"));
            device.Add(new Device("Device Name 04"));
            device.Add(new Device("Device Name 05"));
            device.Add(new Device("Device Name 06"));
            devices.Add(device);

            DeviceGroup service = new DeviceGroup("Services");
            service.Add(new Device("Service Name 01"));
            service.Add(new Device("Service Name 02"));
            service.Add(new Device("Service Name 03"));
            service.Add(new Device("Service Name 04"));
            devices.Add(service);

            return devices;
        }


        /// Modify this method for adding other examples.
        public EditableGroupedListViewSample() : base()
        {
            Log.Info(this.GetType().Name, $"{this.GetType().Name} is contructed\n");

            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;
            // Navigator bar title is added here.
            AppBar = new AppBar()
            {
                Title = "Editable Grouped List View Sample",
            };

            // Example root content view.
            // you can decorate, add children on this view.
            var colView = new CollectionView()
            {
                IsGrouped = true,
                ItemsSource = CreateDevices(),
                ItemsLayouter = new LinearLayouter(),
                SelectionMode = ItemSelectionMode.Multiple,
                ScrollingDirection = ScrollableBase.Direction.Vertical,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                ItemTemplate = new DataTemplate(CreateItem),
                GroupHeaderTemplate = new DataTemplate(CreateTitle),
            };

            Content = colView;
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
                isSelected = value;
                OnPropertyChanged("IsSelected");
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
                isConnected = value;
                OnPropertyChanged("IsConnected");
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
                mGroupName = value;
                OnPropertyChanged( new PropertyChangedEventArgs("GroupName"));
            }
        }
    }
}
