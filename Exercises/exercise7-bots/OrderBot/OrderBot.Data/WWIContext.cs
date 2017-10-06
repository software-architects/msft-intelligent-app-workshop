using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderBot.Data
{
    public class WWIContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:intel-app-workshop.database.windows.net,1433;Initial Catalog=WWI-Standard;Persist Security Info=False;User ID=demo;Password=P@ssw0rd!123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        public virtual DbSet<Stockitem> Stockitems { get; set; }
    }
}
