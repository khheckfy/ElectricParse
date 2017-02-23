﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricParse.Domain.Entities
{
    [Table("ProductImages")]
    public class ProductImage
    {
        [Key]
        public int ProductImageId { set; get; }
        [StringLength(512), Required]
        public string ImageUrl { set; get; }
        [StringLength(512), Required]
        public string Path { set; get; }
    }
}
