using CoreServicesBootcamp.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoreServicesBootcamp.DAL
{
    public class RequestContext : DbContext
    {

        public RequestContext(DbContextOptions<RequestContext> options) : base(options)
        {
        }

        public DbSet<Request> Requests { get; set; }



        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        IConfigurationRoot configuration = new ConfigurationBuilder()
        //           .SetBasePath(Directory.GetCurrentDirectory())
        //           .AddJsonFile("appsettings.json")
        //           .Build();
        //        var connectionString = configuration.GetConnectionString("DbCoreConnectionString");
        //        optionsBuilder.UseSqlServer(connectionString);
        //    }
        //}

        // Specify DbSet properties etc
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // add your own confguration here
            //modelBuilder.Entity<Request>().HasKey(c => new { c.ClientId, c.RequestId }).HasName("IX_MultipleColumns");

        }
    }
}
