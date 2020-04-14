using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafeCaspian.Services.Interfaces
{
    public interface IBillService
    {
        decimal GetTotalBill(IEnumerable<string> products);
        decimal GetServiceCharge(IEnumerable<string> products);
    }
}
