package com.soomla.unity;

import com.soomla.store.BusProvider;
import com.soomla.store.events.*;
import com.squareup.otto.Subscribe;
import com.unity3d.player.UnityPlayer;

public class EventHandler {
    private static EventHandler mLocalEventHandler;

    public static void initialize() {
        mLocalEventHandler = new EventHandler();

    }

    public EventHandler() {
        BusProvider.getInstance().register(this);
    }

    @Subscribe
    public void onBillingSupported(BillingSupportedEvent billingSupportedEvent) {
        UnityPlayer.UnitySendMessage("StoreEvents", "onBillingSupported", "");
    }

    @Subscribe
    public void onBillingNotSupported(BillingNotSupportedEvent billingNotSupportedEvent) {
        UnityPlayer.UnitySendMessage("StoreEvents", "onBillingNotSupported", "");
    }

    @Subscribe
    public void onIabServiceStartedEvent(IabServiceStartedEvent iabServiceStartedEvent) {
        UnityPlayer.UnitySendMessage("StoreEvents", "onIabServiceStarted", "");
    }

    @Subscribe
    public void onIabServiceStoppedEvent(IabServiceStoppedEvent iabServiceStoppedEvent) {
        UnityPlayer.UnitySendMessage("StoreEvents", "onIabServiceStopped", "");
    }

    @Subscribe
    public void onCurrencyBalanceChanged(CurrencyBalanceChangedEvent currencyBalanceChangedEvent) {
        UnityPlayer.UnitySendMessage("StoreEvents", "onCurrencyBalanceChanged",
                currencyBalanceChangedEvent.getCurrency().getItemId() + "#SOOM#" +
                currencyBalanceChangedEvent.getBalance() + "#SOOM#" +
                currencyBalanceChangedEvent.getAmountAdded());
    }

    @Subscribe
    public void onGoodBalanceChanged(GoodBalanceChangedEvent goodBalanceChangedEvent) {
        UnityPlayer.UnitySendMessage("StoreEvents", "onGoodBalanceChanged",
                goodBalanceChangedEvent.getGood().getItemId() + "#SOOM#" +
                        goodBalanceChangedEvent.getBalance() + "#SOOM#" +
                        goodBalanceChangedEvent.getAmountAdded());
    }

    @Subscribe
    public void onGoodEquipped(GoodEquippedEvent goodEquippedEvent) {
        UnityPlayer.UnitySendMessage("StoreEvents", "onGoodEquipped", goodEquippedEvent.getGood().getItemId());
    }

    @Subscribe
    public void onGoodUnequipped(GoodUnEquippedEvent goodUnEquippedEvent) {
        UnityPlayer.UnitySendMessage("StoreEvents", "onGoodUnequipped", goodUnEquippedEvent.getGood().getItemId());
    }

    @Subscribe
    public void onGoodUpgrade(GoodUpgradeEvent goodUpgradeEvent) {
        UnityPlayer.UnitySendMessage("StoreEvents", "onGoodUpgrade",
                goodUpgradeEvent.getGood().getItemId()  + "#SOOM#" +
                goodUpgradeEvent.getCurrentUpgrade().getItemId());
    }

    @Subscribe
    public void onItemPurchased(ItemPurchasedEvent itemPurchasedEvent) {
        UnityPlayer.UnitySendMessage("StoreEvents", "onItemPurchased", itemPurchasedEvent.getPurchasableVirtualItem().getItemId());
    }

    @Subscribe
    public void onItemPurchaseStarted(ItemPurchaseStartedEvent itemPurchaseStartedEvent) {
        UnityPlayer.UnitySendMessage("StoreEvents", "onItemPurchaseStarted", itemPurchaseStartedEvent.getPurchasableVirtualItem().getItemId());
    }

    @Subscribe
    public void onMarketPurchaseCancelled(PlayPurchaseCancelledEvent playPurchaseCancelledEvent) {
        UnityPlayer.UnitySendMessage("StoreEvents", "onMarketPurchaseCancelled",
                playPurchaseCancelledEvent.getPurchasableVirtualItem().getItemId());
    }

    @Subscribe
    public void onMarketPurchase(PlayPurchaseEvent playPurchaseEvent) {
        UnityPlayer.UnitySendMessage("StoreEvents", "onMarketPurchase", playPurchaseEvent.getPurchasableVirtualItem().getItemId());
    }

    @Subscribe
    public void onMarketPurchaseStarted(PlayPurchaseStartedEvent playPurchaseStartedEvent) {
        UnityPlayer.UnitySendMessage("StoreEvents", "onMarketPurchaseStarted", playPurchaseStartedEvent.getPurchasableVirtualItem().getItemId());
    }

    @Subscribe
    public void onMarketRefund(PlayRefundEvent playRefundEvent) {
        UnityPlayer.UnitySendMessage("StoreEvents", "onMarketRefund", playRefundEvent.getPurchasableVirtualItem().getItemId());
    }

    @Subscribe
    public void onRestoreTransactions(RestoreTransactionsEvent restoreTransactionsEvent) {
        UnityPlayer.UnitySendMessage("StoreEvents", "onRestoreTransactions", (restoreTransactionsEvent.isSuccess() ? 1 : 0) + "");
    }

    @Subscribe
    public void onRestoreTransactionsStarted(RestoreTransactionsStartedEvent restoreTransactionsStartedEvent) {
        UnityPlayer.UnitySendMessage("StoreEvents", "onRestoreTransactionsStarted", "");
    }

    @Subscribe
    public void onStoreControllerInitializedEvent(StoreControllerInitializedEvent storeControllerInitializedEvent) {
        UnityPlayer.UnitySendMessage("StoreEvents", "onStoreControllerInitialized", "");
    }

    @Subscribe
    public void onUnexpectedStoreError(UnexpectedStoreErrorEvent unexpectedStoreErrorEvent) {
        String msg = unexpectedStoreErrorEvent.getMessage();
        UnityPlayer.UnitySendMessage("StoreEvents", "onUnexpectedErrorInStore", (msg == null ? "" : msg));
    }
    
    @Subscribe
    public void onItemDetailsRetrieved(ItemDetailsRetrievedEvent itemDetailsRetrievedEvent) {
    	
    	String msg = 	itemDetailsRetrievedEvent.getProductId() + "#SOOM#" + 
    					itemDetailsRetrievedEvent.getPrice() + "#SOOM#" +
    					itemDetailsRetrievedEvent.getTitle() + "#SOOM#" + 
    					itemDetailsRetrievedEvent.getDescription()+ "#SOOM#" + 
    					itemDetailsRetrievedEvent.isFinished();
    	
        UnityPlayer.UnitySendMessage("StoreEvents", "onItemDetailsRetrieved", msg);
    }
    
    @Subscribe
    public void onItemDetailsRetrievedFailed(ItemDetailsRetrievedFailedEvent itemDetailsRetrievedFailedEvent) {
        UnityPlayer.UnitySendMessage("StoreEvents", "onItemDetailsRetrievedFailed", "");
    }

}
