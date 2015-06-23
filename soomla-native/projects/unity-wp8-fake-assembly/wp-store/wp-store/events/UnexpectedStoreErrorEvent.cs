using SoomlaWpStore.domain;
using SoomlaWpCore.events;

namespace SoomlaWpStore.events
{
    public class UnexpectedStoreErrorEvent : SoomlaEvent
    {
        private string mMessage;
        public string GetMessage()
        {
            return mMessage;
        }

        public UnexpectedStoreErrorEvent(string message)
            : this(message,null)
        {
        }

        public UnexpectedStoreErrorEvent(string message, object sender)
            : base(sender)
        {
            mMessage = message;
        }
    }
}
