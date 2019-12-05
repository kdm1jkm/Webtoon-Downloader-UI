using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibWebtoonDownloader;

namespace TestWebtoonDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            Webtoon webtoon = new Webtoon();

            Console.WriteLine(webtoon.setWebtoonByName("유미의 세포들"));

            Console.WriteLine(webtoon.setEpisodeLast());

            webtoon.download();
        }
    }
}
