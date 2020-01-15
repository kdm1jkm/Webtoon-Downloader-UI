using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LibWebtoonDownloader
{
    /// <summary>
    /// 메타데이터 구조체입니다. 파일로 저장하기와 불러오기가 내장되어 있습니다.
    /// </summary>
    [Serializable]
    public struct MetaData
    {
        /// <summary>
        /// 웹툰명
        /// </summary>
        public string WebtoonName { get; set; }

        /// <summary>
        /// 웹툰 Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 웹툰 회차
        /// </summary>
        public int No { get; set; }

        /// <summary>
        /// 이미지 갯수
        /// </summary>
        public int ImgCnt { get; set; }

        /// <param name="webtoonName">웹툰명</param>
        /// <param name="id">웹툰 Id</param>
        /// <param name="no">웹툰 회차</param>
        /// <param name="imgCnt">이미지 갯수</param>
        public MetaData(string webtoonName, int id, int no, int imgCnt)
        {
            this.WebtoonName = webtoonName;
            this.Id = id;
            this.No = no;
            this.ImgCnt = imgCnt;
        }

        /// <summary>
        /// 메타데이터를 파일로 저장합니다.
        /// </summary>
        /// <param name="fileName">파일명</param>
        public void Save(string fileName)
        {
            Stream ws = new FileStream(fileName, FileMode.Create);
            BinaryFormatter serializer = new BinaryFormatter();

            serializer.Serialize(ws, this);

            ws.Close();
        }

        /// <summary>
        /// 메타데이터를 파일로 저장합니다. 기본 파일명은 metaData.dat입니다.
        /// </summary>
        public void Save()
        {
            Save("metaData.dat");
        }

        /// <summary>
        /// 메타데이터를 파일로부터 불러옵니다.
        /// </summary>
        /// <param name="fileName">파일명</param>
        public static MetaData Load(string fileName)
        {
            if(File.Exists(fileName))
            {
                using(Stream rs = new FileStream(fileName, FileMode.Open))
                {
                    BinaryFormatter deserializer = new BinaryFormatter();
                    return (MetaData)deserializer.Deserialize(rs);
                }
            }

            throw new FileNotFoundException("File Not Found", fileName);
        }

        /// <summary>
        /// 메타데이터를 파일로부터 불러옵니다. 기본 파일명은 metaData.dat입니다.
        /// </summary>
        public static MetaData Load()
        {
            return Load("metaData.dat");
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", WebtoonName, No);
        }

        public override int GetHashCode()
        {
            Func<int, int, int> addHash = new Func<int, int, int>((int h, int value) =>
            {
                return h * 31 + value;
            });
            int hash = 19;
            hash = addHash(hash, WebtoonName.GetHashCode());
            hash = addHash(hash, Id.GetHashCode());
            hash = addHash(hash, No.GetHashCode());
            hash = addHash(hash, ImgCnt.GetHashCode());

            return hash;
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

        public static bool operator ==(MetaData left, object right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MetaData left, object right)
        {
            return !left.Equals(right);
        }
    }
}