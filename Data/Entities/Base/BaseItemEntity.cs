using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Data.Entities
{
    public class BaseItemEntity : BaseEntity
    {
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }

        [NotMapped]
        public string CreatedDateStr
        {
            set
            {
                if (value != null)
                {
                    string[] date = new string[3];
                    if (value.IndexOf(".") != -1)
                    {
                        date = value.Split('.');

                    }
                    else if (value.IndexOf("/") != -1)
                    {
                        date = value.Split('/');
                    }
                    CreatedDate = new DateTime(int.Parse(date[2].ToString()), int.Parse(date[1].ToString()), int.Parse(date[0].ToString()));

                }
            }
            get
            {
                return CreatedDate.ToString("dd/MM/yyyy");
            }
        }

        [NotMapped]
        public string UpdatedDateStr
        {
            set
            {
                if (value != null)
                {
                    string[] date = new string[3];
                    if (value.IndexOf(".") != -1)
                    {
                        date = value.Split('.');

                    }
                    else if (value.IndexOf("/") != -1)
                    {
                        date = value.Split('/');
                    }
                    UpdatedDate = new DateTime(int.Parse(date[2].ToString()), int.Parse(date[1].ToString()), int.Parse(date[0].ToString()));

                }
            }
            get
            {
                return UpdatedDate?.ToString("dd/MM/yyyy");
            }
        }
    }
}
