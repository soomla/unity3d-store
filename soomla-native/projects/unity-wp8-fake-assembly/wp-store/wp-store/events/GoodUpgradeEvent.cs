using SoomlaWpStore.domain.virtualGoods;
using SoomlaWpCore.events;

namespace SoomlaWpStore.events
{
    public class GoodUpgradeEvent : SoomlaEvent
    {
        private VirtualGood mGood;
        private UpgradeVG mUpgradeVG;

        public VirtualGood GetGood()
        {
            return mGood;
        }
        public UpgradeVG GetUpgradeVG()
        {
            return mUpgradeVG;
        }

        public GoodUpgradeEvent(VirtualGood good, UpgradeVG upgradeVG)
            : this(good, upgradeVG, null)
        {
        }

        public GoodUpgradeEvent(VirtualGood good, UpgradeVG upgradeVG, object sender)
            : base(sender)
        {
            mGood = good;
            mUpgradeVG = upgradeVG;
        }
    }
}
