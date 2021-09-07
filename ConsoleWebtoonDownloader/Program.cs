using System;
using System.Linq;
using LibWebtoonDownloader.NaverWebtoon;

namespace ConsoleWebtoonDownloader
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var results = NaverWebtoon
                .Search("갓오하")
                .Select(result => $"{result.Name}({result.Id})");

            foreach (string s in results) Console.Out.WriteLine(s);
        }
    }
}