﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OGAOE7_HFT_2021221.Models;

namespace OGAOE7_HFT_2021221.Data
{
    public class PizzaContext : DbContext
    {
        public virtual DbSet<Pizza> Pizzas { get; set; }
        public virtual DbSet<PromoOrder> Orders { get; set; }
        public virtual DbSet<Drink> Drinks { get; set; }

        public PizzaContext()
        {
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies() //do I need this here?
                    .UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\PizzaDB.mdf;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //FLUENT API
            modelBuilder.Entity<Pizza>(entity =>
            {
                entity.HasMany(pizza => pizza.Orders)
                .WithOne(promoOrder => promoOrder.Pizza)
                .HasForeignKey(promoOrder => promoOrder.PizzaName)
                .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Drink>(entity =>
            {
                entity.HasMany(drink => drink.Orders)
                .WithOne(promoOrder => promoOrder.Drink)
                .HasForeignKey(promoOrder => promoOrder.DrinkId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            });

            //TEST DATA
            //Pizzas
            modelBuilder.Entity<Pizza>().HasData(new List<Pizza>()
            {
                new Pizza()
                {
                    Name = "Pizza number 1",
                    Price = 1100
                },
                new Pizza()
                {
                    Name = "Pizza number 2",
                    Price = 1200
                },
                new Pizza()
                {
                    Name = "Pizza number 3",
                    Price = 1300
                },
                new Pizza()
                {
                    Name = "Pizza number 4",
                    Price = 1400
                }
            });



            //Drinks
            modelBuilder.Entity<Drink>().HasData(new List<Drink>()
            {
                new Drink()
                {
                    Id= 1,
                    Name = "Coca Cola",
                    Price = 390,
                    Promotional = true
                },
                new Drink()
                {
                    Id = 2,
                    Name = "Fanta Orange",
                    Price = 390,
                    Promotional = true
                },
                new Drink()
                {
                    Id = 3,
                    Name = "Cafe macchiato",
                    Price = 460,
                    Promotional = false
                }
            });

            //Orders
            modelBuilder.Entity<PromoOrder>().HasData(new List<PromoOrder>()
            {
                new PromoOrder()
                {
                    Id = 1,
                    DrinkId = 1,
                    PizzaName = "Pizza number 2",
                    TimeOfOrder = new DateTime(2022, 01, 09, 20, 30, 35), // YYYY, MM, DD, hh, mm, ss
                    DiscountPercentage = 20
                },

                new PromoOrder()
                {
                    Id = 2,
                    DrinkId = 1,
                    PizzaName = "Pizza number 2",
                    TimeOfOrder = new DateTime(2022, 01, 09, 20, 32, 01), // YYYY, MM, DD, hh, mm, ss
                    DiscountPercentage = 20
                },

                new PromoOrder()
                {
                    Id = 3,
                    DrinkId = 3,
                    PizzaName = "Pizza number 3",
                    TimeOfOrder = new DateTime(2022, 01, 09, 21, 59, 05), // YYYY, MM, DD, hh, mm, ss
                    DiscountPercentage = 20
                }

            });
            ;
        }
    }
}
