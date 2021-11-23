﻿using EShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.DataAccessLayer
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ApplicationType> ApplicationTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, CategoryName = "Italian Marble", DisplayOrder=2 },
                 new Category() { Id = 2, CategoryName = "Hand Printed tiles", DisplayOrder = 1 },
                 new Category() { Id = 3, CategoryName = "Mozaic Tiles", DisplayOrder = 3} 
                );

            modelBuilder.Entity<ApplicationType>().HasData(
               new ApplicationType() { Id = 1, ApplicationName = "TestApplication" }
            );
        }
    }
}
