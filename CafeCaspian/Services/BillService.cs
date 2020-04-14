using CafeCaspian.Models;
using CafeCaspian.Repositories.Interfaces;
using CafeCaspian.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CafeCaspian.Services
{
    public class BillService : IBillService
    {
        private readonly IProductRepository _productRepo;

        public BillService(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public decimal GetTotalBill(IEnumerable<string> products)
        {
            var totalCost = 0.0m;
            var productItems = new List<Product>();

            foreach(var item in products)
            {
                var product = _productRepo.GetByName(item);

                if (product != null)
                {
                    productItems.Add(product);
                    totalCost += product.Cost;
                }
                else
                {
                    // invalid product
                    throw new ProductNotFoundException("Invalid products provided");
                }
            }

            // add service charge
            totalCost += GetServiceCharge(productItems);

            return totalCost;
        }

        public decimal GetServiceCharge(IEnumerable<Product> products)
        {
            if (AllDrinkProducts(products))
            {
                return 0;
            }
            return 0;
        }

        private static bool AllDrinkProducts(IEnumerable<Product> products)
        {
            return products.All(p => p.Type == Enums.ProductType.Drink);
        }
    }
}
