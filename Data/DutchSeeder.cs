using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _ctx;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext ctx, IWebHostEnvironment env, UserManager<StoreUser> userManager)
        {
            _ctx = ctx;
            _env = env;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _ctx.Database.EnsureCreated();

            StoreUser user = await _userManager.FindByEmailAsync("stevan@freeborn.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Stevan",
                    LastName = "Freeborn",
                    Email = "stevan@freeborn.com",
                    UserName = "stevan@freeborn.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create user in seeder.");
                }
            }

            if (_ctx.Products.Any()) return;

            var filePath = Path.Combine(_env.ContentRootPath, "Data/art.json");
            var json = File.ReadAllText(filePath);
            var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);

            _ctx
                .Products
                .AddRange(products);

            var order = _ctx
                .Orders
                .FirstOrDefault(o => o.Id == 1);

            if (order != null)
            {
                order.User = user;

                order = new Order()
                {
                    OrderDate = DateTime.Today,
                    OrderNumber = "10000",
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    }
                };
            }

            _ctx.Orders.Add(order);

            await _ctx.SaveChangesAsync();
        }
    }
}
