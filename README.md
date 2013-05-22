
*This project is a part of [The SOOMLA Project](http://project.soom.la) which is a series of open source initiatives with a joint goal to help mobile game developers get better stores and more in-app purchases.*

Haven't you ever wanted an in-app purchase one liner that looks like this ?!

```cs
    StoreInventory.BuyItem("[itemId]");
```

unity3d-store
---
**The new Virtual Economy model V3 is merged into master. The new model has many new features and it works better than the old one. Old applications may break if they use in this new model so already published games with unity3d-store from before May 1st, 2013 needs to clone the project with tag 'v0.23' and not 'v1.0'.**

Want to learn more about modelV3? Try these:  
* [Economy Model Objects - android-store](https://github.com/soomla/android-store/wiki/Economy-Model-Objects)  
* [Handling Store Operations - android-store](https://github.com/soomla/android-store/wiki/Handling-Store-Operations)
(The same model objects from android-store exist in unity3d-store)

The unity3d-store is the Unity3D flavour of The SOOMLA Project. This project uses [android-store](https://github.com/soomla/android-store) and [ios-store](https://github.com/soomla/ios-store) in order to provide game developers with in-app billing for their **Unity3D** projects.
    
**Before you start**, we suggest that you go over the information in ios-store and android-store so you get acquainted with the SOOMLA framework:
- ios-store [project](https://github.com/wesgt/ios-store) [wiki](https://github.com/wesgt/ios-store/wiki)
- android-store [project](https://github.com/soomla/android-store) [wiki](https://github.com/soomla/android-store/wiki)

>If you also want to create a **storefront** you can do that using SOOMLA's [In-App Purchase Store Designer](http://soom.la).

## Download

We've created a unitypackage and an example project:

####unity3d-store v1.0 (release version)

[Unity 4.0 - unity3d-store v1.0](http://bit.ly/ZC1P7r)  
[Unity 3.5 - unity3d-store v1.0](http://bit.ly/10YZMx7)

####unity3d-store v1.0 example

- The example project is mostly what you have in this Github repo. You can either download it or clone unity3d-store.

[Unity 4.0 - unity3d-store v1.0 example](http://bit.ly/ZC1EJk)  
[Unity 3.5 - unity3d-store v1.0 example](http://bit.ly/10uMqkx)

## Debugging

The download packages and the code in the repo uses the "release" versions of android-store and ios-store. Also, Unity debug messages will only be printed out if you build the project with _Development Build_ checked.

If you want to see full debug messages from android-store and ios-store you'll have to use the debug builds of those libraries. You can find those builds in the repo, in the folder _soomla-native_ ([android](https://github.com/soomla/unity3d-store/blob/master/soomla-native/android/Soomla_debug.jar)  [ios](https://github.com/soomla/unity3d-store/blob/master/soomla-native/ios/release/libSoomlaIOSStore.a)).

**Test purchases on Android** will not work (even in the debug library) if you won't switch its Test Mode on. In order to do that, check the box next to "Android Test Mode" in the Soomla prefab when it's added to your scene. (The example project works with test purchases. Make sure it's running on Test Mode)


## Getting Started

1. Download the unity3d-store unityproject file you want and double-click on it. It'll import all the necessary files into your project.
2. Drag the "Soomla" Prefab into your scene. You should see it listed in the "Hierarchy" panel.
3. Click on the "Soomla" Prefab you just added and in the "Inspector" panel change the values for "Custom Secret", "Public Key" and "Soom Sec":
    - _Custom Secret_ - is an encryption secret you provide that will be used to secure your data.
    - _Public Key_ - is the public key given to you from Google. (iOS doesn't have a public key).
    - _Soom Sec_ - is a special secret SOOMLA uses to increase your data protection.  
    **Choose both secrets wisely. You can't change them after you launch your game!**
4. Create your own implementation of _IStoreAssets_ in order to describe your specific game's assets ([example](https://github.com/soomla/unity3d-store/blob/master/unity4.0/Assets/Soomla/Code/MuffinRushAssets.cs)). Initialize _StoreController_ with the class you just created:

    ```cs
       StoreController.Initialize(new YourStoreAssetsImplementation());
    ```
    
    > Initialize _StoreController_ ONLY ONCE when your application loads.
    
    > Initialize _StoreController_ in the "Start()" function of a 'MonoBehaviour' and **NOT** in the "Awake()" function. SOOMLA has its own 'MonoBehaviour' and it needs to be "Awakened" before you initialize.

5. Now, that you have _StoreController_ loaded, just decide when you want to show/hide your store's UI to the user and let _StoreController_ know about it:

  When you show the store call:

    ```cs
    StoreController.storeOpening();
    ```

  When you hide the store call:

    ```cs
    StoreController.storeClosing();
    ```
    
    > Don't forget to make these calls. _StoreController_ has to know that you opened/closed your in-app purchase store. Just to make it clear: the in-app purchase store is where you sell virtual goods (and not Google Play or App Store).

6. You'll need an event handler in order to be notified about in-app purchasing related events. refer to the [Event Handling](https://github.com/soomla/unity3d-store#event-handling) section for more information.

And that's it ! You have storage and in-app purchasing capabilities... ALL-IN-ONE.

## What's next? In App Purchasing.

When we implemented modelV3, we were thinking about ways people buy things inside apps. We figured many ways you can let your users purchase stuff in your game and we designed the new modelV3 to support 2 of them: PurchaseWithMarket and PurchaseWithVirtualItem.

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


When you initialize _StoreController_, it automatically initializes two other classes: _StoreInventory_ and _StoreInfo_:  
* _StoreInventory_ is a convenience class to let you perform operations on VirtualCurrencies and VirtualGoods. Use it to fetch/change the balances of VirtualItems in your game (using their ItemIds!)  
* _StoreInfo_ is where all meta data information about your specific game can be retrieved. It is initialized with your implementation of `IStoreAssets` and you can use it to retrieve information about your specific game.

**ATTENTION: because we're using JNI (Android) and DllImport (iOS) you should make as little calls as possible to _StoreInfo_. Look in the example project for the way we created a sort of a cache to hold your game's information in order to not make too many calls to _StoreInfo_. We update this cache using an event handler. (see [ExampleLocalStoreInfo](https://github.com/soomla/unity3d-store/blob/master/unity4.0/Assets/Soomla/Code/ExampleLocalStoreInfo.cs) and [ExampleEventHandler](https://github.com/soomla/unity3d-store/blob/master/unity4.0/Assets/Soomla/Code/ExampleEventHandler.cs)).**

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

SOOMLA lets you subscribe to store events, get notified and implement your own application specific behaviour to those events.

> Your behaviour is an addition to the default behaviour implemented by SOOMLA. You don't replace SOOMLA's behaviour.

The 'Events' class is where all event go through. To handle various events, just add your specific behaviour to the delegates in the Events class.

For example, if you want to 'listen' to a MerketPurchase event:

```cs
Events.OnMarketPurchase += onMarketPurchase;
    
public void onMarketPurchase(PurchasableVirtualItem pvi) {
    Debug.Log("Going to purchase an item with productId: " + pvi.ItemId);
}
```

Contribution
---

We want you!

Fork -> Clone -> Implement -> Test -> Pull-Request. We have great RESPECT for contributors.

SOOMLA, Elsewhere ...
---

+ [SOOMLA Website](http://soom.la/)
+ [On Facebook](https://www.facebook.com/pages/The-SOOMLA-Project/389643294427376).
+ [On AngelList](https://angel.co/the-soomla-project)
+ [IAP Unity](http://soom.la/IAP-Unity) Plugin

License
---
MIT License. Copyright (c) 2012 SOOMLA. http://project.soom.la
+ http://www.opensource.org/licenses/MIT


