using System;
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
            OrderCategories = new HashSet<OrderCategory>();
        }

        [Key]
        public int OrderCategoryId { set; get; }
        [Required]
        public int OrderId { set; get; }
        [Required]
        public int CategoryId { set; get; }
        public int? ParentOrderCategoryId { get; set; }
        [StringLength(512)]
        public string Url { set; get; }

        public virtual Order Order { get; set; }
        public virtual Category Category { get; set; }
        [ForeignKey("ParentOrderCategoryId")]
        public virtual ICollection<OrderCategory> OrderCategories { get; set; }
        public virtual OrderCategory ParentOrderCategory { get; set; }
    }
}
