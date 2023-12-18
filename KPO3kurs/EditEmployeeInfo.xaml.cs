using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Globalization;
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
using static System.Net.Mime.MediaTypeNames;

namespace KPO3kurs
{
    public partial class EditEmployeeInfo : Window
    {
        int currentRowIndex;
        List<Employee> employeeToEdit;
        public EditEmployeeInfo()
        {
            InitializeComponent();
        }
        public EditEmployeeInfo(int rowIndex, string employeeData)
        {
            InitializeComponent();
            employeeToEdit = employeeData
            .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
            .Select(Employee.FromString)
            .ToList();
            nameTextBox.Text = employeeToEdit[0].Name;
            rankTextBox.Text = employeeToEdit[0].Rank;
            empDatePicker.Text = employeeToEdit[0].Date.ToShortDateString();
            genderComboBox.SelectedItem = employeeToEdit[0].Gender;
            birthDatePicker.Text = employeeToEdit[0].BirthDate.ToShortDateString();
            if (employeeToEdit[0].Gender == "Мужской")
            {
                genderComboBox.SelectedIndex = 0;
            }
            else
            {
                genderComboBox.SelectedIndex = 1;
            }
            /*currentRowIndex = rowIndex;
            nameTextBox.Text = MainWindow.employees[currentRowIndex].Name;
            rankTextBox.Text = MainWindow.employees[currentRowIndex].Rank;
            empDatePicker.Text = MainWindow.employees[currentRowIndex].Date.ToShortDateString();*/

            /*if (MainWindow.employees[currentRowIndex].Gender == "Мужской")
            {
                genderComboBox.SelectedIndex = 0;
            }
            else
            {
                genderComboBox.SelectedIndex = 1;
            }
            //genderTextBox.Text = MainWindow.employees[currentRowIndex].Gender;
            birthDatePicker.Text = MainWindow.employees[currentRowIndex].BirthDate.ToShortDateString();*/
        }

        private void saveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            string query = $"UPDATE employeesInfo SET empName = '{nameTextBox.Text}', empRank = '{rankTextBox.Text}'," +
                $"empDate = '{empDatePicker.Text}', empGender = '{genderComboBox.Text}', empBirthDate = '{birthDatePicker.Text}' " +
                $"WHERE empID = {employeeToEdit[0].Id}";
            if (CheckIsCorrect() == true)
            {
                bool isSuccessful = InternetConnection.SendDataToServer(query, out string temp);
                if (isSuccessful == true)
                {
                    int index = MainWindow.employees.IndexOf(MainWindow.employees.FirstOrDefault(emp => emp.Id == employeeToEdit[0].Id));

                    if (index != null)
                    {
                        MainWindow.employees[index].Name = nameTextBox.Text;
                        MainWindow.employees[index].Rank = rankTextBox.Text;
                        MainWindow.employees[index].Date = DateTime.Parse(empDatePicker.Text);
                        MainWindow.employees[index].Gender = genderComboBox.Text;
                        MainWindow.employees[index].BirthDate = DateTime.Parse(birthDatePicker.Text);
                    }
                }
                this.Close();
            }  
        }

        public bool CheckIsCorrect()
        {
            bool isCorrect = true;
            if (nameTextBox.Text.Length >= 30 || rankTextBox.Text.Length >= 30)
            {
                MessageBox.Show("Введены некорректные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                isCorrect = false;
            }
            return isCorrect;
        }

    }
}
