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
    /// Логика взаимодействия для PrizevnikWindow.xaml
    /// </summary>
    public partial class PrizevnikWindow : Window
    {
        private MedDocWindow medDocWindow;
        private PlaceWindow place;
        private PassportWindow passport;
        private AddDataWindow addDataWindow;
        Main main;
        Dictionary<int, string> keyValuePairs = new Dictionary<int, string>() { [0] = "Годен", [1] = "Не годен", [2] = "Осуждён"};
        private string connectName = "Data Source=stud-mssql.sttec.yar.ru,38325; Initial Catalog=user209_db; Integrated Security=False; User ID=user209_db;Password = user209";
        public string ConnectName { get { return connectName; } set { connectName = value; } }
        public PrizevnikWindow(MedDocWindow medDocWindow, PlaceWindow place, PassportWindow passport, Main main, AddDataWindow addDataWindow)
        {
            this.main = main;
            this.medDocWindow = medDocWindow;
            this.place = place;
            this.passport = passport;
            this.addDataWindow = addDataWindow;
            InitializeComponent();
        }
        private void CreateConnectionDB(string cmd)
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
                MessageBox.Show("Что-то пошло не так", caption: "Ошибка");
                return;
            }
        }
        private void CreatePrizevnik_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            if (!Email.Text.Contains("@") || !Email.Text.Contains("."))
            {

            }
            if (Email.Text == "" || Email.Text == null || Phone.Text == "" || Phone.Text == null || keyValuePairs[Status.SelectedIndex] == "" || keyValuePairs[Status.SelectedIndex] == null) 
            {
                MessageBox.Show("Какие-то данные упущены", "Ошибка");
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectName))
                {

                    connection.Open();
                    SqlCommand com = new SqlCommand();
                    com.CommandText = $"INSERT INTO Prizevnik (Email, Phone, Status)\r\nVALUES ('{Email.Text}', '{Phone.Text}', '{keyValuePairs[Status.SelectedIndex]}'); ; SELECT SCOPE_IDENTITY();";
                    com.Connection = connection;
                    id = Convert.ToInt32(com.ExecuteScalar());
                    connection.Close();
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Что-то не правильно ввели попробуйте снова", caption: "Ошибка");
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectName))
                {

                    connection.Open();
                    SqlCommand com = new SqlCommand();
                    com.CommandText = $"UPDATE Passport SET idPrizevnik={id}";
                    com.Connection = connection;
                    com.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Что-то не правильно ввели попробуйте снова", caption: "Ошибка");
                return;
            }
            CreateConnectionDB($"INSERT INTO PassportMeddoc (idpassport, idmeddoc)\r\nVALUES ({passport.id}, {medDocWindow.id})");
            CreateConnectionDB($"INSERT INTO PassportPlaceprop (idpassport, idplaceprop)\r\nVALUES ({passport.id}, {place.id})");
            main.FillDataGrid();
            main.Show();
            addDataWindow.Close();
            this.Close();
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            addDataWindow.Show();
        }
    }
}
