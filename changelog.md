### v1.5.3 [view commit logs](https://github.com/soomla/android-store/compare/v1.5.2...v1.5.3)

* New Features
  * You can now use StoreInventory's 'BuyItem' with a payload of your choice. This payload will be returned back to you in the purchase events when it's completed.
  * VirtualItemReward is added.
  * (Android) back button will exit the example app.

* Fixed
  * Firing UnexpectedErrorInStoreEvent when the user tried to buy a NonConsumableItem when it was already owned.

### v1.5.2 [view commit logs](https://github.com/soomla/android-store/compare/v1.5.1...v1.5.2)

* New Features
  * Some core objects and features were extracted to a separate folder called "Core". Will be moved to a separate repo later.
  * You only provide one secret now which is called Soomla Secret when you initialize "Soomla" (soomla core).
  * The option to print debug messages was added to the settings panel.

* Changes
  * StoreController is now called SoomlaStore.

* Fixes
  * Android - Fixed an issue with not getting back to the app well from background during a purchase.

### v1.5.0 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.4.4...v1.5.0)

* Fixes
  * Correctly fetching products' details from market (android). Fixed #194
  * Fixed restoreTransactions and refreshInventory to support changes in android-store.
  * When onGoodUpgrade is being thrown, the current upgrade may be null. We now take care of it.
  * Small fixes in inline docs.
  * When we remove all upgrades from an item, the associated upgrade in the event is null. This is correct but the way it's sent and parsed in Unity's StoreEvents was wrong. Resolved it by fixing the message to unity and the parsing in StoreEvents. Fixed #233
  * The code is arranged better now. Thanks Holymars
  * Added market items to the OnMarketItemsRefreshed event.


* New Features
  * (Android Only!) Added payload to BuyMarketItem function in StoreController. The payload will be returned in OnMarketPurchase event.
  * Added new event OnMarketItemsRefreshStarted
  * Added an option to print debug messages in the SOOMLA Settings panel.
  * Added Amazon billing service and the option to switch between billing services in the SOOMLA Settings panel.
  * Changed folder structure a bit.


### v1.4.4 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.4.3...v1.4.4)

* Fixes
  * Correctly fetching products' details from market (android). Fixed #194

### v1.4.3 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.4.1...v1.4.3)

* Fixes
  * Added "using System" so things will work corrctly on Android. Closes #201
  * Refreshed items were not parsed correctly. Closes #207


### v1.4.2 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.4.1...v1.4.2)

* Fixes
  * Fixed some build issues in native libraries.
  * Fixed warnings for 'save' function in VirtualItems.

### v1.4.1 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.4.0...v1.4.1)

* New Features
  * Added an option to save changed item's metadata (closes #197)

* Fixes
  * Fixed ios static libs to support multiple archs.


### v1.4.0 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.3.0...v1.4.0)

* General
  * Changed directory structure - dropped support for unity 3.5 and changed the main source folder name to Soomla.
  * Added a new event "OnMarketItemsRefreshed" that'll be fired when market items details (MarketPrice, MarketTitle and MarketDescription) are refreshed from the mobile (on device) store. Thanks @Whyser and @Idden
  * Added a function to StoreController called "RefreshInventory". It will refresh market items details from the mobile (on device) store.

* Fixes
  * Fixed some issues in android-store Google Play purchase flow. Thanks to @HolymarsHsieh
