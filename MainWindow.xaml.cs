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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kyrsach
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connectName = "Data Source=stud-mssql.sttec.yar.ru,38325; Initial Catalog=user209_db; Integrated Security=False; User ID=user209_db;Password = user209";
        public string ConnectName { get { return connectName; } set { connectName = value; } }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Entry_Click(object sender, RoutedEventArgs e)
        {
            if (GetDataDB($"SELECT * FROM AuthArm WHERE Login='{Login.Text}' AND Password='{Password.Text}'"))
            {
                MessageBox.Show("Вы вошли!");
                Main main = new Main();
                main.Show();
                this.Close();
            }
            else 
            {
                MessageBox.Show("Вы ввели не правильно логин или пароль!");
            }
            
        }
        private bool GetDataDB(string cmd)
        {
            using (SqlConnection connection = new SqlConnection(ConnectName))
            {
                connection.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = cmd;
                com.Connection = connection;
                SqlDataReader reader = com.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        connection.Close();
                        return true;
                    }
                }
                connection.Close();
                return false;
            }
        }
    }
}
