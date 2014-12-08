using System.Windows.Forms;

namespace TDMaker
{
    public partial class TextViewer : Form
    {
        public TextViewer(string title, string txt)
        {
            InitializeComponent();

            this.Text = title;
            txtText.Text = txt;
        }
    }
}