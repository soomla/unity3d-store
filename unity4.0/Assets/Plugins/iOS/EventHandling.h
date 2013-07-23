/*
 * Copyright (C) 2012 Soomla Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */


#import <UIKit/UIKit.h>

@class AppStoreItem;
@class VirtualGood;
@class VirtualCurrency;
@class PurchasableVirtualItem;
@class UpgradeVG;
@class EquippableVG;

// Events
#define EVENT_BILLING_NOT_SUPPORTED         @"BillingNotSupported"
#define EVENT_BILLING_SUPPORTED             @"BillingSupported"
#define EVENT_CLOSING_STORE                 @"ClosingStore"
#define EVENT_CURRENCY_BALANCE_CHANGED      @"ChangedCurrencyBalance"
#define EVENT_GOOD_BALANCE_CHANGED          @"ChangedGoodBalance"
#define EVENT_GOOD_EQUIPPED                 @"VirtualGoodEquipped"
#define EVENT_GOOD_UNEQUIPPED               @"VirtualGoodUNEQUIPPED"
#define EVENT_GOOD_UPGRADE                  @"VirtualGoodUpgrade"
#define EVENT_ITEM_PURCHASED                @"ItemPurchased"
#define EVENT_ITEM_PURCHASE_STARTED         @"ItemPurchaseProcessStarted"
#define EVENT_OPENING_STORE                 @"OpeningStore"
#define EVENT_APPSTORE_PURCHASE_CANCELLED   @"AppStorePurchaseCancelled"
#define EVENT_APPSTORE_PURCHASED            @"AppStorePurchased"
#define EVENT_APPSTORE_PURCHASE_STARTED     @"AppStorePurchaseProcessStarted"
#define EVENT_TRANSACTION_RESTORED          @"TransactionRestored"
#define EVENT_TRANSACTION_RESTORE_STARTED   @"TransactionRestoreStarted"
#define EVENT_UNEXPECTED_ERROR_IN_STORE     @"UnexpectedErrorInStore"


// UserInfo Elements
#define DICT_ELEMENT_BALANCE           @"balance"
#define DICT_ELEMENT_CURRENCY          @"VirtualCurrency"
#define DICT_ELEMENT_AMOUNT_ADDED      @"amountAdded"
#define DICT_ELEMENT_GOOD              @"VirtualGood"
#define DICT_ELEMENT_EquippableVG      @"EquippableVG"
#define DICT_ELEMENT_UpgradeVG         @"UpgradeVG"
#define DICT_ELEMENT_PURCHASABLE       @"PurchasableVirtualItem"
#define DICT_ELEMENT_RECEIPT           @"receipt"
#define DICT_ELEMENT_SUCCESS           @"success"


/**
 * This class is used register and post all the supported events.
 * Use this class to invoke events on handlers when they occur.
 *
 * SOOMLA uses iOS's NSNotificationCenter to handle events across the SDK.
 */
@interface EventHandling : NSObject

+ (void)observeAllEventsWithObserver:(id)observer withSelector:(SEL)selector;

+ (void)postBillingSupported;
+ (void)postBillingNotSupported;
+ (void)postClosingStore;
+ (void)postChangedBalance:(int)balance forCurrency:(VirtualCurrency*)currency withAmount:(int)amountAdded;
+ (void)postChangedBalance:(int)balance forGood:(VirtualGood*)good withAmount:(int)amountAdded;
+ (void)postGoodEquipped:(EquippableVG*)good;
+ (void)postGoodUnEquipped:(EquippableVG*)good;
+ (void)postGoodUpgrade:(VirtualGood*)good withGoodUpgrade:(UpgradeVG*)goodUpgrade;
+ (void)postItemPurchaseStarted:(PurchasableVirtualItem*)item;
+ (void)postItemPurchased:(PurchasableVirtualItem*)item;
+ (void)postOpeningStore;
+ (void)postAppStorePurchaseCancelled:(PurchasableVirtualItem*)purchasableVirtualItem;
+ (void)postAppStorePurchase:(PurchasableVirtualItem*)purchasableVirtualItem withReceipt:(NSString *)transactionReceipt;
+ (void)postAppStorePurchaseStarted:(PurchasableVirtualItem*)purchasableVirtualItem;
+ (void)postTransactionRestored:(BOOL)success;
+ (void)postTransactionRestoreStarted;
+ (void)postUnexpectedError;

@end

