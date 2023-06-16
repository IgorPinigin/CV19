using Geocoding.Microsoft.Json;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV19.Models
{
    public class MainPoint : INotifyPropertyChanged
    {
        private bool _isSelected = true;
        private string _visibility = "Visible";

        public decimal Value { get; set; }
        public MapControl.Location Location { get; set; }
        public string Name { get; set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                    if (_isSelected)
                    {
                        Visibility = "Visible";
                    }
                    else
                    {
                        Visibility = "Hidden";
                    }
                }
            }
        }

        public string Visibility
        {
            get { return _visibility; }
            set
            {
                if (value != _visibility)
                {
                    _visibility = value;
                    OnPropertyChanged("Visibility");
                }
            }
        }

        public MainPoint(PNZAPoint pNZAPoint)
        {
            Location = new MapControl.Location(pNZAPoint.Location.Latitude, pNZAPoint.Location.Longitude);
            Name = pNZAPoint.Name;
        }

        public MainPoint(SourcePoint source)
        {
            Location = new MapControl.Location(source.Location.Latitude, source.Location.Longitude);
            Name = source.Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
