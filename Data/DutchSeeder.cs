using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Internal;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _ctx;
        private readonly IWebHostEnvironment _env;

        public DutchSeeder(DutchContext ctx, IWebHostEnvironment env)
        {
            _ctx = ctx;
            _env = env;
        }

        public void Seed()
        {
            _ctx.Database.EnsureCreated();

#pragma warning disable EF1001 // Internal EF Core API usage.
            if (EnumerableExtensions.Any(_ctx.Products)) return;
#pragma warning restore EF1001 // Internal EF Core API usage.

            var filePath = Path.Combine(_env.ContentRootPath, "Data/art.json");
            var json = File.ReadAllText(filePath);
            var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);

            _ctx
                .Products
                .AddRange(products);

            var order = new Order()
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

            _ctx.Orders.Add(order);

            _ctx.SaveChanges();
        }
    }
}
