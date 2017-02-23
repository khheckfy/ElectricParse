using ElectricParse.Domain.Entities;
using ElectricParse.Domain.Repositories;
using System.Linq;


namespace ElectricParse.Data.EntityFramework.Repositories
{
    internal class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        internal ProductImageRepository(ModelDataContext context)
            : base(context)
        {
        }

        public ProductImage GetByUrl(string url)
        {
            return Set.FirstOrDefault(n => n.ImageUrl == url);
        }
    }
}