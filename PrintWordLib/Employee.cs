using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace PrintWordLib
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Rank { get; set; }

        public DateTime Date { get; set; }

        public string Gender { get; set; }

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
