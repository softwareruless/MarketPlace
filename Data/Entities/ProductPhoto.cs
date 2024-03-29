﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Data.Entities
{
	public class ProductPhoto : BaseItemEntity 
	{
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
		public string Path { get; set; }

		public virtual Product Product { get; set; }

	}
}
