using DorinaDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

namespace DorinaDemo.Views
{
    /// <summary>
    /// Логика взаимодействия для ShiftsWindow.xaml
    /// </summary>
    public partial class ShiftsWindow : Window
    {
        private readonly DorinaContext _context;
        private NewShiftWindow _newShiftWindow;

        public ShiftsWindow()
        {
            InitializeComponent();
            _context = new DorinaContext();
            LoadShiftsAsync();

            //addButton.Click += NewShiftButton_Click;
        }

        private async Task LoadShiftsAsync()
        {
            try
            {
                var shifts = await _context.Shifts
                    .Include(s => s.ShiftUsers)
                        .ThenInclude(su => su.User)
                        .ThenInclude(sur => sur.Role)
                        .OrderByDescending(s => s.StartShift)
                        .ToListAsync();

                ShiftsGrid.ItemsSource = shifts;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private  void NewShiftButton_Click(object sender, RoutedEventArgs e)
        {
            if (_newShiftWindow == null || !_newShiftWindow.IsVisible)
            {
                _newShiftWindow = new NewShiftWindow();
                _newShiftWindow.Closed += async (s, args) =>
                {
                    _newShiftWindow = null;
                    await LoadShiftsAsync();
                };
                _newShiftWindow.Show();
            }
            else
            {
                _newShiftWindow.Activate();
            }
        }

        
    }
}
