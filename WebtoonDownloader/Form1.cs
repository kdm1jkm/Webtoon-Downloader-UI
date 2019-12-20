using LibWebtoonDownloader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WebtoonDownloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            webtoon = new Webtoon();
        }

        private Webtoon webtoon;

        private void btn_Search_Click(object sender, EventArgs e)
        {
            int titleId = Webtoon.GetIdByName(tBox_WebtoonName.Text);

            if(titleId == -1)
            {
                MessageBox.Show("웹툰을 찾을 수 없습니다.");
            }
        }
    }
}
