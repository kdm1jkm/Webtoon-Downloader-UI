using HtmlAgilityPack;
using LibWebtoonDownloader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TestWebtoonDownloader
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            WebtoonInfo info = new WebtoonInfo();
            info.Id = 570503;

            info.LoadWebtoonInfo();
            info.No = 7;

            info.LoadDetailInfo();

            //info.WebtoonName = "wowwow";

            //info.Weekday[DayOfWeek.Sunday] = true;

            WebtoonInfo info2 = info.Copy();

            return;
        }
    }
}