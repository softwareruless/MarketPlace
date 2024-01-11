namespace MarketPlace.Data.Entities
{
    public class EmailVerification : BaseEntity
    {
        public string? Email { get; set; }
        public string? Code { get; set; }
        public int TryCount { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
