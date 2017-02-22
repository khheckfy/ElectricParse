using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricParse.Domain.Entities
{
    [Table("OrderCategoryProducts")]
    public class OrderCategoryProduct
    {
        public OrderCategoryProduct()
        {

        }

        [Key]
        public int OrderCategoryProductId { set; get; }

        public int OrderCategoryId { set; get; }

        public int ProductId { set; get; }

        [Column(TypeName = "money")]
        public decimal? Price { get; set; }

        [StringLength(1024)]
        public string ImageUrl { set; get; }

        public virtual OrderCategory OrderCategory { set; get; }
        public virtual Product Product { set; get; }
    }
}