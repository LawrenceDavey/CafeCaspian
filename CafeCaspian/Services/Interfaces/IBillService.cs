﻿using CafeCaspian.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafeCaspian.Services.Interfaces
{
    public interface IBillService
    {
        decimal GetTotalBill(IEnumerable<string> products);
        decimal GetServiceCharge(IEnumerable<Product> products);
        decimal GetMaximumServiceCharge(decimal totalCost, decimal serviceCharge);
        decimal GetProductTotal(IEnumerable<string> products, List<Product> productItems);
    }
}
