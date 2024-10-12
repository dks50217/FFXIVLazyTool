namespace FFXIVLazyStore.Model
{
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
    }
}
