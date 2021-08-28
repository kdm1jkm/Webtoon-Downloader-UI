using System;
using LibWebtoonDownloader.Webtoon;

namespace ConsoleWebtoonDownloader
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            NaverWebtoon.Load("갓 오브 하이스쿨")?
                .GetTaskByNo(1)
                .SetThreadCount(10)
                .SetOnEachTaskFinished(i => Console.WriteLine($"Finished image #{i}"))
                .Run().Wait();
        }
    }
}