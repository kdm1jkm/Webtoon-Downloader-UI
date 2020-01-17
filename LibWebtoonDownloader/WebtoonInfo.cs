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
                Weeks[(DayOfWeek)i] = false;
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


        public string WebtoonName { get; private set; }
        public string Author { get; private set; }
        public string DetailInfo { get; private set; }
        public string Genre { get; private set; }


        private int id { get; set; }
        public int Id
        {
            get => id;
            set
            {
                id = value;

                if(id == 0)
                {
                    return;
                }

                string url = $"https://comic.naver.com/webtoon/list.nhn?titleId={id}";

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
            }
        }

        private int no { get; set; }
        public int No
        {
            get => no;
            set
            {
                no = value;

                if(no == 0)
                {
                    return;
                }

                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load(Url);

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
        private Dictionary<DayOfWeek, bool> Weeks { get; set; } = new Dictionary<DayOfWeek, bool>(7);
        public bool IsAvailable { get; private set; }
        public string[] ImageSrcs { get; private set; }


        public override string ToString()
        {
            return $"{WebtoonName}_{No}[{Weeks.Stringify()}]";
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
    }
}