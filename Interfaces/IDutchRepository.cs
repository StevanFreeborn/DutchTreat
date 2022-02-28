using System.Collections.Generic;
using DutchTreat.Data.Entities;

namespace DutchTreat.Interfaces
{
    public interface IDutchRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);
        bool SaveAll();
    }
}