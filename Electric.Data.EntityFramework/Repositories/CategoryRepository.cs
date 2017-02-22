using ElectricParse.Domain.Entities;
using ElectricParse.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricParse.Data.EntityFramework.Repositories
{
    internal class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        internal CategoryRepository(ModelDataContext context)
            : base(context)
        {
        }

        public Category GetByName(string name)
        {
            var category = Set.FirstOrDefault(n => n.Name == name);

            return category;
        }
    }
}