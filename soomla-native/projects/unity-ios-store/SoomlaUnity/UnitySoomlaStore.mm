#import "UnityStoreAssets.h"
#import "UnityStoreEventDispatcher.h"
#import "SoomlaStore.h"
#import "VirtualItemNotFoundException.h"
#import "UnityCommons.h"
#import "UnityStoreCommons.h"
#import "StoreConfig.h"
#import "StoreInfo.h"
#import "PurchasableVirtualItem.h"
#import "PurchaseWithMarket.h"
#import "SoomlaUtils.h"

extern "C"{

    void soomlaStore_SetSSV(bool ssv, const char* verifyUrl) {
		VERIFY_PURCHASES = ssv;

        if (VERIFY_URL) {
            [VERIFY_URL release];
        }
        VERIFY_URL = [[NSString stringWithUTF8String:verifyUrl] retain];
    }

	void soomlaStore_Init(){
        LogDebug(@"SOOMLA Unity UnitySoomlaStore", @"Initializing StoreEventHandler ...");
        [UnityStoreEventDispatcher initialize];

		[[SoomlaStore getInstance] initializeWithStoreAssets:[UnityStoreAssets getInstance]];
	}

	int soomlaStore_BuyMarketItem(const char* productId, const char* payload) {
		@try {
            NSString* payloadS = [NSString stringWithUTF8String:payload];
			PurchasableVirtualItem* pvi = [[StoreInfo getInstance] purchasableItemWithProductId:[NSString stringWithUTF8String:productId]];
			if ([pvi.purchaseType isKindOfClass:[PurchaseWithMarket class]]) {
				MarketItem* asi = ((PurchaseWithMarket*) pvi.purchaseType).marketItem;
				[[SoomlaStore getInstance] buyInMarketWithMarketItem:asi andPayload:payloadS];
			} else {
				NSLog(@"The requested PurchasableVirtualItem is has no PurchaseWithMarket PurchaseType. productId: %@. Purchase is cancelled.", [NSString stringWithUTF8String:productId]);
				return EXCEPTION_ITEM_NOT_FOUND;
			}
		}

        @catch (VirtualItemNotFoundException *e) {
            NSLog(@"Couldn't find a VirtualCurrencyPack with productId: %@. Purchase is cancelled.", [NSString stringWithUTF8String:productId]);
			return EXCEPTION_ITEM_NOT_FOUND;
        }

		return NO_ERR;
	}

	void soomlaStore_RestoreTransactions() {
		[[SoomlaStore getInstance] restoreTransactions];
	}

    void soomlaStore_RefreshInventory() {
		[[SoomlaStore getInstance] refreshInventory];
	}

    void soomlaStore_RefreshMarketItemsDetails() {
		[[SoomlaStore getInstance] refreshMarketItemsDetails];
	}

	void soomlaStore_TransactionsAlreadyRestored(bool* outResult){
		*outResult = [[SoomlaStore getInstance] transactionsAlreadyRestored];
	}

}
