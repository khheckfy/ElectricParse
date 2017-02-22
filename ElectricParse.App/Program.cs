using ElectricParse.BusinessLayer;
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
        private static IUnitOfWork db = new UnitOfWork();

        static void Main(string[] args)
        {
            int orderId = 4;// (new ParserService(db)).ParseCategories();



            Console.ReadLine();
        }
    }
}
