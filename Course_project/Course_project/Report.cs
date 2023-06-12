using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Course_project
{
    public partial class Report : Form
    {
        const string FILE_CARS = "Cars.json";
        const string FILE_CLIENTS = "Clients.json";
        int number = 0;

        public Report()
        {
            Task.Run(() => File.Open(FILE_CARS, FileMode.OpenOrCreate).Close());
            Task.Run(() => File.Open(FILE_CLIENTS, FileMode.OpenOrCreate).Close());

            InitializeComponent();
        }

        async void Report_Load(object sender, EventArgs e)
        {
            if (File.Exists(FILE_CARS))
            {
                var table_of_cars = await ReadFromFile<InfoCars>(FILE_CARS);
                var table_of_clients = await ReadFromFile<InfoClients>(FILE_CLIENTS);

                if (table_of_cars.Count != 0)
                    foreach (var car in table_of_cars)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[number].Cells[0].Value = car.CarID;
                        dataGridView1.Rows[number].Cells[1].Value = car.Model;
                        dataGridView1.Rows[number].Cells[2].Value = car.Price;
                        int count = 0;
                        foreach (var client in table_of_clients)
                            if (car.CarID == client.CarID) count++;
                        dataGridView1.Rows[number].Cells[3].Value = count;
                        dataGridView1.Rows[number].Cells[4].Value = car.Price * count;
                        number++;
                    }
            }
        }

        async Task<List<T>> ReadFromFile<T>(string FILE_NAME)
        {
            using (var streamReader = new StreamReader(FILE_NAME))
            {
                return await Task.Run(async () => JsonConvert.DeserializeObject<List<T>>(await streamReader.ReadToEndAsync()) ?? new List<T>());
            }
        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
