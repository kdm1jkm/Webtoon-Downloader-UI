using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebtoonDownloader
{
    public partial class ErrorShowForm : Form
    {
        public ErrorShowForm()
        {
            InitializeComponent();
        }

        public ErrorShowForm(string Messege)
        {
            InitializeComponent();
            tBox_ErrorMessege.Text = Messege;
        }
    }
}
