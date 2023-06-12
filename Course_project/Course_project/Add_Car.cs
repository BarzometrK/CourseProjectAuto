using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Course_project
{
	public partial class Add_Car : Form
	{
		public static string model = "";
		public static string bodyType = "";
		public static double capacity = 0;
		public static int power = 0;
		public static int productionYear = 0;
		public static int price = 0;
		
		public Add_Car()
		{
			InitializeComponent();
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
				model = tb_Model.Text;
				bodyType = cb_BodyType.SelectedItem as string;
				capacity = Convert.ToDouble(tb_Capacity.Text);
				power = Convert.ToInt32(tb_Power.Text);
				productionYear = Convert.ToInt32(tb_ProductionYear.Text);
				price = Convert.ToInt32(tb_Price.Text);

				Close();
			}
			else
				MessageBox.Show($"Некорректные данные", "Добавление автомобиля", 0, MessageBoxIcon.Information);
		}

		private void btn_Back_Click(object sender, EventArgs e)
		{
			model = "";
			bodyType = "";
			capacity = 0;
			power = 0;
			productionYear = 0;
			price = 0;
			Close();
		}
	}
}
