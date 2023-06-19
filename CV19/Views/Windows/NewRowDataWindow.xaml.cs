using CV19.Services;
using CV19.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
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

namespace CV19.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для NewRowDataWindow.xaml
    /// </summary>
    public partial class NewRowDataWindow : Window
    {
        private readonly string ConnectionString = CV19.Services.ConnectionString.connStr;
        public NewRowDataWindow()
        {
            InitializeComponent();
            DataContext = new NewRowDataWindowViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddValues(IDtb.Text, Nametb.Text, Latitudetb.Text, Longitudetb.Text, CityIDtb.Text);
        }

        public void AddValues(string id, string name, string latitude, string longitude, string cityId)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("INSERT INTO [dbo].[Точечные источники] VALUES (@ID, @Name, @Latitude, @Longitude, @CityID, NULL, NULL)", connection);
                    command.CommandType = CommandType.Text;
                    int Id = Convert.ToInt32(id);
                    int CityId = Convert.ToInt32(cityId);
                    command.Parameters.AddWithValue("@ID", Id);
                    command.Parameters.AddWithValue("@Name", name);
                    double Latitude = Convert.ToDouble(latitude);
                    command.Parameters.AddWithValue("@Latitude", Latitude);
                    double Longitude = Convert.ToDouble(longitude);
                    command.Parameters.AddWithValue("@Longitude", Longitude);
                    command.Parameters.AddWithValue("@CityID", CityId);
                    command.ExecuteNonQuery();
                    connection.Close();
                }


            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
