using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Data.Entities
{
    public class User : IdentityUser<int>
    {
        public string? FullName { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime AccessTokenEndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsRejected { get; set; }

        [NotMapped]
        public string AccessTokenEndDateStr
        {
            set
            {
                if (value != null)
                {
                    String[] date = new String[3];
                    if (value.IndexOf(".") != -1)
                    {
                        date = value.Split('.');

                    }
                    else if (value.IndexOf("/") != -1)
                    {
                        date = value.Split('/');
                    }
                    this.AccessTokenEndDate = new DateTime(int.Parse(date[2].ToString()), int.Parse(date[1].ToString()), int.Parse(date[0].ToString()));

                }
            }
            get
            {
                return (this.AccessTokenEndDate).ToString("dd/MM/yyyy");
            }
        }

        [NotMapped]
        public string CreatedDateStr
        {
            set
            {
                if (value != null)
                {
                    String[] date = new String[3];
                    if (value.IndexOf(".") != -1)
                    {
                        date = value.Split('.');

                    }
                    else if (value.IndexOf("/") != -1)
                    {
                        date = value.Split('/');
                    }
                    this.CreatedDate = new DateTime(int.Parse(date[2].ToString()), int.Parse(date[1].ToString()), int.Parse(date[0].ToString()));

                }
            }
            get
            {
                return (this.CreatedDate).ToString("dd/MM/yyyy");
            }
        }
    }
}
