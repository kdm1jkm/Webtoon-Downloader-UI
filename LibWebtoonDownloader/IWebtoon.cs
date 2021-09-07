using System;
using System.Collections.Generic;

namespace LibWebtoonDownloader
{
    /// <summary>
    ///     한 웹툰 종류를 나타냄. 여기에서 IWebtoonTask 생성.
    /// </summary>
    public interface IWebtoon
    {
        public string WebtoonName { get; }
        public string Author { get; }
        public string Description { get; }
        public Uri Uri { get; }
        public Uri? ThumbnailUrl { get; }

        public IEnumerable<AbstractWebtoonTask>? GetEveryTask();
        AbstractWebtoonTask? GetTask(int number);
    }
}