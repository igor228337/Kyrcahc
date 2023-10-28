using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kyrsach
{
    /// <summary>
    /// Логика взаимодействия для PlaceWindow.xaml
    /// </summary>
    public partial class PlaceWindow : Window
    {
        public int id = 0;
        AddDataWindow addData;
        private string connectName = "Data Source=stud-mssql.sttec.yar.ru,38325; Initial Catalog=user209_db; Integrated Security=False; User ID=user209_db;Password = user209";
        public string ConnectName { get { return connectName; } set { connectName = value; } }
        public int good = 0;
        public PlaceWindow(AddDataWindow addData)
        {
            this.addData = addData;
            InitializeComponent();
        }

        private void Place_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDateStart = DataStart.SelectedDate;
            DateTime? selectedDateEnd = DataEnd.SelectedDate;
            if (Name.Text == "" || Name.Text == null || !selectedDateStart.HasValue || City.Text == "" || City.Text == null || Address.Text == "" || Address.Text == null || Rauon.Text == "" || Rauon.Text == null)
            {
                MessageBox.Show("Какие-то данные пропущены");
                return;
            }
            try
            {
                string end;
                string start = selectedDateStart.Value.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                if (selectedDateEnd.HasValue) 
                {
                    end = selectedDateEnd.Value.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    end = "";
                }
                using (SqlConnection connection = new SqlConnection(ConnectName))
                {

                    connection.Open();
                    SqlCommand com = new SqlCommand();
                    if (end != "")
                    {
                        com.CommandText = $"INSERT INTO Placeprop (Name, DateStart, DateEnd, Citys, Address, Rauon)\r\nVALUES ('{Name.Text}', '{start}', '{end}', '{City.Text}', '{Address.Text}', '{Rauon.Text}'); SELECT SCOPE_IDENTITY();";
                    }
                    else
                    {
                        com.CommandText = $"INSERT INTO Placeprop (Name, DateStart, DateEnd, Citys, Address, Rauon)\r\nVALUES ('{Name.Text}', '{start}', NULL, '{City.Text}', '{Address.Text}', '{Rauon.Text}'); SELECT SCOPE_IDENTITY();";
                    }
                    com.Connection = connection;
                    id = Convert.ToInt32(com.ExecuteScalar());
                    good = 1;
                    addData.Show();
                    this.Close();
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Что-то не правильно ввели попробуйте снова", caption: "Ошибка");
            }
            
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            good = 0;
            this.Close();
            addData.Show();
        }
    }
}
