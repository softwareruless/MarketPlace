﻿using System.ComponentModel.DataAnnotations.Schema;
using MarketPlace.Data.Enums;

namespace MarketPlace.Data.Entities
{
    public class Seller : BaseItemEntity
	{
		[ForeignKey("UserId")]
		public int UserId { get; set; }

		public string Name { get; set; }
		public string FirstName { get; set; }
		public string Surname { get; set; }
		public string TaxNumber { get; set; }
		public string TaxBuilding { get; set; }
		public string Description { get; set; }
		public string Iban { get; set; }

		public string ProfilePhoto { get; set; }
		public string CoverPhoto { get; set; }

		public string RejectReason { get; set; }

		public Status Status { get; set; }

		public virtual User User { get; set; }
	}
}

