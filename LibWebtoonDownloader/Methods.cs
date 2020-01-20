using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace LibWebtoonDownloader
{
    public static class Methods
    {
        public static string Stringify(this Dictionary<DayOfWeek, bool> weeks)
        {
            string result = "";
            string[] weeksStr = { "일", "월", "화", "수", "목", "금", "토" };

            for(int i = 0 ; i < 7 ; i++)
            {
                if(weeks[(DayOfWeek)i])
                {
                    result += weeksStr[i] + ", ";
                }
            }

            result = result.Substring(0, result.Length - 2);

            return result;
        }

        public static HtmlDocument LoadHtmlDocument(this string url)
        {
            HtmlWeb web = new HtmlWeb();
            return web.Load(url);
        }
    }
}