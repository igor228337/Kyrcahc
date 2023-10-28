using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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
    /// Логика взаимодействия для PassportWindow.xaml
    /// </summary>
    public partial class PassportWindow : Window
    {
        public int id = 0;
        AddDataWindow? addData;
        private string connectName = "Data Source=stud-mssql.sttec.yar.ru,38325; Initial Catalog=user209_db; Integrated Security=False; User ID=user209_db;Password = user209";
        public string ConnectName { get { return connectName; } set { connectName = value; } }
        public int good = 0;
        public PassportWindow(AddDataWindow addData)
        {
            this.addData = addData;
            InitializeComponent();
        }

        private void Passport_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDateBurn = DateBurn.SelectedDate;
            if (Name.Text == "" || Name.Text == null || Familiya.Text == "" || Familiya.Text == null || Otchectvo.Text == "" || Otchectvo.Text == null || Seriya.Text == "" || Seriya.Text == null || Number.Text == "" || Number.Text == null || Code.Text == "" || Code.Text == null || !selectedDateBurn.HasValue)
            {
                MessageBox.Show("Какие-то данные пропущены");
                return;
            }
            try
            {
                bool bseriya = int.TryParse(Seriya.Text, out int seriya);
                bool bnumber = int.TryParse(Number.Text, out int number);
                bool bcode = int.TryParse(Code.Text, out int code);
                string dateburn = selectedDateBurn.Value.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                if (!bseriya || !bnumber || !bcode)
                {
                    MessageBox.Show("Вы ввели не правильные данные паспорта!");
                    return;
                }
                using (SqlConnection connection = new SqlConnection(ConnectName))
                {
                    connection.Open();
                    SqlCommand com = new SqlCommand();
                    com.CommandText = $"INSERT INTO Passport (Name, Otchestvo, Familiya, Seriya, Number, Code, dateBurn)\r\nVALUES ('{Name.Text}', '{Otchectvo.Text}', '{Familiya.Text}', {seriya}, {number}, {code}, '{dateburn}'); SELECT SCOPE_IDENTITY();";
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
