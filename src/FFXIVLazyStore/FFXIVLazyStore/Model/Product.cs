namespace FFXIVLazyStore.Model
{
    public class ProductVM
    {
        public int status { get; set; }
        public Product product { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string SkuId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public int DistributionType { get; set; }
        public bool IsGift { get; set; }
        public bool IsOnlyOne { get; set; }
        public bool FreeTrialBuyable { get; set; }
        public string Currency { get; set; } = string.Empty;
        public int Price { get; set; }
        public string PriceText { get; set; } = string.Empty;
        public int? SalePrice { get; set; }
        public string? SalePriceText { get; set; }
        public int CartLimit { get; set; }
        public int? Ranking { get; set; }
        public string? TopLeftIcon { get; set; }
        public string ServiceUrl { get; set; } = string.Empty;
        public bool ToFriend { get; set; }
        public List<ProductItem> items { get; set; }
        public bool IsFavorite { get; set; }
    }

    public class ProductItem
    {
        public string name { get; set; }
        public int number { get; set; }
        public string uriKey { get; set; }
    }

    public class FavoriteProduct
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
    }
}
