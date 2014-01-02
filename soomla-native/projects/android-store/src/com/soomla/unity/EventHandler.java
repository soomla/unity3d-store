package com.soomla.unity;

import com.soomla.store.BusProvider;
import com.soomla.store.SoomlaApp;
import com.soomla.store.events.*;
import com.squareup.otto.Subscribe;

public class EventHandler {
    private static EventHandler mLocalEventHandler;
    private SoomlaAppUnderUnity soomla;
    
    public static void initialize() {
        mLocalEventHandler = new EventHandler();
        BusProvider.getInstance().register(mLocalEventHandler);
    }
    
    public EventHandler() {
        soomla = (SoomlaAppUnderUnity)SoomlaApp.instance();
    }

    @Subscribe
    public void onBillingSupported(BillingSupportedEvent billingSupportedEvent) {
    	soomla.UnitySendMessage("onBillingSupported", "");
    }

    @Subscribe
    public void onBillingNotSupported(BillingNotSupportedEvent billingNotSupportedEvent) {
    	soomla.UnitySendMessage("onBillingNotSupported", "");
    }

    @Subscribe
    public void onClosingStore(ClosingStoreEvent closingStoreEvent) {
    	soomla.UnitySendMessage("onClosingStore", "");
    }

    @Subscribe
    public void onCurrencyBalanceChanged(CurrencyBalanceChangedEvent currencyBalanceChangedEvent) {
    	soomla.UnitySendMessage("onCurrencyBalanceChanged",
                currencyBalanceChangedEvent.getCurrency().getItemId() + "#SOOM#" +
                currencyBalanceChangedEvent.getBalance() + "#SOOM#" +
                currencyBalanceChangedEvent.getAmountAdded());
    }

    @Subscribe
    public void onGoodBalanceChanged(GoodBalanceChangedEvent goodBalanceChangedEvent) {
    	soomla.UnitySendMessage("onGoodBalanceChanged",
                goodBalanceChangedEvent.getGood().getItemId() + "#SOOM#" +
                        goodBalanceChangedEvent.getBalance() + "#SOOM#" +
                        goodBalanceChangedEvent.getAmountAdded());
    }

    @Subscribe
    public void onGoodEquipped(GoodEquippedEvent goodEquippedEvent) {
    	soomla.UnitySendMessage("onGoodEquipped", goodEquippedEvent.getGood().getItemId());
    }

    @Subscribe
    public void onGoodUnequipped(GoodUnEquippedEvent goodUnEquippedEvent) {
    	soomla.UnitySendMessage("onGoodUnequipped", goodUnEquippedEvent.getGood().getItemId());
    }

    @Subscribe
    public void onGoodUpgrade(GoodUpgradeEvent goodUpgradeEvent) {
    	soomla.UnitySendMessage("onGoodUpgrade",
                goodUpgradeEvent.getGood().getItemId()  + "#SOOM#" +
                goodUpgradeEvent.getCurrentUpgrade().getItemId());
    }

    @Subscribe
    public void onItemPurchased(ItemPurchasedEvent itemPurchasedEvent) {
    	soomla.UnitySendMessage("onItemPurchased", itemPurchasedEvent.getPurchasableVirtualItem().getItemId());
    }

    @Subscribe
    public void onItemPurchaseStarted(ItemPurchaseStartedEvent itemPurchaseStartedEvent) {
    	soomla.UnitySendMessage("onItemPurchaseStarted", itemPurchaseStartedEvent.getPurchasableVirtualItem().getItemId());
    }

    @Subscribe
    public void onOpeningStore(OpeningStoreEvent openingStoreEvent) {
    	soomla.UnitySendMessage("onOpeningStore", "");
    }

    @Subscribe
    public void onMarketPurchaseCancelled(PlayPurchaseCancelledEvent playPurchaseCancelledEvent) {
    	soomla.UnitySendMessage("onMarketPurchaseCancelled",
                playPurchaseCancelledEvent.getPurchasableVirtualItem().getItemId());
    }

    @Subscribe
    public void onMarketPurchase(PlayPurchaseEvent playPurchaseEvent) {
    	soomla.UnitySendMessage("onMarketPurchase", playPurchaseEvent.getPurchasableVirtualItem().getItemId());
    }

    @Subscribe
    public void onMarketPurchaseStarted(PlayPurchaseStartedEvent playPurchaseStartedEvent) {
    	soomla.UnitySendMessage("onMarketPurchaseStarted", playPurchaseStartedEvent.getPurchasableVirtualItem().getItemId());
    }

    @Subscribe
    public void onMarketRefund(PlayRefundEvent playRefundEvent) {
    	soomla.UnitySendMessage("onMarketRefund", playRefundEvent.getPurchasableVirtualItem().getItemId());
    }

    @Subscribe
    public void onRestoreTransactions(RestoreTransactionsEvent restoreTransactionsEvent) {
    	soomla.UnitySendMessage("onRestoreTransactions", (restoreTransactionsEvent.isSuccess() ? 1 : 0) + "");
    }

    @Subscribe
    public void onRestoreTransactionsStarted(RestoreTransactionsStartedEvent restoreTransactionsStartedEvent) {
    	soomla.UnitySendMessage("onRestoreTransactionsStarted", "");
    }

    @Subscribe
    public void onStoreControllerInitializedEvent(StoreControllerInitializedEvent storeControllerInitializedEvent) {
    	soomla.UnitySendMessage("onStoreControllerInitialized", "");
    }

    @Subscribe
    public void onUnexpectedStoreError(UnexpectedStoreErrorEvent unexpectedStoreErrorEvent) {
    	soomla.UnitySendMessage("onUnexpectedErrorInStore", unexpectedStoreErrorEvent.getMessage());
    }

}
