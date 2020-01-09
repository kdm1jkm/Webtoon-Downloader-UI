using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibWebtoonDownloader;

namespace WebtoonDownloader
{
    public partial class FileManagementForm : Form
    {
        public FileManagementForm()
        {
            InitializeComponent();
            loadWebtoonSrcList();
        }

        private void loadWebtoonSrcList()
        {
            string[] webtoonSrcDirs = Directory.GetDirectories("src");
            foreach(string webtoonSrcDir in webtoonSrcDirs)
            {
                string metaDataPath = webtoonSrcDir + "\\metaData.dat";
                if(File.Exists(metaDataPath))
                {
                    cLstBx_webtoonList.Items.Add(MetaData.Load(metaDataPath));
                }
            }
        }

        private void btn_convertHtml_Click(object sender, EventArgs e)
        {
            if(cLstBx_webtoonList.CheckedItems == null)
            {
                return;
            }
            foreach(MetaData curData in cLstBx_webtoonList.CheckedItems)
            {
                string htmlDir = string.Format(@"html\{0}_{1}\html.html", curData.WebtoonName, curData.No);
                List<string> imgs = new List<string>();

                for(int i = 1 ; i <= curData.ImgCnt ; i++)
                {
                    string sourceFileName = string.Format(@"src\{0}_{1}\{2}.jpg", curData.WebtoonName, curData.No, i);
                    string destFileName = string.Format(@"html\{0}_{1}\{2}.jpg", curData.WebtoonName, curData.No, i);
                    string imgSrc = string.Format(@"{0}.jpg", i);
                    if(!File.Exists(sourceFileName))
                    {
                        sourceFileName = string.Format(@"src\{0}_{1}\{2}.png", curData.WebtoonName, curData.No, i);
                        destFileName = string.Format(@"html\{0}_{1}\{2}.png", curData.WebtoonName, curData.No, i);
                        imgSrc = string.Format(@"{0}.png", i);
                    }
                    string dirPath = string.Format(@"!html\{0}_{1}", curData.WebtoonName, curData.No);
                    Directory.CreateDirectory(dirPath);
                    File.Copy(sourceFileName, destFileName);
                    imgs.Add(imgSrc);
                }

                Webtoon.MakeHtml(htmlDir, imgs);
            }
        }
    }
}
