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
        /// indentyfikator oferty
        /// </summary>
        public int id;

        /// <summary>
        /// nazwa hotelu
        /// </summary>
        public string hotel_name;

        /// <summary>
        /// data zlecenia
        /// </summary>
        public DateTime date;

        /// <summary>
        /// liczba godzin
        /// </summary>
        public int hours;

        /// <summary>
        /// płaca
        /// </summary>
        public int salary;

        public work_offer(int id, string hotel_name, DateTime date, int hours, int salary)
        {
            this.id = id;
            this.hotel_name = hotel_name;
            this.date = date;
            this.hours = hours;
            this.salary = salary;
        }
    }
}