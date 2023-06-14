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
    public struct work_offer
    {
        public const string DEFAULT_ID = "-- id --";
        public const string DEFAULT_HOTEL_NAME = "-- hotel --";
        public const string DEFAULT_DATE = "-- data --";
        public const string DEFAULT_HOURS = "-- godziny --";
        public const string DEFAULT_SALARY = "-- wynagordzenie --";

        /// <summary>
        /// indentyfikator oferty
        /// </summary>
        public string id { get; set; } = DEFAULT_DATE;

        /// <summary>
        /// nazwa hotelu
        /// </summary>
        public string hotel_name { get; set; } = DEFAULT_HOTEL_NAME;

        /// <summary>
        /// data zlecenia
        /// </summary>
        public string date { get; set; } = DEFAULT_DATE;

        /// <summary>
        /// liczba godzin
        /// </summary>
        public string hours { get; set; } = DEFAULT_HOURS;

        /// <summary>
        /// płaca
        /// </summary>
        public string salary { get; set; } = DEFAULT_SALARY;

        public work_offer(string id, string hotel_name, string date, string hours, string salary)
        {
            this.id = id;
            this.hotel_name = hotel_name;
            this.date = date;
            this.hours = hours;
            this.salary = salary;
        }

        public override string ToString()
        {
            return $"Oferta: {hotel_name}, {date}, {hours}, {salary}";
        }
    }
}