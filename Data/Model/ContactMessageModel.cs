using System;
namespace MarketPlace.Data.Model
{
	public class ContactMessageModel
	{
        public int Id { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string CreatedDate { get; set; }
        public bool IsAnswered { get; set; }
    }

    public class ContactMessageAddModel
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }

    public class AnswerContactMessageModel
    {
        public int Id { get; set; }
        public bool IsAnswered { get; set; }
    }
}

