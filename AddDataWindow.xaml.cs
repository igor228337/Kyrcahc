using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AddDataWindow.xaml
    /// </summary>
    public partial class AddDataWindow : Window
    {
        MedDocWindow? medDocWindow;
        PlaceWindow? place;
        PassportWindow? passport;
        PrizevnikWindow? prizevnik;
        Main main;
        private string connectName = "Data Source=stud-mssql.sttec.yar.ru,38325; Initial Catalog=user209_db; Integrated Security=False; User ID=user209_db;Password = user209";
        public string ConnectName { get { return connectName; } set { connectName = value; } }
        public AddDataWindow(Main main)
        {
            this.main = main;
            InitializeComponent();
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            this.main.Show();
            this.Close();
        }

        private void MedDocBTN_Click(object sender, RoutedEventArgs e)
        {
            medDocWindow = new MedDocWindow(this);
            medDocWindow.Show();
            this.Hide();
        }

        private void PlaceBTN_Click(object sender, RoutedEventArgs e)
        {
            place = new PlaceWindow(this);
            place.Show();
            this.Hide();
            
        }

        private void PassportBTN_Click(object sender, RoutedEventArgs e)
        {
            passport = new PassportWindow(this);
            passport.Show();
            this.Hide();
            
        }

        private void PrizevnikBTN_Click(object sender, RoutedEventArgs e)
        {
            if (passport.good == 0)
            {
                MessageBox.Show("Вы не указали паспорт");
                return;
            }
            else if (place.good == 0)
            {
                MessageBox.Show("Вы не указали место прописки");
                return;
            }
            else if (medDocWindow.good == 0)
            {
                MessageBox.Show("Вы не указали мед документ");
                return;
            }
            prizevnik = new PrizevnikWindow(medDocWindow, place, passport, main, this);
            prizevnik.Show();
            this.Close();
        }
    }
}
