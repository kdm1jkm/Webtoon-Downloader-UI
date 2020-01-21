using System;
using System.Threading.Tasks;

namespace TestWebtoonDownloader
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            int i = 0;
            int j = 0;
            Task[] temp = new Task[2]
            {
                Task.Run(new Action(()=>{ i = 1; })),
                Task.Run(new Action(()=>{ j = 1; }))
            };

            Task.WaitAll(temp);

            return;
        }
    }
}