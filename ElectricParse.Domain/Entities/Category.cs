using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricParse.Domain.Entities
{
    [Table("Categories")]
    public class Category
    {
        public Category()
        {
        }

        [Key]
        public int CategoryId { set; get; }
        [StringLength(512)]
        public string Name { set; get; }
    }
}
