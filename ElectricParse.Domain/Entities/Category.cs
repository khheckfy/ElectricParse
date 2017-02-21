using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricParse.Domain.Entities
{
    public class Category
    {
        public Guid ID { private set; get; }
        public string Name { private set; get; }
        public string Url { set; get; }
        public Guid? ParentCategoryId { set; get; }

        public List<Product> Products { set; get; }

        public Category(string name)
        {
            ID = Guid.NewGuid();
            Name = name;
            Products = new List<Product>();
        }
    }
}
