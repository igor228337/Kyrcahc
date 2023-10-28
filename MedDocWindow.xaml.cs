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
    /// Логика взаимодействия для MedDocWindow.xaml
    /// </summary>
    public partial class MedDocWindow : Window
    {
        AddDataWindow addData;
        public int id;
        private string connectName = "Data Source=stud-mssql.sttec.yar.ru,38325; Initial Catalog=user209_db; Integrated Security=False; User ID=user209_db;Password = user209";
        public string ConnectName { get { return connectName; } set { connectName = value; } }
        public int good = 0;
        public MedDocWindow(AddDataWindow addData)
        {
            this.addData = addData;
            InitializeComponent();
        }

        private void MeddocBTN_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "" || Name.Text == null || Description.Text == "" || Description.Text == null)
            {
                MessageBox.Show("Какие-то данные пропущены");
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectName))
                {

                    connection.Open();
                    SqlCommand com = new SqlCommand();
                    com.CommandText = $"INSERT INTO Meddoc (Name, Description)\r\nVALUES ('{Name.Text}', '{Description.Text}'); SELECT SCOPE_IDENTITY();";
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
