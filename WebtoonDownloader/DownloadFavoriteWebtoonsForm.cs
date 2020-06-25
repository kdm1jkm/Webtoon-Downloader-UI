using LibWebtoonDownloader;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace WebtoonDownloader
{
    public partial class DownloadFavoriteWebtoonsForm : Form
    {
        MainForm motherForm;

        public DownloadFavoriteWebtoonsForm()
        {
            InitializeComponent();
        }

        public DownloadFavoriteWebtoonsForm(MainForm form)
        {
            InitializeComponent();
            motherForm = form;

            DateTime from;
            DateTime to = DateTime.Now;
            to = to.AddHours(-to.Hour);
            to = to.AddMinutes(-to.Minute);
            to = to.AddSeconds(-to.Second);
            to = to.AddMilliseconds(-to.Millisecond);

            if (File.Exists("lastDownloaded.dat"))
            {
                using (Stream ls = new FileStream("lastDownloaded.dat", FileMode.Open))
                {
                    BinaryFormatter deserializer = new BinaryFormatter();
                    from = (DateTime) deserializer.Deserialize(ls);
                }
            }
            else
            {
                from = DateTime.Now;
                from = from.AddHours(-from.Hour);
                from = from.AddMinutes(-from.Minute);
                from = from.AddSeconds(-from.Second);
                from = from.AddMilliseconds(-from.Millisecond);
            }

            if (from > to)
            {
                from = to;
                from = from.AddHours(-from.Hour);
                from = from.AddMinutes(-from.Minute);
                from = from.AddSeconds(-from.Second);
                from = from.AddMilliseconds(-from.Millisecond);
            }

            tmpk_from.Value = from;
            tmpk_to.Value = to;
        }

        private void tmpk_from_ValueChanged(object sender, EventArgs e)
        {
            Console.WriteLine(tmpk_from.Value);
        }

        private void btn_download_Click(object sender, EventArgs e)
        {
            WebtoonInfoCollection infos = WebtoonInfoCollection.Load("favoriteWebtoonInfoCollection.dat");

            Func<DayOfWeek, DayOfWeek, DayOfWeek, bool> isBetween = new Func<DayOfWeek, DayOfWeek, DayOfWeek, bool>((
                DayOfWeek start,
                DayOfWeek end,
                DayOfWeek item) =>
            {
                int startN = (int) start;
                int endN = (int) end;
                int itemN = (int) item;
                if (startN > endN)
                {
                    endN += 7;
                }

                if (itemN < startN)
                {
                    itemN += 7;
                }

                return itemN <= endN;
            });

            LoadingForm loading = new LoadingForm();
            loading.pBar.Maximum = infos.Count;
            loading.Show();

            for (int i = 0; i < infos.Count; i++)
            {
                bool isinPeriod = false;

                foreach (DayOfWeek day in infos[i].Weekdays)
                {
                    isinPeriod |= isBetween(tmpk_from.Value.DayOfWeek, tmpk_to.Value.DayOfWeek, day);
                }

                if (isinPeriod)
                {
                    WebtoonInfoCollection favoriteInPeriod =
                        Webtoon.WebtoonInfoInPeriod(tmpk_from.Value, tmpk_to.Value, infos[i]);

                    foreach (WebtoonInfo info in favoriteInPeriod)
                    {
                        motherForm.curTasks.Enqueue(info);
                    }
                }

                loading.pBar.PerformStep();
            }

            motherForm.LoadQueue();

            using (Stream ws = new FileStream("lastDownloaded.dat", FileMode.Create))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(ws, tmpk_from.Value.AddDays(1));
            }

            loading.Close();

            this.Close();
        }
    }
}