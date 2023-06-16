using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CV19.Infrastructure.Commands
{
    public class SaveImageCommand : ICommand
    {
        private readonly BitmapImage _image;

        public SaveImageCommand(BitmapImage image)
        {
            _image = image;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PNG Image|*.png",
                Title = "Save an Image File"
            };
            dialog.ShowDialog();

            if (!string.IsNullOrWhiteSpace(dialog.FileName))
            {
                using (var fileStream = new FileStream(dialog.FileName, FileMode.Create))
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(_image));
                    encoder.Save(fileStream);
                }
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
