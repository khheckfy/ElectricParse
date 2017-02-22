using ElectricParse.Data.EntityFramework;
using ElectricParse.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricParse.App
{
    class Program
    {
        static void Main(string[] args)
        {
            IUnitOfWork db = new UnitOfWork();
            db.CategoryRepository.GetAll();
        }
    }
}
