using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private string infoFormat = "Фамилия: {0}\nИмя: {1}\nОтчетсво: {2}\nТелефон: {3}\nEmail: {4}\nСтатус: {5}\nДата рождения: {6}\nНомер паспорта: {7}\nСерия: {8}";
        private string quereGetData = "SELECT * FROM Prizevnik";
        private string connectName = "Data Source=stud-mssql.sttec.yar.ru,38325; Initial Catalog=user209_db; Integrated Security=False; User ID=user209_db; Password = user209";
        public string ConnectName { get { return connectName; } set { connectName = value; } }

        public Main()
        {
            InitializeComponent();
            FillDataGrid();
        }

        public void FillDataGrid()
        {
            using (SqlConnection connection = new SqlConnection(ConnectName))
            {
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(quereGetData, ConnectName);
                SqlCommandBuilder command = new SqlCommandBuilder(da);
                DataTable dataTable = new DataTable();
                da.Fill(dataTable);
                MainDB.ItemsSource = dataTable.DefaultView;
                connection.Close();
            }
        }

        private void AddDeleteDataRow(string cmd)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectName))
                {
                    connection.Open();
                    SqlCommand com = new SqlCommand();
                    com.CommandText = cmd;
                    com.Connection = connection;
                    com.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Что-то пошло не так в удалении-добавлении");
            }
        }

        private List<string> FindData(int index)
        {
            List<string> list = new List<string>();
            if (MainDB.SelectedItems.Count > 0)
            {
                for (int i = 0; i < MainDB.SelectedItems.Count; i++)
                {
                    DataRowView selectedFile = (DataRowView)MainDB.SelectedItems[i];
                    string str = Convert.ToString(selectedFile.Row.ItemArray[index]);
                    list.Add(str);
                }
            }
            return list;
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
                        return str;
                    }
                }
                return str;
            }
        }
        
        private void ViewInfo(int id)
        {
            string email = GetData($"SELECT Email FROM Prizevnik WHERE id={id};");
            string phone = GetData($"SELECT Phone FROM Prizevnik WHERE id={id};");
            string status = GetData($"SELECT Status FROM Prizevnik WHERE id={id}");
            string passportName = GetData($"SELECT Passport FROM Prizevnik WHERE id={id}");
            string name = GetData($"SELECT Name FROM Passport WHERE idPassport={passportName}");
            string familiya = GetData($"SELECT Familiya FROM Passport WHERE idPassport={passportName}");
            string Otchectvo = GetData($"SELECT Otchestvo FROM Passport WHERE idPassport={passportName}");
            string dataBorn = GetData($"SELECT dateBurn FROM Passport WHERE idPassport={passportName}");
            string numberPassport = GetData($"SELECT Number FROM Passport WHERE idPassport={passportName}");
            string series = GetData($"SELECT Seriya FROM Passport WHERE idPassport={passportName}");
            MessageBox.Show(string.Format(infoFormat, familiya, name, Otchectvo, phone, email, status, dataBorn, numberPassport, series), "Информация");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            List<string> strs = FindData(4);
            foreach (string item in strs)
            {
                AddDeleteDataRow($"DELETE FROM Prizevnik WHERE id={int.Parse(item)};");
            }
            FillDataGrid();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            List<string> strs = FindData(4);
            if (strs.Count == 1)
            {
                EditWindow editWindow = new EditWindow(this, strs[0]);
                editWindow.Show();
                this.Hide();
                return;
            }
            MessageBox.Show("Нужно выбрать одного", "Внимание!");  
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            List<string> strs = FindData(4);
            foreach (string item in strs)
            {
                ViewInfo(int.Parse(item));
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddDataWindow addDataWindow = new AddDataWindow(this);
            this.Hide();
            addDataWindow.ShowDialog();
        }

        private void conscripts_Click(object sender, RoutedEventArgs e)
        {
            ConscriptsWindow conscriptsWindow = new ConscriptsWindow();
            conscriptsWindow.ShowDialog();
        }
    }
}
