using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace LibWebtoonDownloader.WebtoonTask
{
    public abstract class AbstractWebtoonTask
    {
        private const int DEFAULT_THREAD_COUNT = 3;

        private const string USER_AGENT =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36";

        private Action<int>? _onEachTaskFinished;
        private DirectoryInfo? _targetDirectory;
        private int? _threadCount;

        protected abstract ConcurrentQueue<Uri> ImageQueue { get; }

        private DirectoryInfo TargetDirectory
            => _targetDirectory ?? new DirectoryInfo($"download/{this}");

        private Action<int> OnEachTaskFinished
            => _onEachTaskFinished ?? (_ => { });

        private int ThreadCount
            => _threadCount ?? DEFAULT_THREAD_COUNT;

        public abstract override string ToString();

        public Task Run()
        {
            return Task.Run(() =>
            {
                if (!TargetDirectory.Exists)
                    TargetDirectory.Create();

                SemaphoreSlim semaphore = new SemaphoreSlim(ThreadCount);

                int count = 0;
                var tasks = new List<Task>();

                while (!ImageQueue.IsEmpty)
                {
                    semaphore.Wait();
                    if (!ImageQueue.TryDequeue(out var address))
                        continue;

                    string extension = address.ToString().Split(".")[^1];
                    string fileName = Path.Combine(TargetDirectory.FullName, $"{++count:D4}.{extension}");

                    WebClient client = new WebClient();
                    client.Headers.Add(HttpRequestHeader.UserAgent, USER_AGENT);

                    int localCount = count;

                    var task = client.DownloadFileTaskAsync(address, fileName).ContinueWith(_ =>
                    {
                        OnEachTaskFinished(localCount);
                        semaphore.Release();
                    });
                    tasks.Add(task);
                }

                Task.WaitAll(tasks.ToArray());
            });
        }

        public AbstractWebtoonTask SetTargetDirectory(DirectoryInfo directoryInfo)
        {
            _targetDirectory = directoryInfo;
            return this;
        }

        public AbstractWebtoonTask SetOnEachTaskFinished(Action<int> action)
        {
            _onEachTaskFinished = action;
            return this;
        }

        public AbstractWebtoonTask SetThreadCount(int n)
        {
            _threadCount = n;
            return this;
        }
    }
}