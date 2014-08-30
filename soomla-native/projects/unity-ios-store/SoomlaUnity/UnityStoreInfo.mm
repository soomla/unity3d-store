#import "UnityStoreAssets.h"
#import "VirtualItemNotFoundException.h"
#import "UnityCommons.h"
#import "UnityStoreCommons.h"
#import "VirtualItem.h"
#import "StoreInfo.h"
#import "SoomlaUtils.h"

extern "C"{
	
	int storeInfo_GetItemByItemId(const char* itemId, char** json) {
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
		@try {
			VirtualItem* vi = [[StoreInfo getInstance] virtualItemWithId:itemIdS];
			NSString *className = NSStringFromClass([vi class]);
		    NSDictionary* nameWithClass = [NSDictionary dictionaryWithObjectsAndKeys:
		                                   [vi toDictionary], @"item",
		                                   className, @"className", nil];
			*json = Soom_AutonomousStringCopy([[SoomlaUtils dictToJsonString:nameWithClass] UTF8String]);
		}
		
		@catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualItem with itemId: %@.", itemIdS);
			return EXCEPTION_ITEM_NOT_FOUND;
        }

		return NO_ERR;
	}
	
	int storeInfo_GetPurchasableItemWithProductId(const char* productId, char** json) {
        NSString* productIdS = [NSString stringWithUTF8String:productId];
		@try {
			PurchasableVirtualItem* pvi = [[StoreInfo getInstance] purchasableItemWithProductId:productIdS];
			NSString *className = NSStringFromClass([pvi class]);
		    NSDictionary* nameWithClass = [NSDictionary dictionaryWithObjectsAndKeys:
		                                   [pvi toDictionary], @"item",
		                                   className, @"className", nil];
			*json = Soom_AutonomousStringCopy([[SoomlaUtils dictToJsonString:nameWithClass] UTF8String]);
		}
		
		@catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a PurchasableVirtualItem with productId: %@.", productIdS);
			return EXCEPTION_ITEM_NOT_FOUND;
        }

		return NO_ERR;
	}
	
	int storeInfo_GetCategoryForVirtualGood(const char* goodItemId, char** json){
		NSString* goodItemIdS = [NSString stringWithUTF8String:goodItemId];
		@try {
			VirtualCategory* vc = [[StoreInfo getInstance] categoryForGoodWithItemId:goodItemIdS];
			*json = Soom_AutonomousStringCopy([[SoomlaUtils dictToJsonString:[vc toDictionary]] UTF8String]);
		}
		
		@catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualCategory for VirtualGood with itemId: %@.", goodItemIdS);
			return EXCEPTION_ITEM_NOT_FOUND;
        }

		return NO_ERR;
	}
	
	int storeInfo_GetFirstUpgradeForVirtualGood(const char* goodItemId, char** json){
		NSString* goodItemIdS = [NSString stringWithUTF8String:goodItemId];
		@try {
			UpgradeVG* vgu = [[StoreInfo getInstance] firstUpgradeForGoodWithItemId:goodItemIdS];
			*json = Soom_AutonomousStringCopy([[SoomlaUtils dictToJsonString:[vgu toDictionary]] UTF8String]);
		}
		
		@catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualCategory for VirtualGood with itemId: %@.", goodItemIdS);
			return EXCEPTION_ITEM_NOT_FOUND;
        }

		return NO_ERR;
	}
	
	int storeInfo_GetLastUpgradeForVirtualGood(const char* goodItemId, char** json){
		NSString* goodItemIdS = [NSString stringWithUTF8String:goodItemId];
		@try {
			UpgradeVG* vgu = [[StoreInfo getInstance] lastUpgradeForGoodWithItemId:goodItemIdS];
			*json = Soom_AutonomousStringCopy([[SoomlaUtils dictToJsonString:[vgu toDictionary]] UTF8String]);
		}
		
		@catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualCategory for VirtualGood with itemId: %@.", goodItemIdS);
			return EXCEPTION_ITEM_NOT_FOUND;
        }

		return NO_ERR;
	}
	
	int storeInfo_GetUpgradesForVirtualGood(const char* goodItemId, char** json) {
		NSString* goodItemIdS = [NSString stringWithUTF8String:goodItemId];
		NSArray* upgrades = [[StoreInfo getInstance] upgradesForGoodWithItemId:goodItemIdS];
		NSMutableString* retJson = [NSMutableString string];
		if (upgrades && upgrades.count>0) {
            retJson = [[[NSMutableString alloc] initWithString:@"["] autorelease];
            for(UpgradeVG* vgu in upgrades) {
                [retJson appendString:[NSString stringWithFormat:@"%@,", [SoomlaUtils dictToJsonString:[vgu toDictionary]]]];
            }
            [retJson deleteCharactersInRange:NSMakeRange([retJson length]-1, 1)];
            [retJson appendString:@"]"];
		}
		
		*json = Soom_AutonomousStringCopy([retJson UTF8String]);

		return NO_ERR;
	}
	
	int storeInfo_GetVirtualCurrencies(char** json) {
		NSArray* virtualCurrencies = [[StoreInfo getInstance] virtualCurrencies];
		NSMutableString* retJson = [NSMutableString string];
        if (virtualCurrencies.count > 0) {
            retJson = [[[NSMutableString alloc] initWithString:@"["] autorelease];
            for(VirtualCurrency* vc in virtualCurrencies) {
                [retJson appendString:[NSString stringWithFormat:@"%@,", [SoomlaUtils dictToJsonString:[vc toDictionary]]]];
            }
            [retJson deleteCharactersInRange:NSMakeRange([retJson length]-1, 1)];
            [retJson appendString:@"]"];
        }
		
		*json = Soom_AutonomousStringCopy([retJson UTF8String]);
		
		return NO_ERR;
	}
	
	int storeInfo_GetVirtualGoods(char** json) {
		NSArray* virtualGoods = [[StoreInfo getInstance] virtualGoods];
        NSMutableString* retJson = [NSMutableString string];
        if (virtualGoods.count > 0) {
            retJson = [[[NSMutableString alloc] initWithString:@"["] autorelease];
            for(VirtualGood* vg in virtualGoods) {
                NSString *className = NSStringFromClass([vg class]);
                NSDictionary* nameWithClass = [NSDictionary dictionaryWithObjectsAndKeys:
                                               [vg toDictionary], @"item",
                                               className, @"className", nil];
                [retJson appendString:[NSString stringWithFormat:@"%@,", [SoomlaUtils dictToJsonString:nameWithClass]]];
            }
            [retJson deleteCharactersInRange:NSMakeRange([retJson length]-1, 1)];
            [retJson appendString:@"]"];
        }
		
		
		*json = Soom_AutonomousStringCopy([retJson UTF8String]);
		
		return NO_ERR;
	}
	
	int storeInfo_GetVirtualCurrencyPacks(char** json) {
		NSArray* virtualCurrencyPacks = [[StoreInfo getInstance] virtualCurrencyPacks];
        NSMutableString* retJson = [NSMutableString string];
        if (virtualCurrencyPacks.count > 0) {
            retJson = [[[NSMutableString alloc] initWithString:@"["] autorelease];
            for(VirtualCurrencyPack* vcp in virtualCurrencyPacks) {
                [retJson appendString:[NSString stringWithFormat:@"%@,", [SoomlaUtils dictToJsonString:[vcp toDictionary]]]];
            }
            [retJson deleteCharactersInRange:NSMakeRange([retJson length]-1, 1)];
            [retJson appendString:@"]"];
        }
		
		
		*json = Soom_AutonomousStringCopy([retJson UTF8String]);
		
		return NO_ERR;
	}
	
	int storeInfo_GetNonConsumableItems(char** json) {
		NSArray* nonConsumables = [[StoreInfo getInstance] nonConsumableItems];
        NSMutableString* retJson = [NSMutableString string];
        if (nonConsumables.count > 0) {
            retJson = [[[NSMutableString alloc] initWithString:@"["] autorelease];
            for(NonConsumableItem* non in nonConsumables) {
                [retJson appendString:[NSString stringWithFormat:@"%@,", [SoomlaUtils dictToJsonString:[non toDictionary]]]];
            }
            [retJson deleteCharactersInRange:NSMakeRange([retJson length]-1, 1)];
            [retJson appendString:@"]"];
        }
		
        *json = Soom_AutonomousStringCopy([retJson UTF8String]);
		
		return NO_ERR;
	}
	
	int storeInfo_GetVirtualCategories(char** json) {
		NSArray* virtualCategories = [[StoreInfo getInstance] virtualCategories];
        NSMutableString* retJson = [NSMutableString string];
        if (virtualCategories.count > 0) {
            retJson = [[[NSMutableString alloc] initWithString:@"["] autorelease];
            for(VirtualCategory* vc in virtualCategories) {
                [retJson appendString:[NSString stringWithFormat:@"%@,", [SoomlaUtils dictToJsonString:[vc toDictionary]]]];
            }
            [retJson deleteCharactersInRange:NSMakeRange([retJson length]-1, 1)];
            [retJson appendString:@"]"];
        }
		
		
		*json = Soom_AutonomousStringCopy([retJson UTF8String]);
		
		return NO_ERR;
	}
	
}