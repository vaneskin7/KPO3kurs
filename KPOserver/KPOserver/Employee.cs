using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPOserver
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Rank { get; set; }

        public DateTime Date { get; set; }

        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }

        public override string ToString()
        {
            return $"{Id};{Name};{Rank};{Date:yyyy-MM-dd};{Gender};{BirthDate:yyyy-MM-dd}";
        }
    }
}
