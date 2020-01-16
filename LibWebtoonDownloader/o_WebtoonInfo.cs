using System;
using System.Collections.Generic;

namespace LibWebtoonDownloader
{
    /// <summary>
    /// 웹툰 정보 구조체입니다.
    /// </summary>
    [Serializable]
    public struct o_WebtoonInfo
    {
        public string Name { get; set; }
        public int Id { get; set; }
        /// <summary>
        /// 요일
        /// </summary>
        public List<DayOfWeek> Weekdays { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">웹툰명</param>
        /// <param name="id">웹툰 Id</param>
        /// <param name="weekday">연재 요일</param>
        public o_WebtoonInfo(string name, int id, List<DayOfWeek> weekday)
        {
            this.Name = name;
            this.Id = id;
            this.Weekdays = new List<DayOfWeek>(weekday);
        }


        public override string ToString()
        {
            string weekdayStr = "[";

            for(int i = 0 ; i < this.Weekdays.Count ; i++)
            {
                switch(this.Weekdays[i])
                {
                    case DayOfWeek.Monday:
                        weekdayStr += "월";
                        break;

                    case DayOfWeek.Tuesday:
                        weekdayStr += "화";
                        break;

                    case DayOfWeek.Wednesday:
                        weekdayStr += "수";
                        break;

                    case DayOfWeek.Thursday:
                        weekdayStr += "목";
                        break;

                    case DayOfWeek.Friday:
                        weekdayStr += "금";
                        break;

                    case DayOfWeek.Saturday:
                        weekdayStr += "토";
                        break;

                    case DayOfWeek.Sunday:
                        weekdayStr += "일";
                        break;
                }

                if(i != this.Weekdays.Count - 1)
                {
                    weekdayStr += ",";
                }
            }

            weekdayStr += "]";

            return this.Name + weekdayStr;
        }

        public override bool Equals(object obj)
        {
            if(obj == null || (this.GetType() != obj.GetType()))
            {
                return false;
            }
            else
            {
                o_WebtoonInfo temp = (o_WebtoonInfo)obj;
                return this.Id == temp.Id;
            }
        }

        public override int GetHashCode()
        {
            return this.Id;
        }

        public static bool operator ==(o_WebtoonInfo item1, object item2)
        {
            return item1.Equals(item2);
        }

        public static bool operator !=(o_WebtoonInfo item1, object item2)
        {
            return !(item1 == item2);
        }
    }
}