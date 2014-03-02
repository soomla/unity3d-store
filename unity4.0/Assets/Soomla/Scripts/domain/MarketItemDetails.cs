using UnityEngine;
using System.Collections;

namespace Soomla
{
    public class MarketItemDetails
    {
        public string ProductId
        {
            get;
            private set;
        }
        public string Price
        {
            get;
            private set;
        }
        public string Title
        {
            get;
            private set;
        }
        public string Description
        {
            get;
            private set;
        }

        public MarketItemDetails(string productId, string price, string title, string description)
        {
            ProductId = productId;
            Price = price;
            Title = title;
            Description = description;
        }
    }
}
