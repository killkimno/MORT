//  ---------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// 
//  The MIT License (MIT)
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//  ---------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Windows.System;

namespace Composition.WindowsRuntimeHelpers
{
    public static class CoreMessagingHelper
    {
        enum DISPATCHERQUEUE_THREAD_APARTMENTTYPE
        {
            DQTAT_COM_NONE = 0,
            DQTAT_COM_ASTA = 1,
            DQTAT_COM_STA = 2
        }

        enum DISPATCHERQUEUE_THREAD_TYPE
        {
            DQTYPE_THREAD_DEDICATED = 1,
            DQTYPE_THREAD_CURRENT = 2
        }

        struct DispatcherQueueOptions
        {
            public int dwSize;
            public DISPATCHERQUEUE_THREAD_TYPE threadType;
            public DISPATCHERQUEUE_THREAD_APARTMENTTYPE apartmentType;
        }

        [DllImport(
            "CoreMessaging.dll",
            EntryPoint = "CreateDispatcherQueueController",
            SetLastError = true,
            CharSet = CharSet.Unicode,
            ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall
            )]
        static extern UInt32 CreateDispatcherQueueController(DispatcherQueueOptions options, out IntPtr dispatcherQueueController);

        public static DispatcherQueueController CreateDispatcherQueueControllerForCurrentThread()
        {
            var options = new DispatcherQueueOptions
            {
                dwSize = Marshal.SizeOf<DispatcherQueueOptions>(),
                threadType = DISPATCHERQUEUE_THREAD_TYPE.DQTYPE_THREAD_CURRENT,
                apartmentType = DISPATCHERQUEUE_THREAD_APARTMENTTYPE.DQTAT_COM_NONE
            };

            DispatcherQueueController controller = null;
            uint hr = CreateDispatcherQueueController(options, out IntPtr controllerPointer);
            if (hr == 0)
            {
                controller = Marshal.GetObjectForIUnknown(controllerPointer) as DispatcherQueueController;
                Marshal.Release(controllerPointer);
            }

            return controller;
        }
    }
}
