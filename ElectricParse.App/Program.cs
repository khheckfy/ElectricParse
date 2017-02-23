using ElectricParse.BusinessLayer;
using ElectricParse.Data.EntityFramework;
using ElectricParse.Domain;
using System;
using System.Collections.Generic;
using System.IO;
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
            var ps = new ParserService(db);

            //string dir = string.Format(@"C:\temp\images");
            //DirectoryInfo di = new DirectoryInfo(dir);
            //di.GetFiles().ToList().ForEach(file =>
            //{
            //    var image = ps.resizeImage(40, 40, file.FullName);
            //    image.Save(file.FullName);
            //});

            //return;
            int orderId = 6;// ().ParseCategories();

            ExcelCreator excelCreator = new ExcelCreator(db, orderId);
            excelCreator.Generate();
        }
    }
}
