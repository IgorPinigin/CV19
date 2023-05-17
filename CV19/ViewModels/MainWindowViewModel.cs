using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CV19.Infrastructure.Commands;
using CV19.ViewModels.Base;
using MapControl;
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
        public ICommand CloseApplicationCommand { get; }
        private bool CanCloseApplicationCommandExecute(object p) => true;
        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }
        #endregion

        [ObservableProperty]
        private MapProjection _currentProjection = null!;

        [ObservableProperty]
        private IMapLayer _currentLayer = null!;

        [ObservableProperty]
        private Location _mapCenter = null!;
        
        [ObservableProperty]
        private IEnumerable _placeNpz;
        
        [ObservableProperty]
        private Thickness _marginCheckBox1;

        [ObservableProperty]
        private Thickness _marginCheckBox2;

        [ObservableProperty]
        private Thickness _marginCheckBox3;

        [ObservableProperty]
        private Thickness _marginCheckBox4;

        [ObservableProperty]
        private Thickness _marginCheckBox5;

        [ObservableProperty]
        private Visibility _visibilityCheckBox1;

        [ObservableProperty]
        private Visibility _visibilityCheckBox2;

        [ObservableProperty]
        private Visibility _visibilityCheckBox3;

        [ObservableProperty]
        private Visibility _visibilityCheckBox4;
        
        [ObservableProperty]
        private Visibility _visibilityCheckBox5;

        [ObservableProperty]
        private String _contentCheckBox1;

        [ObservableProperty]
        private String _contentCheckBox2;

        [ObservableProperty]
        private String _contentCheckBox3;

        [ObservableProperty]
        private String _contentCheckBox4;

        [ObservableProperty]
        private String _contentCheckBox5;

        [ObservableProperty]
        private int _selectedTown;

        public string Center => MapCenter?.ToString();


        partial void OnSelectedTownChanged(int value)
        {
            switch (value)
            {
                case 0:
                    MapCenter = new Location(52.53, 103.91); // Ангарск
                    MarginCheckBox1 = new Thickness(-300.0, 210.0, 0.0, 0.0);
                    MarginCheckBox2 = new Thickness(-295.0, 150.0, 0.0, 0.0);
                    MarginCheckBox3 = new Thickness(-305.0, 110.0, 0.0, 0.0);
                    MarginCheckBox4 = new Thickness(-390.0, 120.0, 0.0, 0.0);
                    MarginCheckBox5 = new Thickness(0.0, 0.0, 0.0, 0.0);
                    VisibilityCheckBox1 = Visibility.Visible;
                    VisibilityCheckBox2 = Visibility.Visible;
                    VisibilityCheckBox3 = Visibility.Visible;
                    VisibilityCheckBox4 = Visibility.Visible;
                    VisibilityCheckBox5 = Visibility.Hidden;
                    ContentCheckBox1 = new string("ПНЗА №27");
                    ContentCheckBox2 = new string("ПНЗА №41");
                    ContentCheckBox3 = new string("ПНЗА №25");
                    ContentCheckBox4 = new string("ПНЗА №26");

                    break;
                case 1:
                    MapCenter = new Location(52.76,103.66); // Усолье-Сибирское
                    MarginCheckBox1 = new Thickness(0.0, 0.0, 0.0, 0.0);
                    MarginCheckBox2 = new Thickness(0.0, 0.0, 0.0, 0.0);
                    MarginCheckBox3 = new Thickness(0.0, 0.0, 0.0, 0.0);
                    MarginCheckBox4 = new Thickness(-165.0, 450.0, 0.0, 0.0);
                    MarginCheckBox5 = new Thickness(110.0, 80.0, 0.0, 0.0);
                    VisibilityCheckBox1 = Visibility.Hidden;
                    VisibilityCheckBox2 = Visibility.Hidden;
                    VisibilityCheckBox3 = Visibility.Hidden;
                    VisibilityCheckBox4 = Visibility.Visible;
                    VisibilityCheckBox5 = Visibility.Visible;
                    ContentCheckBox4 = new string("ПНЗА №4");
                    ContentCheckBox5 = new string("ПНЗА №5");
                    break;
                case 2:
                    MapCenter = new Location(52.27, 104.3); // Иркутск
                    MarginCheckBox1 = new Thickness(320.0, 280.0, 0.0, 0.0);
                    MarginCheckBox2 = new Thickness(-150.0, 20.0, 0.0, 0.0);
                    MarginCheckBox3 = new Thickness(350.0, 40.0, 0.0, 0.0);
                    MarginCheckBox4 = new Thickness(-1000.0, 230.0, 0.0, 0.0);
                    MarginCheckBox5 = new Thickness(-10.0, 100.0, 0.0, 0.0);
                    VisibilityCheckBox1 = Visibility.Hidden;
                    VisibilityCheckBox2 = Visibility.Hidden;
                    VisibilityCheckBox3 = Visibility.Visible;
                    VisibilityCheckBox4 = Visibility.Visible;
                    VisibilityCheckBox5 = Visibility.Visible;
                    ContentCheckBox1 = new string("ПНЗА №21");
                    ContentCheckBox2 = new string("ПНЗА №2");
                    ContentCheckBox3 = new string("АВИАЦИОННЫЙ ЗАВОД");
                    ContentCheckBox4 = new string("НОВО-ИРКУСТКАЯ ГЭС");
                    ContentCheckBox5 = new string("ЗАВОД ТЯЖЕЛОГО МАШИНОСТРОЕНИЯ");
                    break;
            }

            OnPropertyChanged(nameof(Center));
        }


        public MainWindowViewModel()
        {
            CurrentLayer = new WmsImageLayer
            {
                ServiceUri = new Uri("http://ows.terrestris.de/osm/service"),
                Layers = "OSM-WMS"
            };
        }

        public static implicit operator ResourceDictionary(MainWindowViewModel v)
        {
            throw new NotImplementedException();
        }
    }
}