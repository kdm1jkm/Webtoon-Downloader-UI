using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System.Text;

namespace LibWebtoonDownloader
{
    [Serializable]
    public class WebtoonInfo
    {
        public WebtoonInfo()
        {
            Initialize();
        }

        private void Initialize()
        {
            IsAvailable = false;

            for(int i = 0 ; i < 7 ; i++)
            {
                Weekday[(DayOfWeek)i] = false;
            }

            Id = 0;
            No = 0;

            WebtoonName = string.Empty;
            Author = string.Empty;
            DetailInfo = string.Empty;
            Genre = string.Empty;

            ImageSrcs = null;
        }

        public string WebtoonName { get; set; }
        public string Author { get; set; }
        public string DetailInfo { get; set; }
        public string Genre { get; set; }

        public int Id { get; set; }
        public int No { get; set; }

        public bool TrySetIdFromName(string name)
        {
            int result = Webtoon.GetWebtoonId(name);

            if(result == -1)
            {
                return false;
            }
            else
            {
                Id = result;
                return true;
            }
        }

        private static HtmlDocument WebtoonMainpage { get; set; } = null;

        public string Url { get => $"https://comic.naver.com/webtoon/detail.nhn?titleId={Id}&no={No}"; }
        public int ImageCount { get => ImageSrcs.Length; }
        public Dictionary<DayOfWeek, bool> Weekday { get; set; } = new Dictionary<DayOfWeek, bool>(7);
        public List<DayOfWeek> Weekdays
        {
            get
            {
                List<DayOfWeek> result = new List<DayOfWeek>();
                for(int i = 0 ; i < 7 ; i++)
                {
                    if(Weekday[(DayOfWeek)i])
                    {
                        result.Add((DayOfWeek)i);
                    }
                }

                return result;
            }
            set
            {
                for(int i = 0 ; i < 7 ; i++)
                {
                    var weekDayQuery = from weekDay in value
                                       where weekDay == (DayOfWeek)i
                                       select weekDay;

                    Weekday[(DayOfWeek)i] = weekDayQuery.Count() != 0;
                }
            }
        }
        public bool IsAvailable { get; private set; }
        public string[] ImageSrcs { get; private set; }


        public string ThumbnailUrl { get; private set; }
        public string ThumbnailPath { get; set; }


        public override string ToString()
        {
            if(Id == 0)
            {
                return "";
            }
            else if(No != 0)
            {
                return $"{WebtoonName}({No})";
            }
            else
            {
                return $"{WebtoonName}[{Weekday.Stringify()}]";
            }
        }
        public override int GetHashCode()
        {
            return Url.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if(this.GetType() == obj.GetType())
            {
                return this.GetHashCode() == obj.GetHashCode();
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(WebtoonInfo left, object right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(WebtoonInfo left, object right)
        {
            return !left.Equals(right);
        }

        public void Save(string fileName)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            using(Stream ws = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(ws, this);
            }
        }

        public static WebtoonInfo Load(string fileName)
        {
            if(!File.Exists(fileName))
            {
                throw new FileNotFoundException("파일을 찾을 수 없습니다.", fileName);
            }
            else
            {
                using(Stream rs = new FileStream(fileName, FileMode.Open))
                {
                    BinaryFormatter deserializer = new BinaryFormatter();
                    return (WebtoonInfo)deserializer.Deserialize(rs);
                }
            }
        }
        public static bool TryLoad(string fileName, out WebtoonInfo info)
        {
            if(!File.Exists(fileName))
            {
                info = null;
                return false;
            }
            else
            {
                using(Stream rs = new FileStream(fileName, FileMode.Open))
                {
                    BinaryFormatter deseralizer = new BinaryFormatter();
                    info = (WebtoonInfo)deseralizer.Deserialize(rs);
                }
                return true;
            }
        }

        public void LoadWebtoonInfo(HtmlDocument doc)
        {
            IsAvailable = Webtoon.IsAvailable(doc);
            if(!IsAvailable)
            {
                Initialize();
                return;
            }
            else
            {
                WebtoonName = Webtoon.GetWebtoonName(doc);
                Author = Webtoon.GetWebtoonAuthor(doc);
                DetailInfo = Webtoon.GetWebtoonDetailInfo(doc);
                Genre = Webtoon.GetWebtoonGenre(doc);
                ThumbnailUrl = Webtoon.GetWebtoonThumbnailUrl(doc);

                if(WebtoonMainpage == null)
                {
                    string url = "https://comic.naver.com/webtoon/weekday.nhn";
                    WebtoonMainpage = url.LoadHtmlDocument();
                }

                WebtoonInfoCollection everyInfo = Webtoon.GetEveryWebtoonInfos(WebtoonMainpage);
                var infoQuery = from info in everyInfo
                                where info.Id == Id
                                select info;

                var temp = infoQuery.ToArray()[0].Weekdays;
                Weekdays = temp;
            }
        }
        public void LoadWebtoonInfo()
        {
            string url = $"https://comic.naver.com/webtoon/list.nhn?titleId={Id}";

            HtmlDocument doc = url.LoadHtmlDocument();

            LoadWebtoonInfo(doc);
        }

        public void LoadDetailInfo(HtmlDocument doc)
        {
            IsAvailable = Webtoon.IsAvailable(doc);

            if(!IsAvailable)
            {
                Initialize();
                return;
            }
            else
            {
                ImageSrcs = Webtoon.GetWebtoonImageSrcs(doc);
            }
        }
        public void LoadDetailInfo()
        {
            HtmlDocument doc = Url.LoadHtmlDocument();

            LoadDetailInfo(doc);
        }

        public WebtoonInfo Copy()
        {
            return new WebtoonInfo()
            {
                Author=Author,
                DetailInfo=DetailInfo,
                Genre=Genre,
                Id=Id,
                ImageSrcs=ImageSrcs,
                IsAvailable=IsAvailable,
                No=No,
                WebtoonName=WebtoonName,
                Weekday=Weekday,
            };
        }
    }
}