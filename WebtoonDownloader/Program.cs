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
                Application.Run(new ErrorShowForm("인터넷 연결이 원활하지 않습니다.\n인터넷에 연결되었는지 확인 후 다시 시도해 주시기 바랍니다."));
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
