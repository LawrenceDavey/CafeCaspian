using CafeCaspian.Repositories.Interfaces;
using CafeCaspian.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

            foreach(var item in products)
            {
                var product = _productRepo.GetByName(item);

                if (product != null)
                {
                    totalCost += product.Cost;
                }
                else
                {
                    // invalid product
                    throw new NotImplementedException();
                }
            }

            return totalCost;
        }
    }
}
