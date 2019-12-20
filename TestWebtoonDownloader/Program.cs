using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibWebtoonDownloader;

namespace TestWebtoonDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            Webtoon newWebtoon = new Webtoon();

            //Task newTask = new Task()
            //{
            //    TitleId = Webtoon.GetIdByName("치즈인더트랩"),
            //    No = Webtoon.GetLatestEpisode(Webtoon.GetIdByName("치즈인더트랩")),
            //    Format = new WebtoonFormat()
            //    {
            //        Html = true,
            //        Zip = true
            //    }
            //};

            //newWebtoon.AddTask(newTask);

            //newWebtoon.DoTask();

            //MetaData newMetaData = MetaData.Load(@"src\연애혁명_292\metaData.dat");

            //Webtoon.MakeHtml(newMetaData);

            newWebtoon.AddFavoriteTasks("2019.12.19", "2019.12.20", Webtoon.GetWebtoonInfos());

            while(newWebtoon.GetTasks.Count != 0)
            {
                newWebtoon.DoTask();
            }
        }
    }
}