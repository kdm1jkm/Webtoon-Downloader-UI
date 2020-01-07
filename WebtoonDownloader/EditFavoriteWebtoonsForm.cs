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

namespace WebtoonDownloader
{
    public partial class EditFavoriteWebtoonsForm : Form
    {
        public EditFavoriteWebtoonsForm()
        {
            InitializeComponent();
            loadWebtoons();
        }

        private void btn_saveAndExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loadWebtoons()
        {
            WebtoonInfos webtoonInfos = Webtoon.GetWebtoonInfos();

            foreach(WebtoonInfo webtoonInfo in webtoonInfos)
            {
                string webtoonNameStr = webtoonInfo.Name;

                string weekdayStr = "[";

                for(int i = 0 ; i < webtoonInfo.weekdays.Count ; i++)
                {
                    switch(webtoonInfo.weekdays[i])
                    {
                        case 0:
                            weekdayStr += "월";
                            break;

                        case 1:
                            weekdayStr += "화";
                            break;

                        case 2:
                            weekdayStr += "수";
                            break;

                        case 3:
                            weekdayStr += "목";
                            break;

                        case 4:
                            weekdayStr += "금";
                            break;

                        case 5:
                            weekdayStr += "토";
                            break;

                        case 6:
                            weekdayStr += "일";
                            break;
                    }

                    if(i != webtoonInfo.weekdays.Count - 1)
                    {
                        weekdayStr += ",";
                    }
                }

                weekdayStr += "]";

                cLstBox_WebtoonList.Items.Add(webtoonNameStr + weekdayStr);
            }
        }
    }
}
