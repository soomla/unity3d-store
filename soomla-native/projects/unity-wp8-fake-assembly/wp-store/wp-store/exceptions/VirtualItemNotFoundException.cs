/// Copyright (C) 2012-2014 Soomla Inc.
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

namespace SoomlaWpStore.exceptions
{

/**
 * This exception is thrown when looking for a <code>VirtualItem</code> that cannot be found.
 */
public class VirtualItemNotFoundException : Exception {

    /**
     * Constructor
     *
     * @param lookupBy the field of the virtual item to look for
     * @param lookupVal the value of the field to look for
     */
    public VirtualItemNotFoundException(String lookupBy, String lookupVal) : base("Virtual item was not found when searching with " + lookupBy + "=" + lookupVal)
    {
    }
}
}