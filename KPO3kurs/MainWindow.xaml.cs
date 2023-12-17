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
//using PrintWordLib;
//using PrintExcelLib;
using Microsoft.Win32;

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
            if (InternetConnection.ConnectToServer() == false)
            {
                this.Close();
            }
        }

        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                ContextMenu contextMenu = button.ContextMenu;
                if (contextMenu != null)
                {
                    contextMenu.IsOpen = true;
                }
            }
        }

        private void SaveWordButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "docx";
            saveFileDialog.Filter = "Файлы Word (*.docx)|*.docx|Все файлы (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                string serializedData = string.Join(Environment.NewLine, employees.Select(e => e.ToString()));
                //PrintWord.SaveInWord(serializedData, saveFileDialog.FileName);
            }    
        }

        private void SaveExcelButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "xls";
            saveFileDialog.Filter = "Файлы Excel (*.xls)|*.xls|Все файлы (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                string serializedData = string.Join(Environment.NewLine, employees.Select(e => e.ToString()));
                //PrintExcel.SaveInExcel(serializedData, saveFileDialog.FileName);
            }
        }

        private void EmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                ContextMenu contextMenu = button.ContextMenu;
                if (contextMenu != null)
                {
                    contextMenu.IsOpen = true;
                }
            }
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
            string serializedData = string.Join(Environment.NewLine, employees.Select(e => e.ToString()));
            //PrintWord.SaveInWord(serializedData);
            //PrintExcel.SaveInExcel(serializedData);
        }

        // add new employee info
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow(this, employees);
            addEmployeeWindow.ShowDialog();
            employeeGrid.ItemsSource = employees;
        }

        // delete employee info
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (employeeGrid.SelectedIndex != -1)
            {
                Employee deletedEmployee = employeeGrid.SelectedItem as Employee;
                MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить запись о сотруднике: {deletedEmployee.Name}?",
                    "Подтвердите действие", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    bool isCorrect = InternetConnection.SendDataToServer($"DELETE FROM employeesInfo WHERE empID = {deletedEmployee.Id}", out string temp);
                    if (isCorrect == true)
                    {
                        employees.Remove(employeeGrid.SelectedItem as Employee);
                        employeeGrid.ItemsSource = employees;
                        employeeGrid.Items.Refresh();
                    }
                }
            }
        }

        private void employeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (employeeGrid.SelectedItem != null)
            {
                int rowIndex = employeeGrid.Items.IndexOf(employeeGrid.SelectedItem);
                EditEmployeeInfo editEmployeeWindow = new EditEmployeeInfo(rowIndex, employeeGrid.SelectedItem.ToString());
                editEmployeeWindow.ShowDialog();
                employeeGrid.Items.Refresh();
            }
        }

        private void employeeGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid.SelectedItem != null && e.RightButton == MouseButtonState.Pressed)
            {
                int rowIndex = dataGrid.Items.IndexOf(dataGrid.SelectedItem);
                EditEmployeeInfo editEmployeeWindow = new EditEmployeeInfo(rowIndex, dataGrid.SelectedItem.ToString());
                editEmployeeWindow.ShowDialog();
                dataGrid.Items.Refresh();
            }
        }


        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = searchBoxText.Text.ToLower();

            if (searchText == "")
            {
                employeeGrid.ItemsSource = employees;
                return;
            }
            else
            {
                List<Employee> filteredItems;
                switch (searchComboBox.SelectedIndex)
                {
                    case 0:
                        {
                            filteredItems = employees.Where(item => item.Name.ToLower().StartsWith(searchText)).ToList();
                            employeeGrid.ItemsSource = filteredItems;
                            break;
                        }
                    case 1:
                        {
                            filteredItems = employees.Where(item => item.Rank.ToLower().StartsWith(searchText)).ToList();
                            employeeGrid.ItemsSource = filteredItems;
                            break;
                        }
                    case 2:
                        {
                            filteredItems = employees.Where(item => item.Gender.ToLower().StartsWith(searchText)).ToList();
                            employeeGrid.ItemsSource = filteredItems;
                            break;
                        }
                    default:
                        {
                            MessageBox.Show("Выберите критерий для поиска", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        }
                }
            }
        }

        private void refreshEmployeesButton_Click(object sender, RoutedEventArgs e)
        {

            string data = InternetConnection.GetDataFromServer();
            if (data != null)
            {
                employees.Clear();
                List<Employee> deserializedEmployees = data
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(Employee.FromString)
                .ToList();
                foreach (Employee employee in deserializedEmployees)
                {
                    employees.Add(employee);
                }
                employeeGrid.ItemsSource = employees;
                employeeGrid.Items.Refresh();
            }         
        }
    }
}
