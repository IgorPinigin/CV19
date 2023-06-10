using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV19.Models
{
    public class SourcePoint : INotifyPropertyChanged
    {
        private bool _isSelected = true;
        private string _visibility = "Visible";

        public decimal Value { get; set; }
        public Location Location { get; set; }
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

        public SourcePoint(double latitude, double longitude, string name)
        {
            Location = new Location(latitude, longitude);
            Name = name;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
