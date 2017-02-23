using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2E.UWP.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<WordFrequency> WordFrequencies { get; set; }
        public DbSet<MessageFrequency> MessageFrequencies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=DatabaseDebug.db");
        }
    }
}
