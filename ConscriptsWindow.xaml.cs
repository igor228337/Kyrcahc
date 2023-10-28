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
    /// Логика взаимодействия для ConscriptsWindow.xaml
    /// </summary>
    public partial class ConscriptsWindow : Window
    {
        private string connectName = "Data Source=stud-mssql.sttec.yar.ru,38325; Initial Catalog=user209_db; Integrated Security=False; User ID=user209_db; Password = user209";
        public string ConnectName { get { return connectName; } set { connectName = value; } }
        private string quereGetData = "SELECT * FROM Prizevnik WHERE Status='Годен'";
        public ConscriptsWindow()
        {
            InitializeComponent();
            FillDataGrid();
        }

        private void GridCon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
                GridCon.ItemsSource = dataTable.DefaultView;
                connection.Close();
            }
        }
    }
}
