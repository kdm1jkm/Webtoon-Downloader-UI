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
