using CafeCaspian.Models;
using CafeCaspian.Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace CafeCaspian.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> PRODUCTS = new List<Product>()
        {
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Cola",
                Cost = 0.50m
            },
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Coffee",
                Cost = 1.00m
            },
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Cheese Sandwich",
                Cost = 2.00m
            },
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Steak Sandwich",
                Cost = 4.50m
            }
        };

        public Product GetByName(string name)
        {
            return PRODUCTS.Find(p => p.Name == name);
        }
    }
}
