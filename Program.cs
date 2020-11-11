using HtmlAgilityPack;
using System;

namespace TestHAP
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "https://www.beleggen.nl/adviezen/default.aspx";
            Console.WriteLine("Getting data from {0}...", url);
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var documentNode = doc.DocumentNode;
            var tableNode = documentNode
                        .SelectSingleNode("//table[@class='gentable ut--tableguru']//tbody");
            var rowsNodesList = tableNode.SelectNodes("tr");

            var rowCount = 1;
            foreach (var row in rowsNodesList)
            {
                var cells = row.SelectNodes("td");
                var link = row.SelectNodes("td[@data-title='Fonds']//a[@href]");
                if (cells != null && cells.Count > 0)
                {
                    var datum = cells[0].InnerText;
                    datum = datum.Replace("\r\n", "").Trim();
                    var fonds = cells[1].InnerText;
                    fonds = fonds.Replace("\r\n", "").Trim();
                    var guru = cells[2].InnerText;
                    guru = guru.Replace("\r\n", "").Trim();
                    var target = cells[3].InnerText;
                    target = target.Replace("\r\n", "").Trim();
                    var advies = cells[4].InnerText;
                    advies = advies.Replace("\r\n", "").Trim();
                    var links = link[0].GetAttributeValue("href", string.Empty).Remove(0, 3);
                    var Price = GetPrice(links);

                    Console.WriteLine("Row: {0}", rowCount);
                    Console.WriteLine("Datum: {0}", datum);
                    Console.WriteLine("Fonds: {0}", fonds);
                    Console.WriteLine("Link: {0}", links);
                    Console.WriteLine("Guru: {0}", guru);
                    Console.WriteLine("Target: {0}", target);
                    Console.WriteLine("Advies: {0}", advies);
                    Console.WriteLine("Huidige Prijs: {0}", Price);
                    Console.WriteLine("--------------------");
                    rowCount++;
                }
            }

            string GetPrice(string url)
            {
                var priceurl = "https://www.beleggen.nl/" + url;
                var pricehtml = new HtmlWeb();
                var source = pricehtml.Load(priceurl);
                var htmlBody = source.DocumentNode.SelectSingleNode("//section[@class='chartblock']/div/div/span");

                HtmlNode firstChild = htmlBody.FirstChild;
                return firstChild.OuterHtml;
            }
        }
    }
}
