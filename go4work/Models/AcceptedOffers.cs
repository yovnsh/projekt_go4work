using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace go4work.Models
{
    /// <summary>
    /// tabela zrealizowanej oferty pracy w bazie danych
    /// </summary>
    [Index(nameof(JobOfferID), IsUnique = true)]
    public class AcceptedOffer
    {
        /// <summary>
        /// identyfikator zrealizowanej ofery - pk
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// id oferty która została przyjęta - fk, unique
        /// </summary>
        [Required]
        public int JobOfferID { get; set; }
        public virtual JobOffer JobOffer { get; set; }

        /// <summary>
        /// użytkownik który zdecydował się przyjąć ofertę
        /// </summary>
        [Required]
        public string UserPesel { get; set; }
        public virtual User User { get; set; } // powiązany użytkownik
    }
}
