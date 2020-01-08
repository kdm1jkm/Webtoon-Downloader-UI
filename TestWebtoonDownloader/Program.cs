using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibWebtoonDownloader;

namespace TestWebtoonDownloader
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Webtoon.GetFavoriteWebtoonInfosFromAccount("id", "password");
        }
    }
}