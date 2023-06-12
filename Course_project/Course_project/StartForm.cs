using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Course_project
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        async void StartForm_Load(object sender, EventArgs e)
        {
            await Task.Delay(3000);
            this.Close();
        }

        private void StartForm_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
