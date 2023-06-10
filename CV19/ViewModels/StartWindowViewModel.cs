using CV19.Infrastructure.Commands;
using CV19.Views.Windows;
using System.Windows;
using System.Windows.Input;

namespace CV19.ViewModels
{
    internal class StartWindowViewModel
    {
        public ICommand ShowDataCommand { get; private set; } // VisualizationCommand
        public ICommand VisualizationCommand { get; private set; }

        public StartWindowViewModel()
        {
            ShowDataCommand = new RelayCommand(ShowData);
            VisualizationCommand = new RelayCommand(ShowVisualization);
        }

        private void ShowData(object parameter)
        {
            DataWindow dataWindow = new DataWindow();

            // Закрываем текущее окно
            Application.Current.MainWindow.Close();

            // Устанавливаем новое окно как главное
            Application.Current.MainWindow = dataWindow;

            // Показываем новое окно
            dataWindow.ShowDialog();
        }

        private void ShowVisualization(object parameter)
        {
            MainWindow mainWindow = new MainWindow();

            // Закрываем текущее окно
            Application.Current.MainWindow.Close();

            // Устанавливаем новое окно как главное
            Application.Current.MainWindow = mainWindow;

            // Показываем новое окно
            mainWindow.ShowDialog();
        }
    }
}
