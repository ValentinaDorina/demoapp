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

namespace DorinaDemo.Views
{
    /// <summary>
    /// Логика взаимодействия для NewShiftWindow.xaml
    /// </summary>
    public partial class NewShiftWindow : Window
    {
        private readonly DorinaContext _context;
        public NewShiftWindow()
        {
            InitializeComponent();

            _context = new DorinaContext();

            //var employees = _context.Users.ToList();
            EmployeesListBox.ItemsSource = _context.Users.Where(u => u.Status == "Active").ToList();
        }

        private async void CreateShiftButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedEmployees = EmployeesListBox.SelectedItems.Cast<User>().ToList();

                var startShiftDate = StartDatePicker.SelectedDate ?? DateTime.Today;
                var startShiftTime = TimeSpan.FromHours(int.Parse(((ComboBoxItem)StartHourComboBox.SelectedItem).Content.ToString())) +
                                     TimeSpan.FromMinutes(int.Parse(((ComboBoxItem)StartMinuteComboBox.SelectedItem).Content.ToString()));
                var startShiftDateTime = startShiftDate.Add(startShiftTime);

                var endShiftDate = EndDatePicker.SelectedDate ?? DateTime.Today;
                var endShiftTime = TimeSpan.FromHours(int.Parse(((ComboBoxItem)EndHourComboBox.SelectedItem).Content.ToString())) +
                                     TimeSpan.FromMinutes(int.Parse(((ComboBoxItem)EndMinuteComboBox.SelectedItem).Content.ToString()));
                var endtShiftDateTime = endShiftDate.Add(endShiftTime);

                string shiftStatus;
                if (DateTime.Now < startShiftDateTime)
                {
                    shiftStatus = "Новая смена";
                }
                else if (DateTime.Now >= startShiftDateTime && DateTime.Now <= endtShiftDateTime)
                {
                    shiftStatus = "active";
                }
                else
                {
                    shiftStatus = "inactive";
                }

                var newShift = new Shift
                {
                    StartShift = startShiftDateTime,
                    EndShift = endtShiftDateTime,
                    StatusShift = shiftStatus
                };

                foreach (var employee in selectedEmployees)
                {
                    newShift.ShiftUsers.Add(new ShiftUser { User = employee });
                }

                _context.Shifts.Add(newShift);
                await _context.SaveChangesAsync();

                MessageBox.Show("Смена успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении смены: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StartHourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    


}
