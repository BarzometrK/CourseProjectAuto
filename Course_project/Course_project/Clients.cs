using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace Course_project
{
    public partial class Clients : Form
    {
        const string FILE_CARS = "Cars.json";
        const string FILE_CLIENTS = "Clients.json";
        int n_clients = 0;
        int numbers = 0;

        public Clients()
        {
            Task.Run(() => File.Open(FILE_CLIENTS, FileMode.OpenOrCreate).Close());
            Task.Run(() => File.Open(FILE_CARS, FileMode.OpenOrCreate).Close());

            InitializeComponent();
        }

        async void Clients_Load(object sender, EventArgs e)
        {
            if (File.Exists(FILE_CLIENTS))
            {
                var table_of_clients = await ReadFromFile<InfoClients>(FILE_CLIENTS);

                n_clients = table_of_clients.Count;
                count_of_clients.Text = Convert.ToString(n_clients);

                if (table_of_clients != null)
                    foreach (var client in table_of_clients)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[numbers].Cells[0].Value = client.FirstName;
                        dataGridView1.Rows[numbers].Cells[1].Value = client.LastName;
                        dataGridView1.Rows[numbers].Cells[2].Value = client.Date;
                        dataGridView1.Rows[numbers].Cells[3].Value = client.Phone;
                        dataGridView1.Rows[numbers].Cells[4].Value = client.CarID;
                        numbers++;
                    }
            }
        }

        //запись в файл json
        async Task WriteToFile<T>(List<T> data, string FILE_NAME)
        {
            using (var streamWriter = new StreamWriter(FILE_NAME, false))
            {
                await streamWriter.WriteAsync(await Task.Run(() => JsonConvert.SerializeObject(data)));
            }
        }

        //чтение из файла json
        async Task<List<T>> ReadFromFile<T>(string FILE_NAME)
        {
            using (var streamReader = new StreamReader(FILE_NAME))
            {
                return await Task.Run(async () => JsonConvert.DeserializeObject<List<T>>(await streamReader.ReadToEndAsync()) ?? new List<T>());
            }
        }

        async void btn_Add_Click(object sender, EventArgs e)
        {
            Add_Client addClient = new Add_Client();
            addClient.ShowDialog();

            string firstName = Add_Client.firstName;
            string lastName = Add_Client.lastName;
            string date = Add_Client.date;
            string phone = Add_Client.phone;
            int carID = Add_Client.carID;

            InfoClients newClient = new InfoClients(firstName, lastName, date, phone, carID);

            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) && !string.IsNullOrEmpty(date) &&
                !string.IsNullOrEmpty(phone) && (carID > 0))
            {
                var clients = await ReadFromFile<InfoClients>(FILE_CLIENTS);
                var cars = await ReadFromFile<InfoCars>(FILE_CARS);

                bool check = false;
                foreach (var car in cars)
                    if (car.CarID == carID)
                        check = true;
                if (!check)
                {
                    MessageBox.Show($"Авто с ID {carID} не существует."
                            , "Добавление клиента", 0, MessageBoxIcon.Information);
                    return;
                }

                clients.Add(newClient);

                n_clients = clients.Count;
                count_of_clients.Text = Convert.ToString(n_clients);

                await WriteToFile(clients, FILE_CLIENTS);

                dataGridView1.Rows.Add();
                dataGridView1.Rows[numbers].Cells[0].Value = firstName;
                dataGridView1.Rows[numbers].Cells[1].Value = lastName;
                dataGridView1.Rows[numbers].Cells[2].Value = date;
                dataGridView1.Rows[numbers].Cells[3].Value = phone;
                dataGridView1.Rows[numbers].Cells[4].Value = carID;
                numbers++;
            }
        }

        async void btn_Remove_Click(object sender, EventArgs e)
        {
            var Items = dataGridView1.SelectedRows;
            int selectCount = Items.Count;

            if (selectCount > 0)
            {
                var clients = await ReadFromFile<InfoClients>(FILE_CLIENTS);

                foreach (var item in Items)
                {
                    var it = (DataGridViewRow)item;
                    string firstName = it.Cells[0].Value.ToString();
                    string lastName = it.Cells[1].Value.ToString();
                    string date = it.Cells[2].Value.ToString();
                    string phone = it.Cells[3].Value.ToString();
                    int carID = Convert.ToInt32(it.Cells[4].Value.ToString());

                    foreach (var cl in clients)
                    {
                        if (firstName == cl.FirstName && lastName == cl.LastName && date == cl.Date &&
                            phone == cl.Phone && carID == cl.CarID)
                        {
                            clients.Remove(cl);
                            n_clients = clients.Count;
                            count_of_clients.Text = Convert.ToString(n_clients);
                            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                            numbers--;
                            MessageBox.Show($"Клиент  {cl.FirstName} {cl.LastName}  удалён!", "Удаление клиента",
                                0, MessageBoxIcon.Information);
                            break;
                        }
                    }
                }
                await WriteToFile(clients, FILE_CLIENTS);
            }
            else
            {
                MessageBox.Show("Нет клиентов", "Удаление", 0, MessageBoxIcon.Information);
                return;
            }
        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
