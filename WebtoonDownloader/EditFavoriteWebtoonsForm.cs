using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibWebtoonDownloader;
using System.IO;

namespace WebtoonDownloader
{
    public partial class EditFavoriteWebtoonsForm : Form
    {
        public EditFavoriteWebtoonsForm()
        {
            InitializeComponent();
            Task load = new Task(loadWebtoons);
            load.Start();
        }

        private WebtoonInfoCollection getCheckedList()
        {
            WebtoonInfoCollection favoriteWebtoonInfos = new WebtoonInfoCollection();

            for(int i = 0 ; i < cLstBox_WebtoonList.Items.Count ; i++)
            {
                if(cLstBox_WebtoonList.GetItemChecked(i))
                {
                    favoriteWebtoonInfos.Add((WebtoonInfo)cLstBox_WebtoonList.Items[i]);
                }
            }

            return favoriteWebtoonInfos;
        }

        private void btn_saveAndExit_Click(object sender, EventArgs e)
        {
            getCheckedList().Save("favoriteWebtoonInfoCollection.dat");

            this.Close();
        }

        private void loadWebtoons()
        {
            WebtoonInfoCollection favoriteWebtoonInfos;

            if(File.Exists("favoriteWebtoonInfoCollection.dat"))
            {
                favoriteWebtoonInfos = WebtoonInfoCollection.Load("favoriteWebtoonInfoCollection.dat");
            }
            else
            {
                favoriteWebtoonInfos = new WebtoonInfoCollection();
            }

            WebtoonInfoCollection everyWebtoonInfos = Webtoon.GetWebtoonInfos();

            foreach(WebtoonInfo webtoonInfo in everyWebtoonInfos)
            {
                this.Invoke(new Action(() =>
                {
                    cLstBox_WebtoonList.Items.Add(webtoonInfo);
                }));
                if(favoriteWebtoonInfos.Contains(webtoonInfo))
                {
                    int curIndex = cLstBox_WebtoonList.Items.Count - 1;
                    this.Invoke(new Action(() =>
                    {
                        cLstBox_WebtoonList.SetItemChecked(curIndex, true);
                    }));
                }
            }
        }

        private void NotDevelopedMessage(object sender, EventArgs e)
        {
            MessageBox.Show("이 기능은 개발중입니다. 개발자가 무능해서 아마 시간이 좀 걸릴 것 같습니다.");
        }

        private void EditFavoriteWebtoonsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WebtoonInfoCollection loadedInfos = WebtoonInfoCollection.Load("favoriteWebtoonInfoCollection.dat");
            WebtoonInfoCollection checkecInfos = getCheckedList();
            if(checkecInfos != loadedInfos)
            {
                DialogResult result = MessageBox.Show("변경 내용을 저장하시겠습니까?", "", MessageBoxButtons.YesNoCancel);

                if(result == DialogResult.Yes)
                {
                    getCheckedList().Save("favoriteWebtoonInfoCollection.dat");
                }
                else if(result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
