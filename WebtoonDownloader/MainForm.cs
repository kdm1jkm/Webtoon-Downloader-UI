using LibWebtoonDownloader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebtoonDownloader
{
    public partial class MainForm : Form
    {
        public WebtoonInfoQueue curTasks;
        private bool isExcuting = false;
        private readonly ManualResetEvent mreExecute = new ManualResetEvent(false);
        private Thread downloadThread;
        private Thread loadQueueThread;
        private bool stopDownloading = false;

        public MainForm()
        {
            InitializeComponent();
            downloadThread = new Thread(new ThreadStart(DoTask));
            downloadThread.Start();

            loadQueueThread = new Thread(new ThreadStart(LoadQueue));
            loadQueueThread.Start();
        }

        public void LoadQueue()
        {
            if(!File.Exists("curQueue.dat"))
            {
                curTasks = new WebtoonInfoQueue();
            }
            else
            {
                curTasks = WebtoonInfoQueue.Load("curQueue.dat");
            }

            WebtoonInfoCollection queueList = curTasks;

            for(int i = 0 ; i < queueList.Count ; i++)
            {
                this.Invoke(new Action(() =>
                {
                    lBox_queue.Items.Add(string.Format("{0}({1})", queueList[i].WebtoonName, queueList[i].No));
                }));
            }
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            int titleId = Webtoon.GetWebtoonId(tBox_WebtoonName.Text);

            if(titleId == -1)
            {
                MessageBox.Show("웹툰을 찾을 수 없습니다.");
                return;
            }
            else
            {
                MessageBox.Show($"{Webtoon.GetWebtoonName(titleId)}({titleId})");
            }

            tBox_titleId.Text = titleId.ToString();
        }

        private void num_StartNo_ValueChanged(object sender, EventArgs e)
        {
            num_endNo.Minimum = num_StartNo.Value;
        }

        private void num_endNo_ValueChanged(object sender, EventArgs e)
        {
            num_StartNo.Maximum = num_endNo.Value;
        }

        private void tBox_titleId_TextChanged(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(tBox_titleId.Text))
            {
                num_endNo.Maximum = 1;
                return;
            }
            if(!Webtoon.IsAvailable(int.Parse(tBox_titleId.Text)))
            {
                num_endNo.Maximum = 1;
                return;
            }
            num_endNo.Maximum = Webtoon.GetLatestEpisode(int.Parse(tBox_titleId.Text));
            num_endNo.Value = num_endNo.Maximum;
            num_StartNo.Value = num_StartNo.Maximum;
        }

        //ToDo: 여기도 한번에 받아와서 추가하는걸로 하자 어차피 WebtoonInfo쪽에서 doc으로 내용추가하는거 넣을거니까 그거이용하기
        private void btn_AddTask_Click(object sender, EventArgs e)
        {
            if(tBox_titleId.Text.Length == 0)
            {
                MessageBox.Show("웹툰 id를 입력해 주세요.");
                return;
            }

            int titleId = int.Parse(tBox_titleId.Text);

            if(!Webtoon.IsAvailable(titleId))
            {
                MessageBox.Show("입력한 id는 유효하지 않습니다.");
                return;
            }

            int startNo = int.Parse(num_StartNo.Value.ToString());
            int endNo = int.Parse(num_endNo.Value.ToString());

            WebtoonInfo baseInfo = new WebtoonInfo()
            {
                Id = titleId
            };
            baseInfo.LoadWebtoonInfo();

            for(int i = startNo ; i <= endNo ; i++)
            {
                WebtoonInfo curInfo = baseInfo.Copy();
                curInfo.No = i;

                curTasks.Enqueue(curInfo);

                lBox_queue.Items.Add(curInfo);
            }

            curTasks.Save("curQueue.dat");
        }

        private void btn_TogglePause_Click(object sender, EventArgs e)
        {
            if(isExcuting)
            {
                isExcuting = false;
                mreExecute.Reset();
                btn_TogglePause.Text = "Start";
            }
            else
            {
                isExcuting = true;
                mreExecute.Set();
                btn_TogglePause.Text = "Pause";
            }
        }

        private void DoTask()
        {
            while(true)
            {
                mreExecute.WaitOne();

                if(stopDownloading)
                {
                    break;
                }

                if(curTasks.Count == 0)
                {
                    isExcuting = false;

                    this.Invoke(new Action(() => { btn_TogglePause.Text = "Start"; }));

                    mreExecute.Reset();

                    continue;
                }

                Semaphore downloadSemaphore = new Semaphore((int)num_Thread.Value, (int)num_Thread.Value);

                WebtoonInfo curInfo = curTasks.First();

                if(curInfo.ImageSrcs == null)
                {
                    curInfo.LoadDetailInfo();
                }

                string saveDir = $@"src\{curInfo.WebtoonName}_{curInfo.No.ToString("D3")}";

                if(!Directory.Exists(saveDir))
                {
                    Directory.CreateDirectory(saveDir);
                }

                Task[] downloadThreads = new Task[curInfo.ImageCount];

                for(int i = 0 ; i < curInfo.ImageCount ; i++)
                {
                    Task downloadThread = new Task(new Action(() =>
                    {
                        downloadSemaphore.WaitOne();
                        Webtoon.DownloadImage(curInfo.ImageSrcs[i], saveDir + $@"\{i}.jpg");
                        downloadSemaphore.Release();
                    }));

                    downloadThreads[i] = downloadThread;
                    downloadThread.Start();
                }

                Task.WaitAll(downloadThreads);

                this.Invoke(new Action(() => { lBox_DownloadedWebtoons.Items.Add(lBox_queue.Items[0]); }));

                this.Invoke(new Action(() => { lBox_queue.Items.RemoveAt(0); }));

                int EveryWebtoonCount = lBox_DownloadedWebtoons.Items.Count + lBox_queue.Items.Count;
                int Ratio = lBox_DownloadedWebtoons.Items.Count * prgsBar_Webtoon.Maximum / EveryWebtoonCount;

                this.Invoke(new Action(() => { prgsBar_Webtoon.Value = Ratio; }));

                curTasks.Save("curQueue.dat");
            }
        }

        private void tBox_titleId_ImeModeChanged(object sender, EventArgs e)
        {
            tBox_titleId.ImeMode = ImeMode.Alpha;
        }

        private void btn_ModifyFavorite_Click(object sender, EventArgs e)
        {
            EditFavoriteWebtoonsForm editFavoriteWebtoonsForm = new EditFavoriteWebtoonsForm();
            editFavoriteWebtoonsForm.ShowDialog();
        }

        private void btn_clrQueue_Click(object sender, EventArgs e)
        {
            if(isExcuting)
            {
                MessageBox.Show("다운로드를 멈추고 실행하세요");
                return;
            }
            else
            {
                lBox_queue.Items.Clear();
                curTasks.Clear();
                curTasks.Save("curQueue.dat");
            }
        }

        private void btn_clearDownloadedList_Click(object sender, EventArgs e)
        {
            lBox_DownloadedWebtoons.Items.Clear();
        }

        private void btn_AddFavorite_Click(object sender, EventArgs e)
        {
            DownloadFavoriteWebtoonsForm form = new DownloadFavoriteWebtoonsForm(this);
            form.ShowDialog();
            curTasks.Save("curQueue.dat");
        }

        private void btn_FileManagement_Click(object sender, EventArgs e)
        {
            FileManagementForm form = new FileManagementForm();
            form.ShowDialog();
        }

        private void InputOnlyNum(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btn_selectWholeRange_Click(object sender, EventArgs e)
        {
            num_StartNo.Value = 1;
            num_endNo.Value = num_endNo.Maximum;
        }
    }
}
