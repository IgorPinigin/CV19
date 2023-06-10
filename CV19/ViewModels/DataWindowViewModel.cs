using CV19.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CV19.Models.DataModels;
using CV19.Views;
using CV19.Views.Windows;

namespace CV19.ViewModels
{
    internal class DataWindowViewModel
    {
            public ICommand AddDataCommand { get; private set; }
            private readonly DataModel _dataModel;
            public DataWindowViewModel()
            {
                AddDataCommand = new RelayCommand(AddData);
                _dataModel = new DataModel();
            }
            private void AddData(object parameter)
            {
                NewRowDataWindow addWindow = new NewRowDataWindow();
                addWindow.ShowDialog();
            }
            private DataTable _dataCity;
            public DataTable DataCity
            {
                get { DataCity = _dataModel.LoadDataCity(); return _dataCity; }
                set
                {

                _dataCity = value;
                    OnPropertyChanged(nameof(DataCity));
                }
            }
        private DataTable _dataPNZA;
        public DataTable DataPNZA
        {
            get { DataPNZA = _dataModel.LoadDataPNZA(); return _dataPNZA; }
            set
            {

                _dataPNZA = value;
                OnPropertyChanged(nameof(DataPNZA));
            }
        }
        private DataTable _dataSource;
        public DataTable DataSource
        {
            get { DataSource = _dataModel.LoadDataSource(); return _dataSource; }
            set
            {

                _dataSource = value;
                OnPropertyChanged(nameof(DataSource));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
}
