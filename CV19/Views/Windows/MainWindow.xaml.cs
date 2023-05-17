using CV19.ViewModels;
using MapControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace CV19
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel viewModel = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
            
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            DataContext = viewModel;
        }
    }

    public class ViewModel : INotifyPropertyChanged
    {
        private MapProjection currentProjection;
        private IMapLayer currentLayer;
        private Location pushpinLocation = new();

        public event PropertyChangedEventHandler PropertyChanged;

        public List<MapProjection> Projections { get; } = new List<MapProjection>();

        public Dictionary<string, IMapLayer> Layers { get; } = new Dictionary<string, IMapLayer>();

        public MapProjection CurrentProjection
        {
            get => currentProjection;
            set
            {
                currentProjection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentProjection)));
            }
        }

        public IMapLayer CurrentLayer
        {
            get => currentLayer;
            set
            {
                currentLayer = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLayer)));
            }
        }
    }
}