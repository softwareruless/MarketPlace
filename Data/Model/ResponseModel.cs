namespace MarketPlace.Data.Model
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class ListResponseModel<T> : ResponseModel
    {
        public List<T> Data { get; set; }
    }

    public class ResponseModel<T> : ResponseModel
    {
        public T Data { get; set; }
    }
}
