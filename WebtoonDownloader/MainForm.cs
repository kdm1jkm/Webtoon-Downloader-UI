using LibWebtoonDownloader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace WebtoonDownloader
{
    public partial class MainForm : Form
    {
        public Webtoon webtoonDownload = new Webtoon();
        private bool isExcuting = false;
        private Task downloadTask;
        private ManualResetEvent mreExecute = new ManualResetEvent(false);

        public MainForm()
        {
            InitializeComponent();
            downloadTask = new Task(DoTask);
            downloadTask.Start();

            if(File.Exists("curTask.dat"))
            {
                webtoonDownload.LoadTask("curTask.dat");
            }

            Task loadQueue = new Task(displayQueue);
            loadQueue.Start();
        }

        public void displayQueue()
        {
            Webtoon.WebtoonTask[] taskArr = webtoonDownload.Tasks.ToArray();

            Dictionary<int, string> webtoonNameIdPairs = new Dictionary<int, string>();

            for(int i = 0 ; i < webtoonDownload.Tasks.Count ; i++)
            {
                string webtoonName;
                if(webtoonNameIdPairs.Keys.Contains(taskArr[i].TitleId))
                {
                    webtoonName = webtoonNameIdPairs[taskArr[i].TitleId];
                }
                else
                {
                    webtoonName = Webtoon.GetNameById(taskArr[i].TitleId);
                    webtoonNameIdPairs.Add(taskArr[i].TitleId, webtoonName);
                }
                this.Invoke(new Action(() =>
                {
                    lBox_queue.Items.Add(String.Format("{0}({1})", webtoonName, taskArr[i].No));
                }));
            }
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            int titleId = Webtoon.GetIdByName(tBox_WebtoonName.Text);

            if(titleId == -1)
            {
                MessageBox.Show("웹툰을 찾을 수 없습니다.");
                return;
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
            if(tBox_titleId.Text == "")
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
        }

        //titleId textbox에 숫자만 입력되도록
        private void tBox_titleId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void checkBox_SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox_SelectAll.Checked)
            {
                num_StartNo.Enabled = num_endNo.Enabled = false;
            }
            else
            {
                num_StartNo.Enabled = num_endNo.Enabled = true;
            }
            num_StartNo.Value = 1;
            num_endNo.Value = num_endNo.Maximum;
        }

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
            string webtoonName = Webtoon.GetNameById(titleId);

            for(int i = startNo ; i <= endNo ; i++)
            {
                webtoonDownload.AddTask(titleId, i, checkBox_HTML.Checked, checkBox_zip.Checked);

                string webtoonInfoString = string.Format("{0}({1})", webtoonName, i);
                lBox_queue.Items.Add(webtoonInfoString);
            }

            webtoonDownload.SaveTask("curTask.dat");
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

                if(webtoonDownload.GetTasks.Count == 0)
                {
                    isExcuting = false;

                    this.Invoke(new Action(() =>
                    {
                        btn_TogglePause.Text = "Start";
                    }));

                    mreExecute.Reset();

                    continue;
                }

                webtoonDownload.DoTask();

                this.Invoke(new Action(() =>
                    {
                        lBox_DownloadedWebtoons.Items.Add(lBox_queue.Items[0]);
                    }));

                this.Invoke(new Action(() =>
                    {
                        lBox_queue.Items.RemoveAt(0);
                    }));

                int EveryWebtoonCount = lBox_DownloadedWebtoons.Items.Count + lBox_queue.Items.Count;
                double Ratio = (double)lBox_queue.Items.Count / EveryWebtoonCount * 100000;

                this.Invoke(new Action(() =>
                {
                    prgsBar_Webtoon.Value = 100000 - (int)Ratio;
                }));

                webtoonDownload.SaveTask("curTask.dat");
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
                webtoonDownload.Tasks.Clear();
                webtoonDownload.SaveTask("curTask.dat");
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
        }
    }
}
