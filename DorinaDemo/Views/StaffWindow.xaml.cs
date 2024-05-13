using DorinaDemo.Models;
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
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace DorinaDemo.Views
{
    /// <summary>
    /// Логика взаимодействия для StaffWindow.xaml
    /// </summary>
    public partial class StaffWindow : Window
    {
        private readonly DorinaContext _context;
        private ObservableCollection<User> _employees;

        public StaffWindow()
        {
            InitializeComponent();

            _context = new DorinaContext();
            LoadEmployees();

        }

        private void LoadEmployees()
        {
            //_employees = new ObservableCollection<User>(_context.Users.Include(u => u.Role).Where(u => u.Status == "Active").ToList());
            _employees = new ObservableCollection<User>(_context.Users.Include(u => u.Role).ToList());
            EmployeesGrid.ItemsSource = _employees;
        }

        private void FireEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if(EmployeesGrid.SelectedItem != null)
            {
                var selectedEmployee = EmployeesGrid.SelectedItem as User;

                using (var context = new DorinaContext())
                {
                    var employeeToUpdate = context.Users.FirstOrDefault(u => u.Id == selectedEmployee.Id);

                    if(employeeToUpdate != null)
                    {
                        employeeToUpdate.Status = "NoActive";
                        context.SaveChanges();
                        //_employees.Remove(selectedEmployee);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите сотрудника для увольнения.");
            }
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            NewUserWindow newUserWindow = new NewUserWindow();
            newUserWindow.ShowDialog();
            LoadEmployees();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            this.Close();
            adminWindow.Show();
        }
    }
}
