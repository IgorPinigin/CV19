using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CV19
{
    class Program
    {
        [STAThread]
        static void Main(string[] args) { 
        var app = new App();
            app.Run();
            app.InitializeComponent();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).ConfigureServices(App.ConfigureServices); 
    }
}
