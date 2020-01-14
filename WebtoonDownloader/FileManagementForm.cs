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
            LoadWebtoonSrcList();
            tBox_SaveDir.Text = Directory.GetCurrentDirectory() + @"\downloaded";
        }

        private void LoadWebtoonSrcList()
        {
            cLstBx_webtoonList.Items.Clear();

            if(!Directory.Exists("src"))
            {
                return;
            }

            string[] webtoonSrcDirs = Directory.GetDirectories("src");

            if(webtoonSrcDirs == null)
            {
                return;
            }

            foreach(string webtoonSrcDir in webtoonSrcDirs)
            {
                string metaDataPath = webtoonSrcDir + "\\metaData.dat";
                if(File.Exists(metaDataPath))
                {
                    cLstBx_webtoonList.Items.Add(MetaData.Load(metaDataPath));
                }
                else
                {
                    foreach(string delDir in Directory.GetFiles(webtoonSrcDir))
                    {
                        File.Delete(delDir);
                    }
                    Directory.Delete(webtoonSrcDir);
                }
            }
        }

        private void btn_convertHtml_Click(object sender, EventArgs e)
        {
            if(cLstBx_webtoonList.CheckedItems == null)
            {
                return;
            }

            string path = tBox_SaveDir.Text;

            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            LoadingForm loading = new LoadingForm();

            loading.Show();
            loading.Invoke(new Action(() =>
            {
                loading.pBar.Value = 0;
            }));

            for(int i = 0 ; i < cLstBx_webtoonList.CheckedItems.Count ; i++)
            {
                MetaData curData = (MetaData)cLstBx_webtoonList.CheckedItems[i];

                string htmlDir = $@"{path}\{curData.WebtoonName}_{curData.No}\!html.html";
                List<string> imgs = new List<string>();

                for(int j = 1 ; j <= curData.ImgCnt ; j++)
                {
                    string sourceFileName = string.Format(@"src\{0}_{1:000}\{2}.jpg", curData.WebtoonName, curData.No, j);
                    string destFileName = $@"{path}\{curData.WebtoonName}_{curData.No}\{j}.jpg";
                    string imgSrc = string.Format(@"{0}.jpg", j);
                    if(File.Exists(destFileName))
                    {
                        continue;
                    }
                    if(!File.Exists(sourceFileName))
                    {
                        sourceFileName = string.Format(@"src\{0}_{1:000}\{2}.png", curData.WebtoonName, curData.No, j);
                        destFileName = $@"{path}\{curData.WebtoonName}_{curData.No}\{j}.png";
                        imgSrc = string.Format(@"{0}.png", j);
                    }
                    string dirPath = $@"{path}\{curData.WebtoonName}_{curData.No}";
                    Directory.CreateDirectory(dirPath);
                    File.Copy(sourceFileName, destFileName);
                    imgs.Add(imgSrc);

                }

                Webtoon.MakeHtml(htmlDir, imgs);

                loading.Invoke(new Action(() =>
                {
                    loading.pBar.Value = (i + 1) * loading.pBar.Maximum / cLstBx_webtoonList.CheckedItems.Count;
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

            string path = tBox_SaveDir.Text;

            LoadingForm loading = new LoadingForm();
            loading.Show();
            loading.Invoke(new Action(() => { loading.pBar.Value = 0; }));

            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            for(int i = 0 ; i < cLstBx_webtoonList.CheckedItems.Count ; i++)
            {
                MetaData curData = (MetaData)cLstBx_webtoonList.CheckedItems[i];

                string sourceDir = $@"src\{curData.WebtoonName}_{curData.No.ToString("D3")}";
                string destinationDir = $@"{path}\{curData.WebtoonName}_{curData.No}.zip";

                loading.Invoke(new Action(() =>
                {
                    loading.pBar.Value = (i) * loading.pBar.Maximum / cLstBx_webtoonList.CheckedItems.Count;
                }));

                if(File.Exists(destinationDir))
                { continue; }

                ZipFile.CreateFromDirectory(sourceDir, destinationDir);
            }

            loading.Close();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if(cLstBx_webtoonList.CheckedItems == null)
            {
                return;
            }

            LoadingForm loading = new LoadingForm();
            loading.Show();
            loading.Invoke(new Action(() => { loading.pBar.Value = 0; }));

            for(int i = 0 ; i < cLstBx_webtoonList.CheckedItems.Count ; i++)
            {
                MetaData curData = (MetaData)cLstBx_webtoonList.CheckedItems[i];

                string sourceDir = $@"src\{curData.WebtoonName}_{curData.No.ToString("D3")}";

                foreach(string file in Directory.GetFiles(sourceDir))
                {
                    File.Delete(file);
                }

                Directory.Delete(sourceDir);

                loading.Invoke(new Action(() =>
                {
                    loading.pBar.Value = (i + 1) * loading.pBar.Maximum / cLstBx_webtoonList.CheckedItems.Count;
                }));
            }

            LoadWebtoonSrcList();

            loading.Close();
        }

        private void checkBox_SelectAll_Click(object sender, EventArgs e)
        {
            bool change = checkBox_SelectAll.Checked;
            for(int i = 0 ; i < cLstBx_webtoonList.Items.Count ; i++)
            {
                cLstBx_webtoonList.SetItemChecked(i, change);
            }
            checkBox_SelectAll.Checked = change;
        }

        private void cLstBx_webtoonList_KeyPress(object sender, KeyPressEventArgs e)
        {
            checkSeletedAll();
        }

        private void cLstBx_webtoonList_MouseUp(object sender, MouseEventArgs e)
        {
            checkSeletedAll();
        }

        private void checkSeletedAll()
        {
            checkBox_SelectAll.Checked = (cLstBx_webtoonList.Items.Count == cLstBx_webtoonList.CheckedItems.Count);
        }

        private void btn_selectDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                Description = "파일을 저장할 위치를 선택하세요"
            };
            DialogResult result = dialog.ShowDialog();

            if(result == DialogResult.OK)
            {
                tBox_SaveDir.Text = dialog.SelectedPath;
            }
        }
    }
}
