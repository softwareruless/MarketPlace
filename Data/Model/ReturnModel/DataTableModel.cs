namespace MarketPlace.Data.Model.ReturnModel
{
    public class DatatableModel<T>
    {
        public string draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public List<T> data { get; set; }
    }

    public class DatatableResponseModel<T> : ResponseModel
    {
        public DatatableModel<T> Data { get; set; }
    }
}
