﻿using CV19.Services;
using CV19.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CV19.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для NewRowDataWindow.xaml
    /// </summary>
    public partial class NewRowDataWindow : Window
    {

        public NewRowDataWindow()
        {
            InitializeComponent();
            DataContext = new NewRowDataWindowViewModel();
        } 
    }
}
