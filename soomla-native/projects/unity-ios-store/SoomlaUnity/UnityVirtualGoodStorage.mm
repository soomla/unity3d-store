#import "VirtualItemNotFoundException.h"
#import "UnityCommons.h"
#import "UnityStoreCommons.h"

#import "StorageManager.h"
#import "VirtualGoodStorage.h"
extern "C"{
    
    int vgStorage_RemoveUpgrades(const char* itemId, bool notify) {
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
        @try {
            [[[StorageManager getInstance] virtualGoodStorage] removeUpgradesFrom:itemIdS withEvent:notify];
        }
        @catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualItem with itemId: %@.", itemIdS);
            return EXCEPTION_ITEM_NOT_FOUND;
        }
        
        return NO_ERR;
    }
    
    int vgStorage_AssignCurrentUpgrade(const char* itemId, const char* upgradeItemId, bool notify) {
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
        NSString* upgradeItemIdS = [NSString stringWithUTF8String:upgradeItemId];
        @try {
            [[[StorageManager getInstance] virtualGoodStorage] assignCurrentUpgrade:upgradeItemIdS toGood:itemIdS withEvent:notify];
        }
        @catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualItem with itemId: %@.", itemIdS);
            return EXCEPTION_ITEM_NOT_FOUND;
        }
        
        return NO_ERR;
    }
    
    int vgStorage_GetCurrentUpgrade(const char* itemId, char** outItemId) {
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
        @try {
            NSString* upgradeItemId = [[[StorageManager getInstance] virtualGoodStorage] currentUpgradeOf:itemIdS];
            *outItemId = Soom_AutonomousStringCopy([upgradeItemId UTF8String]);
        }
        @catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualItem with itemId: %@.", itemIdS);
            return EXCEPTION_ITEM_NOT_FOUND;
        }
        
        return NO_ERR;
    }
    
    int vgStorage_IsEquipped(const char* itemId, bool* outResult) {
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
        @try {
            *outResult = [[[StorageManager getInstance] virtualGoodStorage] isGoodEquipped:itemIdS];
        }
        @catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualItem with itemId: %@.", itemIdS);
            return EXCEPTION_ITEM_NOT_FOUND;
        }
        
        return NO_ERR;
    }
    
    int vgStorage_Equip(const char* itemId, bool notify) {
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
        @try {
            [[[StorageManager getInstance] virtualGoodStorage] equipGood:itemIdS withEvent:notify];
        }
        @catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualItem with itemId: %@.", itemIdS);
            return EXCEPTION_ITEM_NOT_FOUND;
        }
        
        return NO_ERR;
    }
    
    int vgStorage_UnEquip(const char* itemId, bool notify) {
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
        @try {
            [[[StorageManager getInstance] virtualGoodStorage] unequipGood:itemIdS withEvent:notify];
        }
        @catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualItem with itemId: %@.", itemIdS);
            return EXCEPTION_ITEM_NOT_FOUND;
        }
        
        return NO_ERR;
    }
    
    int vgStorage_GetBalance(const char* itemId, int* outBalance) {
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
        @try {
            *outBalance = [[[StorageManager getInstance] virtualGoodStorage] balanceForItem:itemIdS];
        }
        @catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualItem with itemId: %@.", itemIdS);
            return EXCEPTION_ITEM_NOT_FOUND;
        }
        
        return NO_ERR;
    }
    
    int vgStorage_SetBalance(const char* itemId, int balance, bool notify, int* outBalance) {
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
        @try {
            *outBalance = [[[StorageManager getInstance] virtualGoodStorage] setBalance:balance toItem:itemIdS withEvent:notify];
        }
        @catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualItem with itemId: %@.", itemIdS);
            return EXCEPTION_ITEM_NOT_FOUND;
        }
        
        return NO_ERR;
    }
    
    int vgStorage_Add(const char* itemId, int amount, bool notify, int* outBalance) {
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
        @try {
            *outBalance = [[[StorageManager getInstance] virtualGoodStorage] addAmount:amount toItem:itemIdS withEvent:notify];
        }
        @catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualItem with itemId: %@.", itemIdS);
            return EXCEPTION_ITEM_NOT_FOUND;
        }
        
        return NO_ERR;
    }
    
    int vgStorage_Remove(const char* itemId, int amount, bool notify, int* outBalance) {
        NSString* itemIdS = [NSString stringWithUTF8String:itemId];
        @try {
            *outBalance = [[[StorageManager getInstance] virtualGoodStorage] removeAmount:amount fromItem:itemIdS withEvent:notify];
        }
        @catch (VirtualItemNotFoundException* e) {
            NSLog(@"Couldn't find a VirtualItem with itemId: %@.", itemIdS);
            return EXCEPTION_ITEM_NOT_FOUND;
        }
        
        return NO_ERR;
    }
    
}