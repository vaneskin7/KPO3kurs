using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace KPO3kurs
{
 
    public partial class AddEmployeeWindow : Window
    {
        MainWindow mainWindow;
        ObservableCollection<Employee> employees;
        public AddEmployeeWindow(MainWindow mainWindow, ObservableCollection<Employee> employees)
        {
            this.mainWindow = mainWindow;
            this.employees = employees;
            InitializeComponent();
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIsCorrect() == true)
            {
                Employee newPerson = new Employee
                {
                    Name = nameTextBox.Text,
                    Rank = rankTextBox.Text,
                    Date = DateTime.Parse(datePicker.Text),
                    Gender = genderComboBox.Text,
                    BirthDate = DateTime.Parse(birthDatePicker.Text)
                };
                string query = $"INSERT;" +
                    $"{newPerson.Name};{newPerson.Rank};{newPerson.Date.ToString("yyyy-MM-dd")};{newPerson.Gender};{newPerson.BirthDate.ToString("yyyy-MM-dd")}";
                string employeeID;
                bool isSuccessful = InternetConnection.SendDataToServer(query, out employeeID);
                if (isSuccessful = true)
                {
                    newPerson.Id = int.Parse(employeeID);
                    employees.Add(newPerson);
                }
                this.Close();
            }
        }

        public bool CheckIsCorrect()
        {
            bool isCorrect = true;
            if (nameTextBox.Text.Length >= 30 || rankTextBox.Text.Length >= 30 || genderComboBox.SelectedIndex == -1 
                || datePicker.Text == "" || birthDatePicker.Text == "")
            {
                MessageBox.Show("Введены некорректные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                isCorrect = false;
            }
            return isCorrect;
        }
    }
}
