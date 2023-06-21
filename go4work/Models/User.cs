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
    /// tabela użytkowników w bazie danych
    /// </summary>
    public class User
    {
        /// <summary>
        /// pesel - pk
        /// </summary>
        [Key]
        public string Pesel { get; set; }

        /// <summary>
        /// hasło użytkownika
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// imię użytkownika
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// nazwisko użytkownika
        /// </summary>
        [Required]
        public string Surname { get; set; }

        /// <summary>
        /// miasto z adresu zamieszkania użytkownika
        /// </summary>
        [Required]
        public string City { get; set; }

        /// <summary>
        /// ulica z adresu zamieszkania użytkownika
        /// </summary>
        [Required]
        public string Street { get; set; }

        /// <summary>
        /// numer mieszkania z adresu zamieszkania użytkownika
        /// </summary>
        [Required]
        public string ApartamentNumber { get; set; }

        /// <summary>
        /// numer karty płatniczej użytkownika
        /// </summary>
        [Required]
        public string CardNumber { get; set; }

        /// <summary>
        /// numer telefonu użytkownika
        /// </summary>
        [Required]
        public string TelephoneNumber { get; set; }

        /// <summary>
        /// ścieżka do zdjęcia profilowego użytkownika
        /// </summary>
        public string? AvatarPath { get; set; }


        /// <summary>
        /// czu użytkownik jest administratorem
        /// </summary>
        [Required]
        [DefaultValue(false)]
        public bool IsAdmin { get; set; }

        /// <summary>
        /// lista wszystkich przyjętych zleceń
        /// </summary>
        public virtual ICollection<AcceptedOffer> AcceptedOffers { get; set; }
    }
}
