/// Copyright (C) 2012-2015 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoomlaWpCore.util;
using SoomlaWpCore.events;

namespace SoomlaWpCore
{
    public class Foreground
    {
        public const String TAG = "SOOMLA Foreground";
        private static Foreground mInstance;
        private static bool isForeground;
        public static Foreground Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new Foreground();
                    mInstance.Init();
                }
                return mInstance;
            }
            set
            {
            }
        }

        private void Init()
        {
            isForeground = true;
        }


        
        public bool IsForeground()
        {
            return true;
        }

        public bool IsBackground()
        {
            return false;
        }
    }
}
