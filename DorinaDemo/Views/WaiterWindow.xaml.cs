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
    /// Логика взаимодействия для CookAndWaiterWindow.xaml
    /// </summary>
    public partial class WaiterWindow : Window
    {

        private readonly DorinaContext _context;
        private User user;

        public WaiterWindow(User user)
        {
            InitializeComponent();
            _context = new DorinaContext();
            LoadOrders();
            this.user = user;
        }

        private void LoadOrders()
        {
            var orders = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Where(o => o.Date.Date == DateTime.Today)
                .ToList();

            OrdersGrid.ItemsSource = orders;
        }

        private void ButtonAccept_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem != null && OrdersGrid.SelectedItem is Order selectedOrder)
            {
                selectedOrder.Status = "Принят";
                _context.SaveChanges();
                LoadOrders();
                OrdersGrid.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Выберите заказ для изменения статуса.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ButtonPaid_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem != null && OrdersGrid.SelectedItem is Order selectedOrder)
            {
                selectedOrder.Status = "Оплачен";
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

        private void ButtonNewOrder_Click(object sender, RoutedEventArgs e)
        {
            NewOrderWindow newOrderWindow = new NewOrderWindow(user);
            newOrderWindow.ShowDialog();
            LoadOrders();
        }

    }

   
}
