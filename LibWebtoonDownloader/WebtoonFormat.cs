using System;

namespace LibWebtoonDownloader
{
    /// <summary>
    /// html과 zip로 변환 여부를 담고 있는 구조체입니다.
    /// </summary>
    [Serializable]
    public struct WebtoonFormat
    {
        public bool Html { get; set; }
        public bool Zip { get; set; }

        public WebtoonFormat(bool html, bool zip)
        {
            this.Html = html;
            this.Zip = zip;
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

        public override int GetHashCode()
        {
            int hashCode = 0;
            if(Html)
            { hashCode += 1; }
            if(Zip)
            { hashCode += 2; }

            return hashCode;
        }

        public override string ToString()
        {
            return string.Format("Html: {0}, Zip: {1}", Html ? "True" : "False", Zip ? "True" : "False");
        }

        public static bool operator ==(WebtoonFormat format, object obj)
        {
            return format.Equals(obj);
        }

        public static bool operator !=(WebtoonFormat format, object obj)
        {
            return !format.Equals(obj);
        }
    }
}