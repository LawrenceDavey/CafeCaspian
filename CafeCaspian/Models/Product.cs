using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafeCaspian.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
    }

    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException()
        {

        }

        public ProductNotFoundException(string message)
            : base(message)
        {

        }

    }
}
