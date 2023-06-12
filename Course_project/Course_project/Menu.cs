using System;
using System.Windows.Forms;

namespace Course_project
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void btn_cars_Click(object sender, EventArgs e)
        {
            Cars Car = new Cars();
            Car.ShowDialog();
        }

        private void btn_clients_Click(object sender, EventArgs e)
        {
            Clients Client = new Clients();
            Client.ShowDialog();
        }

        private void btn_Report_Click(object sender, EventArgs e)
        {
            Report Report = new Report();
            Report.ShowDialog();
        }
    }
}
