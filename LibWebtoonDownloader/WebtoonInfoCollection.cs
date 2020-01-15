using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LibWebtoonDownloader
{
    /// <summary>
    /// 웹툰 정보 리스트 클래스에 저장 및 불러오기 메소드를 추가한 클래스입니다.
    /// </summary>
    [Serializable]
    public class WebtoonInfoCollection : List<WebtoonInfo>
    {
        public void Save(string fileName)
        {
            Stream ws = new FileStream(fileName, FileMode.Create);
            BinaryFormatter serializer = new BinaryFormatter();

            serializer.Serialize(ws, this);

            ws.Close();
        }
        public void Save()
        {
            Save("WebtoonInfos.dat");
        }

        public static WebtoonInfoCollection Load(string fileName)
        {
            if(!File.Exists(fileName))
            {
                throw new FileNotFoundException("File Not Found", fileName);
            }

            Stream rs = new FileStream(fileName, FileMode.Open);
            BinaryFormatter deserializer = new BinaryFormatter();

            WebtoonInfoCollection result = (WebtoonInfoCollection)deserializer.Deserialize(rs);

            rs.Close();

            return result;
        }

        public static WebtoonInfoCollection Load()
        {
            return Load("WebtoonInfos.dat");
        }

        public override bool Equals(object obj)
        {
            if(obj == null || (this.GetType() != obj.GetType()))
            {
                return false;
            }
            else
            {
                WebtoonInfoCollection temp = (WebtoonInfoCollection)obj;
                if(this.Count == temp.Count)
                {
                    for(int i = 0 ; i < this.Count ; i++)
                    {
                        if(this[i] != temp[i])
                        {
                            return false;
                        }
                    }
                    return true;
                }
                return false;
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 19;
                foreach(WebtoonInfo info in this)
                {
                    hash = hash * 31 + info.GetHashCode();
                }

                return hash;
            }
        }

        public static bool operator ==(WebtoonInfoCollection item1, object item2)
        {
            return item1.Equals(item2);
        }

        public static bool operator !=(WebtoonInfoCollection item1, object item2)
        {
            return !item1.Equals(item2);
        }

        public override string ToString()
        {
            string result = string.Empty;
            for(int i = 0 ; i < this.Count ; i++)
            {
                result += this[i].ToString();
                result += "\n";
            }

            return result;
        }
    }
}