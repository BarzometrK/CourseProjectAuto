using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Course_project
{
    public partial class Cars : Form
    {
        const string FILE_CARS = "Cars.json";
        const string FILE_CLIENTS = "Clients.json";
        int number = 0;
        int n_cars = 0;
        int ID = 1;

        public Cars()
        {
            Task.Run(() => File.Open(FILE_CARS, FileMode.OpenOrCreate).Close());
            Task.Run(() => File.Open(FILE_CLIENTS, FileMode.OpenOrCreate).Close());

            InitializeComponent();
        }

        async void Cars_Load(object sender, EventArgs e)
        {
            if (File.Exists(FILE_CARS))
            {
                var table_of_cars = await ReadFromFile<InfoCars>(FILE_CARS);

                n_cars = table_of_cars.Count;
                count_of_cars.Text = Convert.ToString(n_cars);

                if (table_of_cars.Count != 0)
                {
                    foreach (var car in table_of_cars)
                    {
                        Print(car);
                        if (ID == car.CarID) ID++;
                    }
                }
            }
        }

        async Task WriteToFile<T>(List<T> data, string FILE_NAME)
        {
            using (var streamWriter = new StreamWriter(FILE_NAME, false))
            {
                await streamWriter.WriteAsync(await Task.Run(() => JsonConvert.SerializeObject(data)));
            }
        }

        async Task<List<T>> ReadFromFile<T>(string FILE_NAME)
        {
            using (var streamReader = new StreamReader(FILE_NAME))
            {
                return await Task.Run(async () => JsonConvert.DeserializeObject<List<T>>(await streamReader.ReadToEndAsync()) ?? new List<T>());
            }
        }

        void Control_Click(object sender, EventArgs e) => (sender as Control).BackColor = Color.White;

        async void btn_Add_Click(object sender, EventArgs e)
        {
            btn_Reset_Click(sender, e);
            Add_Car FormAdd = new Add_Car();
            FormAdd.ShowDialog();

            string model = Add_Car.model;
            string bodyType = Add_Car.bodyType;
            double capacity = Add_Car.capacity;
            int power = Add_Car.power;
            int productionYear = Add_Car.productionYear;
            int price = Add_Car.price;
            int carID = ID;

            InfoCars newCar = new InfoCars(model, bodyType, capacity, power, productionYear, price, carID);

            if (!string.IsNullOrEmpty(model) && !string.IsNullOrEmpty(bodyType) && (capacity > 0) && (power > 0)
            && (productionYear > 0) && (price > 0) && (carID > 0))
            {
                var cars = await ReadFromFile<InfoCars>(FILE_CARS);

                if (!cars.Contains(newCar))
                {
                    cars.Add(newCar);

                    n_cars = cars.Count;
                    count_of_cars.Text = Convert.ToString(n_cars);
                    ID++;

                    cars.Sort();

                    await WriteToFile(cars, FILE_CARS);

                    Print(newCar);
                }
                else
                {
                    MessageBox.Show($"Данный автомобиль уже есть в базе данных", 
                        "Добавление автомобиля", 0, MessageBoxIcon.Information);
                    return;
                }
            }
        }

        async void btn_Remove_Click(object sender, EventArgs e)
        {
            var Items = dataGridView1.SelectedRows;
            int selectCount = Items.Count;

            if (selectCount > 0)
            {
                var cars = await ReadFromFile<InfoCars>(FILE_CARS);
                var clients = await ReadFromFile<InfoClients>(FILE_CLIENTS);

                foreach (var item in Items)
                {
                    int ID = Convert.ToInt32(((DataGridViewRow)item).Cells[0].Value);

                    foreach (var car in cars)
                    {
                        if (ID == car.CarID)
                        {
                            bool check = true;

                            foreach (var cl in clients)
                            {
                                if (ID == cl.CarID)
                                {
                                    MessageBox.Show($"Удаление невозможно, данный авто существует в базе продаж",
                                        "Удаление автомобиля", 0, MessageBoxIcon.Information);
                                    check = false;
                                    break;
                                }
                            }
                            if (check)
                            {
                                cars.Remove(car);
                                n_cars = cars.Count;
                                count_of_cars.Text = Convert.ToString(n_cars);
                                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                                number--;
                                MessageBox.Show($"Модель <{car.Model}> удалена",
                                    "Удаление автомобиля", 0, MessageBoxIcon.Information);
                                break;
                            }
                        }
                    }
                }
                
                await WriteToFile(cars, FILE_CARS);
            }
            else
            {
                MessageBox.Show("Выберите авто для удаления", "Удаление", 0, MessageBoxIcon.Information);
                return;
            }
        }

        bool Print(InfoCars car)
        {
            dataGridView1.Rows.Add();
            dataGridView1.Rows[number].Cells[0].Value = car.CarID;
            dataGridView1.Rows[number].Cells[1].Value = car.Model;
            dataGridView1.Rows[number].Cells[2].Value = car.BodyType;
            dataGridView1.Rows[number].Cells[3].Value = car.Capacity;
            dataGridView1.Rows[number].Cells[4].Value = car.Power;
            dataGridView1.Rows[number].Cells[5].Value = car.ProductionYear;
            dataGridView1.Rows[number].Cells[6].Value = car.Price;
            number++;
            return true;
        }

        async void btn_filter_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is null)
            {
                MessageBox.Show("Выберите фильтр", "Фильтрация", 0, MessageBoxIcon.Information);
            }
            else if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Заполните поле", "Фильтрация", 0, MessageBoxIcon.Information);
                textBox1.BackColor = Color.MistyRose;
            }
            else
            {
                textBox1.BackColor = Color.White;
                string filter = textBox1.Text;
                int fil;

                textBox2.Text = "";
                dataGridView1.Rows.Clear();
                number = 0;

                bool flag = false;

                var cars = await ReadFromFile<InfoCars>(FILE_CARS);

                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        foreach (var car in cars)
                            if (car.Model == Convert.ToString(filter))
                                flag = Print(car);

                        if (flag == false)
                            MessageBox.Show($"Автомобили модели {filter} не найдены, проверьте правильность ввода",
                                "Фильтрация", 0, MessageBoxIcon.Information);
                        break;
                    case 1:
                        foreach (var car in cars)
                            if (car.BodyType == Convert.ToString(filter))
                                flag = Print(car);

                        if (flag == false)
                            MessageBox.Show($"Автомобили с типом кузова '{filter}' не найдены, проверьте правильность ввода",
                                "Фильтрация", 0, MessageBoxIcon.Information);
                        break;
                    case 2:
                        if (int.TryParse(filter, out fil) && fil > 0)
                        {
                            foreach (var car in cars)
                                if (car.Capacity >= fil)
                                    flag = Print(car);
                            if (flag == false)
                                MessageBox.Show($"Автомобили с объемом двигателя от {filter} л не найдены, " +
                                    $"проверьте правильность ввода",
                                    "Фильтрация", 0, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show($"Некорректные данные",
                                    "Фильтрация", 0, MessageBoxIcon.Information);
                        break;
                    case 3:
                        if (int.TryParse(filter, out fil) && fil > 0)
                        {
                            foreach (var car in cars)
                                if (car.Capacity < fil)
                                    flag = Print(car);

                            if (flag == false)
                                MessageBox.Show($"Автомобили с объемом двигателя до {filter} л не найдены, " +
                                    $"проверьте правильность ввода",
                                    "Фильтрация", 0, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show($"Некорректные данные",
                                    "Фильтрация", 0, MessageBoxIcon.Information);
                        break;
                    case 4:
                        if (int.TryParse(filter, out fil) && fil > 0)
                        {
                            foreach (var car in cars)
                                if (car.Power >= fil)
                                    flag = Print(car);

                            if (flag == false)
                                MessageBox.Show($"Автомобили мощностью от {filter} лс не найдены, " +
                                    $"проверьте правильность ввода",
                                    "Фильтрация", 0, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show($"Некорректные данные",
                                    "Фильтрация", 0, MessageBoxIcon.Information);
                        break;
                    case 5:
                        if (int.TryParse(filter, out fil) && fil > 0)
                        {
                            foreach (var car in cars)
                                if (car.Power < fil)
                                    flag = Print(car);

                            if (flag == false)
                                MessageBox.Show($"Автомобили мощностью до {filter} лс не найдены, " +
                                    $"проверьте правильность ввода",
                                    "Фильтрация", 0, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show($"Некорректные данные",
                                    "Фильтрация", 0, MessageBoxIcon.Information);
                        break;
                    case 6:
                        if (int.TryParse(filter, out fil) && fil > 0)
                        {
                            foreach (var car in cars)
                                if (car.ProductionYear >= fil)
                                    flag = Print(car);

                            if (flag == false)
                                MessageBox.Show($"Автомобили с {filter} года не найдены, " +
                                    $"проверьте правильность ввода",
                                    "Фильтрация", 0, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show($"Некорректные данные",
                                    "Фильтрация", 0, MessageBoxIcon.Information);
                        break;
                    case 7:
                        if (int.TryParse(filter, out fil) && fil > 0)
                        {
                            foreach (var car in cars)
                                if (car.ProductionYear < fil)
                                    flag = Print(car);

                            if (flag == false)
                                MessageBox.Show($"Автомобили до {filter} года не найдены, " +
                                    $"проверьте правильность ввода",
                                    "Фильтрация", 0, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show($"Некорректные данные",
                                    "Фильтрация", 0, MessageBoxIcon.Information);
                        break;
                    case 8:
                        if (int.TryParse(filter, out fil) && fil > 0)
                        {
                            foreach (var car in cars)
                                if (car.Price >= fil)
                                    flag = Print(car);

                            if (flag == false)
                                MessageBox.Show($"Автомобили стоимостью от {filter} рублей не найдены, " +
                                    $"проверьте правильность ввода",
                                    "Фильтрация", 0, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show($"Некорректные данные",
                                    "Фильтрация", 0, MessageBoxIcon.Information);
                        break;
                    case 9:
                        if (int.TryParse(filter, out fil) && fil > 0)
                        {
                            foreach (var car in cars)
                                if (car.Price < fil)
                                    flag = Print(car);

                            if (flag == false)
                                MessageBox.Show($"Автомобили стоимостью до {filter} рублей не найдены, " +
                                    $"проверьте правильность ввода",
                                    "Фильтрация", 0, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show($"Некорректные данные",
                                    "Фильтрация", 0, MessageBoxIcon.Information);
                        break;
                }
                count_of_cars.Text = Convert.ToString(dataGridView1.RowCount);
                label_Filter.Visible = true;
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            comboBox1.SelectedIndex = -1;
            dataGridView1.ClearSelection();

            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Заполните поле", "Поиск", 0, MessageBoxIcon.Information);
                textBox2.BackColor = Color.MistyRose;
            }
            else
            {
                textBox2.BackColor = Color.White;
                if (int.TryParse(textBox2.Text, out int fil))
                {
                    bool flag = false;
                    var Items = dataGridView1.Rows;

                    foreach (var item in Items)
                        if (Convert.ToInt32(((DataGridViewRow)item).Cells[0].Value) == fil)
                        {
                            ((DataGridViewRow)item).Selected = true;
                            flag = true;
                        }

                    if (flag == false)
                        MessageBox.Show($"Автомобиль с ID {fil} не найдены, " +
                            $"проверьте правильность ввода",
                            "Поиск", 0, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show($"Некорректные данные",
                            "Поиск", 0, MessageBoxIcon.Information);
            }
        }

        async void btn_Reset_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            label_Filter.Visible = false;
            dataGridView1.Rows.Clear();
            comboBox1.SelectedIndex = -1;
            number = 0;

            var cars = await ReadFromFile<InfoCars>(FILE_CARS);

            foreach (var car in cars)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[number].Cells[0].Value = car.CarID;
                dataGridView1.Rows[number].Cells[1].Value = car.Model;
                dataGridView1.Rows[number].Cells[2].Value = car.BodyType;
                dataGridView1.Rows[number].Cells[3].Value = car.Capacity;
                dataGridView1.Rows[number].Cells[4].Value = car.Power;
                dataGridView1.Rows[number].Cells[5].Value = car.ProductionYear;
                dataGridView1.Rows[number].Cells[6].Value = car.Price;
                number++;
            }
            count_of_cars.Text = Convert.ToString(dataGridView1.RowCount);
        }

        async void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int carID = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());
            string model = dataGridView1.SelectedCells[1].Value.ToString();
            string bodyType = dataGridView1.SelectedCells[2].Value.ToString();
            double capacity = Convert.ToDouble(dataGridView1.SelectedCells[3].Value.ToString());
            int power = Convert.ToInt32(dataGridView1.SelectedCells[4].Value.ToString());
            int productionYear = Convert.ToInt32(dataGridView1.SelectedCells[5].Value.ToString());
            int price = Convert.ToInt32(dataGridView1.SelectedCells[6].Value.ToString());

            Edit_Car FormEdit = new Edit_Car(model, bodyType, capacity, power, productionYear, price, carID);
            FormEdit.ShowDialog();

            model = Edit_Car.Model;
            bodyType = Edit_Car.BodyType;
            capacity = Edit_Car.Capacity;
            power = Edit_Car.Power;
            productionYear = Edit_Car.ProductionYear;
            price = Edit_Car.Price;

            textBox2.Text = "";
            dataGridView1.Rows.Clear();
            number = 0;

            var cars = await ReadFromFile<InfoCars>(FILE_CARS);

            if (!string.IsNullOrEmpty(model) && !string.IsNullOrEmpty(bodyType) && (capacity > 0) && (power > 0)
            && (productionYear > 0) && (price > 0) && (carID > 0))
            {
                foreach (var car in cars)
                {
                    if (car.CarID == carID)
                    {
                        car.Model = model;
                        car.BodyType = bodyType;
                        car.Capacity = capacity;
                        car.Power = power;
                        car.ProductionYear = productionYear;
                        car.Price = price;

                        await WriteToFile(cars, FILE_CARS);
                    }
                    Print(car);
                }
            }
        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
