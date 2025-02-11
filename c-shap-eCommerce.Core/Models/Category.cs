﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.Models
{
    public class Category
    {
        public int Id { get; set; }

		[MinLength(3)]
		[MaxLength(64)]
		public string Name { get; set; }

		[MinLength(3)]
		[MaxLength(1024)]
		public string Description { get; set; }
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}