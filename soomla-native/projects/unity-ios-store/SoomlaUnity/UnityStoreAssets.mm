#import "UnityStoreAssets.h"
#import "StoreJSONConsts.h"
#import "VirtualCurrencyPack.h"
#import "VirtualCurrency.h"
#import "VirtualGood.h"
#import "VirtualCategory.h"
#import "NonConsumableItem.h"
#import "SingleUseVG.h"
#import "LifetimeVG.h"
#import "EquippableVG.h"
#import "SingleUsePackVG.h"
#import "UpgradeVG.h"
#import "SoomlaUtils.h"
#import "StoreInfo.h"

extern "C"{
	void storeAssets_Init(int version, const char* storeAssetsJSON){
		NSString* storeAssetsJSONS = [NSString stringWithUTF8String:storeAssetsJSON];
		[UnityStoreAssets createFromJSON:storeAssetsJSONS andVersion:version];
	}
    
    void storeAssets_Save(const char* type, const char* viJSON) {
        NSString* viJSONS = [NSString stringWithUTF8String:viJSON];
        NSString* typeS = [NSString stringWithUTF8String:type];
        NSDictionary* itemDict = [SoomlaUtils jsonStringToDict:viJSONS];
        
        if ([typeS isEqualToString:@"EquippableVG"]) {
            [[StoreInfo getInstance] save:[[EquippableVG alloc] initWithDictionary:itemDict]];
            
        } else if ([typeS isEqualToString:@"LifetimeVG"]) {
            [[StoreInfo getInstance] save:[[LifetimeVG alloc] initWithDictionary:itemDict]];
            
        } else if ([typeS isEqualToString:@"SingleUsePackVG"]) {
            [[StoreInfo getInstance] save:[[SingleUsePackVG alloc] initWithDictionary:itemDict]];
            
        } else if ([typeS isEqualToString:@"SingleUseVG"]) {
            [[StoreInfo getInstance] save:[[SingleUseVG alloc] initWithDictionary:itemDict]];
            
        } else if ([typeS isEqualToString:@"UpgradeVG"]) {
            [[StoreInfo getInstance] save:[[UpgradeVG alloc] initWithDictionary:itemDict]];
            
        } else if ([typeS isEqualToString:@"VirtualCurrency"]) {
            [[StoreInfo getInstance] save:[[VirtualCurrency alloc] initWithDictionary:itemDict]];
            
        } else if ([typeS isEqualToString:@"VirtualCurrencyPack"]) {
            [[StoreInfo getInstance] save:[[VirtualCurrencyPack alloc] initWithDictionary:itemDict]];
            
        } else if ([typeS isEqualToString:@"NonConsumableItem"]) {
            [[StoreInfo getInstance] save:[[NonConsumableItem alloc] initWithDictionary:itemDict]];

        } else {
            LogError(@"SOOMLA UnityStoreAssets", ([NSString stringWithFormat:@"Don't understand what's the type of the item i need to save... type: %@",typeS]));
        }
    }
}

@implementation UnityStoreAssets

static int version;
static NSMutableArray* virtualCurrenciesArray;
static NSMutableArray* virtualGoodsArray;
static NSMutableArray* virtualCurrencyPacksArray;
static NSMutableArray* virtualCategoriesArray;
static NSMutableArray* nonConsumablesArray;

static NSString* TAG = @"SOOMLA UnityStoreAssets";

+ (UnityStoreAssets*)getInstance {
    static UnityStoreAssets* instance = nil;
    if (!instance) {
        instance = [[UnityStoreAssets alloc] init];
    }
    
    return instance;
}

+ (BOOL)createFromJSON:(NSString*)storeAssetsJSON andVersion:(int)oVersion {
    LogDebug(TAG, ([NSString stringWithFormat:@"the storeAssets json is %@", storeAssetsJSON]));
   
    @try {

        NSDictionary* storeInfo = [SoomlaUtils jsonStringToDict:storeAssetsJSON];
        
        NSMutableArray* currencies = [[[NSMutableArray alloc] init] autorelease];
        NSArray* currenciesDicts = [storeInfo objectForKey:JSON_STORE_CURRENCIES];
        for(NSDictionary* currencyDict in currenciesDicts){
            VirtualCurrency* o = [[VirtualCurrency alloc] initWithDictionary: currencyDict];
            [currencies addObject:o];
            [o release];
        }
        if (virtualCurrenciesArray) {
            [virtualCurrenciesArray release];
            virtualCurrenciesArray = nil;
        }
        virtualCurrenciesArray = currencies;
        
        NSMutableArray* currencyPacks = [[[NSMutableArray alloc] init] autorelease];
        NSArray* currencyPacksDicts = [storeInfo objectForKey:JSON_STORE_CURRENCYPACKS];
        for(NSDictionary* currencyPackDict in currencyPacksDicts){
            VirtualCurrencyPack* o = [[VirtualCurrencyPack alloc] initWithDictionary: currencyPackDict];
            [currencyPacks addObject:o];
            [o release];
        }
        if (virtualCurrencyPacksArray) {
            [virtualCurrencyPacksArray release];
            virtualCurrencyPacksArray = nil;
        }
        virtualCurrencyPacksArray = currencyPacks;
        
        
        NSDictionary* goodsDict = [storeInfo objectForKey:JSON_STORE_GOODS];
        NSArray* suGoods = [goodsDict objectForKey:JSON_STORE_GOODS_SU];
        NSArray* ltGoods = [goodsDict objectForKey:JSON_STORE_GOODS_LT];
        NSArray* eqGoods = [goodsDict objectForKey:JSON_STORE_GOODS_EQ];
        NSArray* upGoods = [goodsDict objectForKey:JSON_STORE_GOODS_UP];
        NSArray* paGoods = [goodsDict objectForKey:JSON_STORE_GOODS_PA];
        NSMutableArray* goods = [[[NSMutableArray alloc] init] autorelease];
        for(NSDictionary* gDict in suGoods){
            SingleUseVG* g = [[SingleUseVG alloc] initWithDictionary: gDict];
			[goods addObject:g];
            [g release];
        }
        for(NSDictionary* gDict in ltGoods){
            LifetimeVG* g = [[LifetimeVG alloc] initWithDictionary: gDict];
			[goods addObject:g];
            [g release];
        }
        for(NSDictionary* gDict in eqGoods){
            EquippableVG* g = [[EquippableVG alloc] initWithDictionary: gDict];
			[goods addObject:g];
        }
        for(NSDictionary* gDict in upGoods){
            UpgradeVG* g = [[UpgradeVG alloc] initWithDictionary: gDict];
			[goods addObject:g];
            [g release];
        }
        for(NSDictionary* gDict in paGoods){
            SingleUsePackVG* g = [[SingleUsePackVG alloc] initWithDictionary: gDict];
			[goods addObject:g];
            [g release];
        }
        if (virtualGoodsArray) {
            [virtualGoodsArray release];
            virtualGoodsArray = nil;
        }
        virtualGoodsArray = goods;
        
        NSMutableArray* categories = [[[NSMutableArray alloc] init] autorelease];
        NSArray* categoriesDicts = [storeInfo objectForKey:JSON_STORE_CATEGORIES];
        for(NSDictionary* categoryDict in categoriesDicts){
            VirtualCategory* c = [[VirtualCategory alloc] initWithDictionary: categoryDict];
            [categories addObject:c];
            [c release];
        }
        if (virtualCategoriesArray) {
            [virtualCategoriesArray release];
            virtualCategoriesArray = nil;
        }
        virtualCategoriesArray = categories;
        
        NSMutableArray* nonConsumables = [[[NSMutableArray alloc] init] autorelease];
        NSArray* nonConsumableItemsDict = [storeInfo objectForKey:JSON_STORE_NONCONSUMABLES];
        for(NSDictionary* nonConsumableItemDict in nonConsumableItemsDict){
            NonConsumableItem* non = [[NonConsumableItem alloc] initWithDictionary:nonConsumableItemDict];
            [nonConsumables addObject:non];
            [non release];
        }
        if (nonConsumablesArray) {
            [nonConsumablesArray release];
            nonConsumablesArray = nil;
        }
        nonConsumablesArray = nonConsumables;
        
        version = oVersion;
        
        return YES;
    } @catch (NSException* ex) {
        LogError(TAG, @"An error occured while trying to parse store assets JSON.");
    }
    
    return NO;
}

- (int)getVersion{
	return version;
}

- (NSArray*)virtualCurrencies{
    return virtualCurrenciesArray;
}

- (NSArray*)virtualGoods{
    return virtualGoodsArray;
}

- (NSArray*)virtualCurrencyPacks{
    return virtualCurrencyPacksArray;
}

- (NSArray*)virtualCategories{
    return virtualCategoriesArray;
}

- (NSArray*)nonConsumableItems {
    return nonConsumablesArray;
}

- (void)dealloc {
//    [virtualCurrenciesArray release];
//    virtualCurrenciesArray = nil;
//    [virtualGoodsArray release];
//    virtualGoodsArray = nil;
//    [virtualCurrencyPacksArray release];
//    virtualCurrencyPacksArray = nil;
//    [virtualCategoriesArray release];
//    virtualCategoriesArray = nil;
//    [nonConsumablesArray release];
//    nonConsumablesArray = nil;
    [super dealloc];
}

@end
