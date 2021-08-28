using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibWebtoonDownloader.Webtoon;
using LibWebtoonDownloader.WebtoonTask;
using ShellProgressBar;

namespace ConsoleWebtoonDownloader
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var naverWebtoon = NaverWebtoon.Load("갓 오브 하이스쿨");
            var tasks = naverWebtoon?.GetEveryTask();
            if (tasks == null)
                return;

            var taskQueue = new ConcurrentQueue<AbstractWebtoonTask>(tasks.Take(20));

            int threadCount = 5;
            var semaphore = new SemaphoreSlim(threadCount);

            var running = new List<Task>();

            var totalProgress = new ProgressBar(taskQueue.Count, naverWebtoon.WebtoonName,
                new ProgressBarOptions { ProgressCharacter = '@', CollapseWhenFinished = true });

            while (!taskQueue.IsEmpty)
            {
                if (!taskQueue.TryDequeue(out var webtoonTask)) continue;

                semaphore.Wait();

                var pb = totalProgress.Spawn(webtoonTask.ImageCount, webtoonTask.ToString(),
                    new ProgressBarOptions { ProgressCharacter = '#', CollapseWhenFinished = true });

                var t = webtoonTask.SetThreadCount(5)
                    .SetOnEachTaskFinished((max, count, num) => pb.Tick($"{webtoonTask}-{count}/{max}({num})"))
                    .Run();
                running.Add(t);
                t.ContinueWith(task =>
                {
                    running.Remove(task);
                    pb.Dispose();
                    totalProgress.Tick();
                    semaphore.Release();
                });
            }

            Task.WaitAll(running.ToArray());
            totalProgress.Dispose();
        }
    }
}