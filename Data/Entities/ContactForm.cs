using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Data.Entities
{
    public class ContactForm : BaseItemEntity
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsAnswered { get; set; }

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

    }
}

