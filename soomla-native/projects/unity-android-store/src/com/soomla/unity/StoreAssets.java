package com.soomla.unity;

import java.util.ArrayList;

import com.soomla.store.IStoreAssets;
import com.soomla.SoomlaApp;
import com.soomla.SoomlaUtils;
import com.soomla.store.data.StoreInfo;
import com.soomla.store.data.StoreJSONConsts;
import com.soomla.store.domain.NonConsumableItem;
import com.soomla.store.domain.VirtualCategory;
import com.soomla.store.domain.virtualCurrencies.VirtualCurrency;
import com.soomla.store.domain.virtualCurrencies.VirtualCurrencyPack;
import com.soomla.store.domain.virtualGoods.*;
import com.unity3d.player.UnityPlayer;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class StoreAssets implements IStoreAssets {

    public static ArrayList<VirtualCurrency> currencies = new ArrayList<VirtualCurrency>();
    public static ArrayList<VirtualGood> goods = new ArrayList<VirtualGood>();
    public static ArrayList<VirtualCurrencyPack> currencyPacks = new ArrayList<VirtualCurrencyPack>();
    public static ArrayList<VirtualCategory> categories = new ArrayList<VirtualCategory>();
    public static ArrayList<NonConsumableItem> nonConsumables = new ArrayList<NonConsumableItem>();
    public static int version = 0;

    public static void prepare(int oVersion, String storeAssetsJSON) {
        SoomlaApp.setExternalContext(UnityPlayer.currentActivity);

        SoomlaUtils.LogDebug(TAG, "the storeAssets json is: " + storeAssetsJSON);

        try {
            version = oVersion;

            JSONObject jsonObject = new JSONObject(storeAssetsJSON);

            JSONArray virtualCurrencies = jsonObject.getJSONArray(StoreJSONConsts.STORE_CURRENCIES);
            currencies = new ArrayList<VirtualCurrency>();
            for (int i=0; i<virtualCurrencies.length(); i++){
                JSONObject o = virtualCurrencies.getJSONObject(i);
                VirtualCurrency c = new VirtualCurrency(o);
                currencies.add(c);
            }

            JSONArray currencyPacks = jsonObject.getJSONArray(StoreJSONConsts.STORE_CURRENCYPACKS);
            StoreAssets.currencyPacks = new ArrayList<VirtualCurrencyPack>();
            for (int i=0; i<currencyPacks.length(); i++){
                JSONObject o = currencyPacks.getJSONObject(i);
                VirtualCurrencyPack pack = new VirtualCurrencyPack(o);
                StoreAssets.currencyPacks.add(pack);
            }

            // The order in which VirtualGoods are created matters!
            // For example: VGU and VGP depend on other VGs
            JSONObject virtualGoods = jsonObject.getJSONObject(StoreJSONConsts.STORE_GOODS);
            JSONArray suGoods = virtualGoods.getJSONArray(StoreJSONConsts.STORE_GOODS_SU);
            JSONArray ltGoods = virtualGoods.getJSONArray(StoreJSONConsts.STORE_GOODS_LT);
            JSONArray eqGoods = virtualGoods.getJSONArray(StoreJSONConsts.STORE_GOODS_EQ);
            JSONArray upGoods = virtualGoods.getJSONArray(StoreJSONConsts.STORE_GOODS_UP);
            JSONArray paGoods = virtualGoods.getJSONArray(StoreJSONConsts.STORE_GOODS_PA);
            goods = new ArrayList<VirtualGood>();
            for (int i=0; i<suGoods.length(); i++){
                JSONObject o = suGoods.getJSONObject(i);
                SingleUseVG g = new SingleUseVG(o);
                goods.add(g);
            }
            for (int i=0; i<ltGoods.length(); i++){
                JSONObject o = ltGoods.getJSONObject(i);
                LifetimeVG g = new LifetimeVG(o);
                goods.add(g);
            }
            for (int i=0; i<eqGoods.length(); i++){
                JSONObject o = eqGoods.getJSONObject(i);
                EquippableVG g = new EquippableVG(o);
                goods.add(g);
            }
            for (int i=0; i<paGoods.length(); i++){
                JSONObject o = paGoods.getJSONObject(i);
                SingleUsePackVG g = new SingleUsePackVG(o);
                goods.add(g);
            }
            for (int i=0; i<upGoods.length(); i++){
                JSONObject o = upGoods.getJSONObject(i);
                UpgradeVG g = new UpgradeVG(o);
                goods.add(g);
            }

            // categories depend on virtual goods. That's why the have to be initialized after!
            JSONArray virtualCategories = jsonObject.getJSONArray(StoreJSONConsts.STORE_CATEGORIES);
            categories = new ArrayList<VirtualCategory>();
            for(int i=0; i<virtualCategories.length(); i++){
                JSONObject o = virtualCategories.getJSONObject(i);
                VirtualCategory category = new VirtualCategory(o);
                categories.add(category);
            }

            JSONArray nonConsumables = jsonObject.getJSONArray(StoreJSONConsts.STORE_NONCONSUMABLES);
            StoreAssets.nonConsumables = new ArrayList<NonConsumableItem>();
            for (int i=0; i<nonConsumables.length(); i++){
                JSONObject o = nonConsumables.getJSONObject(i);
                NonConsumableItem non = new NonConsumableItem(o);
                StoreAssets.nonConsumables.add(non);
            }

        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "Couldn't parse storeAssetsJSON (unity)");
        } catch (Exception ex) {
            SoomlaUtils.LogError(TAG, "An error occurred while trying to prepare storeAssets (unity) " + ex.getMessage());
        }
    }

    public static void save(String type, String itemJSON) {
        try {
            JSONObject jsonObject = new JSONObject(itemJSON);


            if (type.equals("EquippableVG")){
                StoreInfo.save(new EquippableVG(jsonObject));

            } else if (type.equals("LifetimeVG")){
                StoreInfo.save(new LifetimeVG(jsonObject));

            } else if (type.equals("SingleUsePackVG")){
                StoreInfo.save(new SingleUsePackVG(jsonObject));

            } else if (type.equals("SingleUseVG")){
                StoreInfo.save(new SingleUseVG(jsonObject));

            } else if (type.equals("UpgradeVG")){
                StoreInfo.save(new UpgradeVG(jsonObject));

            } else if (type.equals("VirtualCurrency")){
                StoreInfo.save(new VirtualCurrency(jsonObject));

            } else if (type.equals("VirtualCurrencyPack")){
                StoreInfo.save(new VirtualCurrencyPack(jsonObject));

            } else if (type.equals("NonConsumableItem")){
                StoreInfo.save(new NonConsumableItem(jsonObject));
            } else {
                SoomlaUtils.LogError(TAG, "Don't understand what's the type of the item i need to save... type: " + type);
            }

        } catch (JSONException e) {
            SoomlaUtils.LogError(TAG, "There was an error parsing item JSON in order to save.");
        }
    }

    @Override
    public int getVersion() {
        return version;
    }

    @Override
    public VirtualCurrency[] getCurrencies() {
        return currencies.toArray(new VirtualCurrency[currencies.size()]);
    }

    @Override
    public VirtualGood[] getGoods() {
        return goods.toArray(new VirtualGood[goods.size()]);
    }

    @Override
    public VirtualCurrencyPack[] getCurrencyPacks() {
        return currencyPacks.toArray(new VirtualCurrencyPack[currencyPacks.size()]);
    }

    @Override
    public VirtualCategory[] getCategories() {
        return categories.toArray(new VirtualCategory[categories.size()]);
    }

    @Override
    public NonConsumableItem[] getNonConsumableItems() {
        return nonConsumables.toArray(new NonConsumableItem[nonConsumables.size()]);
    }

    private static String TAG = "SOOMLA StoreAssets (unity)";
}
