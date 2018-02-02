using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebApplication2.Models
{
    public class OurDbContext : DbContext
    {
        public DbSet<UserAccount> userAccount { get; set; }
        public DbSet<EmailList> emailList { get; set; }
    }

    //protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //{
    //    Database.SetInitializer<OurDbContext>(null);
    //    base.OnModelCreating(modelBuilder);
    //}
}