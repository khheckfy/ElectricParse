using ElectricParse.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElectricParse.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        #region Properties

        ICategoryRepository CategoryRepository { get; }
        IOrderCategoryProductRepository OrderCategoryProductRepository { get; }
        IOrderCategoryRepository OrderCategoryRepository { get; }
        IOrderRepository OrderRepository { get; }
        IProductRepository ProductRepository { get; }

        #endregion

        #region Methods
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        #endregion
    }
}
