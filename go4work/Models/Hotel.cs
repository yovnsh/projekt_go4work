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
    /// tabela hoteli w bazie danych
    /// </summary>
    [Index(nameof(Name), IsUnique = true)]
    public class Hotel
    {
        /// <summary>
        /// pk identyfikator hotelu
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// nazwa hotelu (unique)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// wszystkie oferty pracy powiązane z tym hotelem
        /// </summary>
        public virtual ICollection<JobOffer> JobOffers { get; set; }
    }
}
