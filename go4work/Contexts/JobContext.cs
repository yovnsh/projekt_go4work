using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace go4work.Contexts
{
    /// <summary>
    /// kontekst bazy danych pracowników i ofert pracy
    /// </summary>
    public class JobContext: DbContext
    {
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Hotel> Hotels { get; set; }
        public DbSet<Models.JobOffer> JobOffers { get; set; }
        public DbSet<Models.AcceptedOffer> AcceptedOffers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=.\\Database\\jobs.db");
        }
    }
}
