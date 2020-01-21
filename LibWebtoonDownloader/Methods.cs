using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace LibWebtoonDownloader
{
    /// <summary>
    /// 확장 메서드 정의 클래스
    /// </summary>
    public static class Methods
    {
        /// <summary>
        /// DayOfWeek딕셔너리를 문자화합니다.
        /// </summary>
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

        /// <summary>
        /// url에 접속해 HtmlDocument를 가져옵니다.
        /// </summary>
        /// <param name="url">접속할 Url입니다.</param>
        /// <returns>htmlDocument</returns>
        public static HtmlDocument LoadHtmlDocument(this string url)
        {
            HtmlWeb web = new HtmlWeb();
            return web.Load(url);
        }
    }
}