using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
                SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[Точечные источники]", connection);
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
         public List<SourcePoint> GetSourcePointsByCity(string city)
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
                var sourcePoints = new List<SourcePoint>();
                while (reader.Read())
                {
                    double latitude = Convert.ToDouble(reader["Lattitude"]);
                    double longitude = Convert.ToDouble(reader["Longitude"]);
                    string name = reader["Название"].ToString();

                    sourcePoints.Add(new SourcePoint(latitude, longitude, name));
                }
                return sourcePoints;
            }
        }
    }
}
