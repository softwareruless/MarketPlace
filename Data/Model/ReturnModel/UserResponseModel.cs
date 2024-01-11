namespace MarketPlace.Data.Model.ReturnModel
{
    public class UsersDetailResponseModel : ResponseModel
    {
        public List<UserDetail> Users { get; set; }
    }

    public class UserDetailResponseModel : ResponseModel
    {
        public UserDetail User { get; set; }
    }

    public class UserDetail
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string CreatedDate { get; set; }
        public bool EmailConfirmed { get; set; }
    }

    
}
