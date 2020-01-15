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
    public class Webtoon
    {
        public Webtoon()
        {
            Tasks = new Queue<WebtoonTask>();
        }

        public Semaphore downloadImagePool;

        public Queue<WebtoonTask> Tasks;

        public int DownloadImageSemaphoreCount = 5;



        /// <summary>
        /// 웹툰 Id와 웹툰 회차 정보를 담고 있는 구조체입니다.
        /// </summary>
        [Serializable]
        public struct WebtoonTask
        {
            public int TitleId { get; set; }
            public int No { get; set; }

            public WebtoonFormat Format { get; set; }

            public string Url
            {
                get
                {
                    return string.Format("https://comic.naver.com/webtoon/detail.nhn?titleId={0}&no={1}", TitleId, No);
                }
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
                return Url.GetHashCode() * 19 + Format.GetHashCode();
            }

            public static bool operator ==(WebtoonTask left, object right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(WebtoonTask left, object right)
            {
                return !left.Equals(right);
            }
        }



        /// <summary>
        /// 이미지를 다운로드합니다.
        /// </summary>
        /// <param name="url">이미지 Url</param>
        /// <param name="dir">저장할 위치</param>
        private void DownloadImage(string url, string dir)
        {
            //다운로드 클라이언트 클래스를 생성 후 헤더를 입력
            WebClient downloader = new WebClient();
            downloader.Headers.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_5) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/50.0.2661.102 Safari/537.36");

            //멀티스레딩
            downloadImagePool.WaitOne();

            //다운로드와 로그 남기기
            downloader.DownloadFile(url, dir);
            Console.WriteLine("Download complete");
            Console.WriteLine("Url: " + url);
            Console.WriteLine("dir: " + dir);
            Console.WriteLine();

            //멀티스레딩
            downloadImagePool.Release();
        }



        /// <summary>
        /// Li(Last in)마지막으로 추가된 task를 실행합니다.
        /// </summary>
        public void DoTask()
        {
            //Tasks가 비었으면 바로 종료
            if(Tasks.Count == 0)
            {
                return;
            }

            //task한개 가져오기
            WebtoonTask curTask = Tasks.First();

            //url접속
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();
            doc = web.Load(curTask.Url);
            //doc.Load(@"D:\dropbox\Downloads\test for crawling\WebtoonBody.html", Encoding.UTF8);

            if(!IsAvailable(doc))
            {
                Tasks.Dequeue();
                return;
            }

            HtmlNodeCollection nodeImgCollection = doc.DocumentNode.SelectNodes("//img[@alt='comic content']");
            if(nodeImgCollection == null)
            {
                Tasks.Dequeue();
                return;
            }

            //웹툰명 가져오기
            string webtoonName = GetWebtoonName(doc);

            //폴더 생성
            Directory.CreateDirectory($@"src\{webtoonName}_{curTask.No.ToString("D3")}");

            //이미지 다운로드 멀티쓰레드 모음 리스트(마지막에 기다리기 위해서)
            Thread[] imageDownloadThreads = new Thread[nodeImgCollection.Count];
            //세마포어 생성
            downloadImagePool = new Semaphore(DownloadImageSemaphoreCount, DownloadImageSemaphoreCount);

            //이미지 태그 선택
            for(int i = 0 ; i < nodeImgCollection.Count ; i++)
            {
                HtmlNode nodeImg = nodeImgCollection[i];

                //이미지 주소 파싱
                HtmlAttribute attSrc = nodeImg.Attributes["src"];
                string src = attSrc.Value;
                //string src = @"D:\dropbox\Downloads\test for crawling" + attSrc.Value.Substring(1, attSrc.Value.Length - 1).Replace("/", "\\");

                //저장경로
                string imgSrc = string.Format(@"src\{0}_{1:000}\{2}.{3}", webtoonName, curTask.No, i + 1, src.Split('.').Last());

                //다운로드
                Thread imageDownloadThread = new Thread(new ThreadStart(() => { DownloadImage(src, imgSrc); }));
                imageDownloadThreads[i] = imageDownloadThread;
                imageDownloadThread.Start();

                //다운로드
                //DownloadImage(src, imgSrc);
            }

            //메타데이터 생성
            MetaData curMetaData = new MetaData(webtoonName, curTask.TitleId, curTask.No, nodeImgCollection.Count);
            string dir = string.Format(@"src\{0}_{1:000}\metaData.dat", webtoonName, curTask.No);
            curMetaData.Save(dir);

            //이미지 다운로드 종료
            foreach(Thread thread in imageDownloadThreads)
            {
                thread.Join();
            }

            //html, zip플래그 확인 후 각각 생성
            if(curTask.Format.Html)
            {
                Directory.CreateDirectory("html");
                string htmlDir = string.Format(@"html\{0}_{1:000}\html.html", webtoonName, curTask.No);
                List<string> imgs = new List<string>();
                for(int i = 1 ; i <= curMetaData.ImgCnt ; i++)
                {
                    string sourceFileName = string.Format(@"src\{0}_{1:000}\{2}.jpg", webtoonName, curTask.No, i);
                    string destFileName = string.Format(@"html\{0}_{1:000}\{2}.jpg", webtoonName, curTask.No, i);
                    File.Copy(sourceFileName, destFileName);
                    string imgSrc = string.Format(@"{0}.jpg", i);
                    imgs.Add(imgSrc);
                }

                MakeHtml(htmlDir, imgs);
            }
            if(curTask.Format.Zip)
            {
                Directory.CreateDirectory("zip");
                string srcDir = string.Format(@"src\{0}_{1:000}", webtoonName, curTask.No);
                string zipDir = string.Format(@"zip\{0}_{1:000}.zip", webtoonName, curTask.No);

                if(File.Exists(zipDir))
                {
                    File.Delete(zipDir);
                }

                ZipFile.CreateFromDirectory(srcDir, zipDir);
            }

            //Tasks에서 하나 완료
            Tasks.Dequeue();
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
        /// 웹툰을 PC에서 보기 위해 html파일을 만듭니다.
        /// </summary>
        /// <param name="metaDataDir">metaData파일 경로</param>
        public static void MakeHtml(string metaDataDir)
        {
            MetaData curMetaData = MetaData.Load(metaDataDir);
            MakeHtml(curMetaData);
        }

        /// <summary>
        /// 웹툰을 PC에서 보기 위해 html파일을 만듭니다
        /// </summary>
        /// <param name="curMetaData"></param>
        public static void MakeHtml(MetaData curMetaData)
        {
            string fileName = string.Format(@"html\{0}_{1:000}.html", curMetaData.WebtoonName, curMetaData.No);

            List<string> imgs = new List<string>();

            for(int i = 1 ; i <= curMetaData.ImgCnt ; i++)
            {
                string temp = string.Format(@"..\src\{0}_{1:000}\{2}.jpg", curMetaData.WebtoonName, curMetaData.No, i);
                imgs.Add(temp);
            }

            MakeHtml(fileName, imgs);
        }



        #region save and load tasks
        /// <summary>
        /// tasks를 파일로 저장합니다.
        /// </summary>
        public void SaveTask()
        {
            SaveTask("tasks.dat");
        }


        /// <summary>
        /// tasks를 파일로 저장합니다.
        /// </summary>
        /// <param name="fileName">파일명</param>
        public void SaveTask(string fileName)
        {
            Stream ws = new FileStream(fileName, FileMode.Create);
            BinaryFormatter serializer = new BinaryFormatter();

            serializer.Serialize(ws, Tasks);

            ws.Close();
        }


        /// <summary>
        /// tasks를 파일로부터 불러옵니다.
        /// </summary>
        public void LoadTask()
        {
            LoadTask("tasks.dat");
        }


        /// <summary>
        /// tasks를 파일로부터 불러옵니다.
        /// </summary>
        /// <param name="fileName">파일명</param>
        public void LoadTask(string fileName)
        {
            if(!File.Exists(fileName))
                throw new FileNotFoundException("File Not Found", fileName);

            Stream rs = new FileStream(fileName, FileMode.Open);
            BinaryFormatter deserializer = new BinaryFormatter();

            Tasks = (Queue<WebtoonTask>)deserializer.Deserialize(rs);

            rs.Close();
        }
        #endregion



        /// <summary>
        /// 한 웹툰의 최신 회차가 몇인지 찾아냅니다. 실패 시 -1을 반환합니다.
        /// </summary>
        /// <param name="titleId">웹툰 Id</param>
        /// <returns></returns>
        public static int GetLatestEpisode(int titleId)
        {
            //유효하지 않은 titleId일경우
            if(!IsAvailable(titleId))
            { return -1; }

            //url접속
            string url = string.Format("https://comic.naver.com/webtoon/list.nhn?titleId={0}", titleId);
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc;
            doc = web.Load(url);
            //doc = new HtmlDocument();
            //doc.Load(@"D:\dropbox\Downloads\test for crawling\연애혁명 __ 네이버 만화.htm");

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
        /// 웹툰의 이름으로 Id를 찾아냅니다. 실패 시 -1을 반환합니다.
        /// </summary>
        /// <param name="name">웹툰명</param>
        /// <returns>
        /// 성공: 웹툰명
        /// 실패: -1
        /// </returns>
        public static int GetIdByName(string name)
        {
            //웹툰 이름으로 접속
            string url = string.Format(@"https://comic.naver.com/search.nhn?m=webtoon&keyword={0}", name);
            int titleId;
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc;
            doc = web.Load(url);
            //doc = new HtmlDocument();
            //doc.Load(@"D:\dropbox\Downloads\test for crawling\윰세포_검색_존재.html");
            //doc.Load(@"D:\dropbox\Downloads\test for crawling\검색_결과없음.html");

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



        #region isAvailable
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

        public static bool IsAvailable(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return IsAvailable(doc);
        }

        /// <summary>
        /// task가 실행 가능한지 검사합니다.
        /// </summary>
        /// <param name="task">검사할 task</param>
        /// <returns></returns>
        public static bool IsAvailable(WebtoonTask task)
        {
            HtmlWeb web = new HtmlWeb();
            return IsAvailable(web.Load(task.Url));
        }

        /// <summary>
        /// 한 웹툰의 지정된 회차가 실행 가능한지 검사합니다.
        /// </summary>
        /// <param name="titleId">웹툰 Id</param>
        /// <param name="no">웹툰 회차</param>
        /// <returns></returns>
        public static bool IsAvailable(int titleId, int no)
        {
            WebtoonTask task = new WebtoonTask
            {
                TitleId = titleId,
                No = no
            };

            return IsAvailable(task);
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

        
        #endregion



        #region AddTask
        /// <summary>
        /// task를 tasks에 추가합니다.
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(WebtoonTask task)
        {
            Tasks.Enqueue(task);
        }

        /// <summary>
        /// 한 웹툰의 전체 범위를 tasks에 추가합니다.
        /// </summary>
        /// <param name="titleId">웹툰 Id</param>
        public void AddTask(int titleId)
        {
            AddTask(titleId, false, false);
        }

        /// <summary>
        /// 한 웹툰의 전체 범위를 tasks에 추가합니다.
        /// </summary>
        /// <param name="titleId">웹툰 Id</param>
        /// <param name="html">html파일 생성 여부</param>
        /// <param name="zip">zip파일 생성 여부</param>
        public void AddTask(int titleId, bool html, bool zip)
        {
            int lastEp = GetLatestEpisode(titleId);

            AddTask(titleId, 1, lastEp, html, zip);
        }

        /// <summary>
        /// 한 웹툰의 한 화를 tasks에 추가합니다.
        /// </summary>
        /// <param name="titleId">웹툰 Id</param>
        /// <param name="no">웹툰 회차</param>
        public void AddTask(int titleId, int no)
        {
            AddTask(titleId, no, false, false);
        }

        /// <summary>
        /// 한 웹툰의 한 화를 tasks에 추가합니다.
        /// </summary>
        /// <param name="titleId">웹툰 Id</param>
        /// <param name="no">웹툰 회차</param>
        /// <param name="html">html파일 생성 여부</param>
        /// <param name="zip">zip파일 생성 여부</param>
        public void AddTask(int titleId, int no, bool html, bool zip)
        {
            WebtoonTask task = new WebtoonTask
            {
                TitleId = titleId,
                No = no,
                Format = new WebtoonFormat
                {
                    Html = html,
                    Zip = zip
                }
            };

            AddTask(task);
        }

        /// <summary>
        /// 한 웹툰의 지정된 범위를 tasks에 추가합니다.
        /// </summary>
        /// <param name="titleId">웹툰 Id</param>
        /// <param name="startNo">시작 범위</param>
        /// <param name="endNo">끝 범위</param>
        public void AddTask(int titleId, int startNo, int endNo)
        {
            AddTask(titleId, startNo, endNo, false, false);
        }

        /// <summary>
        /// 한 웹툰의 지정된 범위를 tasks에 추가합니다.
        /// </summary>
        /// <param name="titleId">웹툰 Id</param>
        /// <param name="startNo">시작 회차</param>
        /// <param name="endNo">끝 회차</param>
        /// <param name="html">html파일 생성 여부</param>
        /// <param name="zip">zip파일 생성 여부</param>
        public void AddTask(int titleId, int startNo, int endNo, bool html, bool zip)
        {
            if(endNo < startNo)
                return;

            for(int i = startNo ; i <= endNo ; i++)
            {
                AddTask(titleId, i, html, zip);
            }

        }
        #endregion




        public static string GetWebtoonName(HtmlDocument doc)
        {
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
            if(!IsAvailable(titleId))
                return "";

            HtmlDocument doc;
            HtmlWeb web = new HtmlWeb();

            WebtoonTask tempTask = new WebtoonTask
            {
                TitleId = titleId,
                No = 1
            };

            doc = web.Load(tempTask.Url);
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
                    Name = name,
                    Weekdays = new List<DayOfWeek>()
                };
                tempInfo.Weekdays.Add(weekday);

                //이미 리스트에 존재하는지 확인하는 플래그(여러 요일에 걸쳐서 연재하는 웹툰에 대비)
                bool alreadyExistFlag = false;

                //이미 리스트에 있을 경우에는기존 웹툰 정보에 요일만 추가
                foreach(WebtoonInfo info in webtoonInfos)
                {
                    if(info.Id == titleId)
                    {
                        alreadyExistFlag = true;
                        info.Weekdays.Add(weekday);
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

                    while(!(everyInfos[i].Id == titleId))
                    {
                        i++;
                    }
                    result.Add(everyInfos[i]);
                }
            }
            return result;
        }



        #region AddFavoriteTasks
        /// <summary>
        /// webtooninfos리스트에 있는 웹툰들의 지정된 날짜 사이의 회차들을 전부 task에 등록합니다.
        /// </summary>
        /// <param name="startDate">시작 날짜</param>
        /// <param name="endDate">끝 날짜</param>
        /// <param name="webtoonInfos">웹툰 정보 리스트</param>
        /// <param name="html">html생성 여부</param>
        /// <param name="zip">zip생성 여부</param>
        public void AddFavoriteTasks(DateTime startDate, DateTime endDate, WebtoonInfoCollection webtoonInfos, bool html, bool zip)
        {

            foreach(WebtoonInfo info in webtoonInfos)
            {
                //2중 루프 브레이크용 플래그
                bool keepSearchingFlag = true;

                //if((endDate - startDate).TotalDays < 7)
                //{
                //    int startDay = (int)startDate.DayOfWeek + 6 % 7;
                //    int endDay = ((int)endDate.DayOfWeek + 6 % 7 < startDay) ? (int)endDate.DayOfWeek + 6 % 7 + 7 : (int)endDate.DayOfWeek + 6 % 7;
                //}

                for(int page = 1 ; keepSearchingFlag ; page++)
                {
                    //웹툰 페이지 접속
                    HtmlDocument doc = new HtmlDocument();
                    HtmlWeb web = new HtmlWeb();
                    string url = string.Format("https://comic.naver.com/webtoon/list.nhn?titleId={0}&page={1}", info.Id, page);
                    doc = web.Load(url);

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
                            keepSearchingFlag = false;
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
                                //1화라면 범위 안으로 들어올 수 없기 때문에 루프 탈출
                                keepSearchingFlag = false;
                                break;
                            }
                        }

                        //조건에 맞으므로 task에 추가
                        WebtoonTask newTask = new WebtoonTask
                        {
                            TitleId = int.Parse(HttpUtility.ParseQueryString(query)["titleId"]),
                            No = no,
                            Format = new WebtoonFormat
                            {
                                Html = html,
                                Zip = zip
                            }
                        };
                        AddTask(newTask);

                        //처음 웹툰일경우 무한루프 방지를 위해 브레이크
                        if(no == 1)
                        {
                            keepSearchingFlag = false;
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// webtooninfos리스트에 있는 웹툰들의 지정된 날짜 사이의 회차들을 전부 task에 등록합니다.
        /// </summary>
        /// <param name="startDate">시작 날짜</param>
        /// <param name="endDate">끝 날짜</param>
        /// <param name="webtoonInfos">웹툰 정보 리스트</param>
        public void AddFavoriteTasks(DateTime startDate, DateTime endDate, WebtoonInfoCollection webtoonInfos)
        {

            AddFavoriteTasks(startDate, endDate, webtoonInfos, true, false);
        }
        /// <summary>
        /// webtooninfos리스트에 있는 웹툰들의 지정된 날짜 사이의 회차들을 전부 task에 등록합니다.
        /// </summary>
        /// <param name="startDate">시작 날짜</param>
        /// <param name="endDate">끝 날짜</param>
        /// <param name="webtoonInfos">웹툰 정보 리스트</param>
        public void AddFavoriteTasks(string startDate, string endDate, WebtoonInfoCollection webtoonInfos)
        {

            if(!(DateTime.TryParse(startDate, out DateTime startD) && DateTime.TryParse(endDate, out DateTime endD)))
            {
                return;
            }

            AddFavoriteTasks(startD, endD, webtoonInfos);


        }
        /// <summary>
        /// webtooninfos리스트에 있는 웹툰들의 지정된 날짜 사이의 회차들을 전부 task에 등록합니다.
        /// </summary>
        /// <param name="webtooninfos">웹툰 정보 리스트</param>
        public void AddFavoriteTasks(WebtoonInfoCollection webtooninfos)
        {
            AddFavoriteTasks(DateTime.MinValue, DateTime.Now, webtooninfos);
        }
        #endregion
    }
}