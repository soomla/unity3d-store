using SoomlaWpStore.domain.virtualGoods;
using SoomlaWpCore.events;

namespace SoomlaWpStore.events
{
    public class GoodBalanceChangedEvent : SoomlaEvent
    {
        private VirtualGood mGood;
        private int mBalance;
        private int mAmountAdded;

        public VirtualGood GetGood()
        {
            return mGood;
        }
        public int GetBalance()
        {
            return mBalance;
        }
        public int GetAmountAdded()
        {
            return mAmountAdded;
        }

        public GoodBalanceChangedEvent(VirtualGood good, int balance, int amountAdded)
            : this(good, balance, amountAdded, null)
        {
        }

        public GoodBalanceChangedEvent(VirtualGood good, int balance, int amountAdded, object sender)
            : base(sender)
        {
            mGood = good;
            mBalance = balance;
            mAmountAdded = amountAdded;
        }
    }
}
