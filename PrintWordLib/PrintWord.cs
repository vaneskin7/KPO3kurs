using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Word = Microsoft.Office.Interop.Word;

namespace PrintWordLib
{
    public class PrintWord
    {
        public static void SaveInWord(string serializedData, string filePath) // сохранение в Word
        {
            List<Employee> employees = serializedData
            .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
            .Select(Employee.FromString)
            .ToList();
            
            Word.Application WordApp = new Word.Application();
            Word.Document doc = WordApp.Documents.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            object start = 0, end = 0;
            Word.Range employeeList = doc.Paragraphs[1].Range;
            employeeList.Text = "ФИО, должность, дата устройства, пол, дата рождения.";
            employeeList.Font.Name = "Times New Roman";
            employeeList.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            employeeList.Font.Size = 12;
            int i = 1;
            foreach (Employee emp in employees)
            {
                employeeList.Text += $"{i}. {emp.Name}, {emp.Rank}, {emp.Date.ToShortDateString()}, {emp.Gender}, {emp.BirthDate.ToShortDateString()}";
                i++;
            }
            
            WordApp.Visible = true;
            doc.SaveAs(filePath);
        }
    }

}