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
            var productItems = new List<Product>();
            var productCost = GetProductTotal(products, productItems);

            // add service charge
            var serviceCharge = GetServiceCharge(productItems);
            decimal maxServiceCharge = GetMaximumServiceCharge(productCost, serviceCharge);
            var totalCost = serviceCharge != 0 ? productCost + maxServiceCharge : productCost;

            return Math.Round(totalCost, 2);
        }

        public decimal GetProductTotal(IEnumerable<string> products, List<Product> productItems)
        {
            var totalCost = 0.0m;
            foreach (var item in products)
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

            return totalCost;
        }

        public decimal GetMaximumServiceCharge(decimal totalCost, decimal serviceCharge)
        {
            return totalCost * serviceCharge > 20.00m ? 20.00m : totalCost * serviceCharge;
        }

        public decimal GetServiceCharge(IEnumerable<Product> products)
        {
            if (AllDrinkProducts(products))
            {
                return 0;
            }
            else if (ProductsIncludeColdFood(products))
            {
                return 0.10m;
            }
            else if (ProductsIncludeHotFood(products))
            {
                return 0.20m;
            }

            return 0;
        }

        private static bool ProductsIncludeHotFood(IEnumerable<Product> products)
        {
            return products.Any(p => p.Type == Enums.ProductType.Food && p.IsHot);
        }

        private static bool ProductsIncludeColdFood(IEnumerable<Product> products)
        {
            return products.Any(p => p.Type == Enums.ProductType.Food && !p.IsHot);
        }

        private static bool AllDrinkProducts(IEnumerable<Product> products)
        {
            return products.All(p => p.Type == Enums.ProductType.Drink);
        }
    }
}
