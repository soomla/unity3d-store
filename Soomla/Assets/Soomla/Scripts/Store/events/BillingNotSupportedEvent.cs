using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store
{
    public class BillingNotSupportedEvent : SoomlaEvent
    {
        public BillingNotSupportedEvent(): this(null)
        {

        }

        public BillingNotSupportedEvent(Object sender) : base(sender)
        {

        }
    }
}
