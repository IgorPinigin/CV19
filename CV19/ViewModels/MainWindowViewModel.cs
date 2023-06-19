using CommunityToolkit.Mvvm.ComponentModel;
using CV19.Models;
using CV19.Models.DataModels;
using CV19.Views.Windows;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;

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
        private bool _checkedMin; 
        private List<string> _cities;
        private string _selectedCity;
        private string _textLabel;
        private ObservableCollection<PNZAPoint> _PNZAPoints;
        public ObservableCollection<HeatMapElement> _heatMapElements;
        private ObservableCollection<SourcePoint> _sourcePoints;
        private ObservableCollection<MainPoint> _mainPoints;
        public ICommand NextStepCommand { get; private set; }
        public ICommand CloseApplicationCommand { get; }
        public ICommand ExportDataCommand { get; private set; }
        [ObservableProperty]
        private Visibility _visibilityNextStepButton;
        [ObservableProperty]
        private Visibility _visibleExportData;
        [ObservableProperty]
        private Visibility _visibilityCalculateButton;
        [ObservableProperty]
        private Visibility _visibilityListSources;
        [ObservableProperty]
        private Visibility _visibilitySelectSources;
        [ObservableProperty]
        private Visibility _visibilitySelectPNZA;
        private ICommand _clearMapCommand;
        private ICommand _calculateCommand;
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
            _textLabel = "";
            ExportDataCommand = new Infrastructure.Commands.RelayCommand((Action<object>)ExportData);
            VisibleExportData = Visibility.Hidden;
            VisibilityNextStepButton = Visibility.Hidden;
            VisibilityCalculateButton = Visibility.Hidden;
            VisibilityListSources = Visibility.Hidden;
            VisibilitySelectSources = Visibility.Hidden;
            VisibilitySelectPNZA = Visibility.Hidden;
            _dataModel = new DataModel();
            _cities = _dataModel.GetCities();
            _PNZAPoints = new ObservableCollection<PNZAPoint>();
            _sourcePoints = new ObservableCollection<SourcePoint>();
            _mainPoints = new ObservableCollection<MainPoint>();
            _heatMapElements = new ObservableCollection<HeatMapElement>();
            NextStepCommand = new Infrastructure.Commands.RelayCommand(ShowList);

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
        public bool CheckedMin
        {
            get => _checkedMin;
            set
            {
                _checkedMin = value;
                OnPropertyChanged();
            }
        }
        public string TextLabel
        {
            get => _textLabel;
            set
            {
                _textLabel = value;
                OnPropertyChanged();
            }
        }

        public string SelectedCity
        {
            get => _selectedCity;
            set
            {
                _selectedCity = value;
                _textLabel = "Выберите ПНЗА" as String;
                OnPropertyChanged();
                UpdateMap();
                UpdatePNZAPoints();
                VisibilityNextStepButton = Visibility.Visible;
                VisibilitySelectPNZA = Visibility.Visible;
            }
        }
        private void ShowList(object parameter)
        {
            VisibilityNextStepButton = Visibility.Hidden;
            VisibilityListSources = Visibility.Visible;
            UpdateSourcePoints();
            VisibilityCalculateButton = Visibility.Visible;
            VisibilitySelectSources = Visibility.Visible;
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
        public ObservableCollection<MainPoint> MainPoints
        {
            get => _mainPoints;
            set
            {
                _mainPoints = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<HeatMapElement> HeatMapElements
        {
            get => _heatMapElements;
            set
            {
                _heatMapElements = value;
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
        //private void UpdateMainPoints()
        //{
        //    _mainPoints.Clear();
        //    foreach (var i in PNZAPoints) _mainPoints.Add(new MainPoint(i));
        //}
        private void UpdatePNZAPoints()
        {
            
            List<PNZAPoint> pNZAPoints = _dataModel.GetMonitoringPointsByCity(SelectedCity);
            PNZAPoints.Clear();
            foreach (var point in pNZAPoints)
                PNZAPoints.Add(point);
            _mainPoints.Clear();
            foreach (var i in PNZAPoints) _mainPoints.Add(new MainPoint(i));
        }

        private void UpdateSourcePoints()
        {

            List<PNZAPoint> pNZAPoints = _dataModel.GetSourcePointsByCity(SelectedCity);
            PNZAPoints.Clear();
            foreach (var point in pNZAPoints)
                PNZAPoints.Add(point);
        }

        private void ExportData(object parameter)
        {
            var map = App.Current.MainWindow.FindName("myMap") as Map;
            double width = map.ActualWidth;
            double height = map.ActualHeight;
            RenderTargetBitmap renderTarget = new RenderTargetBitmap((int)width, (int)height, 96, 96, PixelFormats.Pbgra32);
            renderTarget.Render(map);
            BitmapImage bitmapImage = new BitmapImage();
            PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(renderTarget));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                pngEncoder.Save(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
            }
            ExportWindow exportWindow = new ExportWindow(bitmapImage);
            exportWindow.ShowDialog();
        }

        public ICommand ClearMapCommand => _clearMapCommand ?? (_clearMapCommand = new Infrastructure.Commands.RelayCommand(param => ClearMap(), param => CanClearMap()));

        private bool CanClearMap()
        {
            return App.Current.MainWindow.FindName("myMap") is Map map && map.Children.Count > 1;
        }

        private void ClearMap()
        {
            if (App.Current.MainWindow.FindName("myMap") is Map map && map.Children.Count > 1)
            {
                map.Children.RemoveAt(1);
                _heatMapElements.Clear();

            }
        }

        public ICommand CalculateCommand => _calculateCommand ?? (_calculateCommand = new Infrastructure.Commands.RelayCommand(param => Calculate(), param => CanCalculate()));

        private bool CanCalculate()
        {
            var map = App.Current.MainWindow.FindName("myMap") as Map;
            int selectedCount = PNZAPoints.Count(mp => mp.IsSelected == true);
            return !string.IsNullOrEmpty(SelectedCity) && selectedCount >= 2 && map.Children.Count <= 1;
        }
     
        private void Calculate()
        {
            if (CheckedMin)
            {
                _dataModel.GetPollutionValuesMinForPNZA(PNZAPoints);
            }
            else {
                _dataModel.GetPollutionValuesMaxForPNZA(PNZAPoints);
            }

            VisibleExportData = Visibility.Visible;
            var map = App.Current.MainWindow.FindName("myMap") as Map;
            

            List<GridPolygon> polygons = MapModel.DrawZone(MapModel.CalculateZone(PNZAPoints, 10000), 50, PNZAPoints);
            double minValue = polygons.Min(polygon => polygon.Value);
            double maxValue = polygons.Max(polygon => polygon.Value);
            double step = (maxValue - minValue) / 12;
            Color[] colors = new Color[]
            {
                (Color)ColorConverter.ConvertFromString("#0000FF"), // синий               1
                (Color)ColorConverter.ConvertFromString("#0080FF"), // голубой             2
                (Color)ColorConverter.ConvertFromString("#00FFFF"), // ярко-голубой        3 
                (Color)ColorConverter.ConvertFromString("#00FF80"), // зеленый             4
                (Color)ColorConverter.ConvertFromString("#00FF00"), // ярко-зеленый        5
                (Color)ColorConverter.ConvertFromString("#80FF00"), // светло-зеленый      6
                (Color)ColorConverter.ConvertFromString("#FFFF00"), // желтый              7
                (Color)ColorConverter.ConvertFromString("#FF8000"), // оранжевый           8
                (Color)ColorConverter.ConvertFromString("#FF4000"), // ярко-красный        9
                (Color)ColorConverter.ConvertFromString("#FF0000"), // красный             10
                (Color)ColorConverter.ConvertFromString("#800000"), // темно-красный       11
                (Color)ColorConverter.ConvertFromString("#FF0080")  // розовый             12
            };

            for (int i = 0; i < polygons.Count; i++)
            {
                double value = polygons[i].Value;
                int colorIndex = (int)Math.Floor((value - minValue) / (maxValue - minValue) * (colors.Length - 1));
                polygons[i].Color = new SolidColorBrush(colors[colorIndex]);
            }
            _heatMapElements.Clear();
            int o = 0;
            for (int i = 0; i < 12; i++)
                _heatMapElements.Add(new HeatMapElement());
            foreach (var item in HeatMapElements)
            {
                item.Color = colors[o];
                item.MinValue = minValue + (step * o);
                item.MaxValue = minValue + (step * (o + 1));
                o++;
            }

            List<GridPolygon> horizontalOptimize = new List<GridPolygon>();
            GridPolygon previous = polygons[0];
            for (int i = 1; i < polygons.Count; i++)
            {
                GridPolygon current = polygons[i];
                if (previous.Color.ToString() == current.Color.ToString()
                    && previous.Center.Latitude == current.Center.Latitude)
                {
                    previous.RightDown = current.RightDown;
                    previous.RightUp = current.RightUp;
                }
                else
                {
                    horizontalOptimize.Add(previous);
                    previous = new GridPolygon(current.LeftUp, current.LeftDown, current.RightDown, current.RightUp)
                    {
                        Color = current.Color
                    };
                }
            }
            horizontalOptimize.Add(previous);
            MapLayer layer = new MapLayer();
            foreach (GridPolygon polygon in horizontalOptimize)
            {
                LocationCollection locations = new LocationCollection()
                {
                    new Location(polygon.LeftUp.Latitude, polygon.LeftUp.Longitude),
                    new Location(polygon.LeftDown.Latitude, polygon.LeftDown.Longitude),
                    new Location(polygon.RightDown.Latitude, polygon.RightDown.Longitude),
                    new Location(polygon.RightUp.Latitude, polygon.RightUp.Longitude),
                };
                MapPolygon mapPolygon = new MapPolygon
                {
                    Opacity = 0.5,
                    Fill = polygon.Color,
                    Locations = locations,
                };
                layer.Children.Add(mapPolygon);
            }
            map.Children.Add(layer);
            horizontalOptimize.Clear();
            polygons.Clear();
        }

        public static implicit operator ResourceDictionary(MainWindowViewModel v)
        {
            throw new NotImplementedException();
        }
    }
}