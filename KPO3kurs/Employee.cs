using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace KPO3kurs
{
    public class Employee
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("ФИО")]
        public string Name { get; set; }

        [DisplayName("Должность")]
        public string Rank { get; set; }

        [DisplayName("Дата устройства")]
        public DateTime Date { get; set; }

        [DisplayName("Пол")]
        public string Gender { get; set; }

        [DisplayName("Дата рождения")]
        public DateTime BirthDate { get; set; }

        public static Employee FromString(string data)
        {
            var values = data.Split(';');
            return new Employee
            {
                Id = int.Parse(values[0]),
                Name = values[1],
                Rank = values[2],
                Date = DateTime.ParseExact(values[3], "yyyy-MM-dd", null),
                Gender = values[4],
                BirthDate = DateTime.ParseExact(values[5], "yyyy-MM-dd", null)
            };
        }
    }

   

}
