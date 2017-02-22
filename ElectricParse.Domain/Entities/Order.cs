using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricParse.Domain.Entities
{
    [Table("Orders")]
    public class Order
    {
        public Order()
        {
            CreatedDate = DateTime.Now;
            OrderCategories = new HashSet<OrderCategory>();
        }

        [Key]
        public int OrderId { set; get; }
        public DateTime CreatedDate { set; get; }
        public virtual ICollection<OrderCategory> OrderCategories { set; get; }
    }
}
