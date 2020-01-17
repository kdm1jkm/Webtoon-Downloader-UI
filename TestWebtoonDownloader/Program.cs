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
            WebtoonInfo info = new WebtoonInfo();
            DayOfWeek[] week =
            {
                DayOfWeek.Sunday,
                DayOfWeek.Monday,
                DayOfWeek.Friday
            };

            info.Weekdays = week;
        }
    }
}