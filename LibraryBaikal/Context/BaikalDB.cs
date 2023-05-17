using LibraryBaikal.Entityes.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryBaikal.Context
{
    public class BaikalDB : DbContext
    {
        public DbSet<Town> Towns { get; set; }

        public BaikalDB(DbContextOptions<BaikalDB> options) :base(options)
        { 
        
        }

        public BaikalDB() { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Town>((builder)=> {
            builder.HasIndex(x=>x.Name).IsUnique();
            });
        }
    }
}
