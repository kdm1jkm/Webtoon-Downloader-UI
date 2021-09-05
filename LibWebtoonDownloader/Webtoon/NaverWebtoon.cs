using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Web;
using HtmlAgilityPack;
using LibWebtoonDownloader.WebtoonTask;

namespace LibWebtoonDownloader.Webtoon
{
    public class NaverWebtoon : IWebtoon
    {
        private NaverWebtoon(int id, HtmlDocument doc, string webtoonName, string description, Uri uri, string author,
            Uri? thumbnailUrl)
        {
            Id = id;
            Doc = doc;
            WebtoonName = webtoonName;
            Description = description;
            Uri = uri;
            Author = author;
            ThumbnailUrl = thumbnailUrl;
        }

        private int Id { get; }
        private HtmlDocument Doc { get; }

        public string WebtoonName { get; }
        public string Description { get; }
        public Uri Uri { get; }
        public string Author { get; }
        public Uri? ThumbnailUrl { get; }

        public IEnumerable<AbstractWebtoonTask>? GetEveryTask()
        {
            //가장 최신 에피소드의 링크
            HtmlNode link = Doc.DocumentNode.SelectSingleNode("//td[@class=\"title\"]/a");

            //링크 주소 파싱
            HtmlAttribute attHref = link.Attributes["href"];

            //주소에서 no값 파싱후 리턴
            var href = new Uri("https://comic.naver.com" + attHref.Value);
            string? no = HttpUtility.ParseQueryString(href.Query)["no"];
            if (no == null) return null;


            if (!int.TryParse(no, out int lastEp)) return null;

            return Enumerable
                .Range(1, lastEp)
                .Select(i => new NaverWebtoonTask(Id, i, this));
        }

        public AbstractWebtoonTask GetTask(int no)
        {
            return new NaverWebtoonTask(Id, no, this);
        }

        public static NaverWebtoon? Load(string webtoonName)
        {
            HtmlWeb web = new HtmlWeb();

            // 웹 접속
            string encodedName = UrlEncoder.Default.Encode(webtoonName);
            var searchUri = new Uri($"https://comic.naver.com/search.nhn?m=webtoon&keyword={encodedName}");
            var searchDoc = web.Load(searchUri);

            // 첫 번째 검색
            var link = searchDoc.DocumentNode.SelectSingleNode(
                "//div[@class=\"resultBox\"][1]/ul[@class=\"resultList\"]/li/h5/a"
            );
            if (link == null) return null;
            string value = link.Attributes["href"].Value;
            string httpsComicNaverCom = "https://comic.naver.com" + value;
            var linkUri = new Uri(httpsComicNaverCom);

            // titleId
            string? titleId = HttpUtility.ParseQueryString(linkUri.Query)["titleId"];
            if (titleId == null) return null;
            int id = int.Parse(titleId);

            return Load(id);
        }

        private static NaverWebtoon? Load(int id)
        {
            HtmlWeb web = new HtmlWeb();
            // 웹툰 페이지
            var webtoonUri = new Uri($"https://comic.naver.com/webtoon/list?titleId={id}");
            var webtoonDoc = web.Load(webtoonUri);

            string? name = GetWebtoonName(webtoonDoc);
            if (name == null) return null;

            string? author = GetWebtoonAuthor(webtoonDoc);
            if (author == null) return null;
            string? detailInfo = GetWebtoonDetailInfo(webtoonDoc);
            string? genre = GetWebtoonGenre(webtoonDoc);
            string? thumbnailURl = GetWebtoonThumbnailUrl(webtoonDoc);

            return new NaverWebtoon(
                id,
                webtoonDoc,
                name,
                $"({genre})_{detailInfo}",
                webtoonUri,
                author,
                thumbnailURl == null ? null : new Uri(thumbnailURl)
            );
        }

        public static IEnumerable<(string name, int Id)>? Search(string keyWord)
        {
            HtmlWeb web = new HtmlWeb();

            // 웹 접속
            string encodedName = UrlEncoder.Default.Encode(keyWord);
            var searchUri = new Uri($"https://comic.naver.com/search.nhn?m=webtoon&keyword={encodedName}");
            var searchDoc = web.Load(searchUri);

            // 첫 번째 검색
            var links = searchDoc.DocumentNode.SelectNodes(
                "//div[@class=\"resultBox\"][1]/ul[@class=\"resultList\"]/li/h5/a"
            );
            if (links == null) yield break;
            foreach (var link in links)
            {
                string value = link.Attributes["href"].Value;
                string httpsComicNaverCom = "https://comic.naver.com" + value;
                var linkUri = new Uri(httpsComicNaverCom);

                // titleId
                string? titleId = HttpUtility.ParseQueryString(linkUri.Query)["titleId"];
                if (titleId == null) continue;
                int id = int.Parse(titleId);

                string name = link.InnerText;

                yield return (name, id);
            }
        }

        private static string? GetWebtoonName(HtmlDocument doc)
        {
            return doc.DocumentNode.SelectSingleNode("/html/head/title")?
                .InnerText[..^10]?
                .ReplaceSpecialCharacter();
        }


        private static string? GetWebtoonAuthor(HtmlDocument doc)
        {
            return doc.DocumentNode
                .SelectSingleNode("//*[@id='content']/div[@class='comicinfo']/div[@class='detail']/h2/span")?
                .InnerText?
                .Trim();
        }


        private static string? GetWebtoonDetailInfo(HtmlDocument doc)
        {
            return doc.DocumentNode.SelectNodes("//div[@class='detail']/p/text()")?
                .Select(node => node.InnerText)
                .Aggregate("", (s1, s2) => $"{s1}\n{s2}")
                .Trim();
        }


        private static string? GetWebtoonGenre(HtmlDocument doc)
        {
            return doc.DocumentNode.SelectSingleNode("//span[@class='genre']")?.InnerText;
        }


        private static string? GetWebtoonThumbnailUrl(HtmlDocument doc)
        {
            return doc.DocumentNode.SelectSingleNode("//*[@id='content']/div/div/a/img")?.Attributes["src"].Value;
        }

        public override string ToString()
        {
            return $"[NaverWebtoon]{Author}-{WebtoonName}({Id})";
        }
    }
}