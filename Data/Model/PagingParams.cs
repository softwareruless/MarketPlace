namespace MarketPlace.Data.Model
{
    public class PagingParams
    {
        public string? searchValue { get; set; }
        public int? pageSize { get; set; }
        public int? skip { get; set; }
        public int? recordsTotal { get; set; }
        public string? draw { get; set; }
    }
}
