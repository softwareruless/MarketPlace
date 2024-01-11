using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MarketPlace.Data.Model
{
    public class UserLoginModel
    {
        [DisplayName("Email Address")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        [MaxLength(100, ErrorMessage = "{0} adı {1} karakterden fazla olamaz")]
        [MinLength(10, ErrorMessage = "{0} adı {1} karakterden az olamaz")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Password")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        [MaxLength(30, ErrorMessage = "{0} adı {1} karakterden fazla olamaz")]
        [MinLength(5, ErrorMessage = "{0} adı {1} karakterden az olamaz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class UserTokenModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string FullName { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
        public DateTime Expiration { get; set; }
    }

    public class RefreshTokenModel
    {
        //[Required]
        public string Token { get; set; }
    }
}
