/*
 * Copyright (c) 2021 Samsung Electronics Co., Ltd.
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

namespace Tizen.NUI
{
    /// <summary>
    /// It is an internal class for security origin list of web view.
    /// </summary>
    internal class WebSecurityOriginList : Disposable
    {
        internal WebSecurityOriginList(global::System.IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn)
        {
        }

        /// This will not be public opened.
        /// <param name="swigCPtr"></param>
        protected override void ReleaseSwigCPtr(System.Runtime.InteropServices.HandleRef swigCPtr)
        {
            Interop.WebSecurityOriginList.DeleteWebSecurityOriginList(swigCPtr);
        }

        /// <summary>
        /// Count of security origin list.
        /// </summary>
        internal uint ItemCount
        {
            get
            {
                return Interop.WebSecurityOriginList.GetItemCount(SwigCPtr);
            }
        }

        /// <summary>
        /// Gets security origin by index.
        /// </summary>
        /// <param name="index">index of list</param>
        internal WebSecurityOrigin GetItemAtIndex(uint index)
        {
            System.IntPtr dataIntPtr = Interop.WebSecurityOriginList.ValueOfIndex(SwigCPtr, index);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return new WebSecurityOrigin(dataIntPtr, false);
        }
    }
}
