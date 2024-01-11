using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MarketPlace.Data.Model
{
    public class UserAddModel
    {
        [DisplayName("Name")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        [MaxLength(50, ErrorMessage = "{0} adı {1} karakterden fazla olamaz")]
        [MinLength(3, ErrorMessage = "{0} adı {1} karakterden az olamaz")]
        public string FullName { get; set; }
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

    public class UpdateNameModel
    {
        [Required]
        public string Name { get; set; }
    }

    public class UpdateEmailModel
    {
        [Display(Name = "E-Posta Adresiniz")]
        [Required(ErrorMessage = "Lütfen e-posta adresinizi boş geçmeyiniz.")]
        [EmailAddress(ErrorMessage = "Lütfen uygun formatta e-posta giriniz.")]
        public string OldEmail { get; set; }

        [Display(Name = "E-Posta Adresiniz")]
        [Required(ErrorMessage = "Lütfen e-posta adresinizi boş geçmeyiniz.")]
        [EmailAddress(ErrorMessage = "Lütfen uygun formatta e-posta giriniz.")]
        public string NewEmail { get; set; }
        [Required]
        public string Code { get; set; }
    }

    public class UserPasswordChangeModel
    {
        [DisplayName("Current Password")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        [MaxLength(30, ErrorMessage = "{0} adı {1} karakterden fazla olamaz")]
        [MinLength(5, ErrorMessage = "{0} adı {1} karakterden az olamaz")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [DisplayName("New Password")]
        [Required(ErrorMessage = "{0} boş geçilemez")]
        [MaxLength(30, ErrorMessage = "{0} adı {1} karakterden fazla olamaz")]
        [MinLength(5, ErrorMessage = "{0} adı {1} karakterden az olamaz")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }

    public class PasswordResetModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }

}
