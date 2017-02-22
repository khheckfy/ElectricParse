using ElectricParse.Domain.Entities;
using ElectricParse.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricParse.Data.EntityFramework.Repositories
{
    internal class ProductRepository : Repository<Product>, IProductRepository
    {
        internal ProductRepository(ModelDataContext context)
            : base(context)
        {
        }

        public Product GetByName(string name)
        {
            return Set.FirstOrDefault(n => n.Name == name);
        }
    }
}