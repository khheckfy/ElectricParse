using ElectricParse.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricParse.Domain.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetByName(string name);
    }
}
