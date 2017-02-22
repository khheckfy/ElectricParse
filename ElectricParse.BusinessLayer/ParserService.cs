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

        public List<Category> ParseCategories()
        {
            List<Category> categories = new List<Category>();

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
                        var rootCategory = db.CategoryRepository.GetByName(h3.InnerText);
                        if (rootCategory == null)
                            rootCategory = CreateCategory(h3.InnerText);
                        OrderCategory rootOrderCategory = new OrderCategory();
                        rootOrderCategory.Category = rootCategory;


                        //Ищем группы
                        //Найдем в группе все дочерние группы, Это p
                        foreach (var p in g.Descendants("p"))
                        {
                            var rootInnerCategory = db.CategoryRepository.GetByName(p.InnerText);
                            if (rootInnerCategory == null)
                                rootInnerCategory = CreateCategory(p.InnerText);

                            OrderCategory rootInnerOrderCategory = new OrderCategory();
                            rootInnerOrderCategory.Category = rootInnerCategory;
                            rootInnerOrderCategory.ParentOrderCategory = rootOrderCategory;
                            rootInnerOrderCategory.Url = p.ParentNode.Attributes["href"].Value;
                            rootOrderCategory.OrderCategories.Add(rootInnerOrderCategory);
                        }





                        order.OrderCategories.Add(rootOrderCategory);
                    }

                    db.OrderRepository.Add(order);
                    db.SaveChanges();


                    //foreach (var node in nodes)
                    //{
                    //    string href = node.GetAttributeValue("href", string.Empty);
                    //    if (!href.StartsWith(url))
                    //        continue;
                    //    string name = string.Empty;
                    //    if (node.FirstChild == null)
                    //        continue;

                    //    if (node.FirstChild.Name == "div")
                    //        continue;

                    //    name = node.FirstChild.InnerText;
                    //    bool isHead = false;
                    //    if (node.FirstChild.Name == "h3")
                    //    {
                    //        isHead = true;
                    //    }


                    //    Category category = db.CategoryRepository.GetByName(node.InnerText);
                    //    if (category == null)
                    //    {
                    //        CreateCategory(name);
                    //    }
                    //}
                }
            }



            return categories;
        }

        #region private methods

        private Category CreateCategory(string name)
        {
            var cat = new Category()
            {
                Name = name
            };
            db.CategoryRepository.Add(cat);
            db.SaveChanges();

            return cat;
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
                        Thread.Sleep(TimeSpan.FromSeconds(10));
                        continue;
                    }

                    return result;
                }
            }
        }

        #endregion
    }
}
