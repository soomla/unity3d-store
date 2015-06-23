/* 
    Copyright (c) 2012 Microsoft Corporation.  All rights reserved.
    Use of this sample source code is subject to the terms of the Microsoft license 
    agreement under which you licensed this sample source code and is provided AS-IS.
    If you did not accept the terms of the license agreement, you are not authorized 
    to use this sample source code.  For the terms of the license, please see the 
    license agreement between you and Microsoft.
  
    To see all Code Samples for Windows Phone, visit http://go.microsoft.com/fwlink/?LinkID=219604 
  
*/
using System;
using System.Collections.Generic;
using System.Linq;


namespace SoomlaWpStore.billing.wp.store
{
    public class MarketProductInfos
    {
        
        public enum MarketProductType
        {
            DURABLE,
            CONSUMABLE,
            UNKNOWN
        }
        // Summary:
        //     Gets the description for the product.
        //
        // Returns:
        //     The description for the product.
        public string Description { get; set; }
        //
        // Summary:
        //     Gets the app's purchase price with the appropriate formatting for the current
        //     market.
        //
        // Returns:
        //     The app's purchase price with the appropriate formatting for the current
        //     market.
        public string FormattedPrice { get; set; }
        //
        // Summary:
        //     Gets the URI of the image associated with this product.
        //
        // Returns:
        //     The URI of the image associated with this product.
        public Uri ImageUri { get; set; }
        //
        // Summary:
        //     Gets the lsit of keywords associated with this product. These keywords are
        //     useful for filtering product lists by keyword, for example, when calling
        //     LoadListingInformationByKeywordsAsync.
        //
        // Returns:
        //     The keywords associated with this product.
        public IEnumerable<string> Keywords { get; set; }
        //
        // Summary:
        //     Gets the descriptive name of the product or feature that can be shown to
        //     customers in the current market.
        //
        // Returns:
        //     The feature's descriptive name as it is seen by customers in the current
        //     market.
        public string Name { get; set; }
        //
        // Summary:
        //     Gets the ID of an app's feature or product.
        //
        // Returns:
        //     The ID of an app's feature.
        public string ProductId { get; set; }
        //
        // Summary:
        //     Gets the type of this product. The type can be either ProductType.Durable
        //     or ProductType.Consumable.
        //
        // Returns:
        //     The type of this product.
        public MarketProductType ProductType { get; set; }
        //
        // Summary:
        //     Gets the tag string that contains custom information about this product.
        //
        // Returns:
        //     The tag for this product.
        public string Tag { get; set; }

    }
}
