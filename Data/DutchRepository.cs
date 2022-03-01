using System;
using System.Collections.Generic;
using System.Linq;
using DutchTreat.Data.Entities;
using DutchTreat.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _ctx;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DutchContext ctx, ILogger<DutchRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                return _ctx
                    .Products
                    .OrderBy(p => p.Title)
                    .ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get all products: {e}");
                return null;
            }
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            try
            {
                return _ctx
                    .Products
                    .Where(p => p.Category == category)
                    .ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get all products by category: {e}");
                return null;
            }
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            try
            {
                if (includeItems)
                {
                    return _ctx
                        .Orders
                        .Include(o => o.Items)
                        .ThenInclude(oi => oi.Product)
                        .OrderBy(o => o.OrderDate)
                        .ToList();
                }

                return _ctx
                    .Orders
                    .OrderBy(o => o.OrderDate)
                    .ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get all orders: {e}");
                return null;
            }
        }

        public IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems)
        {
            try
            {
                if (includeItems)
                {
                    return _ctx
                        .Orders
                        .Where(o => o.User.UserName == username)
                        .Include(o => o.Items)
                        .ThenInclude(oi => oi.Product)
                        .OrderBy(o => o.OrderDate)
                        .ToList();
                }

                return _ctx
                    .Orders
                    .Where(o => o.User.UserName == username)
                    .OrderBy(o => o.OrderDate)
                    .ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get all orders: {e}");
                return null;
            }
        }

        public Order GetOrderById(string username, int id)
        {
            try
            {
                return _ctx
                    .Orders
                    .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                    .Where(o => o.Id == id && o.User.UserName == username)
                    .FirstOrDefault(o => o.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get order: {e}");
                return null;
            }
        }

        public bool SaveAll()
        {
            try
            {
                return _ctx.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to save changes: {e}");
                return false;
            }
        }

        public void AddEntity(object model)
        {
            _ctx.Add(model);
        }
    }
}
