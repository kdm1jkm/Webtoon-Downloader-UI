using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Web;
using System.Windows;

namespace LibWebtoonDownloader
{
    public static class Webtoon
    {
        /// <summary>
        /// 이미지를 다운로드합니다.
        /// </summary>
        /// <param name="url">이미지 Url</param>
        /// <param name="dir">저장할 위치</param>
        public static void DownloadImage(string url, string dir)
        {
            //다운로드 클라이언트 클래스를 생성 후 헤더를 입력
            WebClient downloader = new WebClient();
            downloader.Headers.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_5) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/50.0.2661.102 Safari/537.36");

            //다운로드와 로그 남기기
            downloader.DownloadFile(url, dir);
            Console.WriteLine("Download complete");
            Console.WriteLine("Url: " + url);
            Console.WriteLine("dir: " + dir);
            Console.WriteLine();
        }



        /// <summary>
        /// 웹툰을 PC에서 보기 위해 html파일을 만듭니다.
        /// </summary>
        /// <param name="fileName">파일명</param>
        /// <param name="imgs">이미지 경로 리스트</param>
        public static void MakeHtml(string fileName, List<string> imgs)
        {
            using(StreamWriter ws = new StreamWriter(fileName))
            {
                ws.Write(
                    "<!DOCTYPE html>" +
                    "<head>" +
                    "<title>" +
                    "Naver Webtoon Downloader" +
                    "</title>" +
                    "</head>" +
                    "<body>" +
                    "<div style='display:flex;flex-direction:column;align-items:center'>"
                    );
                foreach(string img in imgs)
                {
                    ws.Write(string.Format("<img src='{0}'>", img));
                }
                ws.Write(
                    "</div>" +
                    "</body>"
                    );
            }
        }



        public static int GetLatestEpisode(HtmlDocument doc)
        {
            if(!IsAvailable(doc))
            {
                return -1;
            }

            //가장 최신 에피소드의 링크
            HtmlNode link = doc.DocumentNode.SelectSingleNode("//td[@class=\"title\"]/a");

            //링크 주소 파싱
            HtmlAttribute attHref = link.Attributes["href"];

            //주소에서 no값 파싱후 리턴
            string query = attHref.Value.Split('?')[1];
            int lastEp = int.Parse(HttpUtility.ParseQueryString(query)["no"]);
            //lastEp = int.Parse(href.Split('?')[1].Split('&')[1].Split('=')[1]);
            return lastEp;
        }
        /// <summary>
        /// 한 웹툰의 최신 회차가 몇인지 찾아냅니다. 실패 시 -1을 반환합니다.
        /// </summary>
        /// <param name="titleId">웹툰 Id</param>
        /// <returns></returns>
        public static int GetLatestEpisode(int titleId)
        {
            //url접속
            string url = string.Format("https://comic.naver.com/webtoon/list.nhn?titleId={0}", titleId);
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc;
            doc = web.Load(url);
            //doc = new HtmlDocument();
            //doc.Load(@"D:\dropbox\Downloads\test for crawling\연애혁명 __ 네이버 만화.htm");

            return GetLatestEpisode(doc);
        }



        public static int GetWebtoonId(HtmlDocument doc)
        {
            int titleId;

            //첫 번째 검색결과 xpath
            HtmlNode link = doc.DocumentNode.SelectSingleNode("//div[@class=\"resultBox\"]/ul[@class=\"resultList\"]/li/h5/a");

            //검색 결과가 존재하지 않는다면 실패(-1)
            if(link == null)
            {
                return -1;
            }

            //링크 주소 파싱
            HtmlAttribute attHref = link.Attributes["href"];

            //쿼리 부분만 분리
            string query = attHref.Value.Split('?')[1];

            //titleId 파싱 후 리턴
            titleId = int.Parse(HttpUtility.ParseQueryString(query)["titleId"]);
            //titleId = int.Parse(href.Split('?')[1].Split('=')[1]);
            return titleId;
        }

        /// <summary>
        /// 웹툰의 이름으로 Id를 찾아냅니다. 실패 시 -1을 반환합니다.
        /// </summary>
        /// <param name="name">웹툰명</param>
        /// <returns>
        /// 성공: 웹툰명
        /// 실패: -1
        /// </returns>
        public static int GetWebtoonId(string name)
        {
            //웹툰 이름으로 접속
            string url = string.Format(@"https://comic.naver.com/search.nhn?m=webtoon&keyword={0}", name);
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc;

            doc = web.Load(url);
            //doc = new HtmlDocument();
            //doc.Load(@"D:\dropbox\Downloads\test for crawling\윰세포_검색_존재.html");
            //doc.Load(@"D:\dropbox\Downloads\test for crawling\검색_결과없음.html");

            return GetWebtoonId(doc);
        }



        public static bool IsAvailable(HtmlDocument doc)
        {
            //html title 파싱
            HtmlNode nodeTitle = doc.DocumentNode.SelectSingleNode("html/head/title");
            string title = nodeTitle.InnerText;

            //타이틀이 "네이버 만화"일 경우는 존재하지 않는 웹툰 페이지에서 메인페이지로 리다이렉트된 것으므로 유효하지 않다고 판단
            if(title == "네이버 만화")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsAvailable(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);

            return IsAvailable(doc);
        }

        /// <summary>
        /// 한 웹툰의 지정된 회차가 실행 가능한지 검사합니다.
        /// </summary>
        /// <param name="titleId">웹툰 Id</param>
        /// <param name="no">웹툰 회차</param>
        /// <returns></returns>
        public static bool IsAvailable(int titleId, int no)
        {
            WebtoonInfo info = new WebtoonInfo
            {
                Id = titleId,
                No = no
            };

            return IsAvailable(info.Url);
        }

        /// <summary>
        /// 지정된 웹툰이 실행 가능한지 검사합니다.
        /// </summary>
        /// <param name="titleId">웹툰 Id</param>
        /// <returns></returns>
        public static bool IsAvailable(int titleId)
        {
            return IsAvailable(titleId, 1);
        }



        public static string GetWebtoonName(HtmlDocument doc)
        {
            if(!IsAvailable(doc))
            {
                return "";
            }

            HtmlNode nodeTitle = doc.DocumentNode.SelectSingleNode("/html/head/title");

            string title = nodeTitle.InnerText;

            title = title.Substring(0, title.Length - 10);

            return title.
                Replace("\\", "").
                Replace("/", "").
                Replace(":", "").
                Replace("*", "").
                Replace("?", "").
                Replace("\"", "").
                Replace("<", "").
                Replace(">", "").
                Replace("|", "");
        }

        /// <summary>
        /// 웹툰 Id로 이름을 찾아냅니다. 실패시 공백을 반환합니다.
        /// </summary>
        /// <param name="titleId">웹툰 Id</param>
        /// <returns>웹툰명</returns>
        public static string GetWebtoonName(int titleId)
        {
            HtmlDocument doc;
            HtmlWeb web = new HtmlWeb();

            WebtoonInfo tempInfo = new WebtoonInfo
            {
                Id = titleId,
                No = 1
            };

            doc = web.Load(tempInfo.Url);
            //doc = new HtmlDocument();
            //doc.Load(@"D:\dropbox\Downloads\test for crawling\연애혁명 페이지안 __ 네이버 만화.htm", Encoding.UTF8);

            return GetWebtoonName(doc);
        }

        public static string GetWebtoonName(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return GetWebtoonName(doc);
        }



        public static WebtoonInfoCollection GetEveryWebtoonInfos(HtmlDocument doc)
        {
            //웹툰 정보 리스트
            WebtoonInfoCollection webtoonInfos = new WebtoonInfoCollection();

            //웹툰 링크 선택
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//*[@id='content']/div[4]/div/div/ul/li/a");

            foreach(HtmlNode node in nodes)
            {
                //각 웹툰 주소
                HtmlAttribute attribute = node.Attributes["href"];

                //이름 파싱
                string name = node.InnerText;

                //주소에서 파싱을 위해 쿼리부분만 떼옴(맞는 표현인가?)
                string query = attribute.Value.Split('?')[1].Replace("amp;", "");

                //titleID, 요일 파싱
                int titleId = int.Parse(HttpUtility.ParseQueryString(query)["titleId"]);
                string s_weekDay = HttpUtility.ParseQueryString(query)["weekday"];

                //영어로 된 요일을 숫자로 변환(월요일=0 ~ 일요일=6)
                DayOfWeek weekday = new DayOfWeek();
                switch(s_weekDay)
                {
                    case "mon":
                        weekday = DayOfWeek.Monday;
                        break;
                    case "tue":
                        weekday = DayOfWeek.Tuesday;
                        break;
                    case "wed":
                        weekday = DayOfWeek.Wednesday;
                        break;
                    case "thu":
                        weekday = DayOfWeek.Thursday;
                        break;
                    case "fri":
                        weekday = DayOfWeek.Friday;
                        break;
                    case "sat":
                        weekday = DayOfWeek.Saturday;
                        break;
                    case "sun":
                        weekday = DayOfWeek.Sunday;
                        break;
                }

                //웹툰 정보 구조체
                WebtoonInfo tempInfo = new WebtoonInfo
                {
                    Id = titleId,
                    WebtoonName = name
                };
                tempInfo.Weekday[weekday] = true;

                //이미 리스트에 존재하는지 확인하는 플래그(여러 요일에 걸쳐서 연재하는 웹툰에 대비)
                bool alreadyExistFlag = false;

                //이미 리스트에 있을 경우에는기존 웹툰 정보에 요일만 추가
                foreach(WebtoonInfo info in webtoonInfos)
                {
                    if(info.Id == titleId)
                    {
                        alreadyExistFlag = true;
                        info.Weekday[weekday] = true;
                        break;
                    }
                }

                //이미 존재하지 않을 경우에는 리스트에 추가
                if(!alreadyExistFlag)
                {
                    webtoonInfos.Add(tempInfo);
                }
            }

            return webtoonInfos;
        }
        /// <summary>
        /// 네이버 웹툰 메인 페이지에서 모든 웹툰의 정보를 파싱합니다.
        /// </summary>
        /// <returns></returns>
        public static WebtoonInfoCollection GetEveryWebtoonInfos()
        {
            //웹툰 메인 페이지 접속
            HtmlDocument doc;
            HtmlWeb web = new HtmlWeb();
            const string url = "https://comic.naver.com/webtoon/weekday.nhn";
            //doc = new HtmlDocument();
            //doc.Load(@"D:\dropbox\Downloads\webtoonforcrawling\frontPage.htm", Encoding.UTF8);
            doc = web.Load(url);

            return GetEveryWebtoonInfos(doc);
        }



        [STAThread]
        public static WebtoonInfoCollection GetFavoriteWebtoonInfosFromAccount(string id, string password)
        {
            WebtoonInfoCollection result = new WebtoonInfoCollection();
            WebtoonInfoCollection everyInfos = GetEveryWebtoonInfos();

            using(IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl("https://nid.naver.com/nidlogin.login?url=https%3A%2F%2Fcomic.naver.com%2Fwebtoon%2Fweekday.nhn");
                Actions pasteId = new Actions(driver);
                Actions pastePassword = new Actions(driver);

                IWebElement idInput = driver.FindElement(By.XPath("//*[@id='id']"));
                Clipboard.SetText(id);
                pasteId.MoveToElement(idInput).KeyDown(Keys.Control).SendKeys("v").KeyUp(Keys.Control).Perform();
                //idInput.Clear();                
                //idInput.SendKeys(id);

                IWebElement passwordInput = driver.FindElement(By.XPath("//*[@id='pw']"));
                Clipboard.SetText(password);
                pastePassword.MoveToElement(passwordInput).Click().KeyDown(Keys.Control).SendKeys("v").KeyUp(Keys.Control).Perform();
                //passwordInput.Clear();
                //passwordInput.SendKeys(password);

                Clipboard.Clear();

                IWebElement loginBtn = driver.FindElement(By.XPath("//*[@id='log.login']"));
                loginBtn.Click();

                driver.Navigate().GoToUrl("https://comic.naver.com/webtoon/weekday.nhn");

                HtmlDocument mainHtml = new HtmlDocument();
                mainHtml.LoadHtml(driver.PageSource);

                int i = 0;

                HtmlNodeCollection mainNodes = mainHtml.DocumentNode.SelectNodes("//*[@id='content']/div/div/div/ul/li/div/a");
                foreach(HtmlNode mainNode in mainNodes)
                {
                    HtmlAttribute webtoonHref = mainNode.Attributes["href"];
                    string webtoonLink = "https://comic.naver.com" + webtoonHref.Value;
                    webtoonLink = webtoonLink.Replace("amp;", "");
                    driver.Navigate().GoToUrl(webtoonLink);

                    HtmlDocument webtoonHtml = new HtmlDocument();
                    webtoonHtml.LoadHtml(driver.PageSource);

                    HtmlNode webtoonNode = webtoonHtml.DocumentNode.SelectSingleNode("//*[@id='content']/div/div/ul/li/a[@class='book_maker on']");
                    if(webtoonNode == null)
                        continue;

                    string query = webtoonLink.Split('?')[1];
                    int titleId = int.Parse(HttpUtility.ParseQueryString(query)["titleId"]);

                    while(!( everyInfos[i].Id == titleId ))
                    {
                        i++;
                    }
                    result.Add(everyInfos[i]);
                }
            }
            return result;
        }



        public static WebtoonInfoCollection WebtoonInfoInPeriod(DateTime startDate, DateTime endDate, WebtoonInfo info)
        {
            WebtoonInfoCollection result = new WebtoonInfoCollection();

            int page = 0;

            while(true)
            {
                //웹툰 페이지 접속
                string url = $"https://comic.naver.com/webtoon/list.nhn?titleId={info.Id}&page={page}";
                HtmlDocument doc = url.LoadHtmlDocument();

                WebtoonInfoCollection infos = WebtoonInfoInPeriod(startDate, endDate, doc);

                if(infos.Count == 0)
                {
                    break;
                }

                result.AddRange(infos);

                page++;
            }
            return result;
        }
        public static WebtoonInfoCollection WebtoonInfoInPeriod(DateTime startDate, DateTime endDate, HtmlDocument doc)
        {
            WebtoonInfoCollection result = new WebtoonInfoCollection();

            //웹툰 제목 링크 선택
            HtmlNodeCollection wbtnLinkNodes = doc.DocumentNode.SelectNodes("//td[@class='title']/a");

            foreach(HtmlNode wbtnLinkNode in wbtnLinkNodes)
            {
                //현재 선택 웹툰의 업로드 날짜 파싱
                HtmlNode parent = wbtnLinkNode.ParentNode.ParentNode;
                HtmlNode dateNode = parent.SelectSingleNode("td[@class='num']");
                DateTime curWebtoonDate = DateTime.Parse(dateNode.InnerText);

                //하루 더하기(웹툰은 하루 전날 올라온 것으로 취급됨)
                curWebtoonDate = curWebtoonDate.AddDays(1);
                curWebtoonDate = curWebtoonDate.AddHours(12);

                //웹툰 주소로부터 회차 파싱
                HtmlAttribute href = wbtnLinkNode.Attributes["href"];
                string query = href.Value.Split('?')[1].Replace("amp;", "");
                int no = int.Parse(HttpUtility.ParseQueryString(query)["no"]);

                //날짜 범위 검사
                //현재 날짜가 시작 날짜보다 이전이라면 루프 탈출
                if(curWebtoonDate < startDate)
                {
                    break;
                }

                //현재 날짜가 마지막 날짜보다 이후라면
                if(curWebtoonDate > endDate)
                {
                    if(no != 1)
                    {
                        //이전 화로 가면 범위 안으로 들어올 가능성이 있기 때문에 continue
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                //조건에 맞으므로 task에 추가
                WebtoonInfo newInfo = new WebtoonInfo
                {
                    Id = int.Parse(HttpUtility.ParseQueryString(query)["titleId"]),
                    No = no
                };

                result.Add(newInfo);

                //처음 웹툰일경우 무한루프 방지를 위해 브레이크
                if(no == 1)
                {
                    break;
                }
            }

            return result;
        }



        public static string GetWebtoonAuthor(HtmlDocument doc)
        {
            HtmlNode nodeAuthor = doc.DocumentNode.SelectSingleNode("//*[@id='content']/div[@class='comicinfo']/div[@class='detail']/h2/span");
            string result = nodeAuthor.InnerText;
            result = result.Trim();

            return result;
        }



        public static string GetWebtoonDetailInfo(HtmlDocument doc)
        {
            HtmlNodeCollection nodeDetailInfos = doc.DocumentNode.SelectNodes("//div[@class='detail']/p/text()");
            string detailInfo = string.Empty;

            if(nodeDetailInfos == null)
            {
                return "WEBTOON_DETAIL_INFO_NOT_FOUND";
            }

            foreach(HtmlNode node in nodeDetailInfos)
            {
                detailInfo += node.InnerText;
                detailInfo += "\n";
            }

            return detailInfo.Trim();
        }



        public static string GetWebtoonGenre(HtmlDocument doc)
        {
            HtmlNode nodeGenre = doc.DocumentNode.SelectSingleNode("//span[@class='genre']");

            return nodeGenre.InnerText;
        }



        public static int GetWebtoonImageCount(HtmlDocument doc)
        {
            HtmlNodeCollection nodeImgs = doc.DocumentNode.SelectNodes("//img[@alt='comic content']");

            return nodeImgs.Count;
        }



        public static string[] GetWebtoonImageSrcs(HtmlDocument doc)
        {
            HtmlNodeCollection nodeImgs = doc.DocumentNode.SelectNodes("//img[@alt='comic content']");

            int count = nodeImgs.Count;

            string[] srcs = new string[count];

            for(int i = 0 ; i < count ; i++)
            {
                HtmlNode nodeImg = nodeImgs[i];
                HtmlAttribute attSrc = nodeImg.Attributes["src"];

                srcs[i] = attSrc.Value;
            }

            return srcs;
        }



        public static string GetWebtoonThumbnailUrl(HtmlDocument doc)
        {
            HtmlNode nodeThumbnail = doc.DocumentNode.SelectSingleNode("//*[@id='content']/div/div/a/img");
            HtmlAttribute attSrc = nodeThumbnail.Attributes["src"];
            return attSrc.Value;
        }
    }
}