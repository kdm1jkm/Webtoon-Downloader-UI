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
            webtoonDownload = new Webtoon();
        }

        private Webtoon webtoonDownload;

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
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
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
            if(tBox_titleId.Text == "")
            {
                MessageBox.Show("웹툰 id를 입력해 주세요.");
                return;
            }
            int titleId = int.Parse(tBox_titleId.Text);
            if(!Webtoon.IsAvailable(titleId))
            {
                MessageBox.Show("입력한 id는 존재하지 않습니다.");
                return;
            }
            int startNo = int.Parse(num_StartNo.Value.ToString());
            int endNo = int.Parse(num_endNo.Value.ToString());

            string webtoonName = Webtoon.GetNameById(titleId);

            Console.WriteLine("시작전");

            for(int i = startNo ; i <= endNo ; i++)
            {
                webtoonDownload.AddTask(titleId, i, checkBox_HTML.Checked, checkBox_zip.Checked);

                string webtoonInfoString = string.Format("{0}({1})", webtoonName, i);
                lBox_queue.Items.Add(webtoonInfoString);
            }
        }
    }
}
