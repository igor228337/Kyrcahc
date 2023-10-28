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
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public Dictionary<string, string> people;
        private Main? main;
        public string idUser;
        private List<string> table = new List<string>() { "Prizevnik", "Passport", "Meddoc", "Placeprop"};
        private List<string> columns = new List<string>();
        private string connectName = "Data Source=stud-mssql.sttec.yar.ru,38325; Initial Catalog=user209_db; Integrated Security=False; User ID=user209_db;Password = user209";
        public string ConnectName { get { return connectName; } set { connectName = value; } }
        public EditWindow(Main main, string idUser)
        {
            this.idUser = idUser;
            this.main = main;
            InitializeComponent();
            Table.ItemsSource = table;
            Column.IsEnabled = false;
            Edit.IsEnabled = false;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private List<string> GetListColumns(string name)
        {
            List<string> data = new List<string>();
            using (SqlConnection connection = new SqlConnection(ConnectName))
            {
                string str;
                connection.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = $"SELECT name\r\nFROM sys.all_columns\r\nWHERE object_id = OBJECT_ID('{name}')";
                com.Connection = connection;
                SqlDataReader reader = com.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        str = Convert.ToString(reader[0]);
                        data.Add(str);
                    }
                }
                return data;
            }

        }
        private void GetUserAllInfo(string id)
        {
            people = new Dictionary<string, string>()
            {
                { "Prizevnik", id},
                { "Passport", ""},
                { "Meddoc", ""},
                { "Placeprop", ""}
            };
            people["Passport"] = GetData($"SELECT idPassport FROM Passport WHERE idPrizevnik={id}");
            people["Meddoc"] = GetData($"SELECT idmeddoc FROM PassportMeddoc WHERE idpassport={people["Passport"]}");
            people["Placeprop"] = GetData($"SELECT idplaceprop FROM PassportPlaceprop WHERE idpassport={people["Passport"]}");
        }
        private string GetData(string cmd)
        {
            using (SqlConnection connection = new SqlConnection(ConnectName))
            {
                string str = "";
                connection.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = cmd;
                com.Connection = connection;
                SqlDataReader reader = com.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        str = Convert.ToString(reader[0]);
                        connection.Close();
                        return str;
                    }
                }
                connection.Close();
                return str;
            }
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            GetUserAllInfo(idUser);
            if (Value.Text == null || Value.Text == "")
            {
                return;
            }
            bool isNumber = int.TryParse(Value.Text, out int numericValue);
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectName))
                {

                    connection.Open();
                    SqlCommand com = new SqlCommand();
                    if (isNumber)
                    {
                        com.CommandText = $"UPDATE {Table.SelectedValue.ToString()} SET {Column.Text}={numericValue};";
                    }
                    else
                    {
                        com.CommandText = $"UPDATE {Table.SelectedValue.ToString()} SET {Column.Text}='{Value.Text}';";
                    }
                    
                    com.Connection = connection;
                    com.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Что-то пошло не так", caption: "Ошибка");
                return;
            }
            main.FillDataGrid();
            main.Show();
            this.Close();
        }

        private void Table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            columns = GetListColumns($"{Table.SelectedValue.ToString()}");
            Column.ItemsSource = columns;
            Column.IsEnabled = true;
        }

        private void Column_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Column.SelectedValue.ToString() == "")
            {
                Edit.IsEnabled = false;
                return;
            }
            Edit.IsEnabled = true;

        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            main.Show();
        }
    }
}
