using LibWebtoonDownloader;
using System;
using System.IO;
using System.Windows.Forms;

namespace WebtoonDownloader
{
    public partial class EditFavoriteWebtoonsForm : Form
    {
        public EditFavoriteWebtoonsForm()
        {
            InitializeComponent();
            loadWebtoons();
        }

        private o_WebtoonInfoCollection getCheckedList()
        {
            o_WebtoonInfoCollection favoriteWebtoonInfos = new o_WebtoonInfoCollection();

            for(int i = 0 ; i < cLstBox_WebtoonList.Items.Count ; i++)
            {
                if(cLstBox_WebtoonList.GetItemChecked(i))
                {
                    favoriteWebtoonInfos.Add((o_WebtoonInfo)cLstBox_WebtoonList.Items[i]);
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
            o_WebtoonInfoCollection favoriteWebtoonInfos;
            cLstBox_WebtoonList.Items.Clear();

            LoadingForm loading = new LoadingForm();
            loading.pBar.Maximum = 3;

            if(File.Exists("favoriteWebtoonInfoCollection.dat"))
            {
                favoriteWebtoonInfos = o_WebtoonInfoCollection.Load("favoriteWebtoonInfoCollection.dat");
            }
            else
            {
                favoriteWebtoonInfos = new o_WebtoonInfoCollection();
            }

            loading.pBar.PerformStep();

            o_WebtoonInfoCollection everyWebtoonInfos = Webtoon.GetEveryWebtoonInfos();

            loading.pBar.PerformStep();

            foreach(o_WebtoonInfo webtoonInfo in everyWebtoonInfos)
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
            o_WebtoonInfoCollection loadedInfos = o_WebtoonInfoCollection.Load("favoriteWebtoonInfoCollection.dat");
            o_WebtoonInfoCollection checkecInfos = getCheckedList();
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
