using System;
using System.Linq;
using Basket.API.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.API.Data
{
    public static class PrepareDb
    {
        public static void Populate(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<ItemContext>());
            }
        }

        private static void SeedData(ItemContext context)
        {
            if (!context.Items.Any())
            {
                Console.WriteLine("--> Seeding Data...");

                context.Items.AddRange(
                    new Item() { Name = "Harry Potter", Price = 90.0m, Category = "Books", Description = "Though Harry's first year at Hogwarts is the best of his life, not everything is perfect." },
                    new Item() { Name = "Lord of the Rings", Price = 110.0m, Category = "Books", Description = "One Ring to rule them all, One Ring to find them, One Ring to bring them all and in the darkeness bind them." },
                    new Item() { Name = "Star Wars", Price = 45.0m, Category = "Movies", Description = "Luke Skywalker joins forces with a Jedi Knight, a cocky pilot, a Wookiee and two droids to save the galaxy from the Empire's world-destroying battle station, while also attempting to rescue Princess Leia from the mysterious Darth Vader." },
                    new Item() { Name = "Die Hard", Price = 30.0m, Category = "Movies", Description = "An NYPD officer tries to save his wife and several others taken hostage by German terrorists during a Christmas party at the Nakatomi Plaza in Los Angeles." }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
