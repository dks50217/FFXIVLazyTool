namespace FFXIVLazyStore.Model
{
    public class CategoryVM
    {
        public int status { get; set; }
        public Meta meta { get; set; }
        public List<Category> categories { get; set; }
    }

    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<SubCategory> subCategories { get; set; }
        public List<Filter> filters { get; set; }
    }

    public class Filter
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool isNew { get; set; }
        public bool isSale { get; set; }
        public bool isHot { get; set; }
    }

    public class Meta
    {
        public List<Filter> filters { get; set; }
    }

    public class SubCategory
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<Filter> filters { get; set; }
    }
}
