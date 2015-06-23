using SoomlaWpStore.domain.virtualGoods;
using SoomlaWpCore.events;

namespace SoomlaWpStore.events
{
    public class GoodUnEquippedEvent : SoomlaEvent
    {
        private EquippableVG mGood;

        public EquippableVG GetEquippableVG()
        {
            return mGood;
        }
        
        public GoodUnEquippedEvent(EquippableVG good)
            : this(good, null)
        {
        }

        public GoodUnEquippedEvent(EquippableVG good, object sender)
            : base(sender)
        {
            mGood = good;
        }
    }
}
