using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using OpenQA.Selenium.Internal;

namespace LibWebtoonDownloader
{
    public class WebtoonInfoCollection : List<WebtoonInfo>
    {
        public void Save(string fileName)
        {
            using(Stream ws = new FileStream(fileName, FileMode.Create))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(ws, this);
            }
        }

        public static WebtoonInfoCollection Load(string fileName)
        {
            if(File.Exists(fileName))
            {
                using(Stream rs = new FileStream(fileName, FileMode.Open))
                {
                    BinaryFormatter deserializer = new BinaryFormatter();
                    return (WebtoonInfoCollection)deserializer.Deserialize(rs);
                }
            }
            else
            {
                throw new FileNotFoundException("파일을 찾을 수 없습니다.", fileName);
            }
        }
        public static bool TryLoad(string fileName, out WebtoonInfoCollection info)
        {
            if(File.Exists(fileName))
            {
                using(Stream rs = new FileStream(fileName, FileMode.Open))
                {
                    BinaryFormatter deserializer = new BinaryFormatter();
                    info = (WebtoonInfoCollection)deserializer.Deserialize(rs);
                }
                return true;
            }
            else
            {
                info = null;
                return false;
            }
        }


        public override string ToString()
        {
            return this.GetType().ToString();
        }

        public override int GetHashCode()
        {
            int hash = 19;
            for(int i = 0 ; i < this.Count ; i++)
            {
                hash += hash * 31 + this[i].GetHashCode();
            }
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

        public static bool operator ==(WebtoonInfoCollection left, object right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(WebtoonInfoCollection left, object right)
        {
            return left.Equals(right);
        }



        public static implicit operator WebtoonInfoCollection (WebtoonInfoQueue infos)
        {
            WebtoonInfoCollection result = new WebtoonInfoCollection();
            foreach(WebtoonInfo info in infos.ToArray())
            {
                result.Add(info);
            }
            return result;
        }
    }
}
