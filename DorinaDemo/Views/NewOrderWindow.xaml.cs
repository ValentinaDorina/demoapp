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
    /// Логика взаимодействия для NewOrderWindow.xaml
    /// </summary>
    public partial class NewOrderWindow : Window
    {
        private readonly DorinaContext _context;
        List<Product> addedProducts = new List<Product>();
        private User user;
        

        public NewOrderWindow(User user)
        {
            InitializeComponent();
            _context = new DorinaContext();
            LoadProduct();
            this.user = user;
        }

        private void LoadProduct()
        {
            try
            {
                ProductName.ItemsSource = _context.Products.ToList();
                ProductName.DisplayMemberPath = "Title";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке ролей: " + ex.Message);
            }
        }

        private void AddDishButton_Click(object sender, EventArgs e)
        {
            if (ProductName.ItemsSource == null)
            {
                MessageBox.Show("Выберите блюдо / напиток", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Product selectedProduct = (Product)ProductName.SelectedItem;
            addedProducts.Add(selectedProduct);

            ProductName.SelectedItem = null;

            UpdateOrderItemsListBox();
        }

        private async void AddOrderButton_Click(object sender, EventArgs e)
        {
            try
            {
                string place = TableNum.Text;
                int countPerson = int.Parse(CountPerson.Text);
                var currentUser = _context.Users.FirstOrDefault(u => u.Id == this.user.Id).Id;

                if (string.IsNullOrWhiteSpace(place) || string.IsNullOrWhiteSpace(CountPerson.Text))
                {
                    MessageBox.Show("Заполните все поля.");
                    return;
                }

                if (addedProducts.Count == 0)
                {
                    MessageBox.Show("Добавьте данные о блюдах и напитках");
                    return;
                }

                var newOrder = new Order
                {
                    UserId = currentUser,
                    Place = place,
                    CountPerson = countPerson,
                    Status = "new",
                    Date = DateTime.Now,
                };

                foreach (var product in addedProducts)
                {
                    newOrder.OrderProducts.Add(new OrderProduct { Product = product });
                }


                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync();

                

                MessageBox.Show("Заказ успешно создан", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла ошибка: " + ex.Message);
            }
        }

        private void UpdateOrderItemsListBox()
        {
            OrderItemsListBox.Items.Clear();

            foreach (var product in addedProducts)
            {
                OrderItemsListBox.Items.Add($"{product.Title}");
            }
        }
    }
}
