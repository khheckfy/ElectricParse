using ElectricParse.Domain.Entities;

namespace ElectricParse.Domain.Repositories
{
    public interface IProductImageRepository : IRepository<ProductImage>
    {
        ProductImage GetByUrl(string url);
    }
}
