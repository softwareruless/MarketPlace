namespace MarketPlace.Service
{
    public interface IConcurrencyService
    {
        SemaphoreSlim SemaphorePerUser(int userId);
    }
}
