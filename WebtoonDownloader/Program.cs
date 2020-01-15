using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WebtoonDownloader
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                using(MainForm form = new MainForm())
                {
                    Application.Run(form);
                }
            }
            catch(System.Net.WebException)
            {
                Application.Run(new ErrorShowForm("Internet Connection Error.\nPlease retry after connect to internet."));
            }
            //catch(Exception ex)
            //{
            //    Application.Run(new ErrorShowForm(ex.ToString()));
            //}
            finally
            {
                Application.Exit();
            }

        }
    }
}
