using SoomlaWpStore.domain;
using SoomlaWpCore.events;

namespace SoomlaWpStore.events
{
    public class SoomlaStoreInitializedEvent : SoomlaEvent
    {
        public SoomlaStoreInitializedEvent()
            : this(null)
        {
        }

        public SoomlaStoreInitializedEvent(object sender)
            : base(sender)
        {
        }
    }
}
