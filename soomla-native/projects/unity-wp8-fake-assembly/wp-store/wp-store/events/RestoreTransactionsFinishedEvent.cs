using SoomlaWpStore.domain;
using SoomlaWpCore.events;

namespace SoomlaWpStore.events
{
    public class RestoreTransactionsFinishedEvent : SoomlaEvent
    {
        private bool mSuccess;
        public bool GetSuccess()
        {
            return mSuccess;
        }

        public RestoreTransactionsFinishedEvent(bool success)
            : this(success, null)
        {
        }

        public RestoreTransactionsFinishedEvent(bool success, object sender)
            : base(sender)
        {
            mSuccess = success;
        }
    }
}
