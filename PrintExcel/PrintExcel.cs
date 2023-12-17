using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using Microsoft.Office.Interop.Excel;

namespace PrintExcelLib
{
    public class PrintExcel
    {
        public static void SaveInExcel(string serializedData, string filePath) // сохранение в Excel
        {
            Excel.Application excelApp = new Excel.Application();

            // Создаём экземпляр рабочий книги Excel
            Excel.Workbook workBook = excelApp.Workbooks.Add();

            // Создаём экземпляр листа Excel
            Excel.Worksheet workSheet;
            
            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(1);
            
            workSheet.Cells[1, 1] = "ФИО";
            workSheet.Cells[1, 2] = "Должность";
            workSheet.Cells[1, 3] = "Дата устройства";
            workSheet.Cells[1, 4] = "Пол";
            workSheet.Cells[1, 5] = "Дата рождения";
            workSheet.Cells.ColumnWidth = 30;
            Excel.Range firstRow = workSheet.Rows[1];
            firstRow.Font.Bold = true;
            firstRow.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            List<Employee> employees = serializedData
           .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
           .Select(Employee.FromString)
           .ToList();

            int i = 2;
            foreach (Employee employee in employees)
            {
                workSheet.Cells[i, 1] = employee.Name;
                workSheet.Cells[i, 2] = employee.Rank;
                workSheet.Cells[i, 3] = employee.Date;
                workSheet.Cells[i, 4] = employee.Gender;
                workSheet.Cells[i, 5] = employee.BirthDate;
                i++;
            }
            Excel.Range cells = workSheet.Cells;
            cells.Font.Name = "Times New Roman";
            cells.Font.Size = 12;

            excelApp.Visible = true;
            
            workBook.SaveAs(filePath, Excel.XlSaveAsAccessMode.xlNoChange);
        }
    }
}