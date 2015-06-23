using SoomlaWpCore.events;

namespace SoomlaWpStore.events
{
    public class RestoreTransactionsStartedEvent : SoomlaEvent
    {
        public RestoreTransactionsStartedEvent()
            : this(null)
        {
        }

        public RestoreTransactionsStartedEvent(object sender)
            : base(sender)
        {
        }
    }
}
