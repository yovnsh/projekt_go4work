using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace go4work
{
    /// <summary>
    /// struktura z danymi o ofercie pracy (dane z bazy danych)
    /// </summary>
    struct work_offer
    {
        /// <summary>
        /// nazwa hotelu - pierwsza kolumna bazy danych
        /// </summary>
        public string hotel_name;

        /// <summary>
        /// data zlecenia - druga kolumna bazy danych
        /// </summary>
        public DateTime date;

        /// <summary>
        /// liczba godzin - trzecia kolumna bazy danych
        /// </summary>
        public int hours;

        /// <summary>
        /// płaca - czwarta kolumna bazy danych
        /// </summary>
        public int salary;

        public work_offer(string hotel_name, DateTime date, int hours, int salary)
        {
            this.hotel_name = hotel_name;
            this.date = date;
            this.hours = hours;
            this.salary = salary;
        }
    }
}
