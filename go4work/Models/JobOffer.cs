using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace go4work.Models
{
    /// <summary>
    /// tabela wystawionej ofery pracy w bazie danych
    /// </summary>
    [Index(nameof(Date))]
    public class JobOffer
    {
        /// <summary>
        /// indetnyfikator oferty - pk
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// id hotelu do którego odnosi się oferta - fk
        /// </summary>
        [Required]
        public int HotelID { get; set; }
        public Hotel Hotel { get; set; } // powiązany hotel

        /// <summary>
        /// data odbywania się pracy z oferty (indeksowana kolumna)
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// liczba godzin pracy
        /// </summary>
        [Required]
        public int Hours { get; set; }

        /// <summary>
        /// cena z godzinę pracy
        /// </summary>
        [Required]
        public int Salary { get; set; }

        /// <summary>
        /// czy oferta już została przez kogoś przyjęta
        /// </summary>
        [Required]
        [DefaultValue(false)]
        public bool WasAccepted { get; set; } = false;

        /// <summary>
        /// lista zrealizowanych ofert
        /// </summary>
        public virtual ICollection<AcceptedOffer> AcceptedOffers { get; set; }
    }
}
