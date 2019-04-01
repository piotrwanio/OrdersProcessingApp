using CoreServicesBootcamp.DAL.Entities;
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
        public DbSet<Order> Orders { get; set; }

        // Specify DbSet properties etc
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(b =>
            {
                b.HasKey(r => r.OrderId);
                b.Property(r => r.OrderId).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Request>()
                .HasOne(s => s.Order)
                .WithMany()
                .HasForeignKey(e => e.OrderId);

        }
    }
}
