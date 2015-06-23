using SoomlaWpStore.domain.virtualGoods;
using SoomlaWpCore.events;

namespace SoomlaWpStore.events
{
    public class GoodEquippedEvent : SoomlaEvent
    {
        private EquippableVG mGood;

        public EquippableVG GetEquippableVG()
        {
            return mGood;
        }
        
        public GoodEquippedEvent(EquippableVG good)
            : this(good, null)
        {
        }

        public GoodEquippedEvent(EquippableVG good, object sender)
            : base(sender)
        {
            mGood = good;
        }
    }
}
