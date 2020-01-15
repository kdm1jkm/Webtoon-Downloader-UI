using LibWebtoonDownloader;
using System;

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