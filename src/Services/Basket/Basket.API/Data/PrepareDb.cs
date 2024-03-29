﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

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

                context.Baskets.Add(new Model.Basket { new_id = "1", new_name = "Default Basket", new_description = "Default Basket description.", new_totalvalue = 0m });
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
