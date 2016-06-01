using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store
{
    public class IabServiceStartedEvent : SoomlaEvent
    {

        public IabServiceStartedEvent() : this(null)
        {

        }

        public IabServiceStartedEvent(Object sender) : base(sender)
        {
        }
    }
}