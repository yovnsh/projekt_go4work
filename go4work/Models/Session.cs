using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace go4work.Models
{
    /// <summary>
    /// obiekt sesji w bazie danych
    /// pozwala na zapamiętywanie zalogowanego użytkownika
    /// </summary>
    public class Session
    {
        /// <summary>
        /// identyfikator sesji - pk
        /// </summary>
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// pesel użytkownika, który w tej sesji jest zalogowany
        /// </summary>
        public string UserPesel { get; set; }
        public virtual User User { get; set; }
    }
}
