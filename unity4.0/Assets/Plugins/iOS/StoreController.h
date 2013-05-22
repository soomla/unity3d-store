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

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>
#import "IStoreAsssets.h"

@class AppStoreItem;

/**
 * This class holds the most basic assets needed to operate the Store.
 * You can use it to purchase products from the App Store.
 *
 * This is the only class you need to initialize in order to use the SOOMLA SDK.
 *
 * In addition to initializing this class, you'll also have to call
 * StoreController::storeOpening and StoreController::storeClosing when you open the store window or close it. These two
 * calls initializes important components that support billing and storage information (see implementation below).
 * IMPORTANT: if you use the SOOMLA's storefront, then DON'T call these 2 functions.
 *
 */
@interface StoreController : NSObject <SKProductsRequestDelegate, SKPaymentTransactionObserver>{
    @private
    BOOL initialized;
    BOOL storeOpen;
}

@property BOOL initialized;
@property BOOL storeOpen;

+ (StoreController*)getInstance;

/**
 * This initializer also initializes StoreInfo.
 * storeAssets is the definition of your application specific assets.
 * customSecret is your encryption secret (it's used to encrypt your data in the database)
 */
- (void)initializeWithStoreAssets:(id<IStoreAsssets>)storeAssets andCustomSecret:(NSString*)secret;
/**
 * Start an in app purchase process in the App Store.
 * appStoreItem is the item to purchase. This item has to be defined EXACTLY the same in iTunes Connect.
 */
- (BOOL)buyInAppStoreWithAppStoreItem:(AppStoreItem*)appStoreItem;
/**
 * Call this function when you open the actual store window
 */
- (void)storeOpening;
/**
 * Call this function when you close the actual store window.
 */
- (void)storeClosing;
/**
 * Initiate the restoreTransactions process
 */
- (void)restoreTransactions;
/**
 * Answers the question: "Were transactions already restored for this game?"
 */
- (BOOL)transactionsAlreadyRestored;
- (void) setServerVerifyEnable:(NSString*) serverVerifyEnable;
- (void) setVerifyURL:(NSString*) verifyURL;
@end
