using CV19.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CV19.ViewModels
{
    public class ExportViewModel : INotifyPropertyChanged
    {
        private ICommand _saveImageCommand;
        private BitmapImage _image;

        public ExportViewModel(BitmapImage bitmapImage)
        {
            Image = bitmapImage;
        }

        public ICommand SaveImageCommand
        {
            get
            {
                return _saveImageCommand ?? (_saveImageCommand = new SaveImageCommand(_image));
            }
        }

        public BitmapImage Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
