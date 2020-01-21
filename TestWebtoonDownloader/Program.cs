using HtmlAgilityPack;
using LibWebtoonDownloader;
using System;
using System.Threading.Tasks;

namespace TestWebtoonDownloader
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            HtmlWeb web = new HtmlWeb();

            HtmlDocument doc = web.Load("https://comic.naver.com/webtoon/list.nhn?titleId=723714");

            foreach(HtmlNode node in doc.DocumentNode.SelectNodes("/html/body/div/div/div/div/div/p/text()"))
            {
                Console.WriteLine(node.InnerText);
            }
        }
    }
}