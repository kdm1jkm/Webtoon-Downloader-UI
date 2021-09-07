using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace LibWebtoonDownloader.NaverWebtoon
{
    public class NaverWebtoonTask : AbstractWebtoonTask
    {
        private readonly int _id;
        private readonly int _no;
        private readonly NaverWebtoon _parent;
        private readonly Uri _uri;

        private ConcurrentQueue<Uri>? _imageQueue;

        public NaverWebtoonTask(int id, int no, NaverWebtoon parent)
        {
            _id = id;
            _no = no;
            _parent = parent;
            _uri = new Uri($"https://comic.naver.com/webtoon/detail?titleId={id}&no={no}");
        }

        protected override ConcurrentQueue<Uri> ImageQueue
        {
            get { return _imageQueue ??= new ConcurrentQueue<Uri>(GetWebtoonImageSource(new HtmlWeb().Load(_uri))!); }
        }

        private static IEnumerable<Uri>? GetWebtoonImageSource(HtmlDocument doc)
        {
            return doc.DocumentNode
                .SelectNodes("//img[@alt='comic content']")?
                .Select(nodeImg => nodeImg.Attributes["src"].Value)
                .Select(src => new Uri(src));
        }

        public override string ToString()
        {
            return $"{_parent.WebtoonName}({_id})/{_no:D4}";
        }
    }
}