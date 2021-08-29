using System;
using System.Collections.Generic;
using LibWebtoonDownloader.WebtoonTask;

namespace LibWebtoonDownloader.Webtoon
{
    /// <summary>
    ///     한 웹툰 종류를 나타냄. 여기에서 IWebtoonTask 생성.
    /// </summary>
    public interface IWebtoon
    {
        public string WebtoonName { get; }
        public string? Author { get; }
        public string? DetailInfo { get; }
        public string? Genre { get; }
        public Uri Uri { get; }
        public Uri? ThumbnailUrl { get; }

        public IEnumerable<AbstractWebtoonTask>? GetEveryTask();
        AbstractWebtoonTask GetTaskByNo(int no);
    }
}