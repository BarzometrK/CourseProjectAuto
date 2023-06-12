using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Course_project
{
	public partial class Edit_Car : Form
	{
		public static string Model = "";
		public static string BodyType = "";
		public static double Capacity = 0;
		public static int Power = 0;
		public static int ProductionYear = 0;
		public static int Price = 0;
		
		public Edit_Car(string model, string bodyType, double capacity, int power, int productionYear, int price, int carID)
		{
			InitializeComponent();
			Model = model;
			BodyType = bodyType;
			Capacity = capacity;
			Power = power;
			ProductionYear = productionYear;
			Price = price;

			label1.Text = $"Текущий автомобиль (ID: {carID})";
			tb_Model.Text = model;
			cb_BodyType.SelectedItem = bodyType;
			tb_Capacity.Text = capacity.ToString();
			tb_Power.Text = power.ToString();
			tb_ProductionYear.Text = productionYear.ToString();
			tb_Price.Text = price.ToString();
        }

        bool CheckOnCorrectModel(TextBox tb) => tb.Text != "" || 
			(tb.BackColor = Color.MistyRose) != Color.MistyRose;
		bool CheckOnCorrectBodyType(ComboBox cb) => !(cb.SelectedItem is null) || 
			(cb.BackColor = Color.MistyRose) != Color.MistyRose;
		bool CheckOnCorrectNumber(TextBox tb) => int.TryParse(tb.Text, out _) || 
			(tb.BackColor = Color.MistyRose) != Color.MistyRose;
		bool CheckOnCorrectYear(TextBox tb) => Regex.IsMatch(tb.Text, @"\d{4}") ||
			(tb.BackColor = Color.MistyRose) != Color.MistyRose;
		bool CheckOnCorrectCapacity(TextBox tb) => double.TryParse(tb.Text, out _) || 
			(tb.BackColor = Color.MistyRose) != Color.MistyRose;

		bool FlagCorrect =>
			CheckOnCorrectModel(tb_Model) &
			CheckOnCorrectBodyType(cb_BodyType) &
			CheckOnCorrectCapacity(tb_Capacity) &
			CheckOnCorrectNumber(tb_Power) &
			CheckOnCorrectYear(tb_ProductionYear) &
			CheckOnCorrectNumber(tb_Price);

		void Control_Click(object sender, EventArgs e) => (sender as Control).BackColor = Color.WhiteSmoke;

		private void btn_Add_Click(object sender, EventArgs e)
		{

			if (FlagCorrect)
			{
				Model = tb_Model.Text;
				BodyType = cb_BodyType.SelectedItem as string;
				Capacity = Convert.ToDouble(tb_Capacity.Text);
				Power = Convert.ToInt32(tb_Power.Text);
				ProductionYear = Convert.ToInt32(tb_ProductionYear.Text);
				Price = Convert.ToInt32(tb_Price.Text);

				Close();
			}
			else
				MessageBox.Show($"Некорректные данные", "Редактирование автомобиля", 0, MessageBoxIcon.Information);
		}

		private void btn_Back_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
