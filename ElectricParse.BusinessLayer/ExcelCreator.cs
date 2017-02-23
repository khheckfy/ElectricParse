using ElectricParse.Domain;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ElectricParse.BusinessLayer
{
    public class ExcelCreator
    {
        IUnitOfWork db;
        private readonly int OrderId;
        private int ri = 1;
        ExcelWorksheet ws = null;

        public ExcelCreator(IUnitOfWork _db, int orderId)
        {
            db = _db;
            OrderId = orderId;
        }

        public void Generate()
        {
            string fileName = string.Format(@"C:\temp\{0:dd_MM_yyyy_HH_mm_ss}.xlsx", DateTime.Now);
            //string dir = string.Format(@"C:\temp\images");
            //DirectoryInfo di = new DirectoryInfo(dir);
            //di.GetFiles().ToList().ForEach(file => File.Delete(file.FullName));
            string dir = string.Format(@"C:\temp");
            DirectoryInfo di = new DirectoryInfo(dir);
            //di.GetFiles("*.xlsx").ToList().ForEach(file => File.Delete(file.FullName));


            FileInfo newFile = new FileInfo(fileName);
            ExcelPackage pck = new ExcelPackage(newFile);
            ws = pck.Workbook.Worksheets.Add("Каталог");

            //categories
            var categoriesQuery = from orderCategory in db.OrderCategoryRepository.Query()
                                  join category in db.CategoryRepository.Query() on orderCategory.CategoryId equals category.CategoryId
                                  where
                                  orderCategory.OrderId == OrderId && orderCategory.ParentOrderCategoryId == null
                                  select new
                                  {
                                      category.Name,
                                      category.CategoryId,
                                      orderCategory.OrderCategoryId
                                  };

            foreach (var category in categoriesQuery.Distinct().ToArray())
            {
                ws.Cells[string.Format("A{0}", ri)].Value = category.Name;
                SetMainCategoryFormat(string.Format("A{0}:E{0}", ri));
                GenerateLoopCategories(category.OrderCategoryId, string.Empty);
            }
            ws.Column(2).Width = 6;
            pck.Save();
            System.Diagnostics.Process.Start(fileName);
        }

        private void GenerateLoopCategories(int OrderCategoryId, string name)
        {
            var categoriesQuery = from orderCategory in db.OrderCategoryRepository.Query()
                                  join category in db.CategoryRepository.Query() on orderCategory.CategoryId equals category.CategoryId
                                  where
                                  orderCategory.OrderId == OrderId && orderCategory.ParentOrderCategoryId == OrderCategoryId
                                  select new
                                  {
                                      category.Name,
                                      category.CategoryId,
                                      orderCategory.OrderCategoryId
                                  };
            foreach (var category in categoriesQuery.Distinct().Take(3).ToArray())
            {
                ws.Cells[string.Format("A{0}", ri)].Value = category.Name;
                SetUnderCategoryFormat(string.Format("A{0}:F{0}", ri));
                GenerateProducts(category.OrderCategoryId);
                ri++;
                GenerateLoopCategories(category.OrderCategoryId, category.Name);
            }
        }

        private void GenerateProducts(int orderCategoryId)
        {
            var products = (from orderProduct in db.OrderCategoryProductRepository.Query()
                            join product in db.ProductRepository.Query() on orderProduct.ProductId equals product.ID
                            where orderProduct.OrderCategoryId == orderCategoryId
                            select new
                            {
                                orderProduct.Price,
                                ImagePath = (orderProduct.ProductImage == null ? null : orderProduct.ProductImage.Path),
                                product.Name
                            })
                            .OrderBy(n => n.Name)
                            .ToArray();
            if (products.Length == 0)
                return;

            ri++;
            int rowStart = ri;
            GenerateProductHeader();
            int productIndex = 1;
            foreach (var product in products)
            {
                ri++;
                ws.Cells[string.Format("A{0}", ri)].Value = productIndex;
                ws.Cells[string.Format("C{0}", ri)].Value = product.Name;
                ws.Cells[string.Format("D{0}", ri)].Value = product.Price;
                ws.Cells[string.Format("E{0}", ri)].Value = product.Price * 0.9M;
                ws.Cells[string.Format("F{0}", ri)].Value = product.Price * 0.7M;

                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    using (Image logo = Image.FromFile(product.ImagePath))
                    {
                        var picture = ws.Drawings.AddPicture(ri.ToString(), logo);
                        picture.EditAs = OfficeOpenXml.Drawing.eEditAs.OneCell;
                        picture.SetPosition(ri - 1, 1, 1, 1);
                    }
                    ws.Row(ri).Height = 31;
                }

                productIndex++;
            }

            var modelTable = ws.Cells[string.Format("A{0}:F{1}", rowStart, ri)];

            modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            modelTable.AutoFitColumns();
        }


        private void GenerateProductHeader()
        {
            ws.Cells[string.Format("A{0}", ri)].Value = "№";
            ws.Cells[string.Format("B{0}", ri)].Value = "";
            ws.Cells[string.Format("C{0}", ri)].Value = "Наименование";
            ws.Cells[string.Format("D{0}", ri)].Value = "Цена";
            ws.Cells[string.Format("E{0}", ri)].Value = "Мелк";
            ws.Cells[string.Format("F{0}", ri)].Value = "Круп";
            string address = string.Format("A{0}:F{0}", ri);
            ws.Cells[address].Style.Font.Bold = true;
            ws.Cells[address].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        }

        private void SetMainCategoryFormat(string address)
        {
            ws.Cells[address].Style.Font.Bold = true;
            ws.Cells[address].Style.Font.Size = 16;
            ws.Cells[address].Merge = true;
            ws.Cells[address].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

        }

        private void SetUnderCategoryFormat(string address)
        {
            ws.Cells[address].Style.Font.Bold = true;
            ws.Cells[address].Style.Font.Size = 14;
            ws.Cells[address].Merge = true;
            ws.Cells[address].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        }
    }
}
