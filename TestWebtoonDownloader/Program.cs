using HtmlAgilityPack;
using LibWebtoonDownloader;
using System;
using System.Collections.Generic;

namespace TestWebtoonDownloader
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //HtmlDocument doc = new HtmlDocument();
            //doc.Load(@"D:\Dropbox\ownFiles\Documents\Utility\webtoonforcrawling\WebtoonBody.html", System.Text.Encoding.UTF8);

            //Console.WriteLine(Webtoon.GetWebtoonImageSrcs(doc));

            //WebtoonInfo info = new WebtoonInfo();
            //if(info.TrySetIdFromName("연애혁명"))
            //{
            //    Console.WriteLine(info.Author);
            //    Console.WriteLine(info.DetailInfo);
            //    Console.WriteLine(info.Genre);
            //    Console.WriteLine(info.WebtoonName);
            //    Console.WriteLine(info.IsAvailable);

            //    info.No = 1;
            //    Console.WriteLine(info.ImageCount);
            //    Console.WriteLine(info.Url);
            //}

            //info.Save("Test.dat");

            WebtoonInfo info = WebtoonInfo.Load("Test.dat");
            foreach(string s in info.ImageSrcs)
            {
                Console.WriteLine(s);
            }
        }
    }
}