using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Identity.Client;
using static Azure.Core.HttpHeader;
using System.Windows;

namespace CV19.Models.DataModels
{
    internal class DataModel
    {
        private readonly string ConnectionString = CV19.Services.ConnectionString.connStr;
        
        public List<string> GetCities()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Name FROM [Город]", connection);
                var reader = command.ExecuteReader();
                var cities = new List<string>();
                while (reader.Read())
                {
                    cities.Add(reader.GetString(0));
                }
                return cities;
            }
        }
        public DataTable LoadDataCity()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM [Город]", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
        public DataTable LoadDataPNZA()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM [ПНЗА]", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
        public DataTable LoadDataSource()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT (ID),([Название]) FROM [dbo].[Точечные источники]", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
        public List<PNZAPoint> GetMonitoringPointsByCity(string city)
        {
            int city1 = 0;
            switch (city) {
                case "Иркутск":
                    city1 = 1; break;
                case "Ангарск":
                    city1 = 2; break;
                case "Черемхово":
                    city1 = 3; break;
                case "Усолье-Сибирское":
                    city1 = 4; break;
                case "Шелехов":
                    city1 = 5; break;

            }
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT DISTINCT [Название], Lattitude, Longitude FROM [ПНЗА] WHERE [ГородID]=@City", connection);
                command.Parameters.AddWithValue("@City", city1);
                var reader = command.ExecuteReader();
                var pNZAPoints = new List<PNZAPoint>();
                while (reader.Read())
                {
                    double latitude = Convert.ToDouble(reader["Lattitude"]);
                    double longitude = Convert.ToDouble(reader["Longitude"]);
                    string name = reader["Название"].ToString();

                    pNZAPoints.Add(new PNZAPoint(latitude, longitude, name));
                }
                return pNZAPoints;
            }
        }
         public List<PNZAPoint> GetSourcePointsByCity(string city)
        {
            int city1 = 0;
            switch (city) {
                case "Иркутск":
                    city1 = 1; break;
                case "Ангарск":
                    city1 = 2; break;
                case "Черемхово":
                    city1 = 3; break;
                case "Усолье-Сибирское":
                    city1 = 4; break;
                case "Шелехов":
                    city1 = 5; break;

            }
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT DISTINCT [Название], Lattitude, Longitude FROM [dbo].[Точечные источники] WHERE [ГородID]=@City", connection);
                command.Parameters.AddWithValue("@City", city1);
                var reader = command.ExecuteReader();
                var pNZAPoints = new List<PNZAPoint>();
                while (reader.Read())
                {
                    double latitude = Convert.ToDouble(reader["Lattitude"]);
                    double longitude = Convert.ToDouble(reader["Longitude"]);
                    string name = reader["Название"].ToString();

                    pNZAPoints.Add(new PNZAPoint(latitude, longitude, name));
                }
                return pNZAPoints;
            }
            
        }
        public void AddValues(string id, string name, string latitude, string longitude, string cityId)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("INSERT INTO [dbo].[Точечные источники] VALUES (@ID, @Name, @Latitude, @Longitude, @CityID)", connection);
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

        internal void GetPollutionValuesMinForPNZA(ObservableCollection<PNZAPoint> pNZAPoints)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                foreach (var point in pNZAPoints)
                {
                    var command = new SqlCommand("SELECT * FROM [dbo].[Точечные источники] WHERE [Название] =@Name", connection);
                    command.Parameters.AddWithValue("@Name", point.Name);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        point.Value = Convert.ToDecimal(reader[5]);
                    }
                    reader.Close();
                }
            }
        }

        internal void GetPollutionValuesMaxForPNZA(ObservableCollection<PNZAPoint> pNZAPoints)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                foreach (var point in pNZAPoints)
                {
                    var command = new SqlCommand("SELECT * FROM [dbo].[Точечные источники] WHERE [Название] =@Name", connection);
                    command.Parameters.AddWithValue("@Name", point.Name);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        point.Value = Convert.ToDecimal(reader[6]);
                    }
                    reader.Close();
                }
            }
        }
    }
}
