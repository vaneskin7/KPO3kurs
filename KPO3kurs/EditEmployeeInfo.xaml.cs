using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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
    public partial class EditEmployeeInfo : Window
    {
        int currentRowIndex;
        public EditEmployeeInfo()
        {
            InitializeComponent();
        }
        public EditEmployeeInfo(int rowIndex)
        {
            InitializeComponent();
            currentRowIndex = rowIndex;
            nameTextBox.Text = MainWindow.employees[currentRowIndex].Name;
            rankTextBox.Text = MainWindow.employees[currentRowIndex].Rank;
            dateTextBox.Text = MainWindow.employees[currentRowIndex].Date.ToShortDateString();
            genderTextBox.Text = MainWindow.employees[currentRowIndex].Gender;
            birthDateTextBox.Text = MainWindow.employees[currentRowIndex].BirthDate.ToShortDateString();
        }

        private void saveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            string query = $"UPDATE employeesInfo SET empName = '{nameTextBox.Text}', empRank = '{rankTextBox.Text}'," +
                $"empDate = '{dateTextBox.Text}', empGender = '{genderTextBox.Text}', empBirthDate = '{birthDateTextBox.Text}' " +
                $"WHERE empID = {MainWindow.employees[currentRowIndex].Id}";

            bool isCorrect = InternetConnection.SendDataToServer(query);
            if (isCorrect = true)
            {
                MainWindow.employees[currentRowIndex].Name = nameTextBox.Text;
                MainWindow.employees[currentRowIndex].Rank = rankTextBox.Text;
                MainWindow.employees[currentRowIndex].Date = DateTime.Parse(dateTextBox.Text);
                MainWindow.employees[currentRowIndex].Gender = genderTextBox.Text;
                MainWindow.employees[currentRowIndex].BirthDate = DateTime.Parse(birthDateTextBox.Text);
            }
            this.Close();
        }
    }
}
