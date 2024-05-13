using System.Text;
using System.Windows;
using System.Windows.Controls;
using System;
using Microsoft.EntityFrameworkCore;
using DorinaDemo.Models;

namespace DorinaDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            using (var context = new DorinaContext())
            {
                var user = context.Users.Include(u => u.Role).FirstOrDefault(u => u.Login == username && u.Password == password);
                
                if(user != null)
                {
                    switch (user.Role.Name)
                    {
                        case "Администратор":
                            Views.AdminWindow adminWindow = new Views.AdminWindow();
                            adminWindow.Show();
                            this.Close();
                            break;

                        case "Официант":
                            Views.WaiterWindow waiterWindow = new Views.WaiterWindow(user);
                            waiterWindow.Show();
                            this.Close();
                            break;

                        case "Повар":
                            Views.ChefWindow chefWindow = new Views.ChefWindow();
                            chefWindow.Show();
                            this.Close();
                            break;

                        default:
                            MessageBox.Show("Неизвестная роль пользователя");
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
            }
        }
    }
}