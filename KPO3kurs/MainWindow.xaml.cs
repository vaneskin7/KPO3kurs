using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace KPO3kurs
{
    public partial class MainWindow : Window
    {
        public static ObservableCollection<Employee> employees;
        public MainWindow()
        {
            InitializeComponent();
            employees = new ObservableCollection<Employee>();
            employeeGrid.ItemsSource = employees;
            InternetConnection internetConnection = new InternetConnection("vaneskin7", "qwerty228");
            bool isCorrect = InternetConnection.ConnectToServer();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string data = InternetConnection.GetDataFromServer();
            List<Employee> deserializedEmployees = data
            .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
            .Select(Employee.FromString)
            .ToList();
            foreach (Employee employee in deserializedEmployees)
            {
                employees.Add(employee);
            }
            employeeGrid.Items.Refresh();
        }

        // add new employee info
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow(this, employees);
            addEmployeeWindow.ShowDialog();
        }

        // delete employee info
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (employeeGrid.SelectedIndex != -1)
            {
                employees.RemoveAt(employeeGrid.SelectedIndex);
            }
        }

        // edit employee info
        private void employeeGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid.SelectedItem != null && e.LeftButton == MouseButtonState.Pressed)
            {
                int rowIndex = dataGrid.Items.IndexOf(dataGrid.SelectedItem); 
                EditEmployeeInfo editEmployeeWindow = new EditEmployeeInfo(rowIndex);
                editEmployeeWindow.ShowDialog();
                dataGrid.Items.Refresh();
            }
        }
    }
}
