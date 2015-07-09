package com.soomla.unity;


import com.soomla.BusProvider;
import com.soomla.SoomlaUtils;
import com.soomla.data.JSONConsts;
import com.soomla.store.domain.MarketItem;
import com.soomla.store.events.BillingNotSupportedEvent;
import com.soomla.store.events.BillingSupportedEvent;
import com.soomla.store.events.CurrencyBalanceChangedEvent;
import com.soomla.store.events.GoodBalanceChangedEvent;
import com.soomla.store.events.GoodEquippedEvent;
import com.soomla.store.events.GoodUnEquippedEvent;
import com.soomla.store.events.GoodUpgradeEvent;
import com.soomla.store.events.IabServiceStartedEvent;
import com.soomla.store.events.IabServiceStoppedEvent;
import com.soomla.store.events.ItemPurchaseStartedEvent;
import com.soomla.store.events.ItemPurchasedEvent;
import com.soomla.store.events.MarketItemsRefreshFailedEvent;
import com.soomla.store.events.MarketItemsRefreshFinishedEvent;
import com.soomla.store.events.MarketItemsRefreshStartedEvent;
import com.soomla.store.events.MarketPurchaseCancelledEvent;
import com.soomla.store.events.MarketPurchaseEvent;
import com.soomla.store.events.MarketPurchaseStartedEvent;
import com.soomla.store.events.MarketRefundEvent;
import com.soomla.store.events.RestoreTransactionsFinishedEvent;
import com.soomla.store.events.RestoreTransactionsStartedEvent;
import com.soomla.store.events.SoomlaStoreInitializedEvent;
import com.soomla.store.events.UnexpectedStoreErrorEvent;
import com.squareup.otto.Subscribe;
import com.unity3d.player.UnityPlayer;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class StoreEventHandler {
    private static StoreEventHandler mLocalEventHandler;
    private static String TAG = "SOOMLA Unity StoreEventHandler";

    public static void initialize() {
        SoomlaUtils.LogDebug("SOOMLA Unity StoreEventHandler", "Initializing StoreEventHandler ...");
        getInstance();
    }

    public static StoreEventHandler getInstance() {
        if (mLocalEventHandler == null) {
            mLocalEventHandler = new StoreEventHandler();
        }
        return mLocalEventHandler;
    }

    public StoreEventHandler() {
        BusProvider.getInstance().register(this);
    }

    @Subscribe
    public void onBillingSupported(BillingSupportedEvent billingSupportedEvent) {
        if (billingSupportedEvent.Sender == this) {
            return;
        }
        UnityPlayer.UnitySendMessage("StoreEvents", "onBillingSupported", "");
    }

    @Subscribe
    public void onBillingNotSupported(BillingNotSupportedEvent billingNotSupportedEvent) {
        if (billingNotSupportedEvent.Sender == this) {
            return;
        }
        UnityPlayer.UnitySendMessage("StoreEvents", "onBillingNotSupported", "");
    }

    @Subscribe
    public void onIabServiceStarted(IabServiceStartedEvent iabServiceStartedEvent) {
        if (iabServiceStartedEvent.Sender == this) {
            return;
        }
        UnityPlayer.UnitySendMessage("StoreEvents", "onIabServiceStarted", "");
    }

    @Subscribe
    public void onIabServiceStopped(IabServiceStoppedEvent iabServiceStoppedEvent) {
        if (iabServiceStoppedEvent.Sender == this) {
            return;
        }
        UnityPlayer.UnitySendMessage("StoreEvents", "onIabServiceStopped", "");
    }

    @Subscribe
    public void onCurrencyBalanceChanged(CurrencyBalanceChangedEvent currencyBalanceChangedEvent) {
        if (currencyBalanceChangedEvent.Sender == this) {
            return;
        }
        try {
            JSONObject eventJSON = new JSONObject();
            eventJSON.put("itemId", currencyBalanceChangedEvent.getCurrencyItemId());
            eventJSON.put("balance", currencyBalanceChangedEvent.getBalance());
            eventJSON.put("amountAdded", currencyBalanceChangedEvent.getAmountAdded());

            UnityPlayer.UnitySendMessage("StoreEvents", "onCurrencyBalanceChanged", eventJSON.toString());
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "This is BAD! couldn't create JSON for onCurrencyBalanceChanged event.");
        }
    }

    @Subscribe
    public void onGoodBalanceChanged(GoodBalanceChangedEvent goodBalanceChangedEvent) {
        if (goodBalanceChangedEvent.Sender == this) {
            return;
        }
        try {
            JSONObject eventJSON = new JSONObject();
            eventJSON.put("itemId", goodBalanceChangedEvent.getGoodItemId());
            eventJSON.put("balance", goodBalanceChangedEvent.getBalance());
            eventJSON.put("amountAdded", goodBalanceChangedEvent.getAmountAdded());

            UnityPlayer.UnitySendMessage("StoreEvents", "onGoodBalanceChanged", eventJSON.toString());
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "This is BAD! couldn't create JSON for onGoodBalanceChanged event.");
        }
    }

    @Subscribe
    public void onGoodEquipped(GoodEquippedEvent goodEquippedEvent) {
        if (goodEquippedEvent.Sender == this) {
            return;
        }
        try {
            JSONObject eventJSON = new JSONObject();
            eventJSON.put("itemId", goodEquippedEvent.getGoodItemId());

            UnityPlayer.UnitySendMessage("StoreEvents", "onGoodEquipped", eventJSON.toString());
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "This is BAD! couldn't create JSON for onGoodEquipped event.");
        }
    }

    @Subscribe
    public void onGoodUnequipped(GoodUnEquippedEvent goodUnEquippedEvent) {
        if (goodUnEquippedEvent.Sender == this) {
            return;
        }
        try {
            JSONObject eventJSON = new JSONObject();
            eventJSON.put("itemId", goodUnEquippedEvent.getGoodItemId());

            UnityPlayer.UnitySendMessage("StoreEvents", "onGoodUnequipped", eventJSON.toString());
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "This is BAD! couldn't create JSON for onGoodUnequipped event.");
        }
    }

    @Subscribe
    public void onGoodUpgrade(GoodUpgradeEvent goodUpgradeEvent) {
        if (goodUpgradeEvent.Sender == this) {
            return;
        }
        try {
            JSONObject eventJSON = new JSONObject();
            eventJSON.put("itemId", goodUpgradeEvent.getGoodItemId());
            eventJSON.put("upgradeItemId", goodUpgradeEvent.getCurrentUpgrade());

            UnityPlayer.UnitySendMessage("StoreEvents", "onGoodUpgrade", eventJSON.toString());
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "This is BAD! couldn't create JSON for onGoodUpgrade event.");
        }
    }

    @Subscribe
    public void onItemPurchased(ItemPurchasedEvent itemPurchasedEvent) {
        if (itemPurchasedEvent.Sender == this) {
            return;
        }
        try {
            JSONObject eventJSON = new JSONObject();
            eventJSON.put("itemId", itemPurchasedEvent.getItemId());
            eventJSON.put("payload", itemPurchasedEvent.getPayload());

            UnityPlayer.UnitySendMessage("StoreEvents", "onItemPurchased", eventJSON.toString());
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "This is BAD! couldn't create JSON for onItemPurchased event.");
        }
    }

    @Subscribe
    public void onItemPurchaseStarted(ItemPurchaseStartedEvent itemPurchaseStartedEvent) {
        if (itemPurchaseStartedEvent.Sender == this) {
            return;
        }
        try {
            JSONObject eventJSON = new JSONObject();
            eventJSON.put("itemId", itemPurchaseStartedEvent.getItemId());

            UnityPlayer.UnitySendMessage("StoreEvents", "onItemPurchaseStarted", eventJSON.toString());
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "This is BAD! couldn't create JSON for onItemPurchaseStarted event.");
        }
    }

    @Subscribe
    public void onMarketPurchaseCancelled(MarketPurchaseCancelledEvent playPurchaseCancelledEvent) {
        if (playPurchaseCancelledEvent.Sender == this) {
            return;
        }
        try {
            JSONObject eventJSON = new JSONObject();
            eventJSON.put("itemId", playPurchaseCancelledEvent.getPurchasableVirtualItem().getItemId());

            UnityPlayer.UnitySendMessage("StoreEvents", "onMarketPurchaseCancelled", eventJSON.toString());
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "This is BAD! couldn't create JSON for onMarketPurchaseCancelled event.");
        }
    }

    @Subscribe
    public void onMarketPurchase(MarketPurchaseEvent playPurchaseEvent) {
        if (playPurchaseEvent.Sender == this) {
            return;
        }
        try {
            JSONObject eventJSON = new JSONObject();
            eventJSON.put("itemId", playPurchaseEvent.PurchasableVirtualItem.getItemId());
            eventJSON.put("payload", playPurchaseEvent.Payload);
            JSONObject extraJSON = new JSONObject();
            for(String key : playPurchaseEvent.ExtraInfo.keySet()) {
                extraJSON.put(key, playPurchaseEvent.ExtraInfo.get(key));
            }
            eventJSON.put("extra", extraJSON);

            UnityPlayer.UnitySendMessage("StoreEvents", "onMarketPurchase", eventJSON.toString());
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "This is BAD! couldn't create JSON for onMarketPurchase event.");
        }
    }

    @Subscribe
    public void onMarketPurchaseStarted(MarketPurchaseStartedEvent playPurchaseStartedEvent) {
        if (playPurchaseStartedEvent.Sender == this) {
            return;
        }
        try {
            JSONObject eventJSON = new JSONObject();
            eventJSON.put("itemId", playPurchaseStartedEvent.getPurchasableVirtualItem().getItemId());

            UnityPlayer.UnitySendMessage("StoreEvents", "onMarketPurchaseStarted", eventJSON.toString());
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "This is BAD! couldn't create JSON for onMarketPurchaseStarted event.");
        }
    }

    @Subscribe
    public void onMarketRefund(MarketRefundEvent playRefundEvent) {
        if (playRefundEvent.Sender == this) {
            return;
        }
        try {
            JSONObject eventJSON = new JSONObject();
            eventJSON.put("itemId", playRefundEvent.getPurchasableVirtualItem().getItemId());

            UnityPlayer.UnitySendMessage("StoreEvents", "onMarketRefund", eventJSON.toString());
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "This is BAD! couldn't create JSON for onMarketRefund event.");
        }
    }

    @Subscribe
    public void onRestoreTransactionsFinished(RestoreTransactionsFinishedEvent restoreTransactionsFinishedEvent) {
        if (restoreTransactionsFinishedEvent.Sender == this) {
            return;
        }
        try {
            JSONObject eventJSON = new JSONObject();
            eventJSON.put("success", restoreTransactionsFinishedEvent.isSuccess());

            UnityPlayer.UnitySendMessage("StoreEvents", "onRestoreTransactionsFinished", eventJSON.toString());
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "This is BAD! couldn't create JSON for onRestoreTransactionsFinished event.");
        }
    }

    @Subscribe
    public void onRestoreTransactionsStarted(RestoreTransactionsStartedEvent restoreTransactionsStartedEvent) {
        if (restoreTransactionsStartedEvent.Sender == this) {
            return;
        }
        UnityPlayer.UnitySendMessage("StoreEvents", "onRestoreTransactionsStarted", "");
    }

    @Subscribe
    public void onMarketItemsRefreshStarted(MarketItemsRefreshStartedEvent marketItemsRefreshStartedEvent) {
        if (marketItemsRefreshStartedEvent.Sender == this) {
            return;
        }
        UnityPlayer.UnitySendMessage("StoreEvents", "onMarketItemsRefreshStarted", "");
    }

    @Subscribe
    public void onMarketItemsRefreshFinished(MarketItemsRefreshFinishedEvent marketItemsRefreshFinishedEvent) {
        if (marketItemsRefreshFinishedEvent.Sender == this) {
            return;
        }
        try {
            JSONArray eventJSON = new JSONArray();

            for (MarketItem mi : marketItemsRefreshFinishedEvent.getMarketItems()) {
                JSONObject micJSON = new JSONObject();
                micJSON.put(JSONConsts.SOOM_CLASSNAME, SoomlaUtils.getClassName(mi));
                micJSON.put("productId", mi.getProductId());
                micJSON.put("marketPrice", mi.getMarketPriceAndCurrency());
                micJSON.put("marketTitle", mi.getMarketTitle());
                micJSON.put("marketDesc", mi.getMarketDescription());
                micJSON.put("marketCurrencyCode", mi.getMarketCurrencyCode());
                micJSON.put("marketPriceMicros", mi.getMarketPriceMicros());

                eventJSON.put(micJSON);
            }

            UnityPlayer.UnitySendMessage("StoreEvents", "onMarketItemsRefreshFinished", eventJSON.toString());
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "This is BAD! couldn't create JSON for onMarketItemsRefreshFinished event.");
        }
    }

    @Subscribe
    public void onMarketItemsRefreshFailed(MarketItemsRefreshFailedEvent marketItemsRefreshFailedEvent) {
        if (marketItemsRefreshFailedEvent.Sender == this) {
            return;
        }

        try {
            JSONObject eventJSON = new JSONObject();
            eventJSON.put("errorMessage", marketItemsRefreshFailedEvent.ErrorMessage);

            UnityPlayer.UnitySendMessage("StoreEvents", "onMarketItemsRefreshFailed", eventJSON.toString());
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "This is BAD! couldn't create JSON for onMarketItemsRefreshFailed event.");
        }
    }

    @Subscribe
    public void onSoomlaStoreInitialized(SoomlaStoreInitializedEvent soomlaStoreInitializedEvent) {
        if (soomlaStoreInitializedEvent.Sender == this) {
            return;
        }
        UnityPlayer.UnitySendMessage("StoreEvents", "onSoomlaStoreInitialized", "");
    }

    @Subscribe
    public void onUnexpectedStoreError(UnexpectedStoreErrorEvent unexpectedStoreErrorEvent) {
        if (unexpectedStoreErrorEvent.Sender == this) {
            return;
        }

        try {
            JSONObject eventJSON = new JSONObject();
            eventJSON.put("errorCode", unexpectedStoreErrorEvent.getErrorCode().ordinal());

            UnityPlayer.UnitySendMessage("StoreEvents", "onUnexpectedStoreError", eventJSON.toString());
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "This is BAD! couldn't create JSON for onMarketItemsRefreshFailed event.");
        }
    }



    /** pushing events **/

    public void pushEventSoomlaStoreInitialized(String message) {
        BusProvider.getInstance().post(new SoomlaStoreInitializedEvent(this));
    }

    public void pushEventUnexpectedStoreError(String message) {
        try {
            JSONObject eventJSON = new JSONObject(message);

            BusProvider.getInstance().post(new UnexpectedStoreErrorEvent(UnexpectedStoreErrorEvent.ErrorCode.values()[eventJSON.getInt("errorCode")], this));
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "(when pushing event) This is BAD! couldn't create JSON for UnexpectedStoreErrorEvent event.");
        }
    }

    public void pushEventCurrencyBalanceChanged(String message) {
        try {
            JSONObject eventJSON = new JSONObject(message);

            int balance = eventJSON.getInt("balance");
            int amountAdded = eventJSON.getInt("amountAdded");

            BusProvider.getInstance().post(new CurrencyBalanceChangedEvent(eventJSON.getString("itemId"), balance, amountAdded, this));
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "(when pushing event) This is BAD! couldn't create JSON for onGoodBalanceChanged event.");
        }
    }

    public void pushEventGoodBalanceChanged(String message) {
        try {
            JSONObject eventJSON = new JSONObject(message);

            int balance = eventJSON.getInt("balance");
            int amountAdded = eventJSON.getInt("amountAdded");
            BusProvider.getInstance().post(new GoodBalanceChangedEvent(eventJSON.getString("itemId"), balance, amountAdded, this));
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "(when pushing event) This is BAD! couldn't create JSON for onGoodBalanceChanged event.");
        }
    }

    public void pushEventGoodEquipped(String message) {
        try {
            JSONObject eventJSON = new JSONObject(message);

            BusProvider.getInstance().post(new GoodEquippedEvent(eventJSON.getString("itemId"), this));
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "(when pushing event) This is BAD! couldn't create JSON for onGoodEquipped event.");
        }
    }

    public void pushEventGoodUnequipped(String message) {
        try {
            JSONObject eventJSON = new JSONObject(message);

            BusProvider.getInstance().post(new GoodUnEquippedEvent(eventJSON.getString("itemId"), this));
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "(when pushing event) This is BAD! couldn't create JSON for onGoodUnequipped event.");
        }
    }

    public void pushEventGoodUpgrade(String message) {
        try {
            JSONObject eventJSON = new JSONObject(message);

            BusProvider.getInstance().post(new GoodUpgradeEvent(eventJSON.getString("itemId"), eventJSON.getString("upgradeItemId"), this));
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "(when pushing event) This is BAD! couldn't create JSON for onGoodUpgrade event.");
        }
    }

    public void pushEventItemPurchased(String message) {
        try {
            JSONObject eventJSON = new JSONObject(message);

            BusProvider.getInstance().post(new ItemPurchasedEvent(eventJSON.getString("itemId"), eventJSON.getString("payload"), this));
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "(when pushing event) This is BAD! couldn't create JSON for onItemPurchased event.");
        }
    }

    public void pushEventItemPurchaseStarted(String message) {
        try {
            JSONObject eventJSON = new JSONObject(message);

            BusProvider.getInstance().post(new ItemPurchaseStartedEvent(eventJSON.getString("itemId"), this));
        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "(when pushing event) This is BAD! couldn't create JSON for onItemPurchaseStarted event.");
        }
    }

}
