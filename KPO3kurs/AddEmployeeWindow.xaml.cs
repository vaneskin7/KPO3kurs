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
            Employee newPerson = new Employee { Name = nameTextBox.Text, Rank = rankTextBox.Text, Date = DateTime.Parse(dateTextBox.Text),
                                                Gender = genderTextBox.Text, BirthDate = DateTime.Parse(birthDayTextBox.Text)};
            string query = $"INSERT INTO employeesInfo (empName, empRank, empDate, empGender, empBirthDate) VALUES" +
                $"('{newPerson.Name}', '{newPerson.Rank}', '{newPerson.Date}', '{newPerson.Gender}', '{newPerson.BirthDate}')";
            bool isCorrect = InternetConnection.SendDataToServer(query);
            if (isCorrect = true)
            {
                employees.Add(newPerson);
                EditEmployeeInfo editEmployeeInfo = new EditEmployeeInfo();
                editEmployeeInfo.ShowDialog();
            }
            this.Close(); 
        }
    }
}
