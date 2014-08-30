*This project is a part of The [SOOMLA](http://www.soom.la) Framework which is a series of open source initiatives with a joint goal to help mobile game developers do more together. SOOMLA encourages better game designing, economy modeling and faster development.*

Haven't you ever wanted an in-app purchase one liner that looks like this ?!

```cs
StoreInventory.BuyItem("[itemId]");
```

unity3d-store
---

*SOOMLA's Store Module for Unity3d*

**June 20th, 2014**: v1.5.2 presents some significant changes. [Getting Started](https://github.com/soomla/unity3d-store#getting-started) has changed! see [CHANGELOG](changelog.md).
> FYI - SOOMLA menu was moved and it's now under "Window -> Soomla"

**May 28th, 2014:** v1.5.0 is released. Supporting Amazon billing service. see [CHANGELOG](changelog.md).

**October 3rd, 2013:** iOS Server Side Verification is now implemented into unity3d-store. The server is a complimentary server provided by [SOOMLA](http://soom.la) to help you get your in-game purchases a bit more secured. This feature is not enabled by default. In order to enable Server Side verification go to the Soomla prefab and set  **ios Server Side Verification -> true**.

* More documentation and information in SOOMLA's [Knowledge Base](http://know.soom.la/docs/platforms/unity)  
* For issues you can use the [issues](https://github.com/soomla/unity3d-store/issues) section or SOOMLA's [Answers Website](http://answers.soom.la)

unity3d-store is the Unity3d flavor of SOOMLA's Store Module.

## Economy Model
![SOOMLA's Economy Model](http://know.soom.la/img/tutorial_img/soomla_diagrams/EconomyModel.png)


## Download

####Pre baked unitypackages:

[soomla-unity3d-core v1.5.3](https://raw.githubusercontent.com/soomla/unity3d-store/master/soomla-unity3d-core.unitypackage)  
[unity3d-store v1.5.3](http://bit.ly/1rc21Zo)  

## Debugging

If you want to see full debug messages from android-store and ios-store you just need to check the box that says "Debug Messages" in the SOOMLA Settings.
Unity debug messages will only be printed out if you build the project with _Development Build_ checked.

## Cloning

There are some necessary files in submodules lined with symbolic links. If you're cloning the project make sure you clone it with the `--recursive` flag.

```
$ git clone --recursive git@github.com:soomla/unity3d-store.git
```

## Getting Started

1. Download the [soomla-unity3d-core](https://raw.githubusercontent.com/soomla/unity3d-store/master/soomla-unity3d-core.unitypackage) and [unity3d-store](http://bit.ly/1rc21Zo) unitypackages and double-click on them (first 'Core' then 'Store'). It'll import all the necessary files into your project.
2. Drag the "StoreEvents" and "CoreEvents" Prefabs from `../Assets/Soomla/Prefabs` into your scene. You should see it listed in the "Hierarchy" panel. [This step MUST be done for unity3d-store to work properly]
3. On the menu bar click "Window -> Soomla -> Edit Settings" and change the value for "Soomla Secret" (also setup Public Key if you're building for Google Play):
    - _Soomla Secret_ - is an encryption secret you provide that will be used to secure your data. (If you used versions before v1.5.2 this secret MUST be the same as Custom Secret)  
    **Choose the secret wisely. You can't change them after you launch your game!**
    - _Public Key_ - is the public key given to you from Google. (iOS doesn't have a public key).
4. Create your own implementation of _IStoreAssets_ in order to describe your specific game's assets ([example](https://github.com/soomla/unity3d-store/blob/master/Soomla/Assets/Examples/MuffinRush/MuffinRushAssets.cs)). Initialize _SoomlaStore_ with the class you just created:

    ```cs
    SoomlaStore.Initialize(new YourStoreAssetsImplementation());
    ```

    > Initialize _SoomlaStore_ ONLY ONCE when your application loads.

    > Initialize _SoomlaStore_ in the "Start()" function of a 'MonoBehaviour' and **NOT** in the "Awake()" function. SOOMLA has its own 'MonoBehaviour' and it needs to be "Awakened" before you initialize.

5. You'll need an event handler in order to be notified about in-app purchasing related events. refer to the [Event Handling](https://github.com/soomla/unity3d-store#event-handling) section for more information.

And that's it ! You have storage and in-app purchasing capabilities... ALL-IN-ONE.

### Unity & Android

#### Starting IAB Service in background

If you have your own storefront implemented inside your game, it's recommended that you open the IAB Service in the background when the store opens and close it when the store is closed.

```cs
// Start Iab Service
SoomlaStore.StartIabServiceInBg();

// Stop Iab Service
SoomlaStore.StopIabServiceInBg();
```

Don't forget to close the Iab Service when your store is closed. You don't have to do this at all, this is just an optimization.


## What's next? In App Purchasing.

When we implemented modelV3, we were thinking about ways that people buy things inside apps. We figured out many ways you can let your users purchase stuff in your game and we designed the new modelV3 to support 2 of them: PurchaseWithMarket and PurchaseWithVirtualItem.

**PurchaseWithMarket** is a PurchaseType that allows users to purchase a VirtualItem with Google Play or the App Store.  
**PurchaseWithVirtualItem** is a PurchaseType that lets your users purchase a VirtualItem with a different VirtualItem. For Example: Buying 1 Sword with 100 Gems.

In order to define the way your various virtual items (Goods, Coins ...) are purchased, you'll need to create your implementation of IStoreAsset (the same one from step 4 in the "Getting Started" above).

Here is an example:

Lets say you have a _VirtualCurrencyPack_ you call `TEN_COINS_PACK` and a _VirtualCurrency_ you call `COIN_CURRENCY`:

```cs
VirtualCurrencyPack TEN_COINS_PACK = new VirtualCurrencyPack(
	            "10 Coins",                    // name
	            "A pack of 10 coins",      // description
	            "10_coins",                    // item id
				10,								// number of currencies in the pack
	            COIN_CURRENCY_ITEM_ID,         // the currency associated with this pack
	            new PurchaseWithMarket("com.soomla.ten_coin_pack", 1.99)
		);
```

Now you can use _StoreInventory_ to buy your new VirtualCurrencyPack:

```cs
StoreInventory.buyItem(TEN_COINS_PACK.ItemId);
```

And that's it! unity3d-store knows how to contact Google Play or the App Store for you and will redirect your users to their purchasing system to complete the transaction. Don't forget to subscribe to store events in order to get the notified of successful or failed purchases (see [Event Handling](https://github.com/soomla/unity3d-store#event-handling)).


Storage & Meta-Data
---

When you initialize _SoomlaStore_, it automatically initializes two other classes: _StoreInventory_ and _StoreInfo_:  
* _StoreInventory_ is a convenience class to let you perform operations on VirtualCurrencies and VirtualGoods. Use it to fetch/change the balances of VirtualItems in your game (using their ItemIds!)  
* _StoreInfo_ is where all meta data information about your specific game can be retrieved. It is initialized with your implementation of `IStoreAssets` and you can use it to retrieve information about your specific game.

**ATTENTION: because we're using JNI (Android) and DllImport (iOS) you should make as little calls as possible to _StoreInfo_. Look in the example project for the way we created a sort of a cache to hold your game's information in order to not make too many calls to _StoreInfo_. We update this cache using an event handler. (see [ExampleLocalStoreInfo](https://github.com/soomla/unity3d-store/blob/master/Soomla/Assets/Examples/MuffinRush/ExampleLocalStoreInfo.cs) and [ExampleEventHandler](https://github.com/soomla/unity3d-store/blob/master/Soomla/Assets/Examples/MuffinRush/ExampleEventHandler.cs)).**

The on-device storage is encrypted and kept in a SQLite database. SOOMLA is preparing a cloud-based storage service that will allow this SQLite to be synced to a cloud-based repository that you'll define.

**Example Usages**

* Get VirtualCurrency with itemId "currency_coin":

    ```cs
    VirtualCurrency coin = StoreInfo.GetVirtualCurrencyByItemId("currency_coin");
    ```

* Give the user 10 pieces of a virtual currency with itemId "currency_coin":

    ```cs
    StoreInventory.GiveItem("currency_coin", 10);
    ```

* Take 10 virtual goods with itemId "green_hat":

    ```cs
    StoreInventory.TakeItem("green_hat", 10);
    ```

* Get the current balance of green hats (virtual goods with itemId "green_hat"):

    ```cs
    int greenHatsBalance = StoreInventory.GetItemBalance("green_hat");
    ```

Event Handling
---

SOOMLA lets you subscribe to store events, get notified and implement your own application specific behavior to those events.

> Your behavior is an addition to the default behavior implemented by SOOMLA. You don't replace SOOMLA's behavior.

The 'Events' class is where all event go through. To handle various events, just add your specific behavior to the delegates in the Events class.

For example, if you want to 'listen' to a MarketPurchase event:

```cs
StoreEvents.OnMarketPurchase += onMarketPurchase;

public void onMarketPurchase(PurchasableVirtualItem pvi, string purchaseToken, string payload) {
    Debug.Log("Just purchased an item with itemId: " + pvi.ItemId);
}
```

One thing you need to make sure is that you instantiate your EventHandler before SoomlaStore.  
So if you have:
````
private static Soomla.Example.ExampleEventHandler handler;
````
you'll need to do:
````
handler = new Soomla.Example.ExampleEventHandler();
````
before
````
Soomla.SoomlaStore.Initialize(new Soomla.Example.MuffinRushAssets());
````

Contribution
---

We want you!

Fork -> Clone -> Implement -> Insert Comments -> Test -> Pull-Request.

We have great RESPECT for contributors.

Code Documentation
---

android-store follows strict code documentation conventions. If you would like to contribute please read our [Documentation Guidelines](https://github.com/soomla/unity3d-store/tree/master/documentation.md) and follow them. Clear, consistent  comments will make our code easy to understand.

SOOMLA, Elsewhere ...
---

+ [Framework Website](http://www.soom.la/)
+ [On Facebook](https://www.facebook.com/pages/The-SOOMLA-Project/389643294427376).
+ [On AngelList](https://angel.co/the-soomla-project)

License
---
Apache License. Copyright (c) 2012-2014 SOOMLA. http://www.soom.la
+ http://opensource.org/licenses/Apache-2.0
