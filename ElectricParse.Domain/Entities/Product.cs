using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricParse.Domain.Entities
{
    [Table("Products")]
    public class Product
    {
        public Product()
        {

        }

        [Key]
        public int ID { get; set; }
        [StringLength(512)]
        public string Name { set; get; }
        [StringLength(1024)]
        public string ImageUrl { set; get; }
    }
}