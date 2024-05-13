using DorinaDemo.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Логика взаимодействия для ChefWindow.xaml
    /// </summary>
    public partial class ChefWindow : Window
    {
        private readonly DorinaContext _context;

        public ChefWindow()
        {
            InitializeComponent();
            _context = new DorinaContext();
            LoadOrders();
        }

        private void LoadOrders()
        {
            var orders = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Where(o => o.Status == "Принят" || o.Status == "Готовится")
                .ToList();

            OrdersGrid.ItemsSource = orders;
        }

        private void ButtonPrepare_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem != null && OrdersGrid.SelectedItem is Order selectedOrder)
            {
                selectedOrder.Status = "Готовиться";
                _context.SaveChanges();
                LoadOrders();
                OrdersGrid.Items.Refresh();
            } else
            {
                MessageBox.Show("Выберите заказ для изменения статуса.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ButtonReady_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem != null && OrdersGrid.SelectedItem is Order selectedOrder)
            {
                selectedOrder.Status = "Готов";
                _context.SaveChanges();
                LoadOrders();
                OrdersGrid.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Выберите заказ для изменения статуса.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
