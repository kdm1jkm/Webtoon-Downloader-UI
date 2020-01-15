using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            loadWebtoons();
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

        public void loadWebtoons()
        {
            WebtoonInfoCollection favoriteWebtoonInfos;
            cLstBox_WebtoonList.Items.Clear();

            LoadingForm loading = new LoadingForm();
            loading.pBar.Maximum = 3;

            if(File.Exists("favoriteWebtoonInfoCollection.dat"))
            {
                favoriteWebtoonInfos = WebtoonInfoCollection.Load("favoriteWebtoonInfoCollection.dat");
            }
            else
            {
                favoriteWebtoonInfos = new WebtoonInfoCollection();
            }

            loading.pBar.PerformStep();

            WebtoonInfoCollection everyWebtoonInfos = Webtoon.GetEveryWebtoonInfos();

            loading.pBar.PerformStep();

            foreach(WebtoonInfo webtoonInfo in everyWebtoonInfos)
            {
                cLstBox_WebtoonList.Items.Add(webtoonInfo);

                if(favoriteWebtoonInfos.Contains(webtoonInfo))
                {
                    int curIndex = cLstBox_WebtoonList.Items.Count - 1;
                    cLstBox_WebtoonList.SetItemChecked(curIndex, true);

                }
            }

            loading.Close();
        }

        private void NotDevelopedMessage(object sender, EventArgs e)
        {
            MessageBox.Show("이 기능은 개발중입니다.");
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

        private void btn_LoadFromNaverAcnt_Click(object sender, EventArgs e)
        {
            NaverLoginForm form = new NaverLoginForm(this);
            form.ShowDialog();
        }
    }
}
