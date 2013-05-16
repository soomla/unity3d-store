#import "UnityStoreAssets.h"
#import "UnityStoreEventDispatcher.h"
#import "VirtualCategory.h"
#import "VirtualCurrency.h"
#import "VirtualGood.h"
#import "AppStoreItem.h"
#import "VirtualCurrencyPack.h"
#import "StoreController.h"
#import "JSONKit.h"
#import "VirtualItemNotFoundException.h"
#import "NotEnoughGoodsException.h"
#import "InsufficientFundsException.h"
#import "UnityCommons.h"
#import "StoreConfig.h"
#import "NonConsumableItem.h"
#import "StoreInfo.h"
#import "PurchaseWithMarket.h"

UnityStoreAssets * storeAssets;
UnityStoreEventDispatcher * storeEventDispatcher;
extern UIViewController* UnityGetGLViewController();

extern "C"{
	    
	void storeAssets_Init(int version, const char* storeAssetsJSON){
		NSString* storeAssetsJSONS = [NSString stringWithUTF8String:storeAssetsJSON];
		storeAssets = [[UnityStoreAssets alloc] initWithStoreAssetsJSON:storeAssetsJSONS andVersion:version];
	}
	    
    void storeController_SetSoomSec(const char* soomSec) {
        if (SOOM_SEC) {
            [SOOM_SEC release];
        }
        SOOM_SEC = [[NSString stringWithUTF8String:soomSec] retain];
    }
	
	void storeController_Init(const char* secret){
		[[StoreController getInstance] initializeWithStoreAssets:storeAssets andCustomSecret:[NSString stringWithUTF8String:secret]];
        storeEventDispatcher = [[UnityStoreEventDispatcher alloc] init];
	}
	
	int storeController_BuyMarketItem(const char* productId) {
		@try {
			PurchasableVirtualItem* pvi = [[StoreInfo getInstance] purchasableItemWithProductId:[NSString stringWithUTF8String:productId]];
			if ([pvi.purchaseType isKindOfClass:[PurchaseWithMarket class]]) {
				AppStoreItem* asi = ((PurchaseWithMarket*) pvi.purchaseType).appStoreItem;
				[[StoreController getInstance] buyInAppStoreWithAppStoreItem:asi];
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
		
	void storeController_StoreOpening() {
		[[StoreController getInstance] storeOpening];
	}
	
	void storeController_StoreClosing() {
		[[StoreController getInstance] storeClosing];
	}
	
	void storeController_RestoreTransactions() {
		[[StoreController getInstance] restoreTransactions];
	}
	
	void storeController_TransactionsAlreadyRestored(bool* outResult){
		*outResult = [StoreController transactionsAlreadyRestored];
	}

	void storeController_SetServerVerifyEnable(const char* serverVerifyEnable) {
		[[StoreController getInstance] setServerVerifyEnable:[NSString stringWithUTF8String:serverVerifyEnable]];
	}
	
	void storeController_SetVerifyURL(const char* verifyURL) {
		[[StoreController getInstance] setVerifyURL:[NSString stringWithUTF8String:verifyURL]];
	}
	
}