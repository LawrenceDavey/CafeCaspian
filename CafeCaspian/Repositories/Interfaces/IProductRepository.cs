using CafeCaspian.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafeCaspian.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Product GetByName(string name);
    }
}
