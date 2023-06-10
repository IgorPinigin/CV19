using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CV19.Infrastructure.Commands;
using CV19.Models;
using CV19.Models.DataModels;
using CV19.ViewModels.Base;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CV19.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        #region Заголовок окна


        #endregion
        #region Статус программы

        #endregion
        #region Команды 
        private readonly DataModel _dataModel;
        private List<string> _cities;
        private string _selectedCity;
        private ObservableCollection<PNZAPoint> _PNZAPoints;
        private ObservableCollection<SourcePoint> _sourcePoints;
        public ICommand CloseApplicationCommand { get; }
        private bool CanCloseApplicationCommandExecute(object p) => true;
        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }
        #endregion



        
        [ObservableProperty]
        private IEnumerable _placeNpz;
        

        public MainWindowViewModel()
        {
            _dataModel = new DataModel();
            _cities = _dataModel.GetCities();
            _PNZAPoints = new ObservableCollection<PNZAPoint>();
            _sourcePoints = new ObservableCollection<SourcePoint>();

        }
        public List<string> Cities
        {
            get => _cities;
            set
            {
                _cities = value;
                OnPropertyChanged();
            }
        }
        public string SelectedCity
        {
            get => _selectedCity;
            set
            {
                _selectedCity = value;
                OnPropertyChanged();
                UpdateMap();
                UpdatePNZAPoints();
                UpdateSourcePoints();
            }
        }
        private async void UpdateMap()
        {
            try
            {
                var coordinates = await GeoCodingModel.GetCityCoordinates(SelectedCity);
                var location = new Location(coordinates.Item1, coordinates.Item2);
                var pushpin = new Pushpin();
                MapLayer.SetPosition(pushpin, location);
                var map = App.Current.MainWindow.FindName("myMap") as Map;
                map.Center = location;
                map.ZoomLevel = 12;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public ObservableCollection<PNZAPoint> PNZAPoints
        {
            get => _PNZAPoints;
            set
            {
                _PNZAPoints = value;
                OnPropertyChanged();
                
            }
        }
        public ObservableCollection<SourcePoint> SourcePoints
        {
            get => _sourcePoints;
            set
            {
                _sourcePoints = value;
                OnPropertyChanged();

            }
        }
        private void UpdatePNZAPoints()
        {
            List<PNZAPoint> pNZAPoints = _dataModel.GetMonitoringPointsByCity(SelectedCity);
            PNZAPoints.Clear();
            foreach (var point in pNZAPoints)
                PNZAPoints.Add(point);
        }

        private void UpdateSourcePoints()
        {
            List<SourcePoint> sourcePoints = _dataModel.GetSourcePointsByCity(SelectedCity);
            SourcePoints.Clear();
            foreach (var point in sourcePoints)
                SourcePoints.Add(point);
        }

        public static implicit operator ResourceDictionary(MainWindowViewModel v)
        {
            throw new NotImplementedException();
        }
    }
}