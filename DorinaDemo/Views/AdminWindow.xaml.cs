using DorinaDemo.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
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

namespace DorinaDemo.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void EmployeeManagementButton_Click(object sender, RoutedEventArgs e)
        {
            Views.StaffWindow staffWindow = new Views.StaffWindow();
            staffWindow.Show();


        }

        private void OrderManagementButton_Click(object sender, RoutedEventArgs e)
        {
            Views.OrdersWindow ordersWindow = new Views.OrdersWindow();
            ordersWindow.Show();


        }

        private void ShiftManagementButton_Click(object sender, RoutedEventArgs e)
        {
            Views.ShiftsWindow shiftsWindow = new Views.ShiftsWindow();
            shiftsWindow.Show();
   

        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();

        }
    }
}
