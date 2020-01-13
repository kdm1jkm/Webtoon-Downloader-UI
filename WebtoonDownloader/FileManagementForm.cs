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
using System.IO.Compression;

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

            LoadingForm loading = new LoadingForm();

            loading.Show();
            loading.Invoke(new Action(() =>
            {
                loading.pBar.Value = 10000;
            }));

            for(int i = 0 ; i < cLstBx_webtoonList.CheckedItems.Count ; i++)
            {
                MetaData curData = (MetaData)cLstBx_webtoonList.CheckedItems[i];

                string htmlDir = string.Format(@"html\{0}_{1:000}\!html.html", curData.WebtoonName, curData.No);
                List<string> imgs = new List<string>();

                for(int j = 1 ; j <= curData.ImgCnt ; j++)
                {
                    string sourceFileName = string.Format(@"src\{0}_{1:000}\{2}.jpg", curData.WebtoonName, curData.No, j);
                    string destFileName = string.Format(@"html\{0}_{1:000}\{2}.jpg", curData.WebtoonName, curData.No, j);
                    string imgSrc = string.Format(@"{0}.jpg", j);
                    if(File.Exists(destFileName))
                    {
                        continue;
                    }
                    if(!File.Exists(sourceFileName))
                    {
                        sourceFileName = string.Format(@"src\{0}_{1:000}\{2}.png", curData.WebtoonName, curData.No, j);
                        destFileName = string.Format(@"html\{0}_{1:000}\{2}.png", curData.WebtoonName, curData.No, j);
                        imgSrc = string.Format(@"{0}.png", j);
                    }
                    string dirPath = string.Format(@"html\{0}_{1:000}", curData.WebtoonName, curData.No);
                    Directory.CreateDirectory(dirPath);
                    File.Copy(sourceFileName, destFileName);
                    imgs.Add(imgSrc);
                    
                }

                Webtoon.MakeHtml(htmlDir, imgs);

                loading.Invoke(new Action(() =>
                {
                    loading.pBar.Value = (i + 1) * 10000 / cLstBx_webtoonList.CheckedItems.Count;
                }));
            }

            loading.Close();
        }

        private void btn_convertZip_Click(object sender, EventArgs e)
        {
            if(cLstBx_webtoonList.CheckedItems == null)
            {
                return;
            }

            LoadingForm loading = new LoadingForm();
            loading.Show();
            loading.Invoke(new Action(() => { loading.pBar.Value = 0; }));

            if(!Directory.Exists("zip"))
            {
                Directory.CreateDirectory("zip");
            }

            for(int i = 0 ; i < cLstBx_webtoonList.CheckedItems.Count ; i++)
            {
                MetaData curData = (MetaData)cLstBx_webtoonList.CheckedItems[i];

                string sourceDir = $@"src\{curData.WebtoonName}_{curData.No.ToString("D3")}";
                string destinationDir = $@"zip\{curData.WebtoonName}_{curData.No}.zip";

                ZipFile.CreateFromDirectory(sourceDir, destinationDir);

                loading.Invoke(new Action(() =>
                {
                    loading.pBar.Value = (i + 1) * 10000 / cLstBx_webtoonList.CheckedItems.Count;
                }));
            }

            loading.Close();
        }
    }
}
