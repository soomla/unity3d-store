
#import "UnityStoreEventDispatcher.h"
#import "StoreEventHandling.h"
#import "MarketItem.h"
#import "VirtualGood.h"
#import "VirtualCurrency.h"
#import "EquippableVG.h"
#import "UpgradeVG.h"
#import "PurchasableVirtualItem.h"
#import "SoomlaUtils.h"

static UnityStoreEventDispatcher* instance = nil;

extern "C"{
    void eventDispatcher_Init() {
        LogDebug(@"SOOMLA Unity UnityStoreEventDispatcher", @"Initializing StoreEventHandler ...");
        [UnityStoreEventDispatcher initialize];
    }
    
    void eventDispatcher_PushEventSoomlaStoreInitialized(const char* message) {
        [[NSNotificationCenter defaultCenter] postNotificationName:EVENT_SOOMLASTORE_INIT object:instance userInfo:nil];
    }
    
    void eventDispatcher_PushEventUnexpectedStoreError(const char *message) {
        NSString* messageS = [NSString stringWithUTF8String:message];
        NSDictionary* eventJSON = [SoomlaUtils jsonStringToDict:messageS];
        NSDictionary *userInfo = @{ DICT_ELEMENT_ERROR_CODE: eventJSON[@"errorCode"]};
        [[NSNotificationCenter defaultCenter] postNotificationName:EVENT_UNEXPECTED_STORE_ERROR object:instance userInfo:userInfo];
    }
    
    void eventDispatcher_PushEventCurrencyBalanceChanged(const char* message) {
        
        NSString* messageS = [NSString stringWithUTF8String:message];
        NSDictionary* eventJSON = [SoomlaUtils jsonStringToDict:messageS];
        NSDictionary *userInfo = @{ DICT_ELEMENT_BALANCE: [eventJSON objectForKey:@"balance"],
                                    DICT_ELEMENT_CURRENCY: [eventJSON objectForKey:@"itemId"],
                                    DICT_ELEMENT_AMOUNT_ADDED: [eventJSON objectForKey:@"amountAdded"]};
        
        [[NSNotificationCenter defaultCenter] postNotificationName:EVENT_CURRENCY_BALANCE_CHANGED object:instance userInfo:userInfo];
    }
    
    void eventDispatcher_PushEventGoodBalanceChanged(const char* message) {
        NSString* messageS = [NSString stringWithUTF8String:message];
        NSDictionary* eventJSON = [SoomlaUtils jsonStringToDict:messageS];
        NSDictionary *userInfo = @{ DICT_ELEMENT_BALANCE: [eventJSON objectForKey:@"balance"],
                                    DICT_ELEMENT_GOOD: [eventJSON objectForKey:@"itemId"],
                                    DICT_ELEMENT_AMOUNT_ADDED: [eventJSON objectForKey:@"amountAdded"]};
        
        [[NSNotificationCenter defaultCenter] postNotificationName:EVENT_GOOD_BALANCE_CHANGED object:instance userInfo:userInfo];
    }
    
    void eventDispatcher_PushEventGoodEquipped(const char* message) {
        NSString* messageS = [NSString stringWithUTF8String:message];
        NSDictionary* eventJSON = [SoomlaUtils jsonStringToDict:messageS];
        NSDictionary *userInfo = @{ DICT_ELEMENT_EquippableVG: [eventJSON objectForKey:@"itemId"] };
        
        [[NSNotificationCenter defaultCenter] postNotificationName:EVENT_GOOD_EQUIPPED object:instance userInfo:userInfo];
    }
    
    void eventDispatcher_PushEventGoodUnEquipped(const char* message) {
        NSString* messageS = [NSString stringWithUTF8String:message];
        NSDictionary* eventJSON = [SoomlaUtils jsonStringToDict:messageS];
        NSDictionary *userInfo = @{ DICT_ELEMENT_EquippableVG: [eventJSON objectForKey:@"itemId"] };
        
        [[NSNotificationCenter defaultCenter] postNotificationName:EVENT_GOOD_UNEQUIPPED object:instance userInfo:userInfo];
    }
    
    void eventDispatcher_PushEventGoodUpgrade(const char* message) {
        NSString* messageS = [NSString stringWithUTF8String:message];
        NSDictionary* eventJSON = [SoomlaUtils jsonStringToDict:messageS];
        NSDictionary *userInfo = @{ DICT_ELEMENT_GOOD: [eventJSON objectForKey:@"itemId"], DICT_ELEMENT_UpgradeVG: [eventJSON objectForKey:@"upgradeItemId"] };
        
        [[NSNotificationCenter defaultCenter] postNotificationName:EVENT_GOOD_UPGRADE object:instance userInfo:userInfo];
    }
    
    void eventDispatcher_PushEventItemPurchased(const char* message) {
        NSString* messageS = [NSString stringWithUTF8String:message];
        NSDictionary* eventJSON = [SoomlaUtils jsonStringToDict:messageS];
        NSDictionary *userInfo = @{ DICT_ELEMENT_PURCHASABLE_ID: [eventJSON objectForKey:@"itemId"], DICT_ELEMENT_DEVELOPERPAYLOAD: [eventJSON objectForKey:@"payload"] };

        [[NSNotificationCenter defaultCenter] postNotificationName:EVENT_ITEM_PURCHASED object:instance userInfo:userInfo];
    }
    
    void eventDispatcher_PushEventItemPurchaseStarted(const char* message) {
        NSString* messageS = [NSString stringWithUTF8String:message];
        NSDictionary* eventJSON = [SoomlaUtils jsonStringToDict:messageS];
        NSDictionary *userInfo = @{ DICT_ELEMENT_PURCHASABLE_ID: [eventJSON objectForKey:@"itemId"] };
        
        [[NSNotificationCenter defaultCenter] postNotificationName:EVENT_ITEM_PURCHASE_STARTED object:instance userInfo:userInfo];
    }
    
    void eventDispatcher_PushEventMarketPurchaseVerified(const char* message) {
        NSString* messageS = [NSString stringWithUTF8String:message];
        NSDictionary* eventJSON = [SoomlaUtils jsonStringToDict:messageS];
        NSDictionary *userInfo = @{ DICT_ELEMENT_PURCHASABLE_ID: [eventJSON objectForKey:@"itemId"] };
        
        [[NSNotificationCenter defaultCenter] postNotificationName:EVENT_ITEM_PURCHASE_STARTED object:instance userInfo:userInfo];
    }
}

@implementation UnityStoreEventDispatcher

+ (void)initialize {
    if (!instance) {
        instance = [[UnityStoreEventDispatcher alloc] init];
    }
}

- (id) init {
    if (self = [super init]) {
        [StoreEventHandling observeAllEventsWithObserver:self withSelector:@selector(handleEvent:)];
    }

    return self;
}

- (void)handleEvent:(NSNotification*)notification{
    if (notification.object == self) {
        return;
    }
    
	if ([notification.name isEqualToString:EVENT_BILLING_NOT_SUPPORTED]) {
	        UnitySendMessage("StoreEvents", "onBillingNotSupported", "");
	}
	else if ([notification.name isEqualToString:EVENT_BILLING_SUPPORTED]) {
	    UnitySendMessage("StoreEvents", "onBillingSupported", "");
	}
	else if ([notification.name isEqualToString:EVENT_CURRENCY_BALANCE_CHANGED]) {
	    NSDictionary* userInfo = [notification userInfo];
        NSString* jsonStr = [SoomlaUtils dictToJsonString:@{
                                        @"itemId": [userInfo objectForKey:DICT_ELEMENT_CURRENCY],
                                        @"balance": (NSNumber*)[userInfo objectForKey:DICT_ELEMENT_BALANCE],
                                        @"amountAdded": (NSNumber*)[userInfo objectForKey:DICT_ELEMENT_AMOUNT_ADDED]
                                        }];
	    UnitySendMessage("StoreEvents", "onCurrencyBalanceChanged", [jsonStr UTF8String]);
	}
	else if ([notification.name isEqualToString:EVENT_GOOD_BALANCE_CHANGED]) {
        NSDictionary* userInfo = [notification userInfo];
        NSString* jsonStr = [SoomlaUtils dictToJsonString:@{
                                                            @"itemId": [userInfo objectForKey:DICT_ELEMENT_GOOD],
                                                            @"balance": (NSNumber*)[userInfo objectForKey:DICT_ELEMENT_BALANCE],
                                                            @"amountAdded": (NSNumber*)[userInfo objectForKey:DICT_ELEMENT_AMOUNT_ADDED]
                                                            }];
        UnitySendMessage("StoreEvents", "onGoodBalanceChanged", [jsonStr UTF8String]);
    }
	else if ([notification.name isEqualToString:EVENT_GOOD_EQUIPPED]) {
        NSDictionary* userInfo = [notification userInfo];
        NSString* jsonStr = [SoomlaUtils dictToJsonString:@{
                                                            @"itemId": [userInfo objectForKey:DICT_ELEMENT_EquippableVG]
                                                            }];
        UnitySendMessage("StoreEvents", "onGoodEquipped", [jsonStr UTF8String]);
    }
	else if ([notification.name isEqualToString:EVENT_GOOD_UNEQUIPPED]) {
        NSDictionary* userInfo = [notification userInfo];
        NSString* jsonStr = [SoomlaUtils dictToJsonString:@{
                                                            @"itemId": [userInfo objectForKey:DICT_ELEMENT_EquippableVG]
                                                            }];
        UnitySendMessage("StoreEvents", "onGoodUnequipped", [jsonStr UTF8String]);
    }
	else if ([notification.name isEqualToString:EVENT_GOOD_UPGRADE]) {
        NSDictionary* userInfo = [notification userInfo];
        NSString* jsonStr = [SoomlaUtils dictToJsonString:@{
                                                            @"itemId": [userInfo objectForKey:DICT_ELEMENT_GOOD],
                                                            @"upgradeItemId": [userInfo objectForKey:DICT_ELEMENT_UpgradeVG]
                                                            }];
        UnitySendMessage("StoreEvents", "onGoodUpgrade", [jsonStr UTF8String]);
    }
	else if ([notification.name isEqualToString:EVENT_ITEM_PURCHASED]) {
        NSDictionary* userInfo = [notification userInfo];
        NSString* jsonStr = [SoomlaUtils dictToJsonString:@{
                                                            @"itemId": [userInfo objectForKey:DICT_ELEMENT_PURCHASABLE_ID],
                                                            @"payload": [userInfo objectForKey:DICT_ELEMENT_DEVELOPERPAYLOAD]
                                                            }];
        UnitySendMessage("StoreEvents", "onItemPurchased", [jsonStr UTF8String]);
    }
	else if ([notification.name isEqualToString:EVENT_ITEM_PURCHASE_STARTED]) {
        NSDictionary* userInfo = [notification userInfo];
        NSString* jsonStr = [SoomlaUtils dictToJsonString:@{
                                                            @"itemId": [userInfo objectForKey:DICT_ELEMENT_PURCHASABLE_ID]
                                                            }];
        UnitySendMessage("StoreEvents", "onItemPurchaseStarted", [jsonStr UTF8String]);
    }
	else if ([notification.name isEqualToString:EVENT_MARKET_PURCHASE_CANCELLED]) {
        NSDictionary* userInfo = [notification userInfo];
        PurchasableVirtualItem* pvi = (PurchasableVirtualItem*)[userInfo objectForKey:DICT_ELEMENT_PURCHASABLE];
        NSString* jsonStr = [SoomlaUtils dictToJsonString:@{
                                                            @"itemId": pvi.itemId
                                                            }];
        UnitySendMessage("StoreEvents", "onMarketPurchaseCancelled", [jsonStr UTF8String]);
    }
	else if ([notification.name isEqualToString:EVENT_MARKET_PURCHASED]) {
        NSDictionary* userInfo = [notification userInfo];
        PurchasableVirtualItem* pvi = (PurchasableVirtualItem*)[userInfo objectForKey:DICT_ELEMENT_PURCHASABLE];
        NSString* jsonStr = [SoomlaUtils dictToJsonString:@{
                                                            @"itemId": pvi.itemId,
                                                            @"payload": [userInfo objectForKey:DICT_ELEMENT_DEVELOPERPAYLOAD],
                                                            @"extra": [userInfo objectForKey:DICT_ELEMENT_EXTRA_INFO]
                                                            }];
        UnitySendMessage("StoreEvents", "onMarketPurchase", [jsonStr UTF8String]);
    }
    else if ([notification.name isEqualToString:EVENT_MARKET_PURCHASE_STARTED]) {
        NSDictionary* userInfo = [notification userInfo];
        PurchasableVirtualItem* pvi = (PurchasableVirtualItem*)[userInfo objectForKey:DICT_ELEMENT_PURCHASABLE];
        NSString* jsonStr = [SoomlaUtils dictToJsonString:@{
                                                            @"itemId": pvi.itemId
                                                            }];
        UnitySendMessage("StoreEvents", "onMarketPurchaseStarted", [jsonStr UTF8String]);
	}
    else if ([notification.name isEqualToString:EVENT_RESTORE_TRANSACTIONS_FINISHED]) {
        NSDictionary* userInfo = [notification userInfo];
        NSString* jsonStr = [SoomlaUtils dictToJsonString:@{
                                                            @"success": [userInfo objectForKey:DICT_ELEMENT_SUCCESS]
                                                            }];
        UnitySendMessage("StoreEvents", "onRestoreTransactionsFinished", [jsonStr UTF8String]);
    }
    else if ([notification.name isEqualToString:EVENT_RESTORE_TRANSACTIONS_STARTED]) {
        UnitySendMessage("StoreEvents", "onRestoreTransactionsStarted", "");
    }
    else if ([notification.name isEqualToString:EVENT_MARKET_ITEMS_REFRESH_STARTED]) {
        UnitySendMessage("StoreEvents", "onMarketItemsRefreshStarted", "");
    }
    else if ([notification.name isEqualToString:EVENT_MARKET_ITEMS_REFRESH_FINISHED]) {
        NSArray* marketItems = [notification.userInfo objectForKey:DICT_ELEMENT_MARKET_ITEMS];
        NSMutableArray* eventJSON = [NSMutableArray array];
        for(MarketItem* mi in marketItems) {
            NSDictionary* micJSON = @{
                                      @"productId": mi.productId,
                                      @"marketPrice": (mi.marketPriceAndCurrency ?: @""),
                                      @"marketTitle": (mi.marketTitle ?: @""),
                                      @"marketDesc": (mi.marketDescription ?: @""),
                                      @"marketCurrencyCode": (mi.marketCurrencyCode ?: @""),
                                      @"marketPriceMicros": @(mi.marketPriceMicros)
                                      };
            [eventJSON addObject:micJSON];
        }
        NSString* jsonStr = [SoomlaUtils arrayToJsonString:eventJSON];
        UnitySendMessage("StoreEvents", "onMarketItemsRefreshFinished", [jsonStr UTF8String]);
    }
    else if ([notification.name isEqualToString:EVENT_MARKET_ITEMS_REFRESH_FAILED]) {
        NSDictionary* userInfo = [notification userInfo];
        NSString* jsonStr = [SoomlaUtils dictToJsonString:@{
                @"errorMessage": ([userInfo objectForKey:DICT_ELEMENT_ERROR_MESSAGE] ?: @"")
        }];
        UnitySendMessage("StoreEvents", "onMarketItemsRefreshFailed", [jsonStr UTF8String]);
    }
    else if ([notification.name isEqualToString:EVENT_UNEXPECTED_STORE_ERROR]) {
        NSDictionary* userInfo = [notification userInfo];
        NSString* jsonStr = [SoomlaUtils dictToJsonString:@{
                                                      @"errorCode": (userInfo[DICT_ELEMENT_ERROR_CODE] ?: @0)
                                                      }];
        UnitySendMessage("StoreEvents", "onUnexpectedStoreError", [jsonStr UTF8String]);
    }
    else if ([notification.name isEqualToString:EVENT_SOOMLASTORE_INIT]) {
        UnitySendMessage("StoreEvents", "onSoomlaStoreInitialized", "");
    }
}

@end
