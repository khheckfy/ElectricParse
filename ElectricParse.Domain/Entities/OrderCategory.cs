﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricParse.Domain.Entities
{
    [Table("OrderCategories")]
    public class OrderCategory
    {
        public OrderCategory()
        {

        }

        [Key]
        public int OrderCategoryId { set; get; }

        public int OrderId { set; get; }
        public int CategoryId { set; get; }

        public virtual Order Order { get; set; }
        public virtual Category Category { get; set; }
    }
}