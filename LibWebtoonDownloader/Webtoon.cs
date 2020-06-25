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
using System.Threading.Tasks;
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
            //다운로드 클라이언트 클래스
            WebClient downloader = new WebClient();

            //다운로드 헤더 입력
            downloader.Headers.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_5) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/50.0.2661.102 Safari/537.36");

            //다운로드
            downloader.DownloadFile(url, dir);

            //콘솔에 로그남기기
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



        /// <summary>
        /// HtmlDocument클래스에서 가장 최신 회차의 정보를 파싱합니다.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
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
            return lastEp;
        }

        /// <summary>
        /// 한 웹툰의 최신 회차가 몇인지 찾아냅니다. 실패 시 -1을 반환합니다.
        /// </summary>
        /// <param name="id">웹툰 Id</param>
        /// <returns></returns>
        public static int GetLatestEpisode(int id)
        {
            //url접속
            string url = string.Format("https://comic.naver.com/webtoon/list.nhn?titleId={0}", id);
            HtmlDocument doc = url.LoadHtmlDocument();
            return GetLatestEpisode(doc);
        }



        /// <summary>
        /// HtmlDocument클래스로부터 웹툰Id를 파싱합니다.
        /// </summary>
        /// <param name="doc">파싱할 HtmlDocument클래스</param>
        /// <returns></returns>
        public static int GetWebtoonId(HtmlDocument doc)
        {
            //반환할 값
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
            return titleId;
        }

        /// <summary>
        /// 웹툰의 이름으로 Id를 찾아냅니다. 실패 시 -1을 반환합니다.
        /// </summary>
        /// <param name="webtoonName">웹툰명</param>
        /// <returns>
        /// 성공: 웹툰명
        /// 실패: -1
        /// </returns>
        public static int GetWebtoonId(string webtoonName)
        {
            //웹툰 이름으로 접속
            string url = string.Format(@"https://comic.naver.com/search.nhn?m=webtoon&keyword={0}", webtoonName);
            HtmlDocument doc = url.LoadHtmlDocument();

            return GetWebtoonId(doc);
        }



        /// <summary>
        /// HtmlDocument클래스가 올바른 웹툰 정보를 담고 있는지 검사합니다.
        /// </summary>
        /// <param name="doc">검사할 HtmlDocument클래스</param>
        /// <returns>올바른지 여부</returns>
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

        /// <summary>
        /// 지정된 Url주소가 올바른 웹툰 정보를 담고 있는지 검사합니다.
        /// </summary>
        /// <param name="url">검사할 Url주소</param>
        /// <returns>올바른지 여부</returns>
        public static bool IsAvailable(string url)
        {
            HtmlDocument doc = url.LoadHtmlDocument();

            return IsAvailable(doc);
        }

        /// <summary>
        /// 한 웹툰의 지정된 회차가 유효한지 검사합니다.
        /// </summary>
        /// <param name="id">웹툰 Id</param>
        /// <param name="no">웹툰 회차</param>
        /// <returns>유효성 여부</returns>
        public static bool IsAvailable(int id, int no)
        {
            WebtoonInfo info = new WebtoonInfo
            {
                Id = id,
                No = no
            };

            return IsAvailable(info.Url);
        }

        /// <summary>
        /// 지정된 웹툰이 유효한지 검사합니다.
        /// </summary>
        /// <param name="id">웹툰 Id</param>
        /// <returns>유효성 여부</returns>
        public static bool IsAvailable(int id)
        {
            return IsAvailable(id, 1);
        }



        /// <summary>
        /// HtmlDocument클래스로부터 웹툰 이름을 파싱합니다.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
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
        /// <param name="id">웹툰 Id</param>
        /// <returns>웹툰명</returns>
        public static string GetWebtoonName(int id)
        {
            HtmlDocument doc;
            HtmlWeb web = new HtmlWeb();

            WebtoonInfo tempInfo = new WebtoonInfo
            {
                Id = id,
                No = 1
            };

            doc = web.Load(tempInfo.Url);

            return GetWebtoonName(doc);
        }



        public static WebtoonInfoCollection GetEveryWebtoonInfos(HtmlDocument doc)
        {
            //웹툰 정보 리스트
            WebtoonInfoCollection result = new WebtoonInfoCollection();

            //웹툰 링크 선택
            HtmlNodeCollection nodeWebtoonLinks = doc.DocumentNode.SelectNodes("//*[@id='content']/div[4]/div/div/ul/li/a");

            for(int i = 0 ; i < nodeWebtoonLinks.Count ; i++)
            {
                HtmlNode nodeWebtoonLink = nodeWebtoonLinks[i];

                //각 웹툰 주소
                HtmlAttribute attWebtoonHref = nodeWebtoonLink.Attributes["href"];

                //이름 파싱
                string webtoonName = nodeWebtoonLink.InnerText;

                //주소에서 파싱을 위해 쿼리부분만 떼옴(맞는 표현인가?)
                string webtoonLinkQuery = attWebtoonHref.Value.Split('?')[1].Replace("amp;", "");

                //titleID, 요일 파싱
                int titleId = int.Parse(HttpUtility.ParseQueryString(webtoonLinkQuery)["titleId"]);
                string dayOfWeekString = HttpUtility.ParseQueryString(webtoonLinkQuery)["weekday"];

                //영어로 된 요일을 숫자로 변환(월요일=0 ~ 일요일=6)
                DayOfWeek curWebtoonDayOfWeek = new DayOfWeek();
                switch(dayOfWeekString)
                {
                    case "mon":
                        curWebtoonDayOfWeek = DayOfWeek.Monday;
                        break;
                    case "tue":
                        curWebtoonDayOfWeek = DayOfWeek.Tuesday;
                        break;
                    case "wed":
                        curWebtoonDayOfWeek = DayOfWeek.Wednesday;
                        break;
                    case "thu":
                        curWebtoonDayOfWeek = DayOfWeek.Thursday;
                        break;
                    case "fri":
                        curWebtoonDayOfWeek = DayOfWeek.Friday;
                        break;
                    case "sat":
                        curWebtoonDayOfWeek = DayOfWeek.Saturday;
                        break;
                    case "sun":
                        curWebtoonDayOfWeek = DayOfWeek.Sunday;
                        break;
                }

                //이미 리스트에 존재하는지 확인하는 플래그(여러 요일에 걸쳐서 연재하는 웹툰에 대비)
                bool alreadyExistFlag = false;

                //이미 리스트에 있을 경우에는기존 웹툰 정보에 요일만 추가
                foreach(WebtoonInfo info in result)
                {
                    if(info.Id == titleId)
                    {
                        alreadyExistFlag = true;
                        info.Weekday[curWebtoonDayOfWeek] = true;
                        break;
                    }
                }

                //웹툰 정보 구조체
                WebtoonInfo curWebtoonInfo = new WebtoonInfo
                {
                    Id = titleId,
                    WebtoonName = webtoonName
                };
                curWebtoonInfo.Weekday[curWebtoonDayOfWeek] = true;

                //이미 존재하지 않을 경우에는 리스트에 추가
                if(!alreadyExistFlag)
                {
                    result.Add(curWebtoonInfo);
                }
            }

            return result;
        }

        /// <summary>
        /// 네이버 웹툰 메인 페이지에서 모든 웹툰의 정보를 파싱합니다.
        /// </summary>
        /// <returns></returns>
        public static WebtoonInfoCollection GetEveryWebtoonInfos()
        {
            const string url = "https://comic.naver.com/webtoon/weekday.nhn";
            HtmlDocument doc = url.LoadHtmlDocument();

            return GetEveryWebtoonInfos(doc);
        }



        [STAThread]
        public static WebtoonInfoCollection GetFavoriteWebtoonInfosFromAccount(string id, string password)
        {
            //반환할 결괏값
            WebtoonInfoCollection result = new WebtoonInfoCollection();

            //Selenium 열기
            using(IWebDriver driver = new ChromeDriver())
            {
                //네이버 로그인 주소 접속
                const string naverLoginUrl = "https://nid.naver.com/nidlogin.login?url=https%3A%2F%2Fcomic.naver.com%2Fwebtoon%2Fweekday.nhn";
                driver.Navigate().GoToUrl(naverLoginUrl);

                //Id와password를 붙여넣는 액션
                Actions pasteId = new Actions(driver);
                Actions pastePassword = new Actions(driver);

                //Id입력창 찾기
                IWebElement idInput = driver.FindElement(By.XPath("//*[@id='id']"));

                //클립보드에 Id복사
                Clipboard.SetText(id);

                //Id입력창에 붙여넣기
                pasteId.MoveToElement(idInput).KeyDown(Keys.Control).SendKeys("v").KeyUp(Keys.Control).Perform();

                //Password입력창 찾기
                IWebElement passwordInput = driver.FindElement(By.XPath("//*[@id='pw']"));

                //클립보드에 Password복사
                Clipboard.SetText(password);

                //Password입력창에 붙여넣기
                pastePassword.MoveToElement(passwordInput).Click().KeyDown(Keys.Control).SendKeys("v").KeyUp(Keys.Control).Perform();

                //클립보드 비우기
                Clipboard.Clear();

                //로그인버튼 클릭
                IWebElement loginBtn = driver.FindElement(By.XPath("//*[@id='log.login']"));
                loginBtn.Click();

                //웹툰 메인페이지로 이동
                const string WebtoonMainPageUrl = "https://comic.naver.com/webtoon/weekday.nhn";
                driver.Navigate().GoToUrl(WebtoonMainPageUrl);

                //HtmlDocument로 파싱
                HtmlDocument mainPageDoc = new HtmlDocument();
                mainPageDoc.LoadHtml(driver.PageSource);

                //웹툰 목록 선택
                HtmlNodeCollection webtoonLinkNodes = mainPageDoc.DocumentNode.SelectNodes("//*[@id='content']/div/div/div/ul/li/div/a");

                //더 빠른 작업을 위해 WebtoonInfoCollection에 추가하는 작업은 비동기적으로 진행
                Task[] webtoonAddTask = new Task[webtoonLinkNodes.Count];

                //각 웹툰마다 실행
                for(int i = 0 ; i < webtoonLinkNodes.Count ; i++)
                {
                    //웹툰 링크노드
                    HtmlNode webtoonLinkNode = webtoonLinkNodes[i];

                    //웹툰 링크 파싱
                    HtmlAttribute webtoonHrefAtt = webtoonLinkNode.Attributes["href"];
                    string webtoonLink = "https://comic.naver.com" + webtoonHrefAtt.Value;
                    webtoonLink = webtoonLink.Replace("amp;", "");

                    //각 웹툰으로 이동(Selenium)
                    driver.Navigate().GoToUrl(webtoonLink);

                    //페이지 파싱
                    HtmlDocument webtoonPageDoc = new HtmlDocument();
                    webtoonPageDoc.LoadHtml(driver.PageSource);

                    //Favorite웹툰에 추가되있지 않으면 다음 웹툰으로
                    HtmlNode favoriteWebtoonNode = webtoonPageDoc.DocumentNode.SelectSingleNode("//*[@id='content']/div/div/ul/li/a[@class='book_maker on']");
                    if(favoriteWebtoonNode == null)
                        continue;

                    //추가되있으므로 Id파싱
                    string webtoonLinkQuery = webtoonLink.Split('?')[1];
                    int webtoonId = int.Parse(HttpUtility.ParseQueryString(webtoonLinkQuery)["titleId"]);

                    //Id추가
                    WebtoonInfo newInfo = new WebtoonInfo()
                    {
                        Id = webtoonId
                    };

                    //나머지 정보 추가 후 목록에 추가(비동기적으로 실행)
                    webtoonAddTask[i] = Task.Run(new Action(() =>
                    {
                        newInfo.LoadWebtoonInfo(driver.PageSource);
                        result.Add(newInfo);
                    }));
                }

                //목록에 추가하는 작업 멈출때까지 기다리기
                Task.WaitAll(webtoonAddTask);
            }

            //결과 반환
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