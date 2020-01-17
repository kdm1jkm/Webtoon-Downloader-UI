using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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

            ImageCount = 0;

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


        public string Url { get => $"https://comic.naver.com/webtoon/detail.nhn?titleId={Id}&no={No}"; }
        public int ImageCount { get; private set; }
        public Dictionary<DayOfWeek, bool> Weekday { get; set; } = new Dictionary<DayOfWeek, bool>(7);
        public DayOfWeek[] Weekdays
        {
            get
            {
                DayOfWeek[] result = new DayOfWeek[7];
                for(int i = 0 ; i < 7 ; i++)
                {
                    if(Weekday[(DayOfWeek)i])
                    {
                        result[result.Length] = (DayOfWeek)i;
                    }
                }

                return result;
            }
            set
            {
                int j = 0;
                for(int i = 0 ; i < value.Length ; i++)
                {
                    while(j < (int)value[i])
                    {
                        Weekday[(DayOfWeek)j] = false;
                        j++;
                    }
                    Weekday[(DayOfWeek)j] = true;
                    j++;
                }
            }
        }
        public bool IsAvailable { get; private set; }
        public string[] ImageSrcs { get; private set; }


        public override string ToString()
        {
            return $"{WebtoonName}_{No}[{Weekday.Stringify()}]";
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

        public void LoadInformations()
        {
            if(Id != 0)
            {
                string url = $"https://comic.naver.com/webtoon/list.nhn?titleId={Id}";

                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc;
                doc = web.Load(url);

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
                }

                if(No != 0)
                {
                    web = new HtmlWeb();
                    doc = web.Load(Url);

                    IsAvailable = Webtoon.IsAvailable(doc);

                    if(!IsAvailable)
                    {
                        Initialize();
                        return;
                    }
                    else
                    {
                        ImageCount = Webtoon.GetWebtoonImageCount(doc);
                        ImageSrcs = Webtoon.GetWebtoonImageSrcs(doc);

                        //ToDo: 요일 가져오기
                    }
                }
            }
        }
    }
}