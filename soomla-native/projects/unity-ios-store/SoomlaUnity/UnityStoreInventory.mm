#import "StoreInventory.h"
#import "VirtualItemNotFoundException.h"
#import "UnityCommons.h"
#import "UnityStoreCommons.h"
#import "InsufficientFundsException.h"
#import "SoomlaUtils.h"

extern "C"{
	
	int storeInventory_BuyItem(const char* itemId, const char* payload) {
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
        NSString* payloadS = [NSString stringWithUTF8String:payload];
		@try {
			[StoreInventory buyItemWithItemId: itemIdS andPayload:payloadS];
		}
		
		@catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualItem with itemId: %@.", itemIdS);
			return EXCEPTION_ITEM_NOT_FOUND;
        }
		@catch (InsufficientFundsException* e) {
            NSLog(@"Not enough funds to purchase VirtualItem with itemId: %@.", itemIdS);
			return EXCEPTION_INSUFFICIENT_FUNDS;
        }
		
		return NO_ERR;
	}
	
	int storeInventory_GetItemBalance(const char* itemId, int* outBalance){
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
		@try {
			*outBalance = [StoreInventory getItemBalance:itemIdS];
		}
		
		@catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualItem with itemId: %@.", itemIdS);
			return EXCEPTION_ITEM_NOT_FOUND;
        }

		return NO_ERR;
	}
	
	int storeInventory_GiveItem(const char* itemId, int amount){
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
		@try {
			[StoreInventory giveAmount:amount ofItem: itemIdS];
		}
		
		@catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualItem with itemId: %@.", itemIdS);
			return EXCEPTION_ITEM_NOT_FOUND;
        }

		return NO_ERR;
	}
	
	int storeInventory_TakeItem(const char* itemId, int amount){
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
		@try {
			[StoreInventory takeAmount:amount ofItem: itemIdS];
		}
		
		@catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualItem with itemId: %@.", itemIdS);
			return EXCEPTION_ITEM_NOT_FOUND;
        }

		return NO_ERR;
	}
	
	int storeInventory_EquipVirtualGood(const char* itemId){
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
		@try {
			[StoreInventory equipVirtualGoodWithItemId:itemIdS];
		}
		
		@catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualGood with itemId: %@.", itemIdS);
			return EXCEPTION_ITEM_NOT_FOUND;
        }

		return NO_ERR;
	}
	
	int storeInventory_UnEquipVirtualGood(const char* itemId){
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
		@try {
			[StoreInventory unEquipVirtualGoodWithItemId:itemIdS];
		}
		
		@catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualGood with itemId: %@.", itemIdS);
			return EXCEPTION_ITEM_NOT_FOUND;
        }

		return NO_ERR;
	}
	
	int storeInventory_IsVirtualGoodEquipped(const char* itemId, bool* outResult){
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
		@try {
			*outResult = [StoreInventory isVirtualGoodWithItemIdEquipped:itemIdS];
		}
		
		@catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualGood with itemId: %@.", itemIdS);
			return EXCEPTION_ITEM_NOT_FOUND;
        }

		return NO_ERR;
	}
	
	int storeInventory_GetGoodUpgradeLevel(const char* itemId, int* outResult){
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
		@try {
			*outResult = [StoreInventory goodUpgradeLevel:itemIdS];
		}
		
		@catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualGood with itemId: %@.", itemIdS);
			return EXCEPTION_ITEM_NOT_FOUND;
        }

		return NO_ERR;
	}

	int storeInventory_GetGoodCurrentUpgrade(const char* itemId, const char** outResult){
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
		@try {
			*outResult = Soom_AutonomousStringCopy([[StoreInventory goodCurrentUpgrade:itemIdS] UTF8String]);
		}
		
		@catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualGood with itemId: %@.", itemIdS);
			return EXCEPTION_ITEM_NOT_FOUND;
        }

		return NO_ERR;
	}
	
	int storeInventory_UpgradeGood(const char* itemId){
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
		@try {
			[StoreInventory upgradeVirtualGood:itemIdS];
		}
		
		@catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualGood with itemId: %@.", itemIdS);
			return EXCEPTION_ITEM_NOT_FOUND;
        }

		return NO_ERR;
	}
	
	int storeInventory_RemoveGoodUpgrades(const char* itemId){
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
		@try {
			[StoreInventory removeUpgrades:itemIdS];
		}
		
		@catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualGood with itemId: %@.", itemIdS);
			return EXCEPTION_ITEM_NOT_FOUND;
        }

		return NO_ERR;
	}
	
	int storeInventory_NonConsumableItemExists(const char* itemId, bool* outResult){
		NSString* itemIdS = [NSString stringWithUTF8String:itemId];
		@try {
			*outResult = [StoreInventory nonConsumableItemExists:itemIdS];
		}

		@catch (VirtualItemNotFoundException* e) {
	    NSLog(@"Couldn't find a NonConsumableItem with itemId: %@.", itemIdS);
			return EXCEPTION_ITEM_NOT_FOUND;
		}

		return NO_ERR;
	}

	int storeInventory_AddNonConsumableItem(const char* itemId){
	NSString* itemIdS = [NSString stringWithUTF8String:itemId];
		@try {
			[StoreInventory addNonConsumableItem:itemIdS];
		}

		@catch (VirtualItemNotFoundException* e) {
	    NSLog(@"Couldn't find a NonConsumableItem with itemId: %@.", itemIdS);
			return EXCEPTION_ITEM_NOT_FOUND;
		}

		return NO_ERR;
	}

	int storeInventory_RemoveNonConsumableItem(const char* itemId){
	NSString* itemIdS = [NSString stringWithUTF8String:itemId];
		@try {
			[StoreInventory removeNonConsumableItem:itemIdS];
		}

		@catch (VirtualItemNotFoundException* e) {
	    NSLog(@"Couldn't find a NonConsumableItem with itemId: %@.", itemIdS);
			return EXCEPTION_ITEM_NOT_FOUND;
		}

		return NO_ERR;
	}

}