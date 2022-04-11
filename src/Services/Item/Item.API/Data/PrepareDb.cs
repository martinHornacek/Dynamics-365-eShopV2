using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Item.API.Data
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
                Console.WriteLine("--> Seeding Items Data...");

                context.Items.AddRange(
                    new Model.Item() { new_id = "1", new_name = "Harry Potter", new_price = 90.0m, new_category = "Books", new_description = "Though Harry's first year at Hogwarts is the best of his life, not everything is perfect." },
                    new Model.Item() { new_id = "2", new_name = "Lord of the Rings", new_price = 110.0m, new_category = "Books", new_description = "One Ring to rule them all, One Ring to find them, One Ring to bring them all and in the darkeness bind them." },
                    new Model.Item() { new_id = "3", new_name = "Star Wars", new_price = 45.0m, new_category = "Movies", new_description = "Luke Skywalker joins forces with a Jedi Knight, a cocky pilot, a Wookiee and two droids to save the galaxy from the Empire's world-destroying battle station, while also attempting to rescue Princess Leia from the mysterious Darth Vader." },
                    new Model.Item() { new_id = "4", new_name = "Die Hard", new_price = 30.0m, new_category = "Movies", new_description = "An NYPD officer tries to save his wife and several others taken hostage by German terrorists during a Christmas party at the Nakatomi Plaza in Los Angeles." }
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
