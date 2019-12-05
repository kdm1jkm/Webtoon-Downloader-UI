using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace LibWebtoonDownloader
{
    public class Webtoon
    {
        private int titleId;

        private int startNo;
        private int endNo;

        public Webtoon()
        {
            this.titleId = startNo = endNo = 0;
        }

        public Webtoon(int titleId)
        {
            this.titleId = titleId;
            startNo = endNo = 0;
        }

        public Webtoon(int titleId, int no)
        {
            this.titleId = titleId;
            startNo = endNo = no;
        }

        public Webtoon(int titleId, int startNo, int endNo)
        {
            this.titleId = titleId;
            this.startNo = startNo;
            this.endNo = endNo;
        }

        private void downloadImage(string url, string dir)
        {
            Console.WriteLine("Testdownload complete");
            Console.WriteLine("Url: " + url);
            Console.WriteLine("dir: " + dir);
            Console.WriteLine();
        }

        private void downloadEpisode(int titleId, int no)
        {
            HtmlWeb web = new HtmlWeb();
            string url = string.Format("https://comic.naver.com/webtoon/detail.nhn?titleId={0}&no={1}", titleId, no);

            HtmlDocument doc = new HtmlDocument();

            //doc = web.Load(url);
            doc.Load(@"D:\dropbox\Downloads\test for crawling\WebtoonBody.html");

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//*[@alt=\"comic content\"]"))
            {
                HtmlAttribute attSrc = link.Attributes["src"];

                //string src = attSrc.Value;
                string src = @"D:\dropbox\Downloads\test for crawling" + attSrc.Value.Substring(1, attSrc.Value.Length - 1);

                string dir = string.Format("download\\{0}", src.Split('/')[src.Split('/').Length - 1]);

                //string alt = attAlt.Value;
                //Console.WriteLine(alt);

                downloadImage(src, dir);
            }
        }

        public int download()
        {
            if (this.titleId * this.startNo * this.endNo == 0)
            {
                return -1;
                //정보 없음
            }

            for (int i = startNo; i <= endNo; i++)
            {
                downloadEpisode(this.titleId, i);
            }

            return 0;
        }

        public int setEpisodeLast()
        {
            string url = string.Format("https://comic.naver.com/webtoon/list.nhn?titleId={0}", this.titleId);

            HtmlWeb web = new HtmlWeb();

            HtmlDocument doc = new HtmlDocument();

            //doc = web.Load(url);
            doc.Load(@"D:\dropbox\Downloads\test for crawling\연애혁명 __ 네이버 만화.htm");

            int lastEp;

            HtmlNode link = doc.DocumentNode.SelectSingleNode("//td[@class=\"title\"]/a");

            HtmlAttribute attHref = link.Attributes["href"];

            string href = attHref.Value;

            lastEp = int.Parse(href.Split('?')[1].Split('&')[1].Split('=')[1]);

            this.startNo = this.endNo = lastEp;

            return lastEp;
        }

        public int setWebtoonByName(string name)
        {
            string url = string.Format(@"https://comic.naver.com/search.nhn?m=webtoon&keyword={0}", name);

            int titleId;

            HtmlWeb web = new HtmlWeb();

            HtmlDocument doc = new HtmlDocument();

            //doc = web.Load(url);
            doc.Load(@"D:\dropbox\Downloads\test for crawling\윰세포_검색_존재.html");
            //doc.Load(@"D:\dropbox\Downloads\test for crawling\검색_결과없음.html");

            HtmlNode link = doc.DocumentNode.SelectSingleNode("//div[@class=\"resultBox\"]/ul[@class=\"resultList\"]/li/h5/a");

            if(link == null)
            {
                this.titleId = 0;
                return -1;
            }

            HtmlAttribute attHref = link.Attributes["href"];

            string href = attHref.Value;

            titleId = int.Parse(href.Split('?')[1].Split('=')[1]);

            this.titleId = titleId;

            return titleId;
        }
    }
}
