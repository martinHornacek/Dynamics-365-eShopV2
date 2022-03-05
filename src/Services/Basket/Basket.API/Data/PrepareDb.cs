using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.API.Data
{
    public static class PrepareDb
    {
        public static void Populate(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<BasketContext>());
            }
        }

        private static void SeedData(BasketContext context)
        {
            if (!context.Baskets.Any())
            {
                Console.WriteLine("--> Seeding Basket Data...");

                context.Baskets.Add(new Model.Basket { Name = "Default Basket", Description = "Default Basket description." });
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
