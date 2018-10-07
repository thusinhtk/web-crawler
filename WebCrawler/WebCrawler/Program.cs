using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebCrawler
{
    public class Item
    {
        public string Link { get; set; }
        public string Text { get; set; }
        public string ReadCount { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {


            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8  //Set UTF8 để hiển thị tiếng Việt
            };

            //Load website, load html into document
            HtmlDocument document = htmlWeb.Load("http://www.webtretho.com/forum/f26/");

            #region Cach 1 - xPath
            /*

                //Load li tags in tag 
                var threadItems = document.DocumentNode.SelectNodes("//ul[@id='threads']/li").ToList();

                var items = new List<object>();
                foreach (var item in threadItems)
                {
                    //Extract các giá trị từ các tag con của tag li
                    var linkNode = item.SelectSingleNode(".//a[contains(@class,'title')]");
                    var link = linkNode.Attributes["href"].Value;
                    var text = linkNode.InnerText;
                    var readCount = item.SelectSingleNode(".//div[@class='folTypPost']/ul/li/b").InnerText;

                    items.Add(new { text, readCount, link });
                }
             */
            #endregion

            #region Cach 2 - LINQ to Object and lambda expression

            //var threadItems = document.DocumentNode.Descendants("ul")
            //    .First(node => node.Attributes.Contains("id") && node.Attributes["id"].Value == "threads")
            //    .ChildNodes.Where(node => node.Name == "li").ToList();

            //var items = new List<object>();
            //foreach (var item in threadItems)
            //{
            //    var linkNode = item.Descendants("a").First(node =>
            //    node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("title"));
            //    var link = linkNode.Attributes["href"].Value;
            //    var text = linkNode.InnerText;
            //    var readCount = item.Descendants("b").First().InnerText;

            //    items.Add(new { text, readCount, link });
            //}

            #endregion

            #region Cach 3 - Using Fizzler

            var threadItems = document.DocumentNode.QuerySelectorAll("ul#threads > li").ToList();

            var objs = new List<Item>();
            foreach (var item in threadItems)
            {
                var linkNode = item.QuerySelector("a.title");
                var link = linkNode.Attributes["href"].Value;
                var text = linkNode.InnerText;
                var readCount = item.QuerySelector("div.folTypPost > ul > li > b").InnerText;

                objs.Add(new Item { Link = link, Text = text, ReadCount = readCount });
            }
            #endregion

            #region Export to .txt

            TextWriter sw = new StreamWriter(@"C:\Users\NguyenMinhPhuong\source\repos\web-crawler\WebCrawler\WebCrawler\Data.txt");
            for (int i = 0; i < objs.Count(); i++)
            {
                sw.WriteLine(objs[i].Text + '\t' +
                             objs[i].ReadCount + '\t' +
                             objs[i].Link);
            }
            sw.Close();     //Don't Forget Close the TextWriter Object(sw)

            #endregion

            Console.WriteLine("Done");
            Environment.Exit(0);

        }
    }
}
