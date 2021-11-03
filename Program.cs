using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

/*
https://www.olx.ba/robots.txt
This link says what is allowed and what is disallowed during crawling
 */

namespace SimpleScraper
{

    static class Program
    {
        public static async Task Main()
        {
            var link = "https://www.olx.ba/kompjuteri";
            var client = new HttpClient();
            var html = await client.GetStringAsync(link);
            var document = new HtmlDocument();
            document.LoadHtml(html);
            var divs1 = document.DocumentNode.Descendants("div").Where(node => node.GetAttributeValue("class", "").Equals("naslov")).ToList();
            var divs2 = document.DocumentNode.Descendants("div").Where(node => node.GetAttributeValue("class", "").Equals("datum")).ToList();
            var computers = new List<Computer>();
            foreach (var div in divs1)
            {
                var computer = new Computer();
                computer.Name = div.Descendants("p").Single().InnerText;
                computers.Add(computer);
            }
            int i = 0;
            foreach (var div in divs2)
            {
                computers[i].Price = div.Descendants("span").FirstOrDefault().InnerText;
                i++;
                if (i == 30) break;
            }
            foreach (var computer in computers)
            {
                Console.WriteLine(computer.Name + "    " + computer.Price + Environment.NewLine);
            }
        }

        public class Computer
        {
            public string Name { get; set; }
            public string Price { get; set; }
        }
    }
}

