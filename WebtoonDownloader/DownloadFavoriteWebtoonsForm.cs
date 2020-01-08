using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LibWebtoonDownloader;

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

            if(File.Exists("lastDownloaded.dat"))
            {
                using(Stream ls = new FileStream("lastDownloaded.dat", FileMode.Open))
                {
                    BinaryFormatter deserializer = new BinaryFormatter();
                    from = (DateTime)deserializer.Deserialize(ls);
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

            if(from > to)
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
            //Todo Delegate로 테인 연결해서 좀더 갈끔하게 들어가기
            motherForm.webtoonDownload.AddFavoriteTasks(
                tmpk_from.Value,
                tmpk_to.Value.AddDays(1),
                WebtoonInfoCollection.Load("favoriteWebtoonInfoCollection.dat"),
                checkBox_HTML.Checked,
                checkBox_zip.Checked);

            Task reloadQueue = new Task(motherForm.displayQueue);
            reloadQueue.Start();

            using(Stream ws = new FileStream("lastDownloaded.dat", FileMode.Create))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(ws, tmpk_from.Value.AddDays(1));
            }

            this.Close();
        }
    }
}
