﻿using ElectricParse.Domain.Entities;
using ElectricParse.Domain.Repositories;

namespace ElectricParse.Data.EntityFramework.Repositories
{
    internal class OrderRepository : Repository<Order>, IOrderRepository
    {
        internal OrderRepository(ModelDataContext context)
            : base(context)
        {
        }
    }
}