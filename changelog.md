### v1.10.2 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.10.1...v1.10.2)

* Fixes
  * Fixed editor cpu usage

### v1.10.1 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.10.0...v1.10.1)

* Changes
  * Removed binaries from repo

### v1.10.0 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.9.3...v1.10.0)

* Changes
  * Added validation - not allowing null pointers in IStoreAssets

* Fixes
  * Fixed building in Unity 5.3

### v1.9.3 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.9.2...v1.9.3)

* Changes
  * Added saving user settings to PlayerPrefs

### v1.9.2 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.9.1...v1.9.2)

* New Features
  * Added OnVerificationStarted event

* Changes
  * Minor improvements and rearrangements to Editor script

### v1.9.1 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.9.0...v1.9.1)

* Changes
  * Minor improvements in Editor script

### v1.9.0 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.8.6...v1.9.0)

* Features
  * Added checking for latest version in Soomla Settings panel
  * Added a new way of event handling without prefabs
  * Add Market Purchase Deferred handling

* Changes
  * Moved all SOOMLA plugins into 'Soomla' folders

### v1.8.6 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.8.5...v1.8.6)

* Fixes
  * StoreInventory.RemoveGoodUpgrades crash in iOS when upgrade id is null

### v1.8.5 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.8.4...v1.8.5)

* Fixes
  * Soomla works when game is paused

### v1.8.4 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.8.3...v1.8.4)

* Changes
  * Removed binaries and improved build scripts

* Fixes
  * Wrong behaviour if purchasable items have the same product ID

### v1.8.3 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.8.2...v1.8.3)

* Changes
  * Improved docs
  * Supporting changes in submodules

### v1.8.2 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.8.1...v1.8.2)

* New Features
  * Supporting Verify-On-iTunes-Failure param

* Changes
  * New build way

### v1.8.1 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.8.0...v1.8.1)

* Fixes
  * Fixed StoreSetting for Unity4
  * Fixed billing services softlinks
  * Fixed getting PlaySsvValidation
  * Merged with verification fixes in ios-store v3.6.11 - https://github.com/soomla/ios-store/blob/master/changelog.md

### v1.8.0 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.7.16...v1.8.0)

* New Features
  * WP8 integration
  * Support Google Play verification

* Changes
  * Price getter added to Purchase Types.
  * For MarketPurchaseEvent we bundle the specific provider extra params inside a Hash
  * GetEquippedVirtualGood in Category
  * Changed OnUnexpectedErrorInStore to OnUnexpectedStoreError which now returns an error code

* Fixes
  * Fix Editor Market purchase event firing to match device


### v1.7.16 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.7.15...v1.7.16)

* New Features
  * In StoreEventHandler.onMarketPurchase, pass back originalJson and signature to Unity for server-side receipt verification
  * Guarding against non-existing error message in refresh failed (iOS6 only)

* Changes
  * Removed Consumable enum from MarketItem (***breaking change***)

### v1.7.15 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.7.14...v1.7.15)

* Changes
  * Making Store module Unity 5 compatible

### v1.7.14 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.7.13...v1.7.14)

* Changes
  * Added core post build script

* Fixes
  * In post-build, making dependent libraries add only once

### v1.7.13 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.7.12...v1.7.13)

* Changes
  * Updating Amazon plugin (v2.0.1)

### v1.7.12 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.7.11...v1.7.12)

* Fixes
  * Fixed NullRef exception calling BuyItem on editor #409

* Changes
  * Updated submodules

### v1.7.11 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.7.10...v1.7.11)

* Changes
  * Removing saving items to DB upon market items refresh, since it's now done in native
  * Added OnMarketItemsRefreshFailed event

### v1.7.10 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.7.9...v1.7.10)

* Fixes
  * Fixed issues with purchases on edge cases on Google Play
  * Fixed nil event issues from iOS purchases
  * Fixed #404

### v1.7.9 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.7.8...v1.7.9)

* Fixes
  * Fixed a crash on versions < ICS

### v1.7.8 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.7.7...v1.7.8)

* New Features
  * Added CanAfford method
  * Add onMarketPurchaseStarted and onMarketPurchase fake events when in Unity Editor
  * Updated features from submodules

### v1.7.7 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.7.6...v1.7.7)

* Changes
  * Fixes for Amazon support
  * Updated changes from submodules

### v1.7.6 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.7.5...v1.7.6)

* New Features
  * Amazon integration supports v2
  * Another Save function in StoreInfo that handles list of items

* Fixed
  * Multiple fixes from updated submodules


### v1.7.5 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.7.4...v1.7.5)

* Fixes
  * Fixed an issue with ItemPurchaseEvent being thrown before balance/currency changes.
  * Fixed an issue with some dictionaries not being used correctly
  * Fixed event pushing into native

### v1.7.4 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.7.3...v1.7.4)

* Fixes
  * Fixed an issue with Google Play.
  * Fixed a bug in VirtualGoodsStorage not being built b/c of wrong compiler flag.

### v1.7.3 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.7.2...v1.7.3)

* Fixes
  * Fixed some calls to wrong functions in native code.

### v1.7.2 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.7.1...v1.7.2)

* New Features
  * SOOMLA Core now supports custom events.

* Fixes
  * There was an issue with manipulation of Dictionaries. It's fixed now using an extension method called 'AddOrUpdate' which is implemented in Soomla Core. Make sure to update both Store and Core for 1.7.2.
  * Fixed issues with Android Store Google Play's handling of some null variables.

### v1.7.1 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.7.0...v1.7.1)

* Fixes
  * Fixed an issue where events were not sent with the correct variables.
  * Fixed an issue with StoreInventory not refreshing "local" balnces correctly.

### v1.7.0 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.6.0...v1.7.0)

***Important***: there are some breaking changes. Read the changes carefully.

* New Features
  * Work In Editor!
  * New local inventory that keeps all balances in memory so you don't need to go to the native DB. Saves JNI calls.

* Changes
  * In [StoreInfo](https://github.com/soomla/unity3d-store/blob/master/Soomla/Assets/Plugins/Soomla/Store/data/StoreInfo.cs) we replaces some funtions with public [variables](https://github.com/soomla/unity3d-store/blob/master/Soomla/Assets/Plugins/Soomla/Store/data/StoreInfo.cs#L512) that represent the collections of store metadata.
  * A class like the old [ExampleLocalStoreInfo](https://github.com/soomla/unity3d-store/blob/4dddde50607f83840fa1524f997c2568b90add11/Soomla/Assets/Examples/MuffinRush/ExampleLocalStoreInfo.cs) is **not** needed anymore. [StoreInventory](https://github.com/soomla/unity3d-store/commits/master/Soomla/Assets/Plugins/Soomla/Store/StoreInventory.cs) caches that info in memory and will be updated on runtime.
  * OnMarketPurchase event signature has changed. It now has a dictionary of extra information about the specific purchase for different native platforms (Android / iOS). More information [here](http://know.soom.la/docs/platforms/unity/events).

### v1.6.0 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.5.4...v1.6.0)

***Important***: this is a breaking release. Read the changes carefully.

* Changes
  * **BREAKING**: `NonConsumableItem` class removed, use `LifeTimeVG` instead.

### v1.5.4 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.5.3...v1.5.4)

* Changes
  * Some changes from SOOMLA Core module are supported.
  * Fixes and improvements in ios bridge project in order to prepare for Profile and LevelUp.

* Fixes
  * Fixed saving of market details after refresh (issue #300)


### v1.5.3 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.5.2...v1.5.3)

* New Features
  * You can now use StoreInventory's 'BuyItem' with a payload of your choice. This payload will be returned back to you in the purchase events when it's completed.
  * VirtualItemReward is added.
  * (Android) back button will exit the example app.

* Fixed
  * Firing UnexpectedErrorInStoreEvent when the user tried to buy a NonConsumableItem when it was already owned.

### v1.5.2 [view commit logs](https://github.com/soomla/unity3d-store/compare/v1.5.1...v1.5.2)

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
