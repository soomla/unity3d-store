
#import "UnityStoreEventDispatcher.h"
#import "EventHandling.h"
#import "AppStoreItem.h"
#import "VirtualGood.h"
#import "VirtualCurrency.h"
#import "EquippableVG.h"
#import "UpgradeVG.h"
#import "PurchasableVirtualItem.h"

@implementation UnityStoreEventDispatcher

- (id) init {
    if (self = [super init]) {
        [EventHandling observeAllEventsWithObserver:self withSelector:@selector(handleEvent:)];
    }
    
    return self;
}

- (void)handleEvent:(NSNotification*)notification{
	
	if ([notification.name isEqualToString:EVENT_BILLING_NOT_SUPPORTED]) {
	        UnitySendMessage("Soomla", "onBillingNotSupported", "");
	} 
	else if ([notification.name isEqualToString:EVENT_BILLING_SUPPORTED]) {
	    UnitySendMessage("Soomla", "onBillingSupported", "");
	}
	else if ([notification.name isEqualToString:EVENT_CLOSING_STORE]) {
	    UnitySendMessage("Soomla", "onClosingStore", "");
	}
	else if ([notification.name isEqualToString:EVENT_CURRENCY_BALANCE_CHANGED]) {
	    NSDictionary* userInfo = [notification userInfo];
	    VirtualCurrency* vc = [userInfo objectForKey:DICT_ELEMENT_CURRENCY];
		int balance = [(NSNumber*)[userInfo objectForKey:DICT_ELEMENT_BALANCE] intValue];
		int added = [(NSNumber*)[userInfo objectForKey:DICT_ELEMENT_AMOUNT_ADDED] intValue];
	    UnitySendMessage("Soomla", "onCurrencyBalanceChanged", [[NSString stringWithFormat:@"%@#SOOM#%d#SOOM#%d", vc.itemId, balance, added] UTF8String]);
	}
	else if ([notification.name isEqualToString:EVENT_GOOD_BALANCE_CHANGED]) {
	    NSDictionary* userInfo = [notification userInfo];
	    VirtualGood* vg = [userInfo objectForKey:DICT_ELEMENT_GOOD];
		int balance = [(NSNumber*)[userInfo objectForKey:DICT_ELEMENT_BALANCE] intValue];
		int added = [(NSNumber*)[userInfo objectForKey:DICT_ELEMENT_AMOUNT_ADDED] intValue];
        UnitySendMessage("Soomla", "onGoodBalanceChanged", [[NSString stringWithFormat:@"%@#SOOM#%d#SOOM#%d", vg.itemId, balance, added] UTF8String]);
    }
	else if ([notification.name isEqualToString:EVENT_GOOD_EQUIPPED]) {
        EquippableVG* vg = [notification.userInfo objectForKey:DICT_ELEMENT_EquippableVG];
        UnitySendMessage("Soomla", "onGoodEquipped", [vg.itemId UTF8String]);
    }
	else if ([notification.name isEqualToString:EVENT_GOOD_UNEQUIPPED]) {
        EquippableVG* vg = [notification.userInfo objectForKey:DICT_ELEMENT_EquippableVG];
        UnitySendMessage("Soomla", "onGoodUnequipped", [vg.itemId UTF8String]);
    }
	else if ([notification.name isEqualToString:EVENT_GOOD_UPGRADE]) {
		VirtualGood* vg = [notification.userInfo objectForKey:DICT_ELEMENT_GOOD];
        UpgradeVG* vgu = [notification.userInfo objectForKey:DICT_ELEMENT_UpgradeVG];
        UnitySendMessage("Soomla", "onGoodUpgrade", [[NSString stringWithFormat:@"%@#SOOM#%@", vg.itemId, vgu.itemId] UTF8String]);
    }
	else if ([notification.name isEqualToString:EVENT_ITEM_PURCHASED]) {
        PurchasableVirtualItem* pvi = [notification.userInfo objectForKey:DICT_ELEMENT_PURCHASABLE];
        UnitySendMessage("Soomla", "onItemPurchased", [pvi.itemId UTF8String]);
    }
	else if ([notification.name isEqualToString:EVENT_ITEM_PURCHASE_STARTED]) {
		PurchasableVirtualItem* pvi = [notification.userInfo objectForKey:DICT_ELEMENT_PURCHASABLE];
        UnitySendMessage("Soomla", "onItemPurchaseStarted", [pvi.itemId UTF8String]);
    }
	else if ([notification.name isEqualToString:EVENT_OPENING_STORE]) {
        UnitySendMessage("Soomla", "onOpeningStore", "");
    }
	else if ([notification.name isEqualToString:EVENT_APPSTORE_PURCHASE_CANCELLED]) {
        PurchasableVirtualItem* pvi = [notification.userInfo objectForKey:DICT_ELEMENT_PURCHASABLE];
        UnitySendMessage("Soomla", "onMarketPurchaseCancelled", [pvi.itemId UTF8String]);
    }
	else if ([notification.name isEqualToString:EVENT_APPSTORE_PURCHASED]) {
        PurchasableVirtualItem* pvi = [notification.userInfo objectForKey:DICT_ELEMENT_PURCHASABLE];
        NSString *receipt = [notification.userInfo objectForKey:DICT_ELEMENT_RECEIPT];
        UnitySendMessage("Soomla", "onMarketPurchase", [[NSString stringWithFormat:@"%@#SOOM#%@", pvi.itemId, receipt] UTF8String]);
    }
    else if ([notification.name isEqualToString:EVENT_APPSTORE_PURCHASE_STARTED]) {
	    PurchasableVirtualItem* pvi = [notification.userInfo objectForKey:DICT_ELEMENT_PURCHASABLE];
	    UnitySendMessage("Soomla", "onMarketPurchaseStarted", [pvi.itemId UTF8String]);
	}
    else if ([notification.name isEqualToString:EVENT_TRANSACTION_RESTORED]) {
		NSNumber* successNum = [notification.userInfo objectForKey:DICT_ELEMENT_SUCCESS];
        UnitySendMessage("Soomla", "onRestoreTransactions", [[NSString stringWithFormat:@"%d", [successNum intValue]] UTF8String]);
    }
    else if ([notification.name isEqualToString:EVENT_TRANSACTION_RESTORE_STARTED]) {
        UnitySendMessage("Soomla", "onRestoreTransactionsStarted", "");
    }
    else if ([notification.name isEqualToString:EVENT_UNEXPECTED_ERROR_IN_STORE]) {
        UnitySendMessage("Soomla", "onUnexpectedErrorInStore", "");
    }
}

@end
