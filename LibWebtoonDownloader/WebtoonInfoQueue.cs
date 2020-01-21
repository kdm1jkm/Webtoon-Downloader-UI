using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace LibWebtoonDownloader
{
    public class WebtoonInfoQueue:Queue<WebtoonInfo>
    {
        public void Save(string fileName)
        {
            using(Stream ws = new FileStream(fileName, FileMode.Create))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(ws, this);
            }
        }

        public static WebtoonInfoQueue Load(string fileName)
        {
            if(!File.Exists(fileName))
            {
                throw new FileNotFoundException("File Not Found", fileName);
            }
            else
            {
                using(Stream rs = new FileStream(fileName, FileMode.Open))
                {
                    BinaryFormatter deserializer = new BinaryFormatter();
                    return (WebtoonInfoQueue)deserializer.Deserialize(rs);
                }
            }
        }
    }
}
