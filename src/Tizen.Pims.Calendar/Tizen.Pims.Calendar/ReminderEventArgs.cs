/*
 * Copyright (c) 2016 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;

namespace Tizen.Pims.Calendar
{
    /// <summary>
    /// Event arguments passed when alarm is alerted.
    /// </summary>
    public class ReminderAlertedEventArgs : EventArgs
    {
        internal ReminderAlertedEventArgs(string param)
        {
            Param = param;
            Log.Debug(Globals.LogTag, "[TEST]" + param);
        }

        /// <summary>
        /// The paramter which data is combined(Value string like id=value&time=value&tick=value&unit=value&type=value)
        /// </summary>
        public string Param
        {
            get;
            internal set;
        }
    }
}
