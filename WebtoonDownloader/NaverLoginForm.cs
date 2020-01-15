using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using LibWebtoonDownloader;

namespace WebtoonDownloader
{
    public partial class NaverLoginForm : Form
    {
        EditFavoriteWebtoonsForm motherForm;

        public NaverLoginForm()
        {
            InitializeComponent();
        }
        public NaverLoginForm(EditFavoriteWebtoonsForm form)
        {
            InitializeComponent();
            motherForm = form;
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            WebtoonInfoCollection favoriteInfos = Webtoon.GetFavoriteWebtoonInfosFromAccount(tBox_id.Text, tBox_password.Text);
            favoriteInfos.Save("favoriteWebtoonInfoCollection.dat");

            Thread loadThread = new Thread(new ThreadStart(motherForm.loadWebtoons));
            loadThread.Start();

            this.Close();
        }
    }
}
