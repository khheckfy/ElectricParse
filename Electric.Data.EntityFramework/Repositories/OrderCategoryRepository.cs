using ElectricParse.Domain.Entities;
using ElectricParse.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricParse.Data.EntityFramework.Repositories
{
    internal class OrderCategoryRepository : Repository<OrderCategory>, IOrderCategoryRepository
    {
        internal OrderCategoryRepository(ModelDataContext context)
            : base(context)
        {
        }
    }
}