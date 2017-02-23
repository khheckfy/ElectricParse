using ElectricParse.Domain.Entities;
using ElectricParse.Domain.Repositories;

namespace ElectricParse.Data.EntityFramework.Repositories
{
    internal class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        internal ProductImageRepository(ModelDataContext context)
            : base(context)
        {
        }
    }
}