using ElectricParse.Domain;
using ElectricParse.Domain.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElectricParse.BusinessLayer
{
    public class ParserService
    {
        private static readonly string site = "http://www.etres.ru/";
        IUnitOfWork db;

        public ParserService(IUnitOfWork _db)
        {
            db = _db;
        }

        public int ParseCategories()
        {
            Order order = new Order();

            string url = string.Format("{0}catalog/", site);
            string categoriesHtml = null;
            categoriesHtml = GetPageHtml(url);

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

            // There are various options, set as needed
            htmlDoc.OptionFixNestedTags = true;

            // filePath is a path to a file containing the html
            htmlDoc.LoadHtml(categoriesHtml);
            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                throw new Exception("Ошибка парсинга сайта");
            }
            else
            {
                if (htmlDoc.DocumentNode != null)
                {
                    var query = from table in htmlDoc.DocumentNode.SelectNodes("//table").Cast<HtmlNode>()
                                from row in table.SelectNodes("tr").Cast<HtmlNode>()
                                from cell in row.SelectNodes("td").Cast<HtmlNode>()
                                where table.Attributes.Contains("class") && table.Attributes["class"].Value == "tovar_cat"
                                select cell;


                    var groups = query.ToList();

                    foreach (var g in groups)
                    {
                        //Ищем группы
                        //Найдем в группе ее название, Это h3
                        var h3 = g.Descendants("h3").FirstOrDefault();
                        var rootCategory = CreateCategory(h3.InnerText);
                        OrderCategory rootOrderCategory = new OrderCategory();
                        rootOrderCategory.Category = rootCategory;
                        rootOrderCategory.IsMain = true;

                        //Ищем группы
                        //Найдем в группе все дочерние группы, Это p
                        foreach (var p in g.Descendants("p"))
                        {
                            if (p.ParentNode.Attributes.Contains("class") && p.ParentNode.Attributes["class"].Value == "ur2")
                                continue;

                            var rootInnerCategory = CreateCategory(p.InnerText);

                            OrderCategory rootInnerOrderCategory = new OrderCategory();
                            rootInnerOrderCategory.Category = rootInnerCategory;
                            rootInnerOrderCategory.ParentOrderCategory = rootOrderCategory;
                            rootInnerOrderCategory.Url = p.ParentNode.Attributes["href"].Value;
                            rootInnerOrderCategory.IsMain = true;

                            //Перейти на новую страницы подкатегории и скачать все категории оттуда
                            LoadUnderCategories(rootInnerOrderCategory);

                            rootOrderCategory.OrderCategories.Add(rootInnerOrderCategory);
                        }

                        order.OrderCategories.Add(rootOrderCategory);
                    }

                    db.OrderRepository.Add(order);
                    db.SaveChanges();
                }
            }



            return order.OrderId;
        }

        private void LoadUnderCategories(OrderCategory rootOrderCategory)
        {
            string html = GetPageHtml(rootOrderCategory.Url);
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(html);

            var queryTreeNode = from a in htmlDoc.DocumentNode.SelectNodes("//a").Cast<HtmlNode>()
                                where
                                a.Attributes.Contains("class") &&
                                (a.Attributes["class"].Value == "ur2" || a.Attributes["class"].Value == "ur2_vib")
                                select a;
            var treeNodes = queryTreeNode.ToList();
            if (treeNodes.Count == 0)
                treeNodes.Add(htmlDoc.GetElementbyId("vib"));

            treeNodes.ForEach(node =>
                {
                    string name = node.Descendants("p").First().InnerHtml;
                    Category category = CreateCategory(name);
                    if (category.CategoryId == rootOrderCategory.CategoryId)
                    {
                        LoadProducts(rootOrderCategory, null, html);
                    }
                    else
                    {
                        string url = node.Attributes["href"].Value;
                        OrderCategory orderCategory = new OrderCategory();
                        orderCategory.Category = category;
                        orderCategory.Url = url;
                        orderCategory.ParentOrderCategory = rootOrderCategory;
                        orderCategory.IsMain = false;

                        LoadProducts(orderCategory, orderCategory.Url, null);

                        rootOrderCategory.OrderCategories.Add(orderCategory);
                    }
                });
        }

        private void LoadProducts(OrderCategory orderCategory, string url = null, string html = null)
        {
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            if (string.IsNullOrEmpty(html))
            {
                html = GetPageHtml(url);
                htmlDoc.OptionFixNestedTags = true;
                htmlDoc.LoadHtml(html);
            }
            else
            {
                htmlDoc.LoadHtml(html);
            }

            var tovars = (from div in htmlDoc.DocumentNode.SelectNodes("//div").Cast<HtmlNode>()
                          from table in div.Descendants("table").Cast<HtmlNode>()
                          from row in table.Descendants("tr").Cast<HtmlNode>()
                          where
                               div.Attributes.Contains("class") && div.Attributes["class"].Value == "tovar"
                          select row
                         ).Skip(1).ToList();

            tovars.ForEach(row =>
            {
                string imageUrl = (from x in row.Descendants("a") where x.Attributes.Contains("rel") && x.Attributes["rel"].Value == "prettyPhoto" select x.Attributes["href"].Value).FirstOrDefault();
                string name = row.Descendants("td").Skip(1).First().FirstChild.InnerText;
                string sPrice = row.Descendants("td").Skip(2).First().FirstChild.InnerText;
                decimal price = 0;
                if (!decimal.TryParse(sPrice, out price))
                    if (!decimal.TryParse(sPrice.Replace(",", "."), out price))
                        if (!decimal.TryParse(sPrice.Replace(".", ","), out price))
                            price = 0;
                Product product = CreateProduct(name);
                OrderCategoryProduct ocp = new OrderCategoryProduct()
                {
                    Product = product,
                    ImageUrl = imageUrl,
                    Price = price
                };
                orderCategory.OrderCategoryProducts.Add(ocp);
            });


        }

        #region private methods

        private Category CreateCategory(string name)
        {
            var cat = db.CategoryRepository.GetByName(name);
            if (cat != null)
                return cat;

            cat = new Category()
            {
                Name = name
            };
            db.CategoryRepository.Add(cat);
            db.SaveChanges();

            return cat;
        }

        private Product CreateProduct(string name)
        {
            var prod = db.ProductRepository.GetByName(name);
            if (prod != null)
                return prod;

            prod = new Product()
            {
                Name = name
            };
            db.ProductRepository.Add(prod);
            db.SaveChanges();

            return prod;
        }

        string GetPageHtml(string link, WebProxy proxy = null)
        {
            WebClient client = new WebClient() { Encoding = Encoding.GetEncoding("windows-1251") };
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            using (client)
            {
                while (true)
                {

                    string result = client.DownloadString(link);

                    if (result.Contains("шибка соединени"))
                    {
                        Thread.Sleep(TimeSpan.FromSeconds((new Random().Next(7, 20))));
                        continue;
                    }

                    return result;
                }
            }
        }

        #endregion
    }
}
