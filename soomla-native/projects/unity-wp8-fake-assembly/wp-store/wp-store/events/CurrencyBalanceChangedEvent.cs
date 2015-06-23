using SoomlaWpStore.domain.virtualCurrencies;
using SoomlaWpCore.events;

namespace SoomlaWpStore.events
{
    public class CurrencyBalanceChangedEvent : SoomlaEvent
    {
        private VirtualCurrency mCurrency;
        private int mBalance;
        private int mAmountAdded;

        public VirtualCurrency GetCurrency()
        {
            return mCurrency;
        }
        public int GetBalance()
        {
            return mBalance;
        }
        public int GetAmountAdded()
        {
            return mAmountAdded;
        }

        public CurrencyBalanceChangedEvent(VirtualCurrency good, int balance, int amountAdded)
            : this(good, balance, amountAdded, null)
        {
        }

        public CurrencyBalanceChangedEvent(VirtualCurrency good, int balance, int amountAdded, object sender)
            : base(sender)
        {
            mCurrency = good;
            mBalance = balance;
            mAmountAdded = amountAdded;
        }
    }
}
