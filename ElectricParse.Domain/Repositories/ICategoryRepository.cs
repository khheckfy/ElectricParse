﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricParse.Domain.Entities;

namespace ElectricParse.Domain.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category GetByName(string name);
    }
}
