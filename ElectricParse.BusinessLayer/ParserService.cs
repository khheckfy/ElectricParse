using ElectricParse.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ElectricParse.BusinessLayer
{
    public class ParserService
    {
        public List<Category> ParseCategories()
        {
            List<Category> categories = new List<Category>();



            return categories;
        }

        #region private methods

        string GetPageHtml(string link, WebProxy proxy = null)
        {
            WebClient client = new WebClient() { Encoding = Encoding.GetEncoding("windows-1251") };
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            using (client)
            {
                return client.DownloadString(link);
            }
        }

        #endregion
    }
}
